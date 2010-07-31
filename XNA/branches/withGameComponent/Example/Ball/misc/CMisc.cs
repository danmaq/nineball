////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using Microsoft.Xna.Framework;

namespace danmaq.ball.misc
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>雑多な関数群クラス。</summary>
	public static class CMisc
	{

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
