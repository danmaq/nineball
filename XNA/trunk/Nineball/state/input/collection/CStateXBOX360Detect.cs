﻿////////////////////////////////////////////////////////////////////////////////
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
using danmaq.nineball.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.input.collection
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// マンマシンI/F入力制御・管理クラスコレクション用XBOX360ゲーム コントローラ自動認識状態。
	/// </summary>
	public sealed class CStateXBOX360Detect : CState<CInputCollection, List<SInputState>>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateXBOX360Detect instance = new CStateXBOX360Detect();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateXBOX360Detect()
		{
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
		/// <param name="buttonsState">ボタン押下情報一覧。</param>
		public override void setup(CInputCollection entity, List<SInputState> buttonsState)
		{
			setCapacity(entity);
			entity.releaseAwayController = true;
			base.setup(entity, buttonsState);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="buttonsState">ボタン押下情報一覧。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(
			CInputCollection entity, List<SInputState> buttonsState, GameTime gameTime
		)
		{
			if(entity.Count == 0)
			{
				foreach(PlayerIndex playerIndex in CInputXBOX360.allPlayerIndex)
				{
					GamePadState state = GamePad.GetState(playerIndex);
					if(state.IsConnected && state.getPress() != 0)
					{
						entity.Add(CInputXBOX360.getInstance(playerIndex, entity.playerNumber));
						break;
					}
				}
			}
			if(entity.Count != 0)
			{
				setCapacity(entity);
				entity.nextState = CStateWaitDetect.instance;
			}
			base.update(entity, buttonsState, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>子入力クラスとして受け入れる最大値を初期化します。</summary>
		/// 
		/// <param name="entity">この状態を終了したオブジェクト。</param>
		/// <exception cref="System.InvalidOperationException">
		/// 現在の保有個数が2個以上の場合。
		/// </exception>
		private void setCapacity(CInputCollection entity)
		{
			try
			{
				entity.capacity = 1;
			}
			catch(Exception e)
			{
				throw new InvalidOperationException(Resources.ERR_INPUT_DETECT_DUPLICATION, e);
			}
		}
	}
}