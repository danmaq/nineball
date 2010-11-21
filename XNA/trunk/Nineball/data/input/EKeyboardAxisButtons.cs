////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
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

		/// <summary>WSADキー。</summary>
		wsad = -1,

		/// <summary>IJKLキー。</summary>
		ijkl = -2,

		/// <summary>十字キー。</summary>
		arrow = -3,

		/// <summary>テンキー。</summary>
		numpad = -4,

		/// <summary>予約(使用できません)。</summary>
		__reserved = -5,
	}
}
