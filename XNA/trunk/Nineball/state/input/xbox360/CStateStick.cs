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
using danmaq.nineball.entity.input;
using danmaq.nineball.entity.input.data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.input.xbox360
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ゲーム コントローラ既定の入力状態。</summary>
	public sealed class CStateStick : CStateDefaultBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateStick left = new CStateStick(EAxisXBOX360.LeftStick);

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateStick right = new CStateStick(EAxisXBOX360.RightStick);

		/// <summary>上ボタンに対応する定数。</summary>
		private readonly Buttons upButton;

		/// <summary>下ボタンに対応する定数。</summary>
		private readonly Buttons downButton;

		/// <summary>左ボタンに対応する定数。</summary>
		private readonly Buttons leftButton;

		/// <summary>右ボタンに対応する定数。</summary>
		private readonly Buttons rightButton;

		/// <summary>ベクトルを取得する関数。</summary>
		private readonly Func<GamePadThumbSticks, Vector2> vectorGetter;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="axisType">使用する方向ボタン。</param>
		private CStateStick(EAxisXBOX360 axisType)
			: base(axisType)
		{
			switch(axisType)
			{
				case EAxisXBOX360.LeftStick:
					upButton = Buttons.LeftThumbstickUp;
					downButton = Buttons.LeftThumbstickDown;
					leftButton = Buttons.LeftThumbstickLeft;
					rightButton = Buttons.LeftThumbstickRight;
					vectorGetter = thumb => thumb.Left;
					break;
				case EAxisXBOX360.RightStick:
					upButton = Buttons.RightThumbstickUp;
					downButton = Buttons.RightThumbstickDown;
					leftButton = Buttons.RightThumbstickLeft;
					rightButton = Buttons.RightThumbstickRight;
					vectorGetter = thumb => thumb.Right;
					break;
				default:
					throw new ArgumentOutOfRangeException("axisType");
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンの状態を更新します。</summary>
		/// 
		/// <param name="state">最新の入力情報。</param>
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		protected override void refleshAxis(
			GamePadState state, CInputXBOX360 entity,
			CInputXBOX360.CPrivateMembers privateMembers, GameTime gameTime)
		{
			privateMembers.axisVector = vectorGetter(state.ThumbSticks);
			List<bool> axis = new List<bool>
			{
				state.IsButtonDown(upButton),
				state.IsButtonDown(downButton),
				state.IsButtonDown(leftButton),
				state.IsButtonDown(rightButton),
			};
			EDirectionFlags axisFlags;
			CHelper.createVector(axis, out axisFlags);
			privateMembers.axisFlag = axisFlags;
		}
	}
}
