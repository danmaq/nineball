////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.entity.input.low;
using danmaq.nineball.state;
using danmaq.nineball.state.input;
using danmaq.nineball.util.collection.input;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>キーボード専用入力管理クラス。</para>
	/// <para>
	/// このクラスは、
	/// <c>CInputAdapter&lt;CXNAInput&lt;MouseState&gt;, MouseState&gt;</c>
	/// の事実上のシノニムです。</para>
	/// </summary>
	public class CGamePad
		: CInputAdapter<CXNAInput<GamePadState>, GamePadState>
	{

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CGamePad()
			: this(CStateGamePadInput.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="firstState">初期状態。</param> 
		public CGamePad(IState firstState)
			: base(firstState)
		{
			lowerInput = CGamePadInputCollection.instance.inputList[0];
		}
	}
}
