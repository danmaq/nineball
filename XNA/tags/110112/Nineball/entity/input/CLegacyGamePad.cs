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
using danmaq.nineball.state.input;

#if WINDOWS
using System.Collections.Generic;
using danmaq.nineball.util.collection.input;
using Microsoft.DirectX.DirectInput;
#endif

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>キーボード専用入力管理クラス。</para>
	/// <para>
	/// このクラスは、
	/// <c>CInputAdapter&lt;CXNAInput&lt;JoystickState&gt;, JoystickState&gt;</c>
	/// の事実上のシノニムです。</para>
	/// </summary>
	/// <remarks>
	/// XBOX360版では
	/// </remarks>
	public class CLegacyGamePad
#if WINDOWS
		: CInputAdapter<CLegacyInput, JoystickState>
#else
		: CInputEmptyAdapter
#endif
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

#if !WINDOWS
		/// <summary>低位入力制御・管理クラス。</summary>
		public object lowerInput;
#endif

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CLegacyGamePad()
			: this(
#if WINDOWS
				CStateLegacyInput.instance
#else
				CState.empty
#endif
)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="firstState">初期状態。</param> 
		public CLegacyGamePad(IState firstState)
			: base(firstState)
		{
#if WINDOWS
			IList<CLegacyInput> collection = CLegacyInputCollection.instance.inputList;
			if (collection.Count > 0)
			{
				lowerInput = collection[0];
			}
#endif
		}
	}
}
