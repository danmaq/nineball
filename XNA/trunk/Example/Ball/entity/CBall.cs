////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.data.phase;
using danmaq.nineball.entity;
using danmaq.nineball.misc;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.entity
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>玉オブジェクト。</summary>
	public abstract class CBall : CEntity
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		// TODO : マジックナンバーのResource化。
		/// <summary>玉の基準速度。</summary>
		private const float SPEED = 1f;

		/// <summary>加速度グラフ。</summary>
		private static readonly float[] accelerateGraph;

		/// <summary>フェーズ・カウンタ管理クラス。</summary>
		protected readonly SPhase phaseManager = SPhase.initialized;

		// TODO : 移動キューはGCに悪影響を与えるため、XBOX360で動かすためには修正が必要。
		/// <summary>移動キュー。</summary>
		private readonly Queue<int> moveQueueList = new Queue<int>(1);

		/// <summary>Y座標。</summary>
		private readonly float y;

		/// <summary>玉の色。</summary>
		private readonly Color color;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>現在の速度。</summary>
		private float m_fSpeed = 0f;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static CBall()
		{
			// TODO : マジックナンバーのResource化。
			List<float> accelerateGraph = new List<float>(20);
			ushort[] timeLimit = { 5, 5, 10 };
			float fPrevSpeed = 0f;
			for(SPhase phase = SPhase.initialized; phase.phase < 3; phase.count++)
			{
				int nPCount = phase.countPhase;
				ushort uPLimit = timeLimit[phase.phase];
				float fSpeed = SPEED;
				switch(phase.phase)
				{
					case 0:
						fSpeed = CInterpolate._clampSlowFastSlow(0, SPEED, nPCount, uPLimit);
						break;
					case 2:
						fSpeed = CInterpolate._clampAccelerate(SPEED, 0, nPCount, uPLimit);
						break;
				}
				accelerateGraph.Add(fSpeed - fPrevSpeed);
				fPrevSpeed = fSpeed;
				phase.reserveNextPhase = nPCount >= uPLimit;
			}
			CBall.accelerateGraph = accelerateGraph.ToArray();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="color">玉の色。</param>
		/// <param name="y">表示されるY座標。</param>
		private CBall(Color color, float y)
		{
			this.color = color;
			this.y = y;
		}
	}
}
