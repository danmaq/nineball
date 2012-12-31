////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.ball.entity;
using danmaq.ball.entity.font;
using danmaq.nineball.state;
using danmaq.nineball.util.math;
using Microsoft.Xna.Framework;

namespace danmaq.ball.state.ball
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>敵の玉の状態。</summary>
	sealed class CStateEnemy
		: CStateBallBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState<CBall, object> instance = new CStateEnemy();

		/// <summary>難易度別行動パターン一覧。</summary>
		private readonly Func<CBall, bool>[] movePatterns;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateEnemy()
			: base(240, new Rectangle(0, 0, 64, 64))
		{
			movePatterns = new Func<CBall, bool>[]
			{
				movePatternLowLevel,	movePatternLowLevel,	movePatternLowLevel,
				movePatternLowLevel,	movePatternLowLevel,	movePatternLowLevel,
				movePatternLevel7,		movePatternLevel8,		movePatternLevel9
			};
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>玉に対し移動すべきかを指示します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <returns>移動すべき場合、<c>true</c>。</returns>
		protected override bool getMoveOrder(CBall entity)
		{
			return movePatterns[CCursor.instance.level](entity);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>低～中難易度状態の移動判定を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <returns>移動と判断した場合、<c>true</c>。</returns>
		private bool movePatternLowLevel(CBall entity)
		{
			return entity.counter % (int)MathHelper.Lerp(
				40, 6, CInterpolate._amountSlowdown(CCursor.instance.level, 5)) == 0;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>難易度7状態の移動判定を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <returns>移動と判断した場合、<c>true</c>。</returns>
		private bool movePatternLevel7(CBall entity)
		{
			int counter = entity.counter;
			bool result = counter % 40 == 0;
			if (!result)
			{
				if (counter > 200)
				{
					result = counter % (entity.accelerateSpeed ? 3 : 10) == 0;
				}
				else if(!entity.accelerateSpeed &&
					CBall.player.position.X - entity.position.X > 10)
				{
					entity.accelerateSpeed = true;
				}
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>難易度8状態の移動判定を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <returns>移動と判断した場合、<c>true</c>。</returns>
		private bool movePatternLevel8(CBall entity)
		{
			return entity.counter > 200;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>難易度9状態の移動判定を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <returns>移動と判断した場合、<c>true</c>。</returns>
		private bool movePatternLevel9(CBall entity)
		{
			return entity.counter > 30 || (entity.counter & 1) == 0;
		}
	}
}
