////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.ball.core;
using danmaq.ball.data;
using danmaq.ball.entity;
using danmaq.nineball.entity.input;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;

namespace danmaq.ball.state.ball
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>プレイヤーの玉の状態。</summary>
	sealed class CStatePlayer
		: CStateBallBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState<CBall, object> instance = new CStatePlayer();

		/// <summary>入力状態。</summary>
		private readonly IList<SInputInfo> inputData = CInput.instance.collection.buttonList;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStatePlayer()
			: base(120, new Rectangle(64, 0, 64, 64))
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>玉に対し移動すべきかを指示します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <returns>移動すべき場合、<c>true</c>。</returns>
		protected override bool getMoveOrder(CBall entity)
		{
			return inputData[(int)EInputActionMap.enter].push;
		}
	}
}
