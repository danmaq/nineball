////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.data.input;
using danmaq.nineball.entity.input.low;
using danmaq.nineball.state.input.low;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.util.collection.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>マウス専用低位入力制御・管理クラスのコレクション。</summary>
	public sealed class CMouseInputCollection
		: IInputCollection
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CMouseInputCollection instance = new CMouseInputCollection();

		/// <summary>マウス専用低位入力制御・管理クラス。</summary>
		public readonly CXNAInput<MouseState> input =
			new CXNAInput<MouseState>(CStateMouseInput.instance);

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CMouseInputCollection()
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ボタン入力を検出します。</summary>
		/// <remarks>
		/// 注意: このメソッドを呼び出すと、自動的に登録されているクラスに対して
		/// <c>update()</c>が実行されます。レガシ ゲームパッドが高位入力管理クラスにて
		/// アクティブの状態でこのメソッドを呼び出すと、高位入力側の判定が
		/// 1フレーム分欠落します。
		/// </remarks>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		/// <returns>検出されたボタン。</returns>
		public EMouseButtons detectInput(GameTime gameTime)
		{
			EMouseButtons result = EMouseButtons.None;
			input.update(gameTime);
			MouseState now = input.nowInputState;
			MouseState prev = input.prevInputState;
			if (now != prev)
			{
				// TODO : 醜い。整理すべし
				if (Math.Abs(now.X - prev.X) > 0 || Math.Abs(now.Y - prev.Y) > 0)
				{
					result = EMouseButtons.position;
				}
				if (Math.Abs(now.ScrollWheelValue - prev.ScrollWheelValue) > 0)
				{
					result = EMouseButtons.scrollWheel;
				}
				if (now.LeftButton == ButtonState.Pressed && now.LeftButton != prev.LeftButton)
				{
					result = EMouseButtons.leftButton;
				}
				if (now.MiddleButton == ButtonState.Pressed && now.MiddleButton != prev.MiddleButton)
				{
					result = EMouseButtons.middleButton;
				}
				if (now.RightButton == ButtonState.Pressed && now.RightButton != prev.RightButton)
				{
					result = EMouseButtons.rightButton;
				}
				if (now.XButton1 == ButtonState.Pressed && now.XButton1 != prev.XButton1)
				{
					result = EMouseButtons.xButton1;
				}
				if (now.XButton2 == ButtonState.Pressed && now.XButton2 != prev.XButton2)
				{
					result = EMouseButtons.xButton2;
				}
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ボタン入力を検出します。</summary>
		/// <remarks>
		/// 注意: このメソッドを呼び出すと、自動的に登録されているクラスに対して
		/// <c>update()</c>が実行されます。レガシ ゲームパッドが高位入力管理クラスにて
		/// アクティブの状態でこのメソッドを呼び出すと、高位入力側の判定が
		/// 1フレーム分欠落します。
		/// </remarks>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		/// <returns>
		/// ボタン入力が検出された場合、<c>true</c>。
		/// </returns>
		bool IInputCollection.detectInput(GameTime gameTime)
		{
			return detectInput(gameTime) != EMouseButtons.None;
		}
	}
}
