////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.nineball.util.math
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>擬似乱数ジェネレータの拡張機能。</summary>
	public static class RandomExtension
	{

		//* -----------------------------------------------------------------------*
		/// <summary>指定した範囲内の乱数を返します。</summary>
		/// 
		/// <param name="rnd">疑似乱数ジェネレータ。</param>
		/// <param name="expr1">返される乱数の下限値。</param>
		/// <param name="expr2">返される乱数の上限値。</param>
		/// <returns>
		/// <paramref name="expr1"/>から<paramref name="expr2"/>までの単精度浮動小数点数。
		/// </returns>
		public static float Next(this Random rnd, float expr1, float expr2)
		{
			return expr1 + rnd.nextFloat() * (expr2 - expr1);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// -<paramref name="range"/>と+<paramref name="range"/>の間の乱数を返します。
		/// </summary>
		/// 
		/// <param name="rnd">疑似乱数ジェネレータ。</param>
		/// <param name="range">ブレ幅。</param>
		/// <returns>
		/// -<paramref name="range"/>～+<paramref name="range"/>の単精度浮動小数点数。
		/// </returns>
		public static float nextBlur(this Random rnd, float range)
		{
			return (rnd.nextFloat() - 0.5f) * range * 2;
		}

		//* -----------------------------------------------------------------------*
		/// <summary><c>0.0</c>と<c>1.0</c>の間の乱数を返します。</summary>
		/// 
		/// <param name="rnd">疑似乱数ジェネレータ。</param>
		/// <returns><c>0.0</c>以上<c>1.0</c>未満の単精度浮動小数点数。</returns>
		public static float nextFloat(this Random rnd)
		{
			return (float)rnd.NextDouble();
		}
	}
}
