////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.entity.input.low;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.input.low
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ゲームパッド入力制御・管理クラスの既定の状態。</summary>
	public sealed class CStateGamePadInput
		: CStateXNAInput<GamePadState>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly
			IState<CXNAInput<GamePadState>, CXNAInput<GamePadState>.CPrivateMembers> player1 =
			new CStateGamePadInput(() => GamePad.GetState(PlayerIndex.One));

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly
			IState<CXNAInput<GamePadState>, CXNAInput<GamePadState>.CPrivateMembers> player2 =
			new CStateGamePadInput(() => GamePad.GetState(PlayerIndex.Two));

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly
			IState<CXNAInput<GamePadState>, CXNAInput<GamePadState>.CPrivateMembers> player3 =
			new CStateGamePadInput(() => GamePad.GetState(PlayerIndex.Three));

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly
			IState<CXNAInput<GamePadState>, CXNAInput<GamePadState>.CPrivateMembers> player4 =
			new CStateGamePadInput(() => GamePad.GetState(PlayerIndex.Four));

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="getState">
		/// キーボードの状態を取得するためのデリゲート。
		/// </param>
		private CStateGamePadInput(Func<GamePadState> getState)
			: base(getState)
		{
		}
	}
}
