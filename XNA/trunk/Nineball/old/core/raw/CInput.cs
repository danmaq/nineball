////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using danmaq.nineball.data;
using danmaq.nineball.entity.manager;
using danmaq.nineball.old.core.data;
using danmaq.nineball.old.data;
using danmaq.nineball.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.old.core.raw
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>入力制御・管理クラス。</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete("このクラスは今後サポートされません。danmaq.nineball.util.collection.input.CInputHelper使用してください。")]
	public sealed class CInput : IDisposable
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>ボタンの入力状態を保持するクラス。</summary>
		public struct SButtonState
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>連続入力となるまでのフレーム時間間隔。</summary>
			private readonly ushort KEYLOOP_START;

			/// <summary>押しっぱなしで連続入力となるフレーム時間間隔。</summary>
			private readonly ushort KEYLOOP_INTERVAL;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="uKeyLoopStart">
			/// 連続入力となるまでのフレーム時間間隔
			/// </param>
			/// <param name="uKeyLoopInterval">
			/// 押しっぱなしで連続入力となるフレーム時間間隔
			/// </param>
			public SButtonState(ushort uKeyLoopStart, ushort uKeyLoopInterval)
			{
				KEYLOOP_START = uKeyLoopStart;
				KEYLOOP_INTERVAL = uKeyLoopInterval;
				press = false;
				count = 0;
			}

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>現在ボタンが押されているかどうか。</summary>
			public bool press;

			/// <summary>最後にボタンの状態が更新されてからの時間。</summary>
			public int count;

			//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* properties ──────────────────────────────*

			/// <summary>現在のフレームでボタンが押されたかどうか。</summary>
			public bool push
			{
				get
				{
					return (press && count == 0);
				}
			}

			/// <summary>現在のフレームでボタンが離されたかどうか。</summary>
			public bool release
			{
				get
				{
					return (!press && count == 0);
				}
			}

			/// <summary><c>push</c>の押しっぱなしループ対応版。</summary>
			public bool pushLoop
			{
				get
				{
					return (press && countLoop == 0);
				}
			}

			/// <summary><c>count</c>の押しっぱなしループ対応版。</summary>
			public int countLoop
			{
				get
				{
					return ((count >= KEYLOOP_START) ? (count % KEYLOOP_INTERVAL) : count);
				}
			}

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>現在ボタンが押されているかどうかを取得します。</summary>
			/// 
			/// <param name="b">ボタン状態</param>
			/// <returns>現在ボタンが押されている場合、<c>true</c></returns>
			public static implicit operator bool(SButtonState b)
			{
				return b.press;
			}

			//* -----------------------------------------------------------------------*
			/// <summary>ボタン状態の更新をします。</summary>
			/// 
			/// <param name="bState">最新のボタン状態</param>
			public void refresh(bool bState)
			{
				if(press == bState)
				{
					count++;
				}
				else
				{
					press = bState;
					count = 0;
				}
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		//		/// <summary>入力関係初期設定用構造体。</summary>
		//		public readonly SInputInitializeData INITIAL_DATA;

		/// <summary>接続されているXBOX360コントローラ一覧。</summary>
		public readonly IEnumerable<PlayerIndex> CONNECTED;

		/// <summary>ボタンの入力状態一覧。</summary>
		public readonly SButtonState[] BUTTON_STATE;

		/// <summary>マイクロスレッド管理 クラス。</summary>
		private readonly CCoRoutineManager MICROTHREAD_MANAGER = new CCoRoutineManager();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>XBOX360コントローラ用のボタン割り当て値の一覧。</summary>
		public Buttons[] assignXBOX360;

		/// <summary>キーボード用のキー割り当て値の一覧。</summary>
		public Keys[] assignKeyboard;

#if WINDOWS
		/// <summary>レガシ ゲームパッド用のボタン割り当て値の一覧。</summary>
		public int[] assignLegacy;

		/// <summary>レガシ デバイス用ゲームパッド入力管理クラス。</summary>
		private CLegacyInputManager m_mgrLegacy = null;

		/// <summary>レガシ ゲームパッド デバイス。</summary>
		private CLegacyInput m_legacy = null;

#endif
		/// <summary>左モーターのフォースフィードバックの強さ。</summary>
		private float m_fFFLeft = 0;

		/// <summary>右モーターのフォースフィードバックの強さ。</summary>
		private float m_fFFRight = 0;

		/// <summary>ボタン状態を格納する一時バッファ。</summary>
		private bool[] bState = new bool[0];

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="hWnd">ウィンドウハンドル</param>
		/// <param name="buttons">十字キーを除くボタンの数。</param>
		/// <param name="keyLoopStart">連続入力となるまでのフレーム時間間隔。</param>
		/// <param name="keyLoopInterval">押しっぱなしで連続入力となるフレーム時間間隔。</param>
		public CInput(IntPtr hWnd, byte buttons, ushort keyLoopStart, ushort keyLoopInterval)
			:
			this(hWnd, buttons, keyLoopStart, keyLoopInterval, detectConnected())
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="hWnd">ウィンドウハンドル</param>
		/// <param name="buttons">十字キーを除くボタンの数。</param>
		/// <param name="keyLoopStart">連続入力となるまでのフレーム時間間隔。</param>
		/// <param name="keyLoopInterval">押しっぱなしで連続入力となるフレーム時間間隔。</param>
		/// <param name="connected">接続されているXBOX360コントローラ一覧</param>
		public CInput(IntPtr hWnd, byte buttons, ushort keyLoopStart, ushort keyLoopInterval, PlayerIndex[] connected)
		{
			CLogger.add("入力処理・XBOX360コントローラの初期化をしています...");
			int nButtons = buttons;
			int nFullButtons = nButtons + 4;
			CONNECTED = connected.Distinct();
			isUseXBOX360GamePad = CONNECTED.Count() > 0;
			if(!isUseXBOX360GamePad)
			{
				CLogger.add("XBOX360コントローラを使用しません。");
			}
#if WINDOWS
			try
			{
				legacyManager = new CLegacyInputManager(hWnd);
			}
			catch(FileNotFoundException e)
			{
				CLogger.add(
					"Managed DirectXの初期化に失敗しました。\r\n" +
					"最新版のDirectX(update June 2008)がインストールされているか再確認してください。");
				CLogger.add(e.ToString());
			}
			assignLegacy = new int[nButtons];
			if(legacy == null)
			{
				CLogger.add("レガシ ゲームパッドを使用しません。");
			}
#endif
			assignXBOX360 = new Buttons[nButtons];
			assignKeyboard = new Keys[nFullButtons];
			BUTTON_STATE = new SButtonState[nFullButtons];
			for(int i = 0; i < nFullButtons; i++)
			{
				BUTTON_STATE[i] = new SButtonState(keyLoopStart, keyLoopInterval);
			}
			CLogger.add("入力処理・XBOX360コントローラの初期化完了。");
		}

		//* -----------------------------------------------------------------------*
		/// <summary>デストラクタ。</summary>
		~CInput()
		{
			if(isUseXBOX360GamePad)
			{
				GamePad.SetVibration(PlayerIndex.One, 0, 0);
			}
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>XBOX360コントローラが接続されているかどうか。</summary>
		public bool isUseXBOX360GamePad
		{
			get;
			private set;
		}

		/// <summary>任意のボタンが現在のフレームで押されたかどうか。</summary>
		public bool pushAnyKey
		{
			get
			{
				foreach(SButtonState __state in BUTTON_STATE)
				{
					if(__state.push)
					{
						return true;
					}
				}
				return false;
			}
		}

		/// <summary>左モーターのフォースフィードバックの強さ。</summary>
		public float forceLeft
		{
			get
			{
				return m_fFFLeft;
			}
			private set
			{
				m_fFFLeft = value;
			}
		}

		/// <summary>右モーターのフォースフィードバックの強さ。</summary>
		public float forceRight
		{
			get
			{
				return m_fFFRight;
			}
			private set
			{
				m_fFFRight = value;
			}
		}

		/// <summary>移動ボタンが現在のフレームで押されたかどうか。</summary>
		public bool pushDirKey
		{
			get
			{
				return (
					BUTTON_STATE[(int)EDirection.up].push ||
					BUTTON_STATE[(int)EDirection.down].push ||
					BUTTON_STATE[(int)EDirection.left].push ||
					BUTTON_STATE[(int)EDirection.right].push);
			}
		}

		/// <summary>駆動させるフォースフィードバックのプリセット。</summary>
		public EForcePreset force
		{
			set
			{
				switch(value)
				{
					case EForcePreset.Square:
						MICROTHREAD_MANAGER.Add(threadForceFeedback(0.1f, 0.25f, 70, 70));
						break;
					case EForcePreset.Short:
						MICROTHREAD_MANAGER.Add(threadForceFeedback(0, new SGradation(0.2f, 0, 0, 1), 0, 5));
						break;
					case EForcePreset.Mild:
						MICROTHREAD_MANAGER.Add(threadForceFeedback(
							new SGradation(0.3f, 0, 0, 1), new SGradation(0.7f, 0, 0, 1), 120, 80));
						break;
					case EForcePreset.Hard:
						MICROTHREAD_MANAGER.Add(threadForceFeedback(
							new SGradation(2, 0, 0, 1), new SGradation(2, 0, 0, 1), 160, 240));
						break;
				}
			}
		}
#if WINDOWS

		/// <summary>レガシ ゲームパッド管理クラス。</summary>
		public CLegacyInputManager legacyManager
		{
			get
			{
				return m_mgrLegacy;
			}
			private set
			{
				m_mgrLegacy = value;
				m_legacy = null;
			}
		}

		/// <summary>レガシ ゲームパッド デバイス。</summary>
		public CLegacyInput legacy
		{
			get
			{
				if(m_legacy != null && m_legacy.isDisposed)
				{
					return null;
				}
				else if(m_legacy == null && legacyManager != null && legacyManager.DEVICES.Count > 0)
				{
					CLegacyInput obj = legacyManager.getDevice();
					if(!(obj.isDisposed))
					{
						m_legacy = obj;
					}
				}
				return m_legacy;
			}
		}
#endif

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>接続されているXBOX360コントローラの一覧を取得します。</summary>
		/// 
		/// <returns>接続されているXBOX360コントローラの一覧</returns>
		public static PlayerIndex[] detectConnected()
		{
			PlayerIndex[] all = new PlayerIndex[] {
				PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };
			List<PlayerIndex> connects = new List<PlayerIndex>();
			foreach(PlayerIndex i in all)
			{
				GamePadState state = GamePad.GetState(i);
				if(state.IsConnected)
				{
					connects.Add(i);
				}
			}
			return connects.ToArray();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ボタンが押されているかどうかを取得します。</summary>
		/// 
		/// <param name="connects">接続されているXBOX360コントローラの一覧</param>
		/// <param name="button">ボタンID</param>
		/// <returns>ボタンが押されている場合、<c>true</c></returns>
		public static bool getState(IEnumerable<PlayerIndex> connects, Buttons button)
		{
			bool bResult = false;
			foreach(PlayerIndex index in connects)
			{
				bResult = bResult || GamePad.GetState(index).IsButtonDown(button);
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ボタンが押されているかどうかを取得します。</summary>
		/// 
		/// <param name="button">ボタンID</param>
		/// <returns>ボタンが押されている場合、<c>true</c></returns>
		public bool getState(Buttons button)
		{
			return getState(CONNECTED, button);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// アンマネージ リソースの解放およびリセットに関連付けられている
		/// アプリケーション定義のタスクを実行します。
		/// </summary>
		public void Dispose()
		{
#if WINDOWS
			legacyManager.Dispose();
			legacyManager = null;
#endif
		}

		//* -----------------------------------------------------------------------*
		/// <summary>入力状態の更新をします。</summary>
		/// <remarks>毎フレーム実行してください。</remarks>
		public void update()
		{
			MICROTHREAD_MANAGER.update(null);
			stateReflesh();
			if(isUseXBOX360GamePad)
			{
				GamePad.SetVibration(PlayerIndex.One, forceLeft, forceRight);
				forceLeft = 0;
				forceRight = 0;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>キーの割り当てをします。</summary>
		/// 
		/// <param name="device">入力デバイス</param>
		/// <param name="uType">ボタン番号</param>
		/// <returns>割り当てに成功した場合、true</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 登録されている数以上のボタン番号を指定した場合。
		/// または、対応していない入力デバイスを引数に設定した場合。
		/// </exception>
		/// <exception cref="Microsoft.DirectX.DirectInput.DeviceNotRegisteredException">
		/// レガシ ゲームパッドが初期化されていない状態で割り当てをしようとした場合。
		/// </exception>
		public bool assign(EInputDevice device, ushort uType)
		{
			bool bAssign = false;
			switch(device)
			{
				case EInputDevice.Keyboard:
					if(uType >= assignKeyboard.Length)
					{
						throw new ArgumentOutOfRangeException("uType");
					}
					else
					{
						Keys[] keys = Keyboard.GetState().GetPressedKeys();
						if(keys.Length > 0 && !isReserved(keys[0]))
						{
							bAssign = true;
							assignKeyboard[uType] = keys[0];
						}
					}
					break;
#if WINDOWS
				case EInputDevice.LegacyPad:
					if(legacy == null)
					{
						throw new Microsoft.DirectX.DirectInput.DeviceNotRegisteredException();
					}
					else if(uType >= assignLegacy.Length)
					{
						throw new ArgumentOutOfRangeException("uType");
					}
					else
					{
						byte[] buttons = legacy.state.GetButtons();
						int nBtnID = 0;
						while(nBtnID < buttons.Length)
						{
							if(buttons[nBtnID] != 0)
							{
								bAssign = true;
								assignLegacy[uType] = nBtnID;
								break;
							}
							else
							{
								nBtnID++;
							}
						}
					}
					break;
#endif
				case EInputDevice.XBOX360:
					if(uType >= assignXBOX360.Length)
					{
						throw new ArgumentOutOfRangeException("uType");
					}
					bAssign = __assignXBOX360(ref assignXBOX360[uType]);
					break;
				default:
					throw new ArgumentOutOfRangeException("device");
			}
			return bAssign;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>XBOX360ボタンの割り当てをします。</summary>
		/// 
		/// <param name="rAssign">割り当てボタンの格納先</param>
		/// <returns>割り当てに成功した場合、true</returns>
		private bool __assignXBOX360(ref Buttons rAssign)
		{
			GamePadState state = GamePad.GetState(PlayerIndex.One);
			Buttons[] btns =
			{
				Buttons.A, Buttons.B, Buttons.X, Buttons.Y,
				Buttons.Back, Buttons.Start,
				Buttons.LeftShoulder, Buttons.LeftTrigger,
				Buttons.RightShoulder, Buttons.RightTrigger
			};
			foreach(Buttons btn in btns)
			{
				if(state.IsButtonDown(btn))
				{
					rAssign = btn;
					return true;
				}
			}
			return false;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定のキーが予約されているかどうかを取得します。</summary>
		/// 
		/// <param name="key">キー列挙体</param>
		/// <returns>予約されているキーの場合、<c>true</c></returns>
		private bool isReserved(Keys key)
		{
			return (
				key == Keys.Apps || key == Keys.Attn ||
				key == Keys.BrowserBack || key == Keys.BrowserFavorites ||
				key == Keys.BrowserForward || key == Keys.BrowserHome ||
				key == Keys.BrowserRefresh || key == Keys.BrowserSearch ||
				key == Keys.BrowserStop || key == Keys.CapsLock ||
				key == Keys.ChatPadGreen || key == Keys.ChatPadOrange ||
				key == Keys.Insert || key == Keys.LaunchApplication1 ||
				key == Keys.LaunchApplication2 || key == Keys.LaunchMail ||
				key == Keys.LeftWindows || key == Keys.MediaNextTrack ||
				key == Keys.MediaPlayPause || key == Keys.MediaPreviousTrack ||
				key == Keys.MediaStop || key == Keys.NumLock ||
				key == Keys.PrintScreen || key == Keys.ProcessKey ||
				key == Keys.RightWindows || key == Keys.Scroll ||
				key == Keys.SelectMedia || key == Keys.Sleep ||
				key == Keys.VolumeDown || key == Keys.VolumeMute ||
				key == Keys.VolumeUp || key == Keys.Zoom);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>入力状態を更新します。</summary>
		private void stateReflesh()
		{
			if (bState.Length < BUTTON_STATE.Length)
			{
				Array.Resize<bool>(ref bState, BUTTON_STATE.Length);
			}
#if WINDOWS
			KeyboardState stateKey =
				Keyboard.GetState();
			for(int i = 0; i < bState.Length; i++)
			{
				bState[i] = stateKey.IsKeyDown(assignKeyboard[i]);
			}
			if(legacy != null)
			{
				Microsoft.DirectX.DirectInput.JoystickState legacystate = legacy.state;
				byte[] buttons = legacystate.GetButtons();
				bState[(int)EDirection.up] = bState[(int)EDirection.up] || (legacystate.Y < -600);
				bState[(int)EDirection.down] = bState[(int)EDirection.down] || (legacystate.Y > 600);
				bState[(int)EDirection.left] = bState[(int)EDirection.left] || (legacystate.X < -600);
				bState[(int)EDirection.right] = bState[(int)EDirection.right] || (legacystate.X > 600);
				for(int i = (int)EDirection.__reserved; i < bState.Length; i++)
				{
					int nButtonID = assignLegacy[i - (int)EDirection.__reserved];
					bState[i] = bState[i] ||
						(buttons.Length > nButtonID && buttons[nButtonID] != 0);
				}
			}
#else
			for( int i = 0; i < bState.Length; bState[ i++ ] = false );
#endif
			if(isUseXBOX360GamePad)
			{
				GamePadState stateButton = GamePad.GetState(PlayerIndex.One);
				Vector2 leftStick = stateButton.ThumbSticks.Left;
				if(leftStick.Length() > 0.7f)
				{
					double dAngle = Math.Atan2(leftStick.Y, leftStick.X);
					bState[(int)EDirection.up] = bState[(int)EDirection.up] || (dAngle <= MathHelper.PiOver4 * 3.5 && dAngle >= MathHelper.PiOver4 * 0.5);
					bState[(int)EDirection.down] = bState[(int)EDirection.down] || (dAngle >= MathHelper.PiOver4 * -3.5 && dAngle <= MathHelper.PiOver4 * -0.5);
					bState[(int)EDirection.left] = bState[(int)EDirection.left] || Math.Abs(dAngle) >= MathHelper.PiOver4 * 2.5;
					bState[(int)EDirection.right] = bState[(int)EDirection.right] || Math.Abs(dAngle) <= MathHelper.PiOver4 * 1.5;
				}
				else
				{
					bState[(int)EDirection.up] = bState[(int)EDirection.up] || stateButton.IsButtonDown(Buttons.DPadUp);
					bState[(int)EDirection.down] = bState[(int)EDirection.down] || stateButton.IsButtonDown(Buttons.DPadDown);
					bState[(int)EDirection.left] = bState[(int)EDirection.left] || stateButton.IsButtonDown(Buttons.DPadLeft);
					bState[(int)EDirection.right] = bState[(int)EDirection.right] || stateButton.IsButtonDown(Buttons.DPadRight);
				}
				for(int i = (int)EDirection.__reserved; i < bState.Length; i++)
				{
					bState[i] = bState[i] || stateButton.IsButtonDown(assignXBOX360[i - (int)EDirection.__reserved]);
				}
			}
			for(int i = 0; i < bState.Length; i++)
			{
				BUTTON_STATE[i].refresh(bState[i]);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// XBOX360コントローラにフォースフィードバックを発信するスレッドです。
		/// </summary>
		/// 
		/// <param name="gradationL">大モーター速度のグラデーション</param>
		/// <param name="gradationS">小モーター速度のグラデーション</param>
		/// <param name="nTimeL">大モーター駆動時間</param>
		/// <param name="nTimeS">小モーター駆動時間</param>
		/// <returns>スレッドが実行される間、<c>true</c></returns>
		private IEnumerator<object> threadForceFeedback(
			SGradation gradationL, SGradation gradationS, int nTimeL, int nTimeS
		)
		{
			for(int i = 0; i < Math.Max(nTimeL, nTimeS); i++)
			{
				yield return null;
				forceLeft = MathHelper.Max(forceLeft, gradationL.smooth(i, nTimeL));
				forceRight = MathHelper.Max(forceRight, gradationS.smooth(i, nTimeS));
			}
		}
	}
}
