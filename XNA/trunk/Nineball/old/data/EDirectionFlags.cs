////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.data;

namespace danmaq.nineball.old.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>方向ボタン列挙体。</summary>
	[Obsolete]
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
