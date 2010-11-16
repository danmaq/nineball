////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.nineball.old.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>方向ボタン列挙体。</summary>
	public enum EDirection : byte
	{
		/// <summary>上。</summary>
		up = 0,

		/// <summary>下。</summary>
		down = 1,

		/// <summary>左。</summary>
		left = 2,

		/// <summary>右。</summary>
		right = 3,

		// TODO : 旧バージョンを潰し次第削除する
		/// <summary>予約(使用できません)</summary>
		/// <remarks>
		/// 旧バージョンとの互換のために残してあります。
		/// 近い将来、この定数は削除されます。
		/// </remarks>
		__reserved = 4
	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>方向ボタン列挙体。</summary>
	[Flags]
	public enum EDirectionFlags : byte
	{
		/// <summary>無効。</summary>
		None = 0,

		/// <summary>上。</summary>
		up = (1 << (byte)EDirection.up),

		/// <summary>下。</summary>
		down = (1 << (byte)EDirection.down),

		/// <summary>左。</summary>
		left = (1 << (byte)EDirection.left),

		/// <summary>右。</summary>
		right = (1 << (byte)EDirection.right),
	}
}
