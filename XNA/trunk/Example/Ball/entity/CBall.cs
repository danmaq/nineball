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

namespace danmaq.ball.entity
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>玉オブジェクト。</summary>
	public abstract class CBall : CEntity
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>フェーズ・カウンタ管理クラス。</summary>
		protected readonly CPhase phaseManager = new CPhase();

		// TODO : 移動キューはGCに悪影響を与えるため、XBOX360で動かすためには修正が必要。
		/// <summary>移動キュー。</summary>
		private readonly Queue<int> moveQueueList = new Queue<int>(1);

	}
}
