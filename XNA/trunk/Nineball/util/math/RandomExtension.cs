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
		/// <summary>
		/// -<paramref name="range"/>と+<paramref name="range"/>の間の乱数を返します。
		/// </summary>
		/// 
		/// <param name="range">ブレ幅。</param>
		/// <param name="rnd">疑似乱数ジェネレータ。</param>
		/// <returns>
		/// -<paramref name="range"/>～+<paramref name="range"/>の単精度浮動小数点数。
		/// </returns>
		public static float nextBlur(this Random rnd, float range)
		{
			return rnd.nextFloat() * 2 - 1 ;
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
