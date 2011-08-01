////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2009 danmaq all rights reserved.
//
//		──雑多な演算関数集クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.Nineball.misc.math {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>雑多な演算関数集クラス。</summary>
	public static class CMisc {

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>値を指定された範囲内に制限します。</summary>
		/// <remarks>
		/// 最小値と最大値を逆さに設定しても内部で自動的に認識・交換しますが、
		/// 無駄なオーバーヘッドが増えるだけなので極力避けてください。
		/// </remarks>
		/// 
		/// <param name="fExpr">対象の値</param>
		/// <param name="fMin">制限値(最小)</param>
		/// <param name="fMax">制限値(最大)</param>
		/// <returns><paramref name="fMin"/>～<paramref name="fMax"/>に制限された値</returns>
		public static float clampLoop( float fExpr, float fMin, float fMax ){
			if( fMin == fMax ) { return fMin; }
			else if( fMin > fMax ) {
				float fBuffer = fMax;
				fMax = fMin;
				fMin = fBuffer;
			}
			while( fExpr >= fMax || fExpr < fMin ) {
				if( fExpr >= fMax ) { fExpr = fMin + fExpr - fMax; }
				if( fExpr < fMax ) { fExpr = fMax - Math.Abs( fExpr - fMin ); }
			}
			return Math.Min( fMax, Math.Max( fMin, fExpr ) );
			// ! TODO : clampのラッパにできないかな？
		}
	}
}
