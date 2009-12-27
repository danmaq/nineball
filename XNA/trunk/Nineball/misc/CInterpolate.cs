////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.misc {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>内分カウンタ機能の関数集クラス。</summary>
	public static class CInterpolate {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>等速変化する内分カウンタ。</summary>
		public static readonly Func<float, float, float> amountSmooth = ( fNow, fLimit ) =>
			fNow / fLimit;

		/// <summary>加速変化する内分カウンタ。</summary>
		public static readonly Func<float, float, float> amountSlowdown = ( fNow, fLimit ) =>
			1 - ( float )Math.Pow( 1 - fNow / fLimit, 2 );

		/// <summary>減速変化する内分カウンタ。</summary>
		public static readonly Func<float, float, float> amountAccelerate = ( fNow, fLimit ) =>
			( float )Math.Pow( fNow / fLimit, 2 );

		/// <summary>等速変化ループする内分カウンタ。</summary>
		public static readonly Func<float, float, float> amountLoopSmooth = ( fNow, fLimit ) =>
			CMisc.clampLoop( fNow, 0, fLimit ) / fLimit;

		/// <summary>加速変化ループする内分カウンタ。</summary>
		public static readonly Func<float, float, float> amountLoopSlowdown = ( fNow, fLimit ) =>
			1 - ( float )Math.Pow( 1 - CMisc.clampLoop( fNow, 0, fLimit ) / fLimit, 2 );

		/// <summary>減速変化ループする内分カウンタ。</summary>
		public static readonly Func<float, float, float> amountLoopAccelerate = ( fNow, fLimit ) =>
			( float )Math.Pow( CMisc.clampLoop( fNow, 0, fLimit ) / fLimit, 2 );

		/// <summary>範囲丸め込み付きの線形補完。</summary>
		public static readonly Func<float, float, float, float> clampLerp =
			( fStart, fEnd, fAmount ) =>
				MathHelper.Clamp( MathHelper.Lerp( fStart, fEnd, fAmount ), fStart, fEnd );

		/// <summary>範囲丸め込み付きの等速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> clampSmooth =
			( fStart, fEnd, fNow, fLimit ) =>
				clampLerp( fStart, fEnd, amountSmooth( fNow, fLimit ) );

		/// <summary>範囲丸め込み付きの減速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> clampSlowdown =
			( fStart, fEnd, fNow, fLimit ) =>
				clampLerp( fStart, fEnd, amountSlowdown( fNow, fLimit ) );

		/// <summary>範囲丸め込み付きの加速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> clampAccelerate =
			( fStart, fEnd, fNow, fLimit ) =>
				clampLerp( fStart, fEnd, amountAccelerate( fNow, fLimit ) );

		/// <summary>範囲丸め込み付きの加速→減速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> clampSlowFastSlow =
			( fStart, fEnd, fNow, fLimit ) => {
				if( fNow <= 0.0f ) { return fStart; }
				if( fNow >= fLimit ) { return fEnd; }
				float fCenter = MathHelper.Lerp( fStart, fEnd, 0.5f );
				float fHalfLimit = fLimit * 0.5f;
				return fNow < fHalfLimit ?
					MathHelper.Lerp( fStart, fCenter, amountAccelerate( fNow, fHalfLimit ) ) :
					MathHelper.Lerp( fCenter, fEnd, amountSlowdown( fNow - fHalfLimit, fHalfLimit ) );
			};

		/// <summary>範囲丸め込み付きの減速→加速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> clampFastSlowFast =
			( fStart, fEnd, fNow, fLimit ) => {
				if( fNow <= 0.0f ) { return fStart; }
				if( fNow >= fLimit ) { return fEnd; }
				float fCenter = MathHelper.Lerp( fStart, fEnd, 0.5f );
				float fHalfLimit = fLimit * 0.5f;
				return fNow < fHalfLimit ?
					MathHelper.Lerp( fStart, fCenter, amountSlowdown( fNow, fHalfLimit ) ) :
					MathHelper.Lerp( fCenter, fEnd, amountAccelerate( fNow - fHalfLimit, fHalfLimit ) );
			};

		/// <summary>範囲ループ付きの等速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> loopSmooth =
			( fStart, fEnd, fNow, fLimit ) =>
				MathHelper.Lerp( fStart, fEnd, amountLoopSmooth( fNow, fLimit ) );

		/// <summary>範囲ループ付きの減速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> loopSlowdown =
			( fStart, fEnd, fNow, fLimit ) =>
				MathHelper.Lerp( fStart, fEnd, amountLoopSlowdown( fNow, fLimit ) );

		/// <summary>範囲ループ付きの加速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> loopAccelerate =
			( fStart, fEnd, fNow, fLimit ) =>
				MathHelper.Lerp( fStart, fEnd, amountLoopAccelerate( fNow, fLimit ) );

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ネヴィル曲線を計算します。</summary>
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
		/// <summary>ベジェ曲線を計算します。</summary>
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
