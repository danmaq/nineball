////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.state;

namespace danmaq.nineball.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>状態変化時に送信されるイベントデータ。</summary>
	public sealed class CEventChangedState : EventArgs
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>以前の状態。</summary>
		public readonly IState previous;

		/// <summary>現在の状態。</summary>
		public readonly IState current;

		/// <summary>変化後の状態。</summary>
		public readonly IState next;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="previous">以前の状態。</param>
		/// <param name="current">現在の状態。</param>
		/// <param name="next">変化後の状態。</param>
		public CEventChangedState(IState previous, IState current, IState next)
		{
			this.previous = previous;
			this.current = current;
			this.next = next;
		}
	}
}
