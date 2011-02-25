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
using danmaq.nineball.Properties;
using Microsoft.Xna.Framework.Storage;

namespace danmaq.nineball.util.storage
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>I/Oに必要なデータを保持するクラス。</summary>
	public sealed class CIOInfo
		: IDisposable
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CIOInfo instance = new CIOInfo();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>データの入出力対象デバイス。</summary>
		public StorageDevice device;

		/// <summary>アプリケーション タイトル文字列。</summary>
		private string m_titleName;

		/// <summary>Windows版におけるXNAセーブデータのルート フォルダ。</summary>
		private string windowsXNARoot = null;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CIOInfo()
		{
#if WINDOWS
			string documents =
				Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			windowsXNARoot = Path.Combine(documents, "SavedGames");
#endif
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>ストレージ ファイルの論理コレクションを取得します。</summary>
		/// 
		/// <value>ストレージ ファイルの論理コレクション。</value>
		public StorageContainer container
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>デバイスの準備が出来ているかどうかを取得します。</summary>
		/// 
		/// <value>デバイスの準備が出来ている場合、<c>true</c>。</value>
		public bool deviceReady
		{
			get
			{
				return (device != null && device.IsConnected) ||
					!(CGuideWrapper.instance == null ||
						CGuideWrapper.instance.isAvaliableUseGamerService);
			}
		}

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
				if (m_titleName == null)
				{
					m_titleName = value;
#if WINDOWS
					windowsXNARoot = Path.Combine(
						Path.Combine(windowsXNARoot, value), "AllPlayers");
					new DirectoryInfo(windowsXNARoot).Create();
#endif
				}
				else if (m_titleName != value)
				{
					throw new InvalidOperationException(Resources.IO_ERR_MODIFY_TITLE);
				}
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ファイルへの絶対パスを取得します。</summary>
		/// 
		/// <param name="strFileName">ファイル名。</param>
		/// <returns>ファイルへの絶対パス。</returns>
		public string getPath(string strFileName)
		{
			string strResult = null;
			if(deviceReady)
			{
				if (device != null)
				{
					if (container == null)
					{
						container = device.OpenContainer(titleName);
					}
					strResult = Path.Combine(container.Path, strFileName);
				}
				else
				{
					strResult = Path.Combine(windowsXNARoot, strFileName);
				}
			}
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// アンマネージ リソースの解放およびリセットに関連付けられている
		/// アプリケーション定義のタスクを実行します。
		/// </summary>
		public void Dispose()
		{
			device = null;
			m_titleName = null;
			if (container != null)
			{
				container.Dispose();
				container = null;
			}
		}
	}
}
