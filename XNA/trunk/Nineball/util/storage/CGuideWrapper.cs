////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;
using danmaq.nineball.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;

namespace danmaq.nineball.util.storage
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>ガイド ユーザー インターフェイスのラッパー。</para>
	/// <para>
	/// ガイド使用の可不可を自動判断し、使用不可の場合はWindowsの機能で代用します。
	/// </para>
	/// </summary>
	/// <remarks>
	/// 当分の間はWindowsの機能で代用可能なダイアログ機能、ストレージ選択機能のみ
	/// サポートします。それ以外の機能は直接Guideを呼び出してください。
	/// </remarks>
	public sealed class CGuideWrapper
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>メソッドの進行状況の追跡に使用されるクラス。</summary>
		/// <remarks>
		/// このクラスを使用する場合、処理は全て同期的に行われるため、
		/// 実質的にダミーのオブジェクトです。
		/// </remarks>
		private sealed class CNullAsyncResult
			: IAsyncResult
		{

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="state">
			/// この要求を一意に識別するユーザー作成オブジェクト。
			/// </param>
			public CNullAsyncResult(object state)
			{
				AsyncState = state;
			}

			//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* properties ──────────────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>
			/// 非同期操作についての情報を限定または格納するユーザー定義の
			/// オブジェクトを取得します。
			/// </summary>
			/// 
			/// <value>
			/// 非同期操作についての情報を限定または格納するユーザー定義のオブジェクト。
			/// </value>
			public object AsyncState
			{
				get;
				private set;
			}

			//* -----------------------------------------------------------------------*
			/// <summary>
			/// 非同期操作が完了するまで待機するために使用するオブジェクトを取得します。
			/// </summary>
			/// 
			/// <value>
			/// <c>null</c>。このオブジェクトが返される場合、非同期処理は行われません。
			/// </value>
			public WaitHandle AsyncWaitHandle
			{
				get
				{
					return null;
				}
			}

			//* -----------------------------------------------------------------------*
			/// <summary>
			/// 非同期操作が同期的に完了したかどうかを示す値を取得します。
			/// </summary>
			/// 
			/// <value>
			/// <c>true</c>。このオブジェクトが返される場合、非同期処理は行われません。
			/// </value>
			public bool CompletedSynchronously
			{
				get
				{
					return true;
				}
			}

			//* -----------------------------------------------------------------------*
			/// <summary>非同期操作が完了したかどうかを示す値を取得します。</summary>
			/// 
			/// <value>
			/// <c>true</c>。このオブジェクトが返された時には、既に処理を完了しています。
			/// </value>
			public bool IsCompleted
			{
				get
				{
					return true;
				}
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>ボタン一覧。</summary>
		private readonly string[] buttons = { "OK" };

		/// <summary>ゲーマー サービス コンポーネント。</summary>
		private readonly GamerServicesComponent gsc;

		/// <summary>ゲーム コンポーネントをアタッチするゲーム。</summary>
		private readonly Game game;

#if WINDOWS
		/// <summary>
		/// ガイド ユーザー インターフェイスに対応するWindowsアイコン一覧。
		/// </summary>
		private readonly System.Windows.Forms.MessageBoxIcon[] iconset =
			new System.Windows.Forms.MessageBoxIcon[4];
#endif

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>オフライン時の体験版状態。</summary>
		public bool m_trialMode = false;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="game">ゲーム コンポーネントをアタッチするゲーム。</param>
		public CGuideWrapper(Game game)
		{
			if (instance != null)
			{
				throw new InvalidOperationException(
					string.Format(Resources.ERR_SINGLETON, typeof(CGuideWrapper).FullName));
			}
			instance = this;
			this.game = game;
			try
			{
				gsc = new GamerServicesComponent(game);
				game.Components.Add(gsc);
				isAvaliableUseGamerService = true;
			}
			catch (Exception e)
			{
				CLogger.add(Resources.WARN_GAMER_SERVICE);
				CLogger.add(e);
				removeGamerServiceComponent();
			}
#if WINDOWS
			iconset[(int)MessageBoxIcon.None] = System.Windows.Forms.MessageBoxIcon.None;
			iconset[(int)MessageBoxIcon.Error] = System.Windows.Forms.MessageBoxIcon.Exclamation;
			iconset[(int)MessageBoxIcon.Warning] = System.Windows.Forms.MessageBoxIcon.Information;
			iconset[(int)MessageBoxIcon.Alert] = System.Windows.Forms.MessageBoxIcon.Stop;
#endif
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>クラス オブジェクトを取得します。</summary>
		/// 
		/// <value>クラス オブジェクト。</value>
		public static CGuideWrapper instance
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ゲーマー サービスが使用可能かどうかを取得します。</summary>
		/// 
		/// <value>ゲーマー サービスが使用可能である場合、<c>true</c>。</value>
		public bool isAvaliableUseGamerService
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// ガイド ユーザ インターフェイス画面が有効であるかどうかを取得します。
		/// </summary>
		/// 
		/// <value>
		/// ガイド ユーザ インターフェイス画面が有効である場合、<c>true</c>。
		/// </value>
		public bool IsVisible
		{
			get
			{
				return isAvaliableUseGamerService ? Guide.IsVisible : false;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>体験版であるかどうかを取得/設定します。</summary>
		/// <remarks>書き込みはゲーマー サービスが無効時のみ反映されます。</remarks>
		/// 
		/// <value>体験版である場合、<c>true</c>。</value>
		public bool IsTrialMode
		{
			get
			{
				return isAvaliableUseGamerService ? Guide.IsTrialMode : m_trialMode;
			}
			set
			{
				m_trialMode = value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>通知が画面上に表示される場所を取得/設定します。</summary>
		/// 
		/// <value>通知メッセージ ボックスの位置。</value>
		public NotificationPosition NotificationPosition
		{
			get
			{
				return isAvaliableUseGamerService ?
					Guide.NotificationPosition : NotificationPosition.BottomCenter;
			}
			set
			{
				if (isAvaliableUseGamerService)
				{
					Guide.NotificationPosition = value;
				}
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ゲーマー サービス コンポーネントを削除します。</summary>
		public void removeGamerServiceComponent()
		{
			game.Components.Remove(gsc);
			isAvaliableUseGamerService = false;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>メッセージボックスの表示を開始します。</summary>
		/// 
		/// <param name="title">メッセージのタイトル。</param>
		/// <param name="text">メッセージ ボックスに表示されるテキスト。</param>
		/// <param name="icon">メッセージ ボックスに表示されるアイコンの種類。</param>
		/// <param name="callback">
		/// 非同期操作が終了すると呼び出されるメソッド。
		/// </param>
		/// <param name="state">
		/// この要求を一意に識別するユーザー作成オブジェクト。
		/// </param>
		/// <returns>メソッドの進行状況の追跡に使用されるオブジェクト。</returns>
		public IAsyncResult BeginShowMessageBox(string title, string text,
			MessageBoxIcon icon, AsyncCallback callback, object state)
		{
			IAsyncResult result = null;
			if (isAvaliableUseGamerService)
			{
				result = Guide.BeginShowMessageBox(
					title, text, buttons, 0, icon, callback, state);
			}
			else
			{
				result = new CNullAsyncResult(state);
#if WINDOWS
				System.Windows.Forms.MessageBox.Show(
					text, title, System.Windows.Forms.MessageBoxButtons.OK, iconset[(int)icon]);
#endif
				callback(result);
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>メッセージボックスの表示を終了します。</summary>
		/// 
		/// <param name="result">
		/// メソッドの進行状況の追跡に使用されるオブジェクト。
		/// </param>
		public int? EndShowMessageBox(IAsyncResult result)
		{
			int? res = null;
			if (isAvaliableUseGamerService)
			{
				res = Guide.EndShowMessageBox(result);
			}
			return res;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// ストレージ セレクター ユーザー インターフェイスの表示を開始します。
		/// </summary>
		/// 
		/// <param name="callback">
		/// 非同期操作が終了すると呼び出されるメソッド。
		/// </param>
		/// <param name="state">
		/// この要求を一意に識別するユーザー作成オブジェクト。
		/// </param>
		/// <returns>メソッドの進行状況の追跡に使用されるオブジェクト。</returns>
		public IAsyncResult BeginShowStorageDeviceSelector(AsyncCallback callback, Object state)
		{
			IAsyncResult result = null;
			if (isAvaliableUseGamerService)
			{
				result = Guide.BeginShowStorageDeviceSelector(callback, state);
			}
			else
			{
				result = new CNullAsyncResult(state);
				callback(result);
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// ストレージ セレクター ユーザー インターフェイスの表示を終了します。
		/// </summary>
		/// 
		/// <param name="result">
		/// メソッドの進行状況の追跡に使用されるオブジェクト。
		/// </param>
		public StorageDevice EndShowStorageDeviceSelector(IAsyncResult result)
		{
			StorageDevice res = null;
			if (isAvaliableUseGamerService)
			{
				res = Guide.EndShowStorageDeviceSelector(result);
			}
			return res;
		}
	}
}
