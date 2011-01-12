////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

namespace danmaq.nineball.data.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>キーボード ボタン一覧列挙体。</summary>
	public enum EKeyboardAxisButtons
		: short
	{

		/// <summary>WSADキーを使用した2D入力。</summary>
		wsad = -1,

		/// <summary>IJKLキーを使用した2D入力。</summary>
		ijkl = -2,

		/// <summary>十字キーを使用した2D入力。</summary>
		arrow = -3,

		/// <summary>テンキーを使用した2D入力。</summary>
		numpad = -4,

		/// <summary>予約(使用できません)。</summary>
		__reserved = -5,
	}
}
