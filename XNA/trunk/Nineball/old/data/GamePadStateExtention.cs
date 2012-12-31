////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.old.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360用コントローラ 入力情報クラス 拡張機能。</summary>
	[Obsolete]
	public static class GamePadStateExtention
	{

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

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>現在の入力状態を取得します。</summary>
		/// 
		/// <param name="state"></param>
		public static Buttons getPress(this GamePadState state)
		{
			Buttons result = 0;
			foreach(Buttons button in allButtons)
			{
				if(state.IsButtonDown(button))
				{
					result |= button;
				}
			}
			return result;
		}
	}
}
