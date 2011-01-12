////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.entity.input.low;
using danmaq.nineball.state;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>高位入力制御・管理クラス。</summary>
	/// 
	/// <typeparam name="_T">低位入力制御・管理クラスの型。</typeparam>
	/// <typeparam name="_S">入力状態の型。</typeparam>
	public class CInputAdapter<_T, _S>
		: CInputEmptyAdapter where _T : ILowerInput<_S>
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>低位入力制御・管理クラス。</summary>
		public _T lowerInput;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CInputAdapter()
			: base(null)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="firstState">初期状態。</param> 
		public CInputAdapter(IState firstState)
			: base(firstState)
		{
		}
	}
}
