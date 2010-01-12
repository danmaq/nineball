////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if WINDOWS

using System;

namespace danmaq.nineball.entity.input {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ用の予約されたボタンID。</summary>
	public enum EReservedButtonAxisID : short {

		/// <summary>POV(上)。</summary>
		povUp = -1,

		/// <summary>POV(下)。</summary>
		povDown = -2,

		/// <summary>POV(左)。</summary>
		povLeft = -3,

		/// <summary>POV(右)。</summary>
		povRight = -4,

		/// <summary>アナログ(上)。</summary>
		analogUp = -5,

		/// <summary>アナログ(下)。</summary>
		analogDown = -6,

		/// <summary>アナログ(左)。</summary>
		analogLeft = -7,

		/// <summary>アナログ(右)。</summary>
		analogRight = -8,

	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ状態の拡張機能。</summary>
	public static class JoystickStateExtension {

//		public static float 

	}
}

#endif
