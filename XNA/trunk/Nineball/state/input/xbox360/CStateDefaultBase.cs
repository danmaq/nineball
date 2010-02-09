////////////////////////////////////////////////////////////////////////////////
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

namespace danmaq.nineball.state.input.xbox360
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ゲーム コントローラ既定の入力状態の基底クラス。</summary>
	public abstract class CStateDefaultBase : CState<CInputXBOX360, CInputXBOX360.CPrivateMembers>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>使用する方向ボタン。</summary>
		public readonly EAxisXBOX360 axisType;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="axisType">使用する方向ボタン。</param>
		protected CStateDefaultBase(EAxisXBOX360 axisType)
		{
			this.axisType = axisType;
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
			CInputXBOX360 entity, CInputXBOX360.CPrivateMembers privateMembers, GameTime gameTime)
		{
			GamePadState state = GamePad.GetState(entity.playerIndex);
			if(state.IsConnected)
			{
				for(int i = entity.assignList.Count; --i >= 0; )
				{
					Buttons button = entity.assignList[i];
					SInputState inputState = privateMembers.buttonStateList[i];
					if(button.isAvailableAnalogInput())
					{
						inputState.refresh(state.getInputState(button));
					}
					else
					{
						inputState.refresh(state.IsButtonDown(button));
					}
					privateMembers.buttonStateList[i] = inputState;
				}
				refleshAxis(state, entity, privateMembers, gameTime);
			}
			else
			{
				entity.Dispose();
			}
			base.update(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンの状態を更新します。</summary>
		/// 
		/// <param name="state">最新の入力情報。</param>
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		protected abstract void refleshAxis(
			GamePadState state, CInputXBOX360 entity,
			CInputXBOX360.CPrivateMembers privateMembers, GameTime gameTime);
	}
}
