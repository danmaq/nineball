////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Xml.Serialization;
using danmaq.nineball.data;
using danmaq.nineball.Properties;
using Microsoft.Xna.Framework.GamerServices;

#if WINDOWS
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

		/// <summary>保存データの圧縮を施すかどうか。</summary>
		/// <remarks>
		/// XBOX360版ではダミー変数となります。
		/// この値をtrueにしても圧縮されません。
		/// </remarks>
		private bool m_compress = true;

		/// <summary>永続データ。</summary>
		public _T data = new _T();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="titleName">XNA Framework タイトル名(半角英数)</param>
		/// <param name="fileName">設定ファイル名</param>
		/// <param name="compress">
		/// <para>保存データの圧縮を施すかどうか。</para>
		/// <para>
		/// XBOX360版ではダミー引数となります。
		/// この値をtrueにしてもいかなる変化もありません。
		/// </para>
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		/// 引数にnullが渡された場合。
		/// </exception>
		public CSerializeHelper(string titleName, string fileName, bool compress)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (titleName == null && CIOInfo.instance.titleName == null)
			{
				throw new ArgumentNullException("titleName");
			}
			CIOInfo.instance.titleName = titleName;
			this.fileName = fileName;
			this.m_compress = compress;
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
			CLogger.add(string.Format(Resources.GENERAL_INFO_INITIALIZED, typeName));
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
			CLogger.add(string.Format(Resources.IO_INFO_LOADING, fileName, typeName));
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
			CLogger.add(string.Format(Resources.IO_INFO_SAVING, typeName, fileName));
			bool result = false;
			if (CIOInfo.instance.deviceReady)
			{
				result = save(CIOInfo.instance.getPath(fileName));
			}
			CLogger.add(string.Format(Resources.IO_INFO_SAVED,
				typeName, fileName, result ? Resources.SUCCEEDED : Resources.FAILED));
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
#if WINDOWS
					if (m_compress)
					{
						stream = new DeflateStream(
							File.Open(path, FileMode.Open, FileAccess.Read),
							CompressionMode.Decompress);
					}
					else
#endif
					{
						stream = File.Open(path, FileMode.Open, FileAccess.Read);
					}
						data = (_T)((new XmlSerializer(typeof(_T), new XmlRootAttribute())).Deserialize(stream));
					if (data != null)
					{
						readed = true;
						CLogger.add(string.Format(Resources.IO_INFO_LOADED, fileName, typeName));
					}
				}
				catch (Exception e)
				{
					CLogger.add(Resources.IO_WARN_XML_COLLISION);
					CLogger.add(e);
				}
				if (stream != null)
				{
					stream.Close();
				}
			}
			else
			{
				CLogger.add(string.Format(Resources.IO_WARN_NOT_FOUND, fileName));
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
				Stream stream = null;
				try
				{
					stream = File.Open(path, FileMode.Create, FileAccess.Write);
#if WINDOWS
					if (m_compress)
					{
						stream = new DeflateStream(stream, CompressionMode.Compress);
					}
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
