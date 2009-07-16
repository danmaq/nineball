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

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

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

		//* -----------------------------------------------------------------------*
		/// <summary>減速変化→加速変化を組み合わせスプラインのような動きを模する内分カウンタです。高速→低速→高速と変化します。</summary>
		/// 
		/// <param name="fStart"><paramref name="fNow"/>が0と等しい場合の値</param>
		/// <param name="fEnd"><paramref name="fNow"/>が<paramref name="fLimit"/>と等しい場合の値</param>
		/// <param name="fNow">現在時間</param>
		/// <param name="fLimit"><paramref name="fEnd"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="fLimit"/>までの<paramref name="fNow"/>に相当する
		/// <paramref name="fStart"/>から<paramref name="fEnd"/>までの値
		/// </returns>
		public static float splineFSF( float fStart, float fEnd, float fNow, float fLimit ) {
			if( fNow <= 0.0f ) { return fStart; }
			if( fNow >= fLimit ) { return fEnd; }
			float fCenter = MathHelper.Lerp( fStart, fEnd, 0.5f );
			float fHalfLimit = fLimit / 2;
			return fNow < fHalfLimit ?
				slowdown( fStart, fCenter, fNow, fHalfLimit ) :
				accelerate( fCenter, fEnd, fNow - fHalfLimit, fHalfLimit );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>加速変化→減速変化を組み合わせスプラインのような動きを模する内分カウンタです。低速→高速→低速と変化します。</summary>
		/// 
		/// <param name="fStart"><paramref name="fNow"/>が0と等しい場合の値</param>
		/// <param name="fEnd"><paramref name="fNow"/>が<paramref name="fLimit"/>と等しい場合の値</param>
		/// <param name="fNow">現在時間</param>
		/// <param name="fLimit"><paramref name="fEnd"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="fLimit"/>までの<paramref name="fNow"/>に相当する
		/// <paramref name="fStart"/>から<paramref name="fEnd"/>までの値
		/// </returns>
		public static float splineSFS( float fStart, float fEnd, float fNow, float fLimit ) {
			if( fNow <= 0.0f ) { return fStart; }
			if( fNow >= fLimit ) { return fEnd; }
			float fCenter = MathHelper.Lerp( fStart, fEnd, 0.5f );
			float fHalfLimit = fLimit / 2;
			return fNow < fHalfLimit ?
				accelerate( fStart, fCenter, fNow, fHalfLimit ) :
				slowdown( fCenter, fEnd, fNow - fHalfLimit, fHalfLimit );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ネヴィル・スプラインのシミュレータです。</summary>
		/// 
		/// <param name="fStart"><paramref name="fNow"/>が0と等しい場合の値</param>
		/// <param name="fMiddle">制御点</param>
		/// <param name="fEnd"><paramref name="fNow"/>が<paramref name="fLimit"/>と等しい場合の値</param>
		/// <param name="fNow">現在時間</param>
		/// <param name="fLimit"><paramref name="fEnd"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="fLimit"/>までの<paramref name="fNow"/>に相当する
		/// <paramref name="fStart"/>～(<paramref name="fMiddle"/>)～<paramref name="fEnd"/>までの値
		/// </returns>
		public static float neville(
			float fStart, float fMiddle, float fEnd, float fNow, float fLimit
		) {
			if( fNow >= fLimit || fStart == fEnd || fLimit <= 0 ) { return fEnd; }
			if( fNow <= 0 ) { return fStart; }
			float fTimePoint = fNow / fLimit * 2;
			fMiddle = fEnd + ( fEnd - fMiddle ) * ( fTimePoint - 2 );
			return fMiddle + ( fMiddle - ( fMiddle + ( fMiddle - fStart ) * ( fTimePoint - 1 ) ) ) *
				( fTimePoint - 2 ) * 0.5f;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ベジェ・スプラインのシミュレータです。</summary>
		/// 
		/// <param name="fStart"><paramref name="fNow"/>が0と等しい場合の値</param>
		/// <param name="fMiddle">制御点</param>
		/// <param name="fEnd"><paramref name="fNow"/>が<paramref name="fLimit"/>と等しい場合の値</param>
		/// <param name="fNow">現在時間</param>
		/// <param name="fLimit"><paramref name="fEnd"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="fLimit"/>までの<paramref name="fNow"/>に相当する
		/// <paramref name="fStart"/>～(<paramref name="fMiddle"/>)～<paramref name="fEnd"/>までの値
		/// </returns>
		public static float bezier(
			float fStart, float fMiddle, float fEnd, float fNow, float fLimit
		) {
			if( fNow >= fLimit || fStart == fEnd || fLimit <= 0 ) { return fEnd; }
			if( fNow <= 0 ) { return fStart; }
			float fTimePoint = fNow / fLimit * 2;
			float fResidual = 1 - fTimePoint;
			return
				( float )Math.Pow( fResidual, 2 ) * fStart +
				( float )Math.Pow( fTimePoint, 2 ) * fEnd +
				( 2 * fResidual * fTimePoint * fMiddle );
		}
	}
}
