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

		/// <summary>方向ボタンに対応する定数一覧。</summary>
		private readonly Buttons[] buttons;

		/// <summary>ベクトルを取得する関数。</summary>
		private readonly Func<GamePadThumbSticks, Vector2> vectorGetter;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="axisType">使用する方向ボタン。</param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 使用する方向ボタンに右スティックまたは左スティック以外の値を設定した場合。
		/// </exception>
		private CStateStick(EAxisXBOX360 axisType)
			: base(axisType)
		{
			switch(axisType)
			{
				case EAxisXBOX360.LeftStick:
					buttons = new Buttons[]
					{
						Buttons.LeftThumbstickUp,
						Buttons.LeftThumbstickDown,
						Buttons.LeftThumbstickLeft,
						Buttons.LeftThumbstickRight,
					};
					vectorGetter = thumb => thumb.Left;
					break;
				case EAxisXBOX360.RightStick:
					buttons = new Buttons[]
					{
						Buttons.RightThumbstickUp,
						Buttons.RightThumbstickDown,
						Buttons.RightThumbstickLeft,
						Buttons.RightThumbstickRight,
					};
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
			for(int i = buttons.Length; --i >= 0; )
			{
				entity.dirInputState[i].refresh(state.IsButtonDown(buttons[i]));
			}
			EDirectionFlags axisFlags;
			CHelper.createVector(entity.dirInputState, out axisFlags);
			privateMembers.axisFlag = axisFlags;
		}
	}
}
