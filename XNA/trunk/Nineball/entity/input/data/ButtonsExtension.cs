////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.entity.input.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>入力デバイスのボタン列挙体 拡張機能。</summary>
	public static class ButtonsExtension
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>アナログ入力可能なボタン一覧。</summary>
		public static readonly ReadOnlyCollection<Buttons> anaglogInputList = new List<Buttons> {
			Buttons.LeftTrigger, Buttons.RightTrigger,
			Buttons.LeftThumbstickDown, Buttons.LeftThumbstickUp,
			Buttons.LeftThumbstickLeft, Buttons.LeftThumbstickRight,
			Buttons.RightThumbstickDown, Buttons.RightThumbstickUp,
			Buttons.RightThumbstickLeft, Buttons.RightThumbstickRight
		}.AsReadOnly();

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ボタンがアナログ入力に対応しているかどうかを取得します。</summary>
		/// 
		/// <param name="button">ボタン。</param>
		/// <returns>アナログ入力に対応している場合、<c>true</c>。</returns>
		public static bool isAvailableAnalogInput(this Buttons button)
		{
			return anaglogInputList.Contains(button);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アナログ入力を取得します。</summary>
		/// 
		/// <param name="state">XBOX360ゲーム コントローラの現在の情報。</param>
		/// <param name="button">ボタン。</param>
		/// <returns>アナログ入力値。</returns>
		public static float getInputState(this GamePadState state, Buttons button)
		{
			float fResult = 0f;
			if(button.isAvailableAnalogInput())
			{
				if((button & (Buttons.LeftTrigger | Buttons.RightTrigger)) != 0)
				{
					fResult = button == Buttons.LeftTrigger ?
						state.Triggers.Left : state.Triggers.Right;
				}
				else
				{
					const Buttons THUMB_LEFT = (
						Buttons.LeftThumbstickUp | Buttons.LeftThumbstickDown |
						Buttons.LeftThumbstickLeft | Buttons.LeftThumbstickRight);
					const Buttons THUMB_BOTH_UP = (
						Buttons.LeftThumbstickUp | Buttons.RightThumbstickUp);
					const Buttons THUMB_BOTH_DOWN = (
						Buttons.LeftThumbstickDown | Buttons.RightThumbstickDown);
					const Buttons THUMB_BOTH_LEFT = (
						Buttons.LeftThumbstickLeft | Buttons.RightThumbstickLeft);
					const Buttons THUMB_BOTH_RIGHT = (
						Buttons.LeftThumbstickRight | Buttons.RightThumbstickRight);
					Vector2 thumb = (button & THUMB_LEFT) != 0 ?
						state.ThumbSticks.Left : state.ThumbSticks.Right;
					if((button & THUMB_BOTH_UP) != 0)
					{
						fResult = MathHelper.Max(thumb.Y, 0);
					}
					if((button & THUMB_BOTH_DOWN) != 0)
					{
						fResult = -MathHelper.Min(thumb.Y, 0);
					}
					if((button & THUMB_BOTH_LEFT) != 0)
					{
						fResult = MathHelper.Max(thumb.X, 0);
					}
					if((button & THUMB_BOTH_RIGHT) != 0)
					{
						fResult = -MathHelper.Min(thumb.X, 0);
					}
				}
			}
			else
			{
				fResult = state.IsButtonDown(button) ? 1f : 0f;
			}
			return fResult;
		}
	}
}
