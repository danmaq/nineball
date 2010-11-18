////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.ObjectModel;
using danmaq.nineball.entity.input.low;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.input.low
{

	using IStateKeyboardInput =
		IState<CXNAInput<KeyboardState>, CXNAInput<KeyboardState>.CPrivateMembers>;

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>キーボード入力制御・管理クラスの既定の状態。</summary>
	public sealed class CStateKeyboardInput
		: CStateXNAInput<KeyboardState>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>キーボード用クラス オブジェクト。</summary>
		public static readonly IStateKeyboardInput keyboard =
			new CStateKeyboardInput(() => Keyboard.GetState());

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly IStateKeyboardInput chatPad1 =
			new CStateKeyboardInput(() => Keyboard.GetState(PlayerIndex.One));

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly IStateKeyboardInput chatPad2 =
			new CStateKeyboardInput(() => Keyboard.GetState(PlayerIndex.Two));

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly IStateKeyboardInput chatPad3 =
			new CStateKeyboardInput(() => Keyboard.GetState(PlayerIndex.Three));

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly IStateKeyboardInput chatPad4 =
			new CStateKeyboardInput(() => Keyboard.GetState(PlayerIndex.Four));

		/// <summary>XBOX360チャットパッドパッド用クラス オブジェクトの一覧。</summary>
		public static ReadOnlyCollection<IStateKeyboardInput> chatPadInstanceList;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static CStateKeyboardInput()
		{
			IStateKeyboardInput[] array = new IStateKeyboardInput[4];
			array[(int)PlayerIndex.One] = chatPad1;
			array[(int)PlayerIndex.Two] = chatPad2;
			array[(int)PlayerIndex.Three] = chatPad3;
			array[(int)PlayerIndex.Four] = chatPad4;
			chatPadInstanceList = Array.AsReadOnly<IStateKeyboardInput>(array);
		}

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

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>プレイヤー番号に該当する状態を取得します。</summary>
		/// 
		/// <param name="playerIndex">割り当てられたプレイヤー番号。</param>
		/// <returns>XBOX360ゲームパッド低位入力制御・管理クラスの状態。</returns>
		public static IStateKeyboardInput getInstance(PlayerIndex playerIndex)
		{
			return chatPadInstanceList[(int)playerIndex];
		}
	}
}
