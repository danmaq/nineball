////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if false

using System;
using danmaq.ball.entity;
using danmaq.ball.state.scene;
using danmaq.nineball.state;
using danmaq.nineball.util.math;
using Microsoft.Xna.Framework;

namespace danmaq.ball.state.ball
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>敵の玉の状態。</summary>
	public sealed class CStateEnemy : CState<CBall, object>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateEnemy instance = new CStateEnemy();

		/// <summary>難易度別行動パターン一覧。</summary>
		private readonly Func<CBall, bool>[] movePatterns;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>ゲーム難易度。</summary>
		private ushort m_wLevel = 0;

		/// <summary>鬼畜加速モードかどうか。</summary>
		private bool m_bAccelerateSpeed = false;

		/// <summary>難易度別行動パターン。</summary>
		private Func<CBall, bool> movePattern;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateEnemy()
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
		/// <summary>
		/// <para>状態が開始された時に呼び出されます。</para>
		/// <para>このメソッドは、遷移元の<c>teardown</c>よりも後に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されたオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		public override void setup(CBall entity, object privateMembers)
		{
			entity.position.Y = 200;
			m_wLevel = CStateGame.instance.level;
			movePattern = movePatterns[m_wLevel];
			base.setup(entity, privateMembers);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(CBall entity, object privateMembers, GameTime gameTime)
		{
			if (movePattern(entity))
			{
				entity.move();
			}
			base.update(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(CBall entity, object privateMembers, GameTime gameTime)
		{
			base.draw(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>低～中難易度状態の移動判定を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <returns>移動と判断した場合、<c>true</c>。</returns>
		private bool movePatternLowLevel(CBall entity)
		{
			return entity.counter %
				MathHelper.Lerp(40, 6, CInterpolate._amountSlowdown(m_wLevel, 5)) == 0;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>難易度7状態の移動判定を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <returns>移動と判断した場合、<c>true</c>。</returns>
		private bool movePatternLevel7(CBall entity)
		{
			bool bResult = false;
			int nCount = entity.counter;
			if (nCount > 200)
			{
				bResult = (m_bAccelerateSpeed && nCount % 3 == 0) || nCount % 10 == 0;
			}
			else
			{
				// TODO : 鬼加速モード未実装
				bResult = nCount % 10 == 0;
			}
			return bResult;
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

#endif