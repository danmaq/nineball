////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.entity.input;
using danmaq.nineball.entity.input.data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.input.xbox360
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ゲーム コントローラ既定の入力状態。</summary>
	public sealed class CStateDpad : CStateDefaultBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateDpad instance = new CStateDpad();

		/// <summary>方向ボタン入力判定用の式一覧。</summary>
		private readonly Predicate<GamePadDPad>[] matchList =
		{
			dpad => dpad.Up == ButtonState.Pressed,
			dpad => dpad.Down == ButtonState.Pressed,
			dpad => dpad.Left == ButtonState.Pressed,
			dpad => dpad.Right == ButtonState.Pressed,
		};

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateDpad()
			: base(EAxisXBOX360.DPad)
		{
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
			GamePadDPad dpad = state.DPad;
			for(int i = entity.dirInputState.Length; --i >= 0; )
			{
				entity.dirInputState[i].refresh(matchList[i](dpad));
			}
			EDirectionFlags axisFlags;
			Vector2 axisVector;
			CHelper.createVector(entity.dirInputState, out axisVector, out axisFlags);
			privateMembers.axisVector = axisVector;
			privateMembers.axisFlag = axisFlags;
		}
	}
}
