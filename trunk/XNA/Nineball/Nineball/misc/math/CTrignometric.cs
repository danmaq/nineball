////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library / (c) 2008-2009 danmaq all rights reserved.
//		──三角関数系の演算関数集クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.Nineball.misc.math {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>三角関数系の演算関数集クラス。</summary>
	public static class CTrignometric {

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>周回軌道の角速度を計算します。</summary>
		/// 
		/// <param name="fRadius">周回半径</param>
		/// <param name="fSpeed">速度</param>
		/// <returns>周回軌道を回り続ける角速度(ラジアン)</returns>
		public static float cycricOrbit( float fRadius, float fSpeed ) { return fSpeed / fRadius; }

		//* -----------------------------------------------------------------------*
		/// <summary>指定された角度のセカントを返します。</summary>
		/// 
		/// <param name="dRadian">ラジアンで計測した角度</param>
		/// <returns><paramref name="dRadian"/>のセカント</returns>
		public static double sec( double dRadian ) { return 1.0 / Math.Cos( dRadian ); }

		//* -----------------------------------------------------------------------*
		/// <summary>指定された角度のコセカントを返します。</summary>
		/// 
		/// <param name="dRadian">ラジアンで計測した角度</param>
		/// <returns><paramref name="dRadian"/>のコセカント</returns>
		public static double cosec( double dRadian ) { return 1.0 / Math.Sin( dRadian ); }

		//* -----------------------------------------------------------------------*
		/// <summary>指定された角度のコタンジェントを返します。</summary>
		/// 
		/// <param name="dRadian">ラジアンで計測した角度</param>
		/// <returns><paramref name="dRadian"/>のコタンジェント</returns>
		public static double cotan( double dRadian ) { return 1.0 / Math.Tan( dRadian ); }

		//* -----------------------------------------------------------------------*
		/// <summary>指定された角度のアークセカントを返します。</summary>
		/// 
		/// <param name="dRadian">ラジアンで計測した角度</param>
		/// <returns><paramref name="dRadian"/>のアークセカント</returns>
		public static double asec( double dRadian ) {
			return ( Math.PI / 2.0 ) *
				Math.Atan( Math.Sqrt( Math.Pow( dRadian, 2 ) - 1 ) ) + Math.Sign( dRadian ) - 1;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定された角度のアークコセカントを返します。</summary>
		/// 
		/// <param name="dRadian">ラジアンで計測した角度</param>
		/// <returns><paramref name="dRadian"/>のアークコセカント</returns>
		public static double acosec( double dRadian ) {
			return ( Math.PI / 2 ) *
				Math.Atan( 1.0 / Math.Sqrt( Math.Pow( dRadian, 2 ) - 1 ) ) +
				Math.Sign( dRadian ) - 1;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定された角度のアークコタンジェントを返します。</summary>
		/// 
		/// <param name="dRadian">ラジアンで計測した角度</param>
		/// <returns><paramref name="dRadian"/>のアークコタンジェント</returns>
		public static double acotan( double dRadian ) {
			return -Math.Atan( dRadian ) + Math.PI / 2.0;
		}
	}
}
