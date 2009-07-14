////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library / (c) 2008-2009 danmaq all rights reserved.
//		──双曲線関数系の演算関数集クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.Nineball.misc.math {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>双曲線関数系の演算関数集クラス。</summary>
	public static class CHyperbolic {

		//* -----------------------------------------------------------------------*
		/// <summary>指定された角度のハイパーボリック セカントを返します。</summary>
		/// 
		/// <param name="dRadian">ラジアンで計測した角度</param>
		/// <returns><paramref name="dRadian"/>のハイパーボリック セカント</returns>
		public static double secH( double dRadian ) {
			return 2.0 / ( Math.Exp( dRadian ) + Math.Exp( -dRadian ) );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定された角度のハイパーボリック コセカントを返します。</summary>
		/// 
		/// <param name="dRadian">ラジアンで計測した角度</param>
		/// <returns><paramref name="dRadian"/>のハイパーボリック コセカント</returns>
		public static double cosecH( double dRadian ) {
			return 2.0 / ( Math.Exp( dRadian ) - Math.Exp( -dRadian ) );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定された角度のハイパーボリック コタンジェントを返します。</summary>
		/// 
		/// <param name="dRadian">ラジアンで計測した角度</param>
		/// <returns><paramref name="dRadian"/>のハイパーボリック コタンジェント</returns>
		public static double cotanH( double dRadian ) {
			return Math.Exp( -dRadian ) / ( Math.Exp( dRadian ) - Math.Exp( -dRadian ) ) * 2 + 1;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定された角度のハイパーボリック アークサインを返します。</summary>
		/// 
		/// <param name="dRadian">ラジアンで計測した角度</param>
		/// <returns><paramref name="dRadian"/>のハイパーボリック アークサイン</returns>
		public static double asinH( double dRadian ) {
			return Math.Log( dRadian + Math.Sqrt( Math.Pow( dRadian, 2 ) + 1 ) );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定された角度のハイパーボリック アークコサインを返します。</summary>
		/// 
		/// <param name="dRadian">ラジアンで計測した角度</param>
		/// <returns><paramref name="dRadian"/>のハイパーボリック アークコサイン</returns>
		public static double acosH( double dRadian ) {
			return Math.Log( dRadian + Math.Sqrt( Math.Pow( dRadian, 2 ) - 1 ) );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定された角度のハイパーボリック アークタンジェントを返します。</summary>
		/// 
		/// <param name="dRadian">ラジアンで計測した角度</param>
		/// <returns><paramref name="dRadian"/>のハイパーボリック アークタンジェント</returns>
		public static double atanH( double dRadian ) {
			return Math.Log( ( 1 + dRadian ) / ( 1 - dRadian ) ) / 2;
		}
		
		// ! TODO : asecH, acosecH, acotanH 実装する
	}
}
