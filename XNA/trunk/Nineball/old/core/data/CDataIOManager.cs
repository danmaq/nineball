////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using danmaq.nineball.data;
using danmaq.nineball.Properties;
using danmaq.nineball.util;

#if XBOX360
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;
using danmaq.nineball.old.core.inner;
#endif

namespace danmaq.nineball.old.core.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>永続データ管理クラスの共有部分。</para>
	/// <para>ジェネリックだとstaticが共有されないので分割。</para>
	/// </summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete("このクラスは今後サポートされません。danmaq.nineball.util.storage.CIOInfoを使用してください。")]
	sealed class CDataIOManager : IDisposable
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CDataIOManager instance = new CDataIOManager();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*
#if XBOX360

		/// <summary>データの入出力対象デバイス。</summary>
		public StorageDevice device = null;

		/// <summary>ストレージ ファイルの論理コレクション。</summary>
		public StorageContainer container = null;
#endif

		/// <summary>アプリケーション タイトル文字列。</summary>
		private string m_titleName;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CDataIOManager()
		{
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

#if XBOX360
		//* -----------------------------------------------------------------------*
		/// <summary>デバイスの準備が出来ているかどうかを取得します。</summary>
		/// 
		/// <value>デバイスの準備が出来ている場合、<c>true</c>。</value>
		public bool deviceReady
		{
			get
			{
				return device != null && device.IsConnected;
			}
		}
#endif

		//* -----------------------------------------------------------------------*
		/// <summary>アプリケーション タイトルを設定/取得します。</summary>
		/// 
		/// <value>アプリケーション タイトル文字列。</value>
		/// <exception cref="System.InvalidOperationException">
		/// アプリケーション タイトルを変更しようとしたとき。
		/// </exception>
		public string titleName
		{
			get
			{
				return m_titleName;
			}
			set
			{
				if(m_titleName != null && m_titleName != value)
				{
					throw new InvalidOperationException(Resources.IO_ERR_MODIFY_TITLE);
				}
				m_titleName = value;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

#if XBOX360
		//* -----------------------------------------------------------------------*
		/// <summary>デバイスを初期化します。</summary>
		/// 
		/// <param name="result">非同期操作のステータス。</param>
		public void initializeDevice(IAsyncResult result)
		{
			device = Guide.EndShowStorageDeviceSelector(result);
		}
#endif

		//* -----------------------------------------------------------------------*
		/// <summary>ファイルへの絶対パスを取得します。</summary>
		/// 
		/// <param name="strFileName">ファイル名。</param>
		/// <returns>ファイルへの絶対パス。</returns>
		public string getPath(string strFileName)
		{
			string strResult = strFileName;
#if !WINDOWS
			if(deviceReady)
			{
				if(container == null)
				{
					container = device.OpenContainer(titleName);
				}
				strResult = Path.Combine(container.Path, strFileName);
			}
#endif
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// アンマネージ リソースの解放およびリセットに関連付けられている
		/// アプリケーション定義のタスクを実行します。
		/// </summary>
		public void Dispose()
		{
#if XBOX360
			try
			{
				device = null;
				if(container != null)
				{
					container.Dispose();
					container = null;
				}
			}
			catch(Exception e)
			{
				CLogger.add(string.Format(Resources.GENERAL_ERR_RELEASE, "I/O"));
				CLogger.add(e);
			}
			GC.SuppressFinalize(this);
#endif
		}

	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>永続データ管理クラス。</summary>
	[Obsolete("このクラスは今後サポートされません。danmaq.nineball.util.storage.CSerializeHelper<T>を使用してください。")]
	public sealed class CDataIOManager<_T> : IDisposable where _T : new()
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>データファイル名。</summary>
		public readonly string FILE;

		/// <summary>データ型名。</summary>
		public readonly string typeName = typeof(_T).FullName;

		/// <summary>静的な情報。</summary>
		private readonly CDataIOManager staticMembers = CDataIOManager.instance;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* events ────────────────────────────────*

		/// <summary>読込完了時に発生するイベント。</summary>
		public event EventHandler<CEventMonoValue<bool>> loaded;

		/// <summary>保存完了時に発生するイベント。</summary>
		public event EventHandler<CEventMonoValue<bool>> saved;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>永続データ。</summary>
		public _T data = new _T();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="strTitleName">XNA Framework タイトル名(半角英数)</param>
		/// <param name="strFile">設定ファイル名</param>
		/// <exception cref="System.ArgumentNullException">
		/// 引数にnullが渡された場合。
		/// </exception>
		public CDataIOManager(string strTitleName, string strFile)
		{
			if(strFile == null || strTitleName == null)
			{
				throw new ArgumentNullException("strFile");
			}
			staticMembers.titleName = strTitleName;
			FILE = strFile;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// アンマネージ リソースの解放およびリセットに関連付けられている
		/// アプリケーション定義のタスクを実行します。
		/// </summary>
		public void Dispose()
		{
			staticMembers.Dispose();
			GC.SuppressFinalize(this);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>設定データを初期状態にリセットします。</summary>
		/// <remarks>
		/// これを実行した段階ではリセットはコミットされません。
		/// 次回起動時もリセットされた状態にするには<c>save()</c>を実行します。
		/// </remarks>
		public void resetData()
		{
			CLogger.add(string.Format(Resources.GENERAL_INFO_INITIALIZED, typeName));
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
		public void load()
		{
			CLogger.add(string.Format(Resources.IO_INFO_LOADING, FILE, typeName));
#if WINDOWS
			load(FILE);
#else
			if(staticMembers.deviceReady)
			{
				load(staticMembers.getPath(FILE));
			}
			else
			{
				CGuideManager.reserveSelectDevice(load);
			}
#endif

		}

		//* -----------------------------------------------------------------------*
		/// <summary>設定データを補助記憶装置へ格納します。</summary>
		/// 
		/// <returns>保存に成功した場合、true</returns>
		public bool save()
		{
			CLogger.add(string.Format(Resources.IO_INFO_SAVING, FILE, typeName));
			bool bResult = false;
#if WINDOWS
			save(FILE);
			bResult = true;
#else
			if(staticMembers.deviceReady)
			{
				bResult = save(staticMembers.getPath(FILE));
			}
#endif
			CLogger.add(string.Format(Resources.IO_INFO_SAVED,
				typeName, FILE, bResult ? Resources.SUCCEEDED : Resources.FAILED));
			if(saved != null)
			{
				saved(this, bResult);
			}
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
			staticMembers.initializeDevice(result);
			load(
				staticMembers.deviceReady ?
				staticMembers.getPath(FILE) : null);
		}
#endif

		//* -----------------------------------------------------------------------*
		/// <summary>設定データを補助記憶装置から読み出します。</summary>
		/// <remarks>
		/// XBOX360版では生のXML、Windows版ではDeflate圧縮されたXMLから読み出します。
		/// </remarks>
		/// 
		/// <param name="strPath">設定データ ファイルへのパス</param>
		private void load(string strPath)
		{
			bool bReaded = false;
			if(strPath != null && File.Exists(strPath))
			{
				Stream stream = null;
				try
				{
#if WINDOWS
					stream = new DeflateStream(
						File.Open(strPath, FileMode.Open, FileAccess.Read),
						CompressionMode.Decompress);
#else
					stream = File.Open( strPath, FileMode.Open, FileAccess.Read );
#endif
					data = (_T)((new XmlSerializer(typeof(_T), new XmlRootAttribute())).Deserialize(stream));
					if(data != null)
					{
						bReaded = true;
						CLogger.add(string.Format(Resources.IO_INFO_LOADED, FILE, typeName));
					}
				}
				catch(Exception e)
				{
					CLogger.add(Resources.IO_WARN_XML_COLLISION);
					CLogger.add(e);
				}
				if(stream != null)
				{
					stream.Close();
				}
			}
			else
			{
				CLogger.add(string.Format(Resources.IO_WARN_NOT_FOUND, FILE));
			}
			if(!bReaded)
			{
				resetData();
				bReaded = save();
			}
			CLogger.add(data.ToString());
			if(loaded != null)
			{
				loaded(this, bReaded);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>設定データを補助記憶装置へ格納します。</summary>
		/// <remarks>
		/// XBOX360版では生のXML、Windows版ではDeflate圧縮されたXMLが格納されます。
		/// </remarks>
		/// 
		/// <param name="strPath">設定データ ファイルへのパス</param>
		/// <returns>正常に保存できた場合、<c>true</c></returns>
		private bool save(string strPath)
		{
			bool bResult = false;
			if(strPath != null)
			{
#if WINDOWS
				DeflateStream stream = null;
#else
				FileStream stream = null;
#endif
				try
				{
#if WINDOWS
					stream = new DeflateStream(
						File.Open(strPath, FileMode.Create, FileAccess.Write),
						CompressionMode.Compress);
#else
					stream = File.Open( strPath, FileMode.Create, FileAccess.Write );
#endif
					(new XmlSerializer(typeof(_T), new XmlRootAttribute())).Serialize(stream, data);
				}
				catch(Exception e)
				{
					CLogger.add(e);
				}
				finally
				{
					if(stream != null)
					{
						stream.Close();
					}
				}
			}
			return bResult;
		}
	}
}
