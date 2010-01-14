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
		public static readonly Func<float, float, float> _amountSmooth = ( fTarget, fLimit ) =>
			fTarget / fLimit;

		/// <summary>加速変化する内分カウンタ。</summary>
		public static readonly Func<float, float, float> _amountSlowdown = ( fTarget, fLimit ) =>
			1 - ( float )Math.Pow( 1 - fTarget / fLimit, 2 );

		/// <summary>減速変化する内分カウンタ。</summary>
		public static readonly Func<float, float, float> _amountAccelerate = ( fTarget, fLimit ) =>
			( float )Math.Pow( fTarget / fLimit, 2 );

		/// <summary>等速変化ループする内分カウンタ。</summary>
		public static readonly Func<float, float, float> _amountLoopSmooth = ( fTarget, fLimit ) =>
			CMisc.clampLoop( fTarget, 0, fLimit ) / fLimit;

		/// <summary>加速変化ループする内分カウンタ。</summary>
		public static readonly Func<float, float, float> _amountLoopSlowdown = ( fTarget, fLimit ) =>
			1 - ( float )Math.Pow( 1 - CMisc.clampLoop( fTarget, 0, fLimit ) / fLimit, 2 );

		/// <summary>減速変化ループする内分カウンタ。</summary>
		public static readonly Func<float, float, float> _amountLoopAccelerate = ( fTarget, fLimit ) =>
			( float )Math.Pow( CMisc.clampLoop( fTarget, 0, fLimit ) / fLimit, 2 );

		/// <summary>範囲丸め込み付きの線形補完。</summary>
		public static readonly Func<float, float, float, float> _clampLerp =
			( fStart, fEnd, fAmount ) => {
				float fResult = MathHelper.Lerp( fStart, fEnd, fAmount );
				if( fStart > fEnd ) {
					float fTemp = fEnd;
					fEnd = fStart;
					fStart = fTemp;
				}
				return MathHelper.Clamp( fResult, fStart, fEnd );
			};

		/// <summary>範囲丸め込み付きの等速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _clampSmooth =
			( fStart, fEnd, fNow, fLimit ) =>
				_clampLerp( fStart, fEnd, _amountSmooth( fNow, fLimit ) );

		/// <summary>範囲丸め込み付きの減速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _clampSlowdown =
			( fStart, fEnd, fNow, fLimit ) =>
				_clampLerp( fStart, fEnd, _amountSlowdown( fNow, fLimit ) );

		/// <summary>範囲丸め込み付きの加速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _clampAccelerate =
			( fStart, fEnd, fNow, fLimit ) =>
				_clampLerp( fStart, fEnd, _amountAccelerate( fNow, fLimit ) );

		/// <summary>範囲丸め込み付きの加速→減速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _clampSlowFastSlow =
			( fStart, fEnd, fNow, fLimit ) => {
				if( fNow <= 0.0f ) { return fStart; }
				if( fNow >= fLimit ) { return fEnd; }
				float fCenter = MathHelper.Lerp( fStart, fEnd, 0.5f );
				float fHalfLimit = fLimit * 0.5f;
				return fNow < fHalfLimit ?
					MathHelper.Lerp( fStart, fCenter, _amountAccelerate( fNow, fHalfLimit ) ) :
					MathHelper.Lerp( fCenter, fEnd, _amountSlowdown( fNow - fHalfLimit, fHalfLimit ) );
			};

		/// <summary>範囲丸め込み付きの減速→加速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _clampFastSlowFast =
			( fStart, fEnd, fNow, fLimit ) => {
				if( fNow <= 0.0f ) { return fStart; }
				if( fNow >= fLimit ) { return fEnd; }
				float fCenter = MathHelper.Lerp( fStart, fEnd, 0.5f );
				float fHalfLimit = fLimit * 0.5f;
				return fNow < fHalfLimit ?
					MathHelper.Lerp( fStart, fCenter, _amountSlowdown( fNow, fHalfLimit ) ) :
					MathHelper.Lerp( fCenter, fEnd, _amountAccelerate( fNow - fHalfLimit, fHalfLimit ) );
			};

		/// <summary>範囲ループ付きの等速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _loopSmooth =
			( fStart, fEnd, fNow, fLimit ) =>
				MathHelper.Lerp( fStart, fEnd, _amountLoopSmooth( fNow, fLimit ) );

		/// <summary>範囲ループ付きの減速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _loopSlowdown =
			( fStart, fEnd, fNow, fLimit ) =>
				MathHelper.Lerp( fStart, fEnd, _amountLoopSlowdown( fNow, fLimit ) );

		/// <summary>範囲ループ付きの加速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _loopAccelerate =
			( fStart, fEnd, fNow, fLimit ) =>
				MathHelper.Lerp( fStart, fEnd, _amountLoopAccelerate( fNow, fLimit ) );

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの<paramref name="fTarget"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ等分で置換します。
		/// </summary>
		/// 
		/// <param name="fTarget">対象の値。</param>
		/// <param name="fLimit">
		/// <paramref name="fTarget"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの<paramref name="fTarget"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountSmooth( float fTarget, float fLimit ) {
			return _amountSmooth( fTarget, fLimit );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの<paramref name="fTarget"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ<c>1.0</c>にやや重みを置いて置換します。
		/// </summary>
		/// 
		/// <param name="fTarget">対象の値。</param>
		/// <param name="fLimit">
		/// <paramref name="fTarget"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの<paramref name="fTarget"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountSlowdown( float fTarget, float fLimit ) {
			return _amountSlowdown( fTarget, fLimit );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの<paramref name="fTarget"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ<c>0.0</c>にやや重みを置いて置換します。
		/// </summary>
		/// 
		/// <param name="fTarget">対象の値。</param>
		/// <param name="fLimit">
		/// <paramref name="fTarget"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの<paramref name="fTarget"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountAccelerate( float fTarget, float fLimit ) {
			return _amountAccelerate( fTarget, fLimit );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの<paramref name="fTarget"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ等分で置換します。
		/// </para>
		/// <para>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの範囲を超過した場合、ループします。
		/// </para>
		/// </summary>
		/// 
		/// <param name="fTarget">対象の値。</param>
		/// <param name="fLimit">
		/// <paramref name="fTarget"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの<paramref name="fTarget"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountLoopSmooth( float fTarget, float fLimit ) {
			return _amountLoopSmooth( fTarget, fLimit );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの<paramref name="fTarget"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ<c>1.0</c>にやや重みを置いて置換します。
		/// </para>
		/// <para>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの範囲を超過した場合、ループします。
		/// </para>
		/// </summary>
		/// 
		/// <param name="fTarget">対象の値。</param>
		/// <param name="fLimit">
		/// <paramref name="fTarget"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの<paramref name="fTarget"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountLoopSlowdown( float fTarget, float fLimit ) {
			return _amountLoopSlowdown( fTarget, fLimit );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの<paramref name="fTarget"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ<c>0.0</c>にやや重みを置いて置換します。
		/// </para>
		/// <para>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの範囲を超過した場合、ループします。
		/// </para>
		/// </summary>
		/// 
		/// <param name="fTarget">対象の値。</param>
		/// <param name="fLimit">
		/// <paramref name="fTarget"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="fLimit"/>までの<paramref name="fTarget"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountLoopAccelerate( float fTarget, float fLimit ) {
			return _amountLoopAccelerate( fTarget, fLimit );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="fAmount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="fValue1">ソース値。</param>
		/// <param name="fValue2">ソース値。</param>
		/// <param name="fAmount">
		/// <paramref name="fValue2"/>の重みを示す<c>0.0</c>から<c>1.0</c>までの値。
		/// </param>
		/// <returns>補完された値。</returns>
		public static float clampLerp( float fValue1, float fValue2, float fAmount ) {
			return _clampLerp( fValue1, fValue2, fAmount );
		}

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
		public static float clampSmooth( float fStart, float fEnd, float fNow, float fLimit ) {
			return _clampSmooth( fStart, fEnd, fNow, fLimit );
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
		public static float clampSlowdown( float fStart, float fEnd, float fNow, float fLimit ) {
			return _clampSlowdown( fStart, fEnd, fNow, fLimit );
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
		public static float clampAccelerate( float fStart, float fEnd, float fNow, float fLimit ) {
			return _clampAccelerate( fStart, fEnd, fNow, fLimit );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>
		/// 加速変化→減速変化を組み合わせ、スプラインの
		/// ような動きを模する内分カウンタです。
		/// </para>
		/// <para>低速→高速→低速と変化します。</para>
		/// </summary>
		/// 
		/// <param name="fStart"><paramref name="fNow"/>が0と等しい場合の値</param>
		/// <param name="fEnd"><paramref name="fNow"/>が<paramref name="fLimit"/>と等しい場合の値</param>
		/// <param name="fNow">現在時間</param>
		/// <param name="fLimit"><paramref name="fEnd"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="fLimit"/>までの<paramref name="fNow"/>に相当する
		/// <paramref name="fStart"/>から<paramref name="fEnd"/>までの値
		/// </returns>
		public static float clampSlowFastSlow(
			float fStart, float fEnd, float fNow, float fLimit
		) { return _clampSlowFastSlow( fStart, fEnd, fNow, fLimit ); }

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>
		/// 減速変化→加速変化を組み合わせ、スプラインの
		/// ような動きを模する内分カウンタです。
		/// </para>
		/// <para>高速→低速→高速と変化します。</para>
		/// </summary>
		/// 
		/// <param name="fStart"><paramref name="fNow"/>が0と等しい場合の値</param>
		/// <param name="fEnd"><paramref name="fNow"/>が<paramref name="fLimit"/>と等しい場合の値</param>
		/// <param name="fNow">現在時間</param>
		/// <param name="fLimit"><paramref name="fEnd"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="fLimit"/>までの<paramref name="fNow"/>に相当する
		/// <paramref name="fStart"/>から<paramref name="fEnd"/>までの値
		/// </returns>
		public static float clampFastSlowFast(
			float fStart, float fEnd, float fNow, float fLimit
		) { return _clampFastSlowFast( fStart, fEnd, fNow, fLimit ); }

		//* -----------------------------------------------------------------------*
		/// <summary>等速変化でループする内分カウンタです。</summary>
		/// 
		/// <param name="fStart"><paramref name="fNow"/>が0と等しい場合の値</param>
		/// <param name="fEnd"><paramref name="fNow"/>が<paramref name="fLimit"/>と等しい場合の値</param>
		/// <param name="fNow">現在時間</param>
		/// <param name="fLimit"><paramref name="fEnd"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="fLimit"/>までの<paramref name="fNow"/>に相当する
		/// <paramref name="fStart"/>から<paramref name="fEnd"/>までの値
		/// </returns>
		public static float loopSmooth( float fStart, float fEnd, float fNow, float fLimit ) {
			return _loopSmooth( fStart, fEnd, fNow, fLimit );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>減速変化でループする内分カウンタです。</summary>
		/// 
		/// <param name="fStart"><paramref name="fNow"/>が0と等しい場合の値</param>
		/// <param name="fEnd"><paramref name="fNow"/>が<paramref name="fLimit"/>と等しい場合の値</param>
		/// <param name="fNow">現在時間</param>
		/// <param name="fLimit"><paramref name="fEnd"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="fLimit"/>までの<paramref name="fNow"/>に相当する
		/// <paramref name="fStart"/>から<paramref name="fEnd"/>までの値
		/// </returns>
		public static float loopSlowdown( float fStart, float fEnd, float fNow, float fLimit ) {
			return _loopSlowdown( fStart, fEnd, fNow, fLimit );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>加速変化でループする内分カウンタです。</summary>
		/// 
		/// <param name="fStart"><paramref name="fNow"/>が0と等しい場合の値</param>
		/// <param name="fEnd"><paramref name="fNow"/>が<paramref name="fLimit"/>と等しい場合の値</param>
		/// <param name="fNow">現在時間</param>
		/// <param name="fLimit"><paramref name="fEnd"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="fLimit"/>までの<paramref name="fNow"/>に相当する
		/// <paramref name="fStart"/>から<paramref name="fEnd"/>までの値
		/// </returns>
		public static float loopAccelerate( float fStart, float fEnd, float fNow, float fLimit ) {
			return _loopAccelerate( fStart, fEnd, fNow, fLimit );
		}

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
