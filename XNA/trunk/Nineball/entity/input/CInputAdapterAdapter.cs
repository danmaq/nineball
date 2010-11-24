////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.state;
using danmaq.nineball.state.input;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>高位入力制御・管理クラスのコレクション。</summary>
	public sealed class CInputAdapterAdapter
		: CInputEmptyAdapter
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>高位入力制御・管理クラス。</summary>
		public readonly List<IInputAdapter> lowerInput = new List<IInputAdapter>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CInputAdapterAdapter()
			: base(CStateAdapterInput.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="firstState">初期状態。</param> 
		public CInputAdapterAdapter(IState firstState)
			: base(firstState)
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿0＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>キーアサインを指定のボタン数で初期化します。</summary>
		/// 
		/// <param name="buttons">ボタン数。</param>
		public void setDefaultAssign(int buttons)
		{
			int[] am = new int[buttons];
			for (int i = am.Length; --i >= 0; am[i] = i)
				;
			assignList = Array.AsReadOnly(am);
		}
	}
}
