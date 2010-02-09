////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if WINDOWS

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using danmaq.nineball.entity.input.data;
using danmaq.nineball.state;
using danmaq.nineball.state.input.collection;
using danmaq.nineball.state.input.legacy;
using danmaq.nineball.util.caps;
using Microsoft.DirectX.DirectInput;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ入力制御・管理クラス。</summary>
	public sealed class CInputLegacy : CInput
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers : IDisposable
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>ウィンドウ ハンドル。</summary>
			public readonly IntPtr hWnd;

			/// <summary>ボタンの入力状態一覧。</summary>
			public readonly List<SInputState> buttonStateList;

			/// <summary>レガシ ゲーム コントローラ デバイス。</summary>
			public readonly Device device;

			/// <summary>レガシ ゲーム コントローラ入力制御・管理クラス。</summary>
			private readonly CInputLegacy input;

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>デバイスの性能レポート。</summary>
			public string capsReport = string.Empty;

			/// <summary>エラー レポート。</summary>
			public string errorReport = string.Empty;

			/// <summary>フォース フィードバックをサポートするかどうか。</summary>
			public bool enableForceFeedback = false;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="input">レガシ ゲーム コントローラ入力制御・管理クラス。</param>
			/// <param name="idDevice">デバイスのインスタンスGUID。</param>
			/// <param name="hWnd">ウィンドウ ハンドル</param>
			/// <param name="buttonStateList">ボタンの入力状態一覧。</param>
			public CPrivateMembers(
				CInputLegacy input, Guid idDevice, IntPtr hWnd, List<SInputState> buttonStateList)
			{
				device = new Device(idDevice);
				this.input = input;
				this.hWnd = hWnd;
				this.buttonStateList = buttonStateList;
				capsReport =
					device.DeviceInformation.createCapsReport() + device.Caps.createCapsReport();
				device.SetDataFormat(DeviceDataFormat.Joystick);
			}

			//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* properties ──────────────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>方向ボタンの状態をベクトルで設定します。</summary>
			/// 
			/// <value>方向ボタンの状態。</value>
			public Vector2 axisVector
			{
				set
				{
					input.axisVector = value;
				}
			}

			//* -----------------------------------------------------------------------*
			/// <summary>方向ボタンの状態をフラグで設定/取得します。</summary>
			/// <example>
			/// bool bDown = (obj.axisFlag &amp; EDirectionFlags.down) != 0;
			/// </example>
			/// 
			/// <value>方向ボタンの状態。</value>
			public EDirectionFlags axisFlag
			{
				get
				{
					return input.axisFlag;
				}
				set
				{
					input.axisFlag = value;
				}
			}

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>このオブジェクトの終了処理を行います。</summary>
			public void Dispose()
			{
				try
				{
					device.Unacquire();
					device.Dispose();
				}
				catch(Exception)
				{
				}
			}

			//* -----------------------------------------------------------------------*
			/// <summary>デバイス状態を更新し、最新のデバイス状態を取得します。</summary>
			/// 
			/// <returns>最新のデバイス状態。</returns>
			public JoystickState poll()
			{
				try
				{
					device.Poll();
				}
				catch(NotAcquiredException)
				{
					device.Acquire();
					device.Poll();
				}
				return device.CurrentJoystickState;
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>
		/// レガシ ゲーム コントローラ入力制御・管理クラス オブジェクト一覧。
		/// </summary>
		public static readonly ReadOnlyCollection<CInputLegacy> instanceList;

		/// <summary>ボタン割り当て値の一覧。</summary>
		private readonly List<short> m_assignList = new List<short>();

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _privateMembers;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>方向ボタンとして使用するボタン種類。</summary>
		private EAxisLegacy m_useForAxis;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static CInputLegacy()
		{
			List<CInputLegacy> inputList = new List<CInputLegacy>();
			DeviceList controllers =
				Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
			foreach(DeviceInstance controller in controllers)
			{
				if(!(Regex.IsMatch(controller.ProductName, "Xbox ?360", RegexOptions.IgnoreCase)))
				{
					inputList.Add(new CInputLegacy(
						-1, controller.InstanceGuid, Process.GetCurrentProcess().Handle));
				}
			}
			instanceList = inputList.AsReadOnly();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="playerNumber">プレイヤー番号。</param>
		/// <param name="idDevice">デバイスのインスタンスGUID</param>
		/// <param name="hWnd">ウィンドウ ハンドル</param>
		private CInputLegacy(short playerNumber, Guid idDevice, IntPtr hWnd)
			: base(playerNumber, CStateNoAxis.instance)
		{
			_privateMembers = new CPrivateMembers(this, idDevice, hWnd, _buttonStateList);
			useForAxis = EAxisLegacy.POV;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>接続されているかどうかを取得します。</summary>
		/// 
		/// <value>接続されている場合、<c>true</c>。</value>
		public override bool connect
		{
			get
			{
				throw new System.NotImplementedException();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// 状態として、nullを設定しようとした場合。
		/// </exception>
		public new IState<CInputLegacy, CPrivateMembers> nextState
		{
			set
			{
				nextStateBase = value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ボタン割り当て値の一覧を設定/取得します。</summary>
		/// 
		/// <value>ボタン割り当て値の一覧。</value>
		public IList<short> assignList
		{
			get
			{
				return m_assignList.AsReadOnly();
			}
			set
			{
				m_assignList.Clear();
				m_assignList.AddRange(value);
				int buttonsNum = ButtonsNum;
				while(m_assignList.Count > buttonsNum)
				{
					m_assignList.RemoveAt(m_assignList.Count - 1);
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンとして使用するボタン種類を設定/取得します。</summary>
		/// 
		/// <value>方向ボタンとして使用するボタン種類。</value>
		public EAxisLegacy useForAxis
		{
			get
			{
				return m_useForAxis;
			}
			set
			{
				if(value != m_useForAxis)
				{
					m_useForAxis = value;
					switch(value)
					{
						case EAxisLegacy.POV:
							nextState = CStatePOV.instance;
							break;
						case EAxisLegacy.Slider:
							nextState = CStateSlider.instance;
							break;
						default:
							nextState = CStateNoAxis.instance;
							break;
					}
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>性能レポートを取得します。</summary>
		/// 
		/// <value>性能レポート。</value>
		public string capsReport
		{
			get
			{
				return _privateMembers.capsReport;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>エラーレポートを取得します。</summary>
		/// 
		/// <value>エラーレポート。</value>
		public string errorReport
		{
			get
			{
				return _privateMembers.errorReport;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>フォース フィードバックが有効かどうかを取得します。</summary>
		/// 
		/// <value>フォース フィードバックが有効である場合、<c>true</c>。</value>
		public bool isEnableForceFeedback
		{
			get
			{
				return _privateMembers.enableForceFeedback;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>デバイス オブジェクトを取得します。</summary>
		/// 
		/// <value>レガシ ゲーム コントローラ デバイス オブジェクト。</value>
		public Device device
		{
			get
			{
				return _privateMembers.device;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// レガシ ゲーム コントローラを自動認識する入力制御・管理クラスを作成します。
		/// </summary>
		/// 
		/// <param name="playerNumber">設定したいプレイヤー番号。</param>
		/// <returns>レガシ ゲーム コントローラ入力制御・管理クラスコレクション。</returns>
		public static CInputCollection createDetector(short playerNumber)
		{
			return new CInputDetector(playerNumber, CStateLegacyDetect.instance);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			_privateMembers.Dispose();
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>任意のボタンが押されたかどうかを判定します。</summary>
		/// 
		/// <param name="pov">POVの操作も判定するかどうか。</param>
		/// <param name="slider">スライダーの操作も判定するかどうか。</param>
		/// <returns>任意のボタンが押された場合、<c>true</c>。</returns>
		public bool isPushAnyKey(bool pov, bool slider)
		{
			JoystickState state = _privateMembers.poll();
			byte[] buttons = state.GetButtons();
			bool bResult = buttons.Any(button => button != 0);
			if(!bResult && pov)
			{
				bResult = state.GetPointOfView()[0] != -1;
			}
			if(!bResult && slider)
			{
				bResult = state.getVector3Slider().Length() >= analogThreshold;
			}
			return bResult;
		}
	}
}

#endif
