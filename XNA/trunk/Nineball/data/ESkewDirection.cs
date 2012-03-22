////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using Microsoft.Xna.Framework;

namespace danmaq.nineball.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>斜め方向情報のための列挙体。</summary>
	public enum ESkewDirection
	{

		/// <summary>左上。</summary>
		leftTop,

		/// <summary>左下。</summary>
		leftBottom,

		/// <summary>右上。</summary>
		rightTop,

		/// <summary>右下。</summary>
		rightBottom,

	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>斜め方向情報のための列挙体の拡張機能。</summary>
	public static class ESkewDirectionExtension
	{

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>矩形の対応する頂点座標を取得します。</summary>
		/// 
		/// <param name="index">斜め方向情報のための列挙体。</param>
		/// <param name="rect">矩形。</param>
		/// <returns>矩形の頂点座標。</returns>
		public static Point getVertex(this ESkewDirection index, Rectangle rect)
		{
			Point result = Point.Zero;
			switch (index)
			{
				case ESkewDirection.leftTop:
					result = new Point(rect.Left, rect.Top);
					break;
				case ESkewDirection.leftBottom:
					result = new Point(rect.Left, rect.Bottom);
					break;
				case ESkewDirection.rightTop:
					result = new Point(rect.Right, rect.Top);
					break;
				case ESkewDirection.rightBottom:
					result = new Point(rect.Right, rect.Bottom);
					break;
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>矩形の対応する頂点座標を取得します。</summary>
		/// 
		/// <param name="index">斜め方向情報のための列挙体。</param>
		/// <param name="rect">矩形。</param>
		/// <returns>矩形の頂点座標。</returns>
		public static Vector2 getVertexVector(this ESkewDirection index, Rectangle rect)
		{
			Point result = index.getVertex(rect);
			return new Vector2(result.X, result.Y);
		}
	}
}
