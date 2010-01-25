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
using danmaq.nineball.state;
using danmaq.nineball.state.input.raw;
using danmaq.nineball.util.caps;
using Microsoft.DirectX.DirectInput;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ入力制御・管理クラス。</summary>
	public sealed class CInputLegacy : CInput
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>ウィンドウ ハンドル。</summary>
			public readonly IntPtr hWnd;

			/// <summary>ボタンの入力状態一覧。</summary>
			public readonly List<SInputState> buttonStateList;

			/// <summary>レガシ ゲーム コントローラ デバイス。</summary>
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
			/// <param name="idDevice">デバイスのインスタンスGUID。</param>
			/// <param name="hWnd">ウィンドウ ハンドル</param>
			/// <param name="buttonStateList">ボタンの入力状態一覧。</param>
			public CPrivateMembers(Guid idDevice, IntPtr hWnd, List<SInputState> buttonStateList)
			{
				device = new Device(idDevice);
				this.hWnd = hWnd;
				this.buttonStateList = buttonStateList;
				capsReport =
					device.DeviceInformation.createCapsReport() + device.Caps.createCapsReport();
				device.SetDataFormat(DeviceDataFormat.Joystick);
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

		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>ボタン割り当て値の一覧。</summary>
		private readonly List<short> m_assignList = new List<short>();

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public readonly CPrivateMembers _privateMembers;

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
			_privateMembers = new CPrivateMembers(idDevice, hWnd, _buttonStateList);
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
		public Device devide
		{
			get
			{
				return _privateMembers.device;
			}
		}
	}
}

#endif
