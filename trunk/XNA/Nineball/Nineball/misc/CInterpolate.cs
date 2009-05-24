////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library / (c) 2008-2009 danmaq all rights reserved.
//		──内分カウンタ機能の関数集
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using Microsoft.Xna.Framework;
using System;

namespace danmaq.Nineball.misc {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>内分カウンタ機能の関数集クラス。</summary>
	public static class CInterpolate {

		//* -----------------------------------------------------------------------*
		/// <summary>等速変化する内分カウンタです。</summary>
		/// 
		/// <param name="fStart"><paramref name="fNow"/>が0と等しい場合の値</param>
		/// <param name="fEnd"><paramref name="fNow"/>が<paramref name="fLimit"/>と等しい場合の値</param>
		/// <param name="fNow">現在時間</param>
		/// <param name="fLimit"><paramref name="fEnd"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="fLimit"/>までの<paramref name="fNow"/>に相当する
		/// <paramref name="fStart"/>から<paramref name="fEnd"/>までの値
		/// </returns>
		public static float smooth( float fStart, float fEnd, float fNow, float fLimit ) {
			if( fNow <= 0.0f ) { return fStart; }
			if( fNow >= fLimit ) { return fEnd; }
			return MathHelper.Lerp( fStart, fEnd, fNow / fLimit );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>減速変化する内分カウンタです。</summary>
		/// 
		/// <param name="fStart"><paramref name="fNow"/>が0と等しい場合の値</param>
		/// <param name="fEnd"><paramref name="fNow"/>が<paramref name="fLimit"/>と等しい場合の値</param>
		/// <param name="fNow">現在時間</param>
		/// <param name="fLimit"><paramref name="fEnd"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="fLimit"/>までの<paramref name="fNow"/>に相当する
		/// <paramref name="fStart"/>から<paramref name="fEnd"/>までの値
		/// </returns>
		public static float slowdown( float fStart, float fEnd, float fNow, float fLimit ) {
			if( fNow <= 0.0f ) { return fStart; }
			if( fNow >= fLimit ) { return fEnd; }
			return MathHelper.Lerp( fStart, fEnd, 1 - ( float )Math.Pow( 1 - fNow / fLimit, 2 ) );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>加速変化する内分カウンタです。</summary>
		/// 
		/// <param name="fStart"><paramref name="fNow"/>が0と等しい場合の値</param>
		/// <param name="fEnd"><paramref name="fNow"/>が<paramref name="fLimit"/>と等しい場合の値</param>
		/// <param name="fNow">現在時間</param>
		/// <param name="fLimit"><paramref name="fEnd"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="fLimit"/>までの<paramref name="fNow"/>に相当する
		/// <paramref name="fStart"/>から<paramref name="fEnd"/>までの値
		/// </returns>
		public static float accelerate( float fStart, float fEnd, float fNow, float fLimit ) {
			if( fNow <= 0.0f ) { return fStart; }
			if( fNow >= fLimit ) { return fEnd; }
			return MathHelper.Lerp( fStart, fEnd, ( float )Math.Pow( fNow / fLimit, 2 ) );
		}

	}
}
