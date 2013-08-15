////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.ball.core;
using danmaq.ball.data;
using danmaq.nineball.data;
using danmaq.nineball.entity.fonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.misc
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>雑多な関数群クラス。</summary>
	public static class CMisc
	{

		//* -----------------------------------------------------------------------*
		/// <summary>よゆ風固定ピッチフォントを生成します。</summary>
		/// 
		/// <param name="locate">基準カーソル位置。</param>
		/// <param name="hAlign">水平位置揃え情報。</param>
		/// <param name="color">文字色。</param>
		/// <param name="text">テキスト。</param>
		/// <returns>フォント オブジェクト。</returns>
		public static CFont create98Font(Point locate, EAlign hAlign, Color color, string text)
		{
			CFont result = new CFont(CONTENT.texFont98, text);
			result.text = text;
			result.alignHorizontal = hAlign;
			result.alignVertical = EAlign.LeftTop;
			result.color = color;
			result.pos = new Vector2(locate.X * 8, locate.Y * 16);
			result.sprite = CGame.sprite;
			result.isDrawShadow = false;
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>カーソル座標からVGAへ座標変換をします。</summary>
		/// 
		/// <param name="src">カーソル座標。</param>
		/// <returns>VGA座標。</returns>
		public static Vector2 Cursor2VGA(Vector2 src)
		{
			return DCGA2VGA(new Vector2((int)src.X * 8, (int)src.Y * 16));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>カーソル座標からVGAへ座標変換をします。</summary>
		/// 
		/// <param name="src">カーソル座標。</param>
		/// <returns>VGA座標。</returns>
		public static Point Cursor2VGA(Point src)
		{
			return DCGA2VGA(new Point(src.X * 8, src.X * 16));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>カーソル座標からVGAへ座標変換をします。</summary>
		/// 
		/// <param name="src">カーソル座標。</param>
		/// <returns>VGA座標。</returns>
		public static Rectangle Cursor2VGA(Rectangle src)
		{
			src.X *= 8;
			src.Y *= 16;
			src.Width *= 8;
			src.Height *= 16;
			return DCGA2VGA(src);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>DCGAからVGAへ座標変換をします。</summary>
		/// 
		/// <param name="src">DCGA座標。</param>
		/// <returns>VGA座標。</returns>
		public static Vector2 DCGA2VGA(Vector2 src)
		{
			src.Y += 40;
			return src;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>DCGAからVGAへ座標変換をします。</summary>
		/// 
		/// <param name="src">DCGA座標。</param>
		/// <returns>VGA座標。</returns>
		public static Point DCGA2VGA(Point src)
		{
			src.Y += 40;
			return src;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>DCGAからVGAへ座標変換をします。</summary>
		/// 
		/// <param name="src">DCGA座標。</param>
		/// <returns>VGA座標。</returns>
		public static Rectangle DCGA2VGA(Rectangle src)
		{
			src.Y += 40;
			return src;
		}
	}
}
