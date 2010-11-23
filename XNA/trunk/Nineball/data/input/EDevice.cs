////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.nineball.data.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>入力デバイス 列挙体。</summary>
	[Flags]
	public enum EDevice
	{

		/// <summary>デバイスなし。</summary>
		none = 0,

		/// <summary>キーボード。</summary>
		keyboard = 1 << 0,

		/// <summary>マウス。</summary>
		mouse = 1 << 1,

		/// <summary>XBOX360 ゲームパッド。</summary>
		gamePad = 1 << 2,

		/// <summary>レガシ ゲームパッド。</summary>
		legacyGamePad = 1 << 3,

		/// <summary>予約(使用できません)。</summary>
		__reserved = 1 << 4,

	}
}
