////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.ObjectModel;
using danmaq.nineball.state.input;

namespace danmaq.nineball.entity.input {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360用コントローラ 入力制御・管理クラス。</summary>
	public sealed class CInputXBOX360 : CInput {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>ボタン一覧。</summary>
		public static readonly ReadOnlyCollection<Buttons> allButtons = new List<Buttons> {
			Buttons.DPadUp, Buttons.DPadDown, Buttons.DPadLeft, Buttons.DPadRight,
			Buttons.Start, Buttons.Back,
			Buttons.LeftStick, Buttons.RightStick, Buttons.LeftShoulder, Buttons.RightShoulder,
			Buttons.BigButton, Buttons.A, Buttons.B, Buttons.X, Buttons.Y,
			Buttons.RightTrigger, Buttons.LeftTrigger,
			Buttons.RightThumbstickUp, Buttons.RightThumbstickDown,
			Buttons.RightThumbstickRight, Buttons.RightThumbstickLeft,
			Buttons.LeftThumbstickUp, Buttons.LeftThumbstickDown,
			Buttons.LeftThumbstickRight, Buttons.LeftThumbstickLeft,
		}.AsReadOnly();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="playerIndex">プレイヤー番号。</param>
		public CInputXBOX360( PlayerIndex playerIndex ) :
			base( CState<CInput, List<SInputState>>.empty )
		{
			this.playerIndex = playerIndex;
			nextState = CStateXBOX360Controller.instance;
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
		public new IState<CInputXBOX360, List<SInputState>> nextState {
			set { nextStateBase = value; }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>プレイヤー番号を取得します。</summary>
		/// 
		/// <value>プレイヤー番号。</value>
		public PlayerIndex playerIndex { get; private set; }
	}
}
