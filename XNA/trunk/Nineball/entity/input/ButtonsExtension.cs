////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.entity.input {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>入力デバイスのボタン列挙体 拡張機能。</summary>
	public static class ButtonsExtension {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>アナログ入力可能なボタン一覧。</summary>
		private static readonly List<Buttons> _anaglogInputList = new List<Buttons> {
			Buttons.LeftTrigger, Buttons.RightTrigger,
			Buttons.LeftThumbstickDown, Buttons.LeftThumbstickUp,
			Buttons.LeftThumbstickLeft, Buttons.LeftThumbstickRight,
			Buttons.RightThumbstickDown, Buttons.RightThumbstickUp,
			Buttons.RightThumbstickLeft, Buttons.RightThumbstickRight
		};

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>アナログ入力可能なボタン一覧を取得します。</summary>
		/// 
		/// <value>アナログ入力可能なボタン一覧。</value>
		public static ReadOnlyCollection<Buttons> anaglogInputList {
			get { return _anaglogInputList.AsReadOnly(); }
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ボタンがアナログ入力に対応しているかどうかを取得します。</summary>
		/// 
		/// <param name="button">ボタン。</param>
		/// <returns>アナログ入力に対応している場合、<c>true</c>。</returns>
		public static bool isAvailableAnalogInput( this Buttons button ) {
			return _anaglogInputList.Contains( button );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アナログ入力を取得します。</summary>
		/// 
		/// <param name="button">ボタン。</param>
		/// <param name="playerIndex">プレイヤー番号。</param>
		/// <returns>アナログ入力値。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// アナログ入力に対応していないボタンでこのメソッドを呼び出した場合。
		/// </exception>
		public static float getAnalogInput( this Buttons button, PlayerIndex playerIndex ) {
			float fResult = 0f;
			if( !button.isAvailableAnalogInput() ) {	// アナログ入力出来ないものは予め弾く
				throw new ArgumentOutOfRangeException( "button" );
			}
			GamePadState state = GamePad.GetState( playerIndex );
			if( ( button & ( Buttons.LeftTrigger | Buttons.RightTrigger ) ) != 0 ) {
				fResult = button == Buttons.LeftTrigger ?
					state.Triggers.Left : state.Triggers.Right;
			}
			else {
				const Buttons THUMB_LEFT = (
					Buttons.LeftThumbstickUp | Buttons.LeftThumbstickDown |
					Buttons.LeftThumbstickLeft | Buttons.LeftThumbstickRight );
				const Buttons THUMB_BOTH_UP = (
					Buttons.LeftThumbstickUp | Buttons.RightThumbstickUp );
				const Buttons THUMB_BOTH_DOWN = (
					Buttons.LeftThumbstickDown | Buttons.RightThumbstickDown );
				const Buttons THUMB_BOTH_LEFT = (
					Buttons.LeftThumbstickLeft | Buttons.RightThumbstickLeft );
				const Buttons THUMB_BOTH_RIGHT = (
					Buttons.LeftThumbstickRight | Buttons.RightThumbstickRight );
				Vector2 thumb = ( button & THUMB_LEFT ) != 0 ?
					state.ThumbSticks.Left : state.ThumbSticks.Right;
				if( ( button & THUMB_BOTH_UP ) != 0 ) { fResult = MathHelper.Max( thumb.Y, 0 ); }
				if( ( button & THUMB_BOTH_DOWN ) != 0 ) { fResult = -MathHelper.Min( thumb.Y, 0 ); }
				if( ( button & THUMB_BOTH_LEFT ) != 0 ) { fResult = MathHelper.Max( thumb.X, 0 ); }
				if( ( button & THUMB_BOTH_RIGHT ) != 0 ) { fResult = -MathHelper.Min( thumb.X, 0 ); }
			}
			return fResult;
		}
	}
}
