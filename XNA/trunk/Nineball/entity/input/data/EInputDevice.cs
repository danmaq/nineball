////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.nineball.entity.input.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>入力デバイス 列挙体。</summary>
	[Flags]
	public enum EInputDevice : byte
	{
		/// <summary>無効。</summary>
		None = 0,

		/// <summary>キーボード。</summary>
		Keyboard = 1 << 0,

		/// <summary>マウス。</summary>
		Mouse = 1 << 1,

		/// <summary>XBOX360ゲーム コントローラ。</summary>
		XBOX360 = 1 << 2,

		/// <summary>XBOX360チャットパッド。</summary>
		XBOX360ChatPad = 1 << 3,

		/// <summary>レガシ ゲーム コントローラ。</summary>
		LegacyPad = 1 << 4,

		/// <summary>キーボードとマウス。</summary>
		KeyboardAndMouse = (Keyboard | Mouse),

		/// <summary>マウス及びレガシ ゲーム コントローラを除く全デバイス。</summary>
		AllWithoutMouseAndLegacy = (Keyboard | XBOX360 | XBOX360ChatPad),

		/// <summary>レガシ ゲーム コントローラを除く全デバイス。</summary>
		AllWithoutLegacy = (Keyboard | Mouse | XBOX360 | XBOX360ChatPad),

		/// <summary>マウスを除く全デバイス。</summary>
		AllWithoutMouse = (Keyboard | XBOX360 | XBOX360ChatPad | LegacyPad),

		/// <summary>全デバイス。</summary>
		All = (Keyboard | Mouse | XBOX360 | XBOX360ChatPad | LegacyPad),
	}
}
