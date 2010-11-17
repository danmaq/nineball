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
	public sealed class CStateInputKeyboard
		: CState<CInputKeyboard, CInputKeyboard.CPrivateMembers>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>キーボード用クラス オブジェクト。</summary>
		public static readonly IState<CInputKeyboard, CInputKeyboard.CPrivateMembers> keyboard =
			new CStateInputKeyboard(() => Keyboard.GetState());

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly IState<CInputKeyboard, CInputKeyboard.CPrivateMembers> chatPad1 =
			new CStateInputKeyboard(() => Keyboard.GetState(PlayerIndex.One));

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly IState<CInputKeyboard, CInputKeyboard.CPrivateMembers> chatPad2 =
			new CStateInputKeyboard(() => Keyboard.GetState(PlayerIndex.Two));

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly IState<CInputKeyboard, CInputKeyboard.CPrivateMembers> chatPad3 =
			new CStateInputKeyboard(() => Keyboard.GetState(PlayerIndex.Three));

		/// <summary>XBOX360チャットパッド用クラス オブジェクト。</summary>
		public static readonly IState<CInputKeyboard, CInputKeyboard.CPrivateMembers> chatPad4 =
			new CStateInputKeyboard(() => Keyboard.GetState(PlayerIndex.Four));

		/// <summary>キーボードの状態を取得するためのデリゲート。</summary>
		private Func<KeyboardState> getKeyboardState;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="getKeyboardState">
		/// キーボードの状態を取得するためのデリゲート。
		/// </param>
		private CStateInputKeyboard(Func<KeyboardState> getKeyboardState)
		{
			this.getKeyboardState = getKeyboardState;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>状態が開始された時に呼び出されます。</para>
		/// <para>このメソッドは、遷移元の<c>teardown</c>よりも後に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されたオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		public override void setup(
			CInputKeyboard entity, CInputKeyboard.CPrivateMembers privateMembers)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(CInputKeyboard entity,
			CInputKeyboard.CPrivateMembers privateMembers, GameTime gameTime)
		{
			privateMembers.prevState = privateMembers.nowState;
			privateMembers.nowState = getKeyboardState();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(CInputKeyboard entity,
			CInputKeyboard.CPrivateMembers privateMembers, GameTime gameTime)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>オブジェクトが別の状態へ移行する時に呼び出されます。</para>
		/// <para>このメソッドは、遷移先の<c>setup</c>よりも先に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を終了したオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="nextState">オブジェクトが次に適用する状態。</param>
		public override void teardown(
			CInputKeyboard entity, CInputKeyboard.CPrivateMembers privateMembers, IState nextState)
		{
			privateMembers.Dispose();
		}
	}
}
