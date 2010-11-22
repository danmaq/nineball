////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if WINDOWS
#define COMPRESS
#endif

using System;
using System.IO;
using System.Xml.Serialization;
using danmaq.nineball.data;
using Microsoft.Xna.Framework.GamerServices;

#if COMPRESS
using System.IO.Compression;
#endif

namespace danmaq.nineball.util.storage
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>永続データ管理クラス。</summary>
	public sealed class CSerializeHelper<_T> where _T : new()
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>データファイル名。</summary>
		public readonly string fileName;

		/// <summary>データ型名。</summary>
		public readonly string typeName = typeof(_T).FullName;

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
		/// <param name="titleName">XNA Framework タイトル名(半角英数)</param>
		/// <param name="fileName">設定ファイル名</param>
		/// <exception cref="System.ArgumentNullException">
		/// 引数にnullが渡された場合。
		/// </exception>
		public CSerializeHelper(string titleName, string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (titleName == null && CIOInfo.instance.titleName == null)
			{
				throw new ArgumentNullException("strFile");
			}
			CIOInfo.instance.titleName = titleName;
			this.fileName = fileName;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>設定データを初期状態にリセットします。</summary>
		/// <remarks>
		/// これを実行した段階ではリセットはコミットされません。
		/// 次回起動時もリセットされた状態にするには<c>save()</c>を実行します。
		/// </remarks>
		public void resetData()
		{
			CLogger.add(string.Format("{0}は初期化されました。", typeName));
			data = new _T();
		}


		//* -----------------------------------------------------------------------*
		/// <summary>設定データを補助記憶装置から読み出します。</summary>
		/// <remarks>
		/// XBOX360版ではアプリケーションとは非同期にダイアログが
		/// 表示されるので、出現中もアプリケーションは停止せず進行を続けます。
		/// </remarks>
		public void load()
		{
			CLogger.add(string.Format("{0}を{1}へ読込しています...。", fileName, typeName));
			CIOInfo info = CIOInfo.instance;
			if (info.deviceReady)
			{
				load(info.getPath(fileName));
			}
			else
			{
				if (CGuideWrapper.instance == null)
				{
					Guide.BeginShowStorageDeviceSelector(onSelectedDevice, this);
				}
				else
				{
					CGuideWrapper.instance.BeginShowStorageDeviceSelector(onSelectedDevice, this);
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>設定データを補助記憶装置へ格納します。</summary>
		/// 
		/// <returns>保存に成功した場合、true</returns>
		public bool save()
		{
			CLogger.add(string.Format("{0}を{1}へ保存しています...。", typeName, fileName));
			bool result = false;
			if (CIOInfo.instance.deviceReady)
			{
				result = save(CIOInfo.instance.getPath(fileName));
			}
			CLogger.add(string.Format("{0}を{1}へ保存{2}。",
				typeName, fileName, result ? "完了" : "に失敗"));
			if (saved != null)
			{
				saved(this, result);
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>設定データを補助記憶装置から読み出します。</summary>
		/// <remarks>
		/// XBOX360版では生のXML、Windows版ではDeflate圧縮されたXMLから読み出します。
		/// </remarks>
		/// 
		/// <param name="path">設定データ ファイルへのパス</param>
		private void load(string path)
		{
			bool readed = false;
			if (path != null && File.Exists(path))
			{
				Stream stream = null;
				try
				{
#if COMPRESS
					stream = new DeflateStream(
						File.Open(path, FileMode.Open, FileAccess.Read),
						CompressionMode.Decompress);
#else
					stream = File.Open(path, FileMode.Open, FileAccess.Read);
#endif
					data = (_T)((new XmlSerializer(typeof(_T), new XmlRootAttribute())).Deserialize(stream));
					if (data != null)
					{
						readed = true;
						CLogger.add(string.Format("{0}を{1}へ読み込みました。",
							fileName, typeName));
					}
				}
				catch (Exception e)
				{
					CLogger.add("設定データに互換性がありません。解決するためにデータをリセットします。");
					CLogger.add(e);
				}
				if (stream != null)
				{
					stream.Close();
				}
			}
			else
			{
				CLogger.add(string.Format("指定した補助記憶装置に{0}が存在しません。", fileName));
			}
			if (!readed)
			{
				resetData();
				readed = save();
			}
			CLogger.add(data.ToString());
			if (loaded != null)
			{
				loaded(this, readed);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>設定データを補助記憶装置へ格納します。</summary>
		/// <remarks>
		/// XBOX360版では生のXML、Windows版ではDeflate圧縮されたXMLが格納されます。
		/// </remarks>
		/// 
		/// <param name="path">設定データ ファイルへのパス</param>
		/// <returns>正常に保存できた場合、<c>true</c></returns>
		private bool save(string path)
		{
			bool result = false;
			if (path != null)
			{
#if COMPRESS
				DeflateStream stream = null;
#else
				FileStream stream = null;
#endif
				try
				{
#if COMPRESS
					stream = new DeflateStream(
						File.Open(path, FileMode.Create, FileAccess.Write),
						CompressionMode.Compress);
#else
					stream = File.Open(strPath, FileMode.Create, FileAccess.Write);
#endif
					(new XmlSerializer(typeof(_T), new XmlRootAttribute())).Serialize(stream, data);
				}
				catch (Exception e)
				{
					CLogger.add(e);
				}
				finally
				{
					if (stream != null)
					{
						stream.Close();
					}
				}
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 読み出しデバイス選択ダイアログを終了した時に非同期で呼び出されます。
		/// ここから自動的に読み出し処理へ入ります。
		/// </summary>
		/// 
		/// <param name="result">非同期操作のステータス</param>
		private void onSelectedDevice(IAsyncResult result)
		{
			CIOInfo.instance.device = CGuideWrapper.instance == null ?
				Guide.EndShowStorageDeviceSelector(result) :
				CGuideWrapper.instance.EndShowStorageDeviceSelector(result);
			CIOInfo info = CIOInfo.instance;
			load(
				info.deviceReady ?
				info.getPath(fileName) : null);
		}
	}
}
