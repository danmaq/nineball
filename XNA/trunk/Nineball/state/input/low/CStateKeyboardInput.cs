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
	/// <summary>キーボード入力制御・管理クラスの既定の状態。</summary>
	public sealed class CStateKeyboardInput
		: CStateXNAInput<KeyboardState>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>キーボード用クラス オブジェクト。</summary>
		public static readonly
			IState<CXNAInput<KeyboardState>, CXNAInput<KeyboardState>.CPrivateMembers> keyboard =
			new CStateKeyboardInput(() => Keyboard.GetState());

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly
			IState<CXNAInput<KeyboardState>, CXNAInput<KeyboardState>.CPrivateMembers> chatPad1 =
			new CStateKeyboardInput(() => Keyboard.GetState(PlayerIndex.One));

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly
			IState<CXNAInput<KeyboardState>, CXNAInput<KeyboardState>.CPrivateMembers> chatPad2 =
			new CStateKeyboardInput(() => Keyboard.GetState(PlayerIndex.Two));

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly
			IState<CXNAInput<KeyboardState>, CXNAInput<KeyboardState>.CPrivateMembers> chatPad3 =
			new CStateKeyboardInput(() => Keyboard.GetState(PlayerIndex.Three));

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly
			IState<CXNAInput<KeyboardState>, CXNAInput<KeyboardState>.CPrivateMembers> chatPad4 =
			new CStateKeyboardInput(() => Keyboard.GetState(PlayerIndex.Four));

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="getState">
		/// キーボードの状態を取得するためのデリゲート。
		/// </param>
		private CStateKeyboardInput(Func<KeyboardState> getState)
			: base(getState)
		{
		}
	}
}
