////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if WINDOWS && !DISABLE_LEGACY

using System;
using System.Collections.Generic;
using danmaq.nineball.state;
using danmaq.nineball.state.input.legacy;
using danmaq.nineball.util.caps;
using Microsoft.DirectX.DirectInput;
using System.Collections.ObjectModel;

namespace danmaq.nineball.entity.input {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ用 入力制御・管理クラス。</summary>
	public sealed class CInputLegacy : CInput {

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers : IDisposable {

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>ボタンの入力状態一覧。</summary>
			public readonly List<SInputState> buttonStateList;

			/// <summary>レガシ ゲームパッド デバイス。</summary>
			public readonly Device device;

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
			/// <param name="guid">デバイスのインスタンスGUID。</param>
			/// <param name="buttonStateList">ボタンの入力状態一覧。</param>
			public CPrivateMembers( Guid guid, List<SInputState> buttonStateList ) {
				device = new Device( guid );
				this.buttonStateList = buttonStateList;
				capsReport =
					device.DeviceInformation.createCapsReport() + device.Caps.createCapsReport();
				device.SetDataFormat( DeviceDataFormat.Joystick );
			}

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>このオブジェクトの終了処理を行います。</summary>
			public void Dispose() {
				try {
					device.Unacquire();
					device.Dispose();
				}
				catch( Exception ) { }
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>ウィンドウ ハンドル。</summary>
		public static IntPtr hWnd = IntPtr.Zero;

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public readonly CPrivateMembers _privateMembers;

		/// <summary>ボタン割り当て値の一覧。</summary>
		private readonly List<int> m_assignList = new List<int>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="guid">デバイスのインスタンスGUID。</param>
		public CInputLegacy( Guid guid ) : base( CState.empty ) {
			_privateMembers = new CPrivateMembers( guid, _buttonStateList );
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// 状態として、nullを設定しようとした場合。
		/// </exception>
		public new IState<CInputLegacy, CPrivateMembers> nextState {
			set { nextStateBase = value; }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ボタン割り当て値の一覧を設定/取得します。</summary>
		/// 
		/// <value>ボタン割り当て値の一覧。</value>
		public IList<int> assignList {
			get { return m_assignList; }
			set {
				m_assignList.Clear();
				m_assignList.AddRange( value );
				ReadOnlyCollection<SInputState> stateList = buttonStateList;
				while( m_assignList.Count > stateList.Count ) {
					m_assignList.RemoveAt( m_assignList.Count - 1 );
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>性能レポートを取得します。</summary>
		/// 
		/// <value>性能レポート。</value>
		public string capsReport {
			get { return _privateMembers.capsReport; }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>エラーレポートを取得します。</summary>
		/// 
		/// <value>エラーレポート。</value>
		public string errorReport {
			get { return _privateMembers.errorReport; }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>フォース フィードバックが有効かどうかを取得します。</summary>
		/// 
		/// <value>フォース フィードバックが有効である場合、<c>true</c>。</value>
		public bool isEnableForceFeedback {
			get { return _privateMembers.enableForceFeedback; }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>入力状態を取得します。</summary>
		/// 
		/// <value>入力状態。</value>
		public JoystickState state {
			get {
				try { _privateMembers.device.Poll(); }
				catch( NotAcquiredException ) {
					_privateMembers.device.Acquire();
					_privateMembers.device.Poll();
				}
				return _privateMembers.device.CurrentJoystickState;
			}
		}
	}
}

#endif
