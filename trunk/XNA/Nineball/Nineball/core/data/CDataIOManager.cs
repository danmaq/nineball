////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library / Copyright (c) 2008-2009 danmaq all rights reserved.
//		──永続データ管理クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using danmaq.Nineball.core.raw;

namespace danmaq.Nineball.core.data {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>永続データ管理クラス。</summary>
	public sealed class CDataIOManager<_T> : IDisposable where _T : new() {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>データファイル名。</summary>
		public readonly string FILE;

		/// <summary>開発コードネーム(半角英数)</summary>
		public readonly string CODENAME;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* events ────────────────────────────────*

		/// <summary>読込完了時に発生するイベント。</summary>
		public event EventHandler loaded;

		/// <summary>保存完了時に発生するイベント。</summary>
		public event EventHandler saved;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>永続データ。</summary>
		public _T data = new _T();

#if XBOX360
		/// <summary>データの入出力対象デバイス。</summary>
		private static StorageDevice device = null;

		/// <summary>ストレージ ファイルの論理コレクション。</summary>
		private static StorageContainer container = null;

#endif
		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="strCodename">開発コードネーム(半角英数)</param>
		/// <param name="strFile">設定ファイル名</param>
		/// <exception cref="System.ArgumentNullException">
		/// 引数にnullが渡された場合。
		/// </exception>
		public CDataIOManager( string strCodename, string strFile ) {
			if( strFile == null ) { throw new ArgumentNullException( "strFile" ); }
			CODENAME = strCodename;
			FILE = strFile;
		}

		//		//* -----------------------------------------------------------------------*
		//		/// <summary>デストラクタ。</summary>
		//		~CDataIOManager() { Dispose(); }

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// アンマネージ リソースの解放およびリセットに関連付けられている
		/// アプリケーション定義のタスクを実行します。
		/// </summary>
		public void Dispose() {
#if XBOX360
			try {
				device = null;
				if( container != null ) {
					container.Dispose();
					container = null;
				}
			}
			catch( Exception ) { }
#endif
		}

		//* -----------------------------------------------------------------------*
		/// <summary>設定データを初期状態にリセットします。</summary>
		/// <remarks>
		/// これを実行した段階ではリセットはコミットされません。
		/// 次回起動時もリセットされた状態にするには<c>save()</c>を実行します。
		/// </remarks>
		public void resetData() {
			CLogger.add( "ゲーム 設定データは初期化されました。" );
			data = new _T();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>設定データを補助記憶装置から読み出します。</summary>
		/// <remarks>
		/// <para>
		/// XBOX360版では読み出すデバイスを選択するダイアログが出現します。
		/// 一方、Windows版では無条件にカレントフォルダが選択されます。
		/// </para>
		/// <para>
		/// また、XBOX360版ではアプリケーションとは非同期にダイアログが
		/// 表示されるので、出現中もアプリケーションは停止せず進行を続けます。
		/// </para>
		/// </remarks>
		public void load() {
			CLogger.add( "ゲーム 設定データを読込しています..." );
#if WINDOWS
			__load( FILE );
#else
			if( ( device == null || !device.IsConnected ) && !Guide.IsVisible ) {
				device = null;
				CGuideManager.reserveSelectDevice( load );
			}
#endif
		}

		//* -----------------------------------------------------------------------*
		/// <summary>設定データを補助記憶装置へ格納します。</summary>
		/// 
		/// <returns>保存に成功した場合、true</returns>
		public bool save() {
			CLogger.add( "ゲーム 設定データを保存しています..." );
			bool bResult = false;
#if WINDOWS
			save( FILE );
			bResult = true;
#else
			if( device != null && device.IsConnected ){
				if( container == null ) { container = device.OpenContainer( CODENAME ); }
				save( Path.Combine( container.Path, FILE ) );
				bResult = true;
			}
#endif
			CLogger.add( "ゲーム 設定データを" + ( bResult ? "保存完了。" : "保存に失敗" ) );
			return bResult;
		}
#if XBOX360
		
		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 読み出しデバイス選択ダイアログを終了した時に非同期で呼び出されます。
		/// ここから自動的に読み出し処理へ入ります。
		/// </summary>
		/// 
		/// <param name="result">非同期操作のステータス</param>
		private void load( IAsyncResult result ){
			device = Guide.EndShowStorageDeviceSelector( result );
			if( device != null && device.IsConnected ){
				if( container == null ) { container = device.OpenContainer( CODENAME ); }
				__load( Path.Combine( container.Path, FILE ) );
			}
			else { __load( null ); }
		}
#endif

		//* -----------------------------------------------------------------------*
		/// <summary>設定データを補助記憶装置から読み出します。</summary>
		/// <remarks>
		/// XBOX360版では生のXML、Windows版ではDeflate圧縮されたXMLから読み出します。
		/// </remarks>
		/// 
		/// <param name="strPath">設定データ ファイルへのパス</param>
		private void __load( string strPath ) {
			bool bReaded = false;
			if( strPath != null && File.Exists( strPath ) ) {
				Stream stream = null;
				try {
#if WINDOWS
					stream = new DeflateStream(
						File.Open( strPath, FileMode.Open, FileAccess.Read ),
						CompressionMode.Decompress );
#else
					stream = File.Open( strPath, FileMode.Open, FileAccess.Read );
#endif
					data = ( _T )( ( new XmlSerializer( typeof( _T ) ) ).Deserialize( stream ) );
					if( data != null ) { bReaded = true; }
				}
				catch( Exception e ) {
					CLogger.add( "設定データに互換性がありません。解決するためにデータをリセットします。" );
					CLogger.add( e );
				}
				if( stream != null ) { stream.Close(); }
			}
			else { CLogger.add( "指定した補助記憶装置に設定データが存在しません。" ); }
			if( !bReaded ) {
				resetData();
				save();
			}
			CLogger.add( data.ToString() );
			if( loaded != null ) { loaded( this, EventArgs.Empty ); }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>設定データを補助記憶装置へ格納します。</summary>
		/// <remarks>
		/// XBOX360版では生のXML、Windows版ではDeflate圧縮されたXMLが格納されます。
		/// </remarks>
		/// 
		/// <param name="strPath">設定データ ファイルへのパス</param>
		private void save( string strPath ) {
			if( strPath != null ) {
#if WINDOWS
				DeflateStream stream = new DeflateStream(
					File.Open( strPath, FileMode.Create, FileAccess.Write ),
					CompressionMode.Compress );
#else
				FileStream stream = File.Open( strPath, FileMode.Create, FileAccess.Write );
#endif
				( new XmlSerializer( typeof( _T ) ) ).Serialize( stream, data );
				stream.Close();
				if( saved != null ) { saved( this, EventArgs.Empty ); }
			}
		}
	}
}
