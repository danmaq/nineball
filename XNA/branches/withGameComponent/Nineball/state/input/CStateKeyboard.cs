﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.entity.input;
using danmaq.nineball.entity.input.data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>キーボード既定の入力状態。</summary>
	public sealed class CStateKeyboard : CState<CInputKeyboard, CInputKeyboard.CPrivateMembers>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateKeyboard instance = new CStateKeyboard();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateKeyboard()
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(
			CInputKeyboard entity, CInputKeyboard.CPrivateMembers privateMembers,
			GameTime gameTime
		)
		{
			KeyboardState state = Keyboard.GetState();
			IList<Keys> assignList = entity.assignList;
			for(int i = assignList.Count; --i >= 0; )
			{
				SInputState inputState = privateMembers.buttonStateList[i];
				inputState.refresh(state.IsKeyDown(assignList[i]));
				privateMembers.buttonStateList[i] = inputState;
			}
			for(int i = entity.dirInputState.Length; --i >= 0; )
			{
				entity.dirInputState[i].refresh(state.IsKeyDown(entity.directionAssignList[i]));
			}
			EDirectionFlags axisFlags;
			Vector2 axisVector;
			CHelper.createVector(entity.dirInputState, out axisVector, out axisFlags);
			privateMembers.axisVector = axisVector;
			privateMembers.axisFlag = axisFlags;
			base.update(entity, privateMembers, gameTime);
		}
	}
}
