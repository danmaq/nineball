////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.misc {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>雑多な演算関数集クラス。</summary>
	public static class CMisc {

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>リストをソートして一意な値のみ抽出します。</summary>
		/// 
		/// <typeparam name="_T">配列の元となる型</typeparam>
		/// <param name="expr">対象となるリスト</param>
		/// <returns>一意なソートされた配列</returns>
		/// <exception cref="System.ArgumentNullException">
		/// 引数にnullを指定した場合。
		/// </exception>
		public static List<_T> getUnique<_T>( this IEnumerable<_T> expr ) {
			if( expr == null ) { throw new ArgumentNullException( "expr" ); }
			List<_T> result;
#if WINDOWS
			result = new List<_T>( new HashSet<_T>( expr ) );
			result.Sort();
#else
			result = new List<_T>();
			foreach( _T value in expr ) {
				if(
					result.FindIndex(
						_expr => ( _expr == null && value == null ) || _expr.Equals( value )
					) == -1
				) { result.Add( value ); }
			}
#endif
			return result;
		}

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
		public static float clampLoop( float fExpr, float fMin, float fMax ) {
			return clampLoop( fExpr, fMin, fMax, false, true );
		}

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
		/// <param name="bClampMinEqual"><paramref name="fExpr"/>が<paramref name="fMin"/>と等しい場合、ループするかどうか</param>
		/// <param name="bClampMaxEqual"><paramref name="fExpr"/>が<paramref name="fMax"/>と等しい場合、ループするかどうか</param>
		/// <returns><paramref name="fMin"/>～<paramref name="fMax"/>に制限された値</returns>
		public static float clampLoop(
			float fExpr, float fMin, float fMax, bool bClampMinEqual, bool bClampMaxEqual
		) {
			if( fMin == fMax ) { return fMin; }
			else if( fMin > fMax ) {
				float fBuffer = fMax;
				fMax = fMin;
				fMin = fBuffer;
			}
			while(
				( bClampMaxEqual ? fExpr >= fMax : fExpr > fMax ) ||
				( bClampMinEqual ? fExpr <= fMin : fExpr < fMin )
			) {
				if( bClampMaxEqual ? fExpr >= fMax : fExpr > fMax ) { fExpr = fMin + fExpr - fMax; }
				if( bClampMinEqual ? fExpr <= fMin : fExpr < fMin ) { fExpr = fMax - Math.Abs( fExpr - fMin ); }
			}
			return MathHelper.Clamp( fExpr, fMin, fMax );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>矩形を中心を原点として拡大します。</summary>
		/// 
		/// <param name="rectExpr">矩形</param>
		/// <param name="fScale">拡大率</param>
		/// <returns>拡大した矩形</returns>
		public static Rectangle Inflate( this Rectangle rectExpr, float fScale ) {
			float fScaleHalf = fScale * 0.5f;
			Rectangle result = rectExpr;
			result.Inflate(
				( int )( result.Width * fScaleHalf ),
				( int )( result.Height * fScaleHalf ) );
			return result;
		}
	}
}
