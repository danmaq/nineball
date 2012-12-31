////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.entity.fonts
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レンダリングされたグラデーション情報を格納する構造体。</summary>
	[Serializable]
	public struct SFontGradationInfo
	{

		/// <summary>位置情報</summary>
		public Vector2 pos;

		/// <summary>文字サイズ</summary>
		public Vector2 charSize;

		/// <summary>拡大率</summary>
		public Vector2 scale;

		/// <summary>回転量</summary>
		public float rotate;

		/// <summary>色輝度(文字本体)</summary>
		public Color argbText;

		/// <summary>色輝度(影)</summary>
		public Color argbShadow;

		/// <summary>単文字</summary>
		public string strByte;
	}
}
