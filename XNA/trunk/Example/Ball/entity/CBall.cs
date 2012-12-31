////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Collections.ObjectModel;
using danmaq.nineball.data.phase;
using danmaq.nineball.entity;
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

		/// <summary>自機。</summary>
		public static readonly CBall player;

		/// <summary>敵機。</summary>
		public static readonly CBall enemy;

		/// <summary>加速度グラフのアタック・サスティン・リリース時間。</summary>
		private static readonly int[] accelerateTime = { 5, 5, 10 };

		/// <summary>速度グラフ情報。</summary>
		private static readonly ReadOnlyCollection<float> speedGraph;

		// TODO : この辺もCEntityで分割したほうがよさげ

		/// <summary>移動キュー。</summary>
		private readonly int[] moveRequest = new int[speedGraph.Count + 1];

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>位置。</summary>
		public Vector2 position = Vector2.Zero;

		/// <summary>鬼畜加速モードかどうか。</summary>
		public bool accelerateSpeed;

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
				graph.Add(fSpeed);
				fPrevSpeed = fSpeed;
				phase.reserveNextPhase = nPCount >= nPLimit;
			}
			speedGraph = graph.AsReadOnly();
			enemy = new CBall();
			player = new CBall();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CBall()
		{
			for (int i = moveRequest.Length; --i >= 0; )
			{
				moveRequest[i] = short.MinValue;
			}
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>ゴールしたかどうかを取得します。</summary>
		/// 
		/// <value>ゴールした場合、<c>true</c>。</value>
		public bool goal
		{
			get
			{
				return position.X > 640;
			}
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
				if (moveRequest[i] == short.MinValue)
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
			int limit = speedGraph.Count;
			for (int i = moveRequest.Length; --i >= 0; )
			{
				int qc = counter - moveRequest[i];
				if (qc > 0)
				{
					if (qc >= limit)
					{
						moveRequest[i] = short.MinValue;
					}
					else
					{
						speed += speedGraph[qc];
					}
				}
			}

			return speed;
		}
	}
}
