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
using danmaq.nineball.entity.input.data;
using danmaq.nineball.state;
using danmaq.nineball.state.input;
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
			public string capsReport = "";

			/// <summary>エラー レポート。</summary>
			public string errorReport = "";

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

		/// <summary>ボタン割り当て値の一覧。</summary>
		private readonly List<short> m_assignList = new List<short>();

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public readonly CPrivateMembers _privateMembers;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>方向ボタンとして使用するボタン種類。</summary>
		private EAxisLegacy m_useForAxis = EAxisLegacy.POV;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="playerNumber">プレイヤー番号。</param>
		/// <param name="idDevice">デバイスのインスタンスGUID</param>
		/// <param name="hWnd">ウィンドウ ハンドル</param>
		public CInputLegacy(short playerNumber, Guid idDevice, IntPtr hWnd)
			: base(playerNumber, CStateLegacy.instance)
		{
			_privateMembers = new CPrivateMembers(this, idDevice, hWnd, _buttonStateList);
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
				return m_assignList;
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
					// TODO : 変更がかかった際の処理(ステート変更など)を挿入する
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
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			_privateMembers.Dispose();
			base.Dispose();
		}
	}
}

#endif
