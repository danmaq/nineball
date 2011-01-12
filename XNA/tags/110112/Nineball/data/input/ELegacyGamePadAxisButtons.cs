﻿////////////////////////////////////////////////////////////////////////////////
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
	/// <summary>レガシ ゲームパッド ボタン一覧列挙体。</summary>
	public enum ELegacyGamePadAxisButtons
		: short
	{

		/// <summary>アナログ スティックを使用した2D(3D)入力。</summary>
		analog = -1,

		/// <summary>Point of Viewを使用した2D入力。</summary>
		pov = -2,

		/// <summary>予約(使用できません)。</summary>
		__reserved = -3,
	}
}
