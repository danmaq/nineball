////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2009 danmaq all rights reserved.
//
//		──雑多な関数群クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace danmaq.Nineball.misc {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>雑多な関数群クラス。</summary>
	public static class CMisc {

		//* -----------------------------------------------------------------------*
		/// <summary>配列をソートして一意な値のみ抽出します。</summary>
		/// 
		/// <typeparam name="_T">配列の元となる型</typeparam>
		/// <param name="expr">配列</param>
		/// <returns>一意なソートされた配列</returns>
		/// <exception cref="System.ArgumentNullException">
		/// 引数にnullを指定した場合。
		/// </exception>
		public static _T[] getUnique<_T>( _T[] expr ) {
			if( expr == null ) { throw new ArgumentNullException( "expr" ); }
			Array.Sort<_T>( expr );
			LinkedList<_T> result = new LinkedList<_T>();
			foreach( _T value in expr ) {
				if( result.Find( value ) == null ) { result.AddLast( value ); }
			}
			return ( new List<_T>( result ).ToArray() );
		}


		//* -----------------------------------------------------------------------*
		/// <summary>値を限界値の範囲内に補正します。</summary>
		/// <remarks>
		/// 限界値を超えている場合、超えた分だけループします。
		/// ここが<c>MathHelper.Clamp()</c>とは違います。
		/// </remarks>
		/// 
		/// <param name="nExpr">対象値</param>
		/// <param name="nLimit1">限界値1</param>
		/// <param name="nLimit2">限界値2</param>
		/// <returns>限界値の範囲に補正された値</returns>
		public static int clampLoop( int nExpr, int nLimit1, int nLimit2 ) {
			if( nLimit1 == nLimit2 ) { return nLimit1; }
			if( nLimit1 > nLimit2 ) {
				int nLimitTemp = nLimit1;
				nLimit1 = nLimit2;
				nLimit2 = nLimitTemp;
			}
			int nResult = nExpr;
			while( nResult >= nLimit2 || nResult < nLimit1 ) {
				if( nResult >= nLimit2 ) { nResult = nLimit1 + nResult - nLimit2; }
				if( nResult < nLimit1 ) { nResult = nLimit2 - Math.Abs( nResult - nLimit1 ); }
			}
			return nResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>値を限界値の範囲内に補正します。</summary>
		/// <remarks>
		/// 限界値を超えている場合、超えた分だけループします。
		/// ここが<c>MathHelper.Clamp()</c>とは違います。
		/// </remarks>
		/// 
		/// <param name="fExpr">対象値</param>
		/// <param name="fLimit1">限界値1</param>
		/// <param name="fLimit2">限界値2</param>
		/// <returns>限界値の範囲に補正された値</returns>
		public static float clampLoop( float fExpr, float fLimit1, float fLimit2 ) {
			if( fLimit1 == fLimit2 ) { return fLimit1; }
			if( fLimit1 > fLimit2 ) {
				float fLimitTemp = fLimit1;
				fLimit1 = fLimit2;
				fLimit2 = fLimitTemp;
			}
			float fResult = fExpr;
			while( fResult >= fLimit2 || fResult < fLimit1 ) {
				if( fResult >= fLimit2 ) { fResult = fLimit1 + fResult - fLimit2; }
				if( fResult < fLimit1 ) { fResult = fLimit2 - Math.Abs( fResult - fLimit1 ); }
			}
			return fResult;
		}
	}
}
