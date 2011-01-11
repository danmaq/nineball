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
using System.Collections.ObjectModel;
using danmaq.nineball.data.phase;
using danmaq.nineball.entity;
using danmaq.nineball.state;
using danmaq.nineball.util.math;
using Microsoft.Xna.Framework;

namespace danmaq.ball.entity
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>玉オブジェクト。</summary>
	sealed class CBall
		: CEntity
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		// TODO : マジックナンバーのResource化。
		/// <summary>玉の最大速度。</summary>
		private const float MAX_SPEED = 1f;

		/// <summary>加速度グラフのアタック・サスティン・リリース時間。</summary>
		private static readonly int[] accelerateTime = { 5, 5, 10 };

		/// <summary>加速度グラフ情報。</summary>
		private static readonly ReadOnlyCollection<float> accelerateGraph;

		// TODO : この辺もCEntityで分割したほうがよさげ

		/// <summary>移動キュー。</summary>
		private readonly int[] moveRequest = new int[accelerateGraph.Count + 1];

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>位置。</summary>
		public Vector2 position = Vector2.Zero;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		/// <remarks>ここで加速度グラフ情報を作成します。</remarks>
		static CBall()
		{
			List<float> graph = new List<float>();
			float fPrevSpeed = 0f;
			for (SPhase phase = SPhase.initialized; phase < 3; phase.count++)
			{
				int nPCount = phase.countPhase;
				int nPLimit = accelerateTime[phase];
				float fSpeed = MAX_SPEED;
				switch (phase)
				{
					case 0:
						fSpeed = CInterpolate._clampSlowFastSlow(0, MAX_SPEED, nPCount, nPLimit);
						break;
					case 2:
						fSpeed = CInterpolate._clampAccelerate(MAX_SPEED, 0, nPCount, nPLimit);
						break;
				}
				graph.Add(fSpeed - fPrevSpeed);
				fPrevSpeed = fSpeed;
				phase.reserveNextPhase = nPCount >= nPLimit;
			}
			accelerateGraph = graph.AsReadOnly();
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(GameTime gameTime)
		{
			base.update(gameTime);
			position.X += calcSpeed();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>玉を移動します。</summary>
		public void move()
		{
			for (int i = moveRequest.Length; --i >= 0; )
			{
				if (moveRequest[i] == 0)
				{
					moveRequest[i] = counter;
					break;
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>移動キューから現在速度を算出します。</summary>
		/// 
		/// <returns>現在速度。</returns>
		private float calcSpeed()
		{
			float speed = 0;
			int limit = accelerateGraph.Count;
			for (int i = moveRequest.Length; --i >= 0; )
			{
				int qc = counter - moveRequest[i];
				if (qc > 0)
				{
					if (qc >= limit)
					{
						moveRequest[i] = 0;
					}
					else
					{
						speed += accelerateGraph[qc];
					}
				}
			}
			return speed;
		}
	}
}
