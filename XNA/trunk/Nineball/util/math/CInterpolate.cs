////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework;

/*
内分カウンタ機能は0.2.1.135より、従来のLuna(http://www.twin-tail.jp/)ベースから、
Tweener(http://code.google.com/p/tweener/)ベースへと変更します。

なるべく従来と互換性を残すつもりですが、数値レベルの完全な互換性は失われます。

Disclaimer for Robert Penner's Easing Equations license:

TERMS OF USE - EASING EQUATIONS

Open source under the BSD License.

Copyright © 2001 Robert Penner
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of the author nor the names of contributors may be used to
      endorse or promote products derived from this software without specific
     prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
POSSIBILITY OF SUCH DAMAGE.
*/

namespace danmaq.nineball.util.math
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>内分カウンタ機能の関数集クラス。</summary>
	/// <remarks>
	/// 参考Webページ。
	/// http://www.tonpoo.com/tweener/misc/transitions.html
	/// </remarks>
	public static class CInterpolate
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>等速変化する内分カウンタ。</summary>
		public static readonly Func<float, float, float> _amountSmooth = (target, limit) =>
			MathHelper.Clamp(target, 0, limit) / limit;

		/// <summary>加速変化する内分カウンタ。</summary>
		public static readonly Func<float, float, float> _amountSlowdown = (target, limit) =>
			1 - (float)Math.Pow(1 - MathHelper.Clamp(target, 0, limit) / limit, 2);

		/// <summary>減速変化する内分カウンタ。</summary>
		public static readonly Func<float, float, float> _amountAccelerate = (target, limit) =>
			(float)Math.Pow(MathHelper.Clamp(target, 0, limit) / limit, 2);

		/// <summary>等速変化ループする内分カウンタ。</summary>
		public static readonly Func<float, float, float> _amountLoopSmooth = (target, limit) =>
			CMisc.clampLoop(target, 0, limit) / limit;

		/// <summary>加速変化ループする内分カウンタ。</summary>
		public static readonly Func<float, float, float> _amountLoopSlowdown = (target, limit) =>
			1 - (float)Math.Pow(1 - CMisc.clampLoop(target, 0, limit) / limit, 2);

		/// <summary>減速変化ループする内分カウンタ。</summary>
		public static readonly Func<float, float, float> _amountLoopAccelerate = (target, limit) =>
			(float)Math.Pow(CMisc.clampLoop(target, 0, limit) / limit, 2);

		/// <summary>範囲丸め込み付きの線形補完。</summary>
		public static readonly Func<float, float, float, float> _clampLerp =
			(start, end, amount) => MathHelper.Lerp(start, end, MathHelper.Clamp(amount, 0, 1));

		/// <summary>範囲丸め込み付きの等速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _clampSmooth =
			(start, end, target, limit) =>
				_clampLerp(start, end, _amountSmooth(target, limit));

		/// <summary>範囲丸め込み付きの減速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _clampSlowdown =
			(start, end, target, limit) =>
				_clampLerp(start, end, _amountSlowdown(target, limit));

		/// <summary>範囲丸め込み付きの加速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _clampAccelerate =
			(start, end, target, limit) =>
				_clampLerp(start, end, _amountAccelerate(target, limit));

		/// <summary>範囲丸め込み付きの加速→減速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _clampSlowFastSlow =
			(start, end, target, limit) =>
			{
				if(target <= 0.0f)
				{
					return start;
				}
				if(target >= limit)
				{
					return end;
				}
				float fCenter = MathHelper.Lerp(start, end, 0.5f);
				float fHallimit = limit * 0.5f;
				return target < fHallimit ?
					MathHelper.Lerp(start, fCenter, _amountAccelerate(target, fHallimit)) :
					MathHelper.Lerp(fCenter, end, _amountSlowdown(target - fHallimit, fHallimit));
			};

		/// <summary>範囲丸め込み付きの減速→加速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _clampFastSlowFast =
			(start, end, target, limit) =>
			{
				if(target <= 0.0f)
				{
					return start;
				}
				if(target >= limit)
				{
					return end;
				}
				float fCenter = MathHelper.Lerp(start, end, 0.5f);
				float fHallimit = limit * 0.5f;
				return target < fHallimit ?
					MathHelper.Lerp(start, fCenter, _amountSlowdown(target, fHallimit)) :
					MathHelper.Lerp(fCenter, end, _amountAccelerate(target - fHallimit, fHallimit));
			};

		/// <summary>範囲ループ付きの等速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _loopSmooth =
			(start, end, target, limit) =>
				MathHelper.Lerp(start, end, _amountLoopSmooth(target, limit));

		/// <summary>範囲ループ付きの減速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _loopSlowdown =
			(start, end, target, limit) =>
				MathHelper.Lerp(start, end, _amountLoopSlowdown(target, limit));

		/// <summary>範囲ループ付きの加速線形補完。</summary>
		public static readonly Func<float, float, float, float, float> _loopAccelerate =
			(start, end, target, limit) =>
				MathHelper.Lerp(start, end, _amountLoopAccelerate(target, limit));

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		#region Obsolete

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ等分で置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountSmooth(float target, float limit)
		{
			return _amountSmooth(target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ<c>1.0</c>にやや重みを置いて置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountSlowdown(float target, float limit)
		{
			return _amountSlowdown(target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ<c>0.0</c>にやや重みを置いて置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountAccelerate(float target, float limit)
		{
			return _amountAccelerate(target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ等分で置換します。
		/// </para>
		/// <para>
		/// <c>0.0</c>から<paramref name="limit"/>までの範囲を超過した場合、ループします。
		/// </para>
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountLoopSmooth(float target, float limit)
		{
			return _amountLoopSmooth(target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ<c>1.0</c>にやや重みを置いて置換します。
		/// </para>
		/// <para>
		/// <c>0.0</c>から<paramref name="limit"/>までの範囲を超過した場合、ループします。
		/// </para>
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountLoopSlowdown(float target, float limit)
		{
			return _amountLoopSlowdown(target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ<c>0.0</c>にやや重みを置いて置換します。
		/// </para>
		/// <para>
		/// <c>0.0</c>から<paramref name="limit"/>までの範囲を超過した場合、ループします。
		/// </para>
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountLoopAccelerate(float target, float limit)
		{
			return _amountLoopAccelerate(target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>等速変化する内分カウンタです。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end"><paramref name="target"/>が<paramref name="limit"/>と等しい場合の値</param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float clampSmooth(float start, float end, float target, float limit)
		{
			return _clampSmooth(start, end, target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>減速変化する内分カウンタです。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end"><paramref name="target"/>が<paramref name="limit"/>と等しい場合の値</param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float clampSlowdown(float start, float end, float target, float limit)
		{
			return _clampSlowdown(start, end, target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>加速変化する内分カウンタです。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end"><paramref name="target"/>が<paramref name="limit"/>と等しい場合の値</param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float clampAccelerate(float start, float end, float target, float limit)
		{
			return _clampAccelerate(start, end, target, limit);
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
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end"><paramref name="target"/>が<paramref name="limit"/>と等しい場合の値</param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float clampSlowFastSlow(
			float start, float end, float target, float limit
		)
		{
			return _clampSlowFastSlow(start, end, target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>
		/// 減速変化→加速変化を組み合わせ、スプラインの
		/// ような動きを模する内分カウンタです。
		/// </para>
		/// <para>高速→低速→高速と変化します。</para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end"><paramref name="target"/>が<paramref name="limit"/>と等しい場合の値</param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float clampFastSlowFast(
			float start, float end, float target, float limit
		)
		{
			return _clampFastSlowFast(start, end, target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>等速変化でループする内分カウンタです。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end"><paramref name="target"/>が<paramref name="limit"/>と等しい場合の値</param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float loopSmooth(float start, float end, float target, float limit)
		{
			return _loopSmooth(start, end, target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>減速変化でループする内分カウンタです。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end"><paramref name="target"/>が<paramref name="limit"/>と等しい場合の値</param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float loopSlowdown(float start, float end, float target, float limit)
		{
			return _loopSlowdown(start, end, target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>加速変化でループする内分カウンタです。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end"><paramref name="target"/>が<paramref name="limit"/>と等しい場合の値</param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float loopAccelerate(float start, float end, float target, float limit)
		{
			return _loopAccelerate(start, end, target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ネヴィル曲線を計算します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="fMiddle">制御点</param>
		/// <param name="end"><paramref name="target"/>が<paramref name="limit"/>と等しい場合の値</param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>～(<paramref name="fMiddle"/>)～<paramref name="end"/>までの値
		/// </returns>
		public static float neville(
			float start, float fMiddle, float end, float target, float limit
		)
		{
			if(target >= limit || start == end || limit <= 0)
			{
				return end;
			}
			if(target <= 0)
			{
				return start;
			}
			float fTimePoint = target / limit * 2;
			fMiddle = end + (end - fMiddle) * (fTimePoint - 2);
			return fMiddle + (fMiddle - (fMiddle + (fMiddle - start) * (fTimePoint - 1))) *
				(fTimePoint - 2) * 0.5f;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ベジェ曲線を計算します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="fMiddle">制御点</param>
		/// <param name="end"><paramref name="target"/>が<paramref name="limit"/>と等しい場合の値</param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>～(<paramref name="fMiddle"/>)～<paramref name="end"/>までの値
		/// </returns>
		public static float bezier(
			float start, float fMiddle, float end, float target, float limit
		)
		{
			if(target >= limit || start == end || limit <= 0)
			{
				return end;
			}
			if(target <= 0)
			{
				return start;
			}
			float fTimePoint = target / limit * 2;
			float fResidual = 1 - fTimePoint;
			return
				(float)Math.Pow(fResidual, 2) * start +
				(float)Math.Pow(fTimePoint, 2) * end +
				(2 * fResidual * fTimePoint * fMiddle);
		}

		#endregion

		#region amount

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountLinear(float target, float limit)
		{
			return target / limit;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInQuad(float target, float limit)
		{
			return (target /= limit) * target;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutQuad(float target, float limit)
		{
			return -(target /= limit) * (target - 2);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInCubic(float target, float limit)
		{
			return (target /= limit) * target * target;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutCubic(float target, float limit)
		{
			return (target = target / limit - 1) * target * target;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInQuart(float target, float limit)
		{
			return (target /= limit) * target * target * target;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutQuart(float target, float limit)
		{
			return -((target = target / limit - 1) * target * target * target - 1);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInQuint(float target, float limit)
		{
			return (target /= limit) * target * target * target * target;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutQuint(float target, float limit)
		{
			return ((target = target / limit - 1) * target * target * target * target + 1);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutSin(float target, float limit)
		{
			return (float)Math.Sin(target / limit * MathHelper.PiOver2);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInOutSin(float target, float limit)
		{
			return -0.5f * ((float)Math.Cos(MathHelper.Pi * target / limit) - 1);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInCirc(float target, float limit)
		{
			return -((float)Math.Sqrt(1 - (target /= limit) * target) - 1);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutCirc(float target, float limit)
		{
			return (float)Math.Sqrt(1 - (target = target / limit - 1) * target);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInBack(float target, float limit)
		{
			return amountInBack(target, limit, 1.70158f);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInBack(float target, float limit, float overshoot)
		{
			return (target /= limit) * target * ((overshoot + 1) * target - overshoot);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutBack(float target, float limit)
		{
			return amountOutBack(target, limit, 1.70158f);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutBack(float target, float limit, float overshoot)
		{
			return (target = target / limit - 1) *
				target * ((overshoot + 1) * target + overshoot) + 1;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutBounce(float target, float limit)
		{
			float result;
			if ((target /= limit) < (1 / 2.75f))
			{
				result = 7.5625f * target * target;
			}
			else if (target < (2 / 2.75f))
			{
				result = 7.5625f * (target -= (1.5f / 2.75f)) * target + 0.75f;
			}
			else if (target < (2.5 / 2.75f))
			{
				result = 7.5625f * (target -= (2.25f / 2.75f)) * target + 0.9375f;
			}
			else
			{
				result = 7.5625f * (target -= (2.625f / 2.75f)) * target + 0.984375f;
			}
			return result;
		}

		#endregion
		#region amountClamp

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountLinearClamp(float target, float limit)
		{
			return amountLinear(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInQuadClamp(float target, float limit)
		{
			return amountInQuad(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutQuadClamp(float target, float limit)
		{
			return amountOutQuad(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInCubicClamp(float target, float limit)
		{
			return amountInCubic(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutCubicClamp(float target, float limit)
		{
			return amountOutCubic(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInQuartClamp(float target, float limit)
		{
			return amountInQuart(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutQuartClamp(float target, float limit)
		{
			return amountOutQuart(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInQuintClamp(float target, float limit)
		{
			return amountInQuint(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutQuintClamp(float target, float limit)
		{
			return amountOutQuint(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutSinClamp(float target, float limit)
		{
			return amountOutSin(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInOutSinClamp(float target, float limit)
		{
			return amountInOutSin(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInCircClamp(float target, float limit)
		{
			return amountInCirc(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutCircClamp(float target, float limit)
		{
			return amountOutCirc(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInBackClamp(float target, float limit)
		{
			return amountInBack(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInBackClamp(float target, float limit, float overshoot)
		{
			return amountInBack(MathHelper.Clamp(target, 0, limit), limit, overshoot);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutBackClamp(float target, float limit)
		{
			return amountOutBack(MathHelper.Clamp(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutBackClamp(float target, float limit, float overshoot)
		{
			return amountOutBack(MathHelper.Clamp(target, 0, limit), limit, overshoot);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutBounceClamp(float target, float limit)
		{
			return amountOutBounce(MathHelper.Clamp(target, 0, limit), limit);
		}

		#endregion
		#region amountLoop

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountLinearLoop(float target, float limit)
		{
			return amountLinear(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInQuadLoop(float target, float limit)
		{
			return amountInQuad(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutQuadLoop(float target, float limit)
		{
			return amountOutQuad(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInCubicLoop(float target, float limit)
		{
			return amountInCubic(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutCubicLoop(float target, float limit)
		{
			return amountOutCubic(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInQuartLoop(float target, float limit)
		{
			return amountInQuart(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutQuartLoop(float target, float limit)
		{
			return amountOutQuart(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInQuintLoop(float target, float limit)
		{
			return amountInQuint(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutQuintLoop(float target, float limit)
		{
			return amountOutQuint(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutSinLoop(float target, float limit)
		{
			return amountOutSin(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInOutSinLoop(float target, float limit)
		{
			return amountInOutSin(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInCircLoop(float target, float limit)
		{
			return amountInCirc(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutCircLoop(float target, float limit)
		{
			return amountOutCirc(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInBackLoop(float target, float limit)
		{
			return amountInBack(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountInBackLoop(float target, float limit, float overshoot)
		{
			return amountInBack(CMisc.clampLoop(target, 0, limit), limit, overshoot);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutBackLoop(float target, float limit)
		{
			return amountOutBack(CMisc.clampLoop(target, 0, limit), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutBackLoop(float target, float limit, float overshoot)
		{
			return amountOutBack(CMisc.clampLoop(target, 0, limit), limit, overshoot);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する値を<c>0.0</c>から<c>1.0</c>までの値へ置換します。
		/// </summary>
		/// 
		/// <param name="target">対象の値。</param>
		/// <param name="limit">
		/// <paramref name="target"/>がこの値と等しい時、<c>1.0</c>となる値。
		/// </param>
		/// <returns>
		/// <c>0.0</c>から<paramref name="limit"/>までの<paramref name="target"/>に
		/// 相当する、<c>0.0</c>から<c>1.0</c>までの値
		/// </returns>
		public static float amountOutBounceLoop(float target, float limit)
		{
			return amountOutBounce(CMisc.clampLoop(target, 0, limit), limit);
		}

		#endregion
		#region lerp

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLinear(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountLinear(target, limit));
		}

		#region quad

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInQuad(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountInQuad(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutQuad(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountOutQuad(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInOutQuad(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return ((target /= limit * 0.5f) < 1) ?
				hc * target * target + start : -hc * ((--target) * (target - 2) - 1) + start; 
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutInQuad(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return (target < limit * 0.5f) ?
				lerpOutQuad(start, start + hc, target * 2, limit) :
				lerpInQuad(start + hc, end, target * 2 - limit, limit);
		}

		#endregion
		#region cubic

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInCubic(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountInCubic(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutCubic(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountOutCubic(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInOutCubic(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return ((target /= limit * 0.5f) < 1) ?
				hc * target * target * target + start :
				-hc * ((target -= 2) * target * target + 2) + start;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutInCubic(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return (target < limit * 0.5f) ?
				lerpOutCubic(start, start + hc, target * 2, limit) :
				lerpInCubic(start + hc, end, target * 2 - limit, limit);
		}

		#endregion
		#region quart

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInQuart(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountInQuart(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutQuart(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountOutQuart(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInOutQuart(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return ((target /= limit * 0.5f) < 1) ?
				hc * target * target * target * target + start :
				-hc * ((target -= 2) * target * target * target - 2) + start;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutInQuart(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return (target < limit * 0.5f) ?
				lerpOutQuart(start, start + hc, target * 2, limit) :
				lerpInQuart(start + hc, end, target * 2 - limit, limit);
		}

		#endregion
		#region quint

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInQuint(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountInQuint(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutQuint(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountOutQuint(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInOutQuint(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return ((target /= limit * 0.5f) < 1) ?
				hc * target * target * target * target * target + start :
				-hc * ((target -= 2) * target * target * target * target + 2) + start;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutInQuint(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return (target < limit * 0.5f) ?
				lerpOutQuint(start, start + hc, target * 2, limit) :
				lerpInQuint(start + hc, end, target * 2 - limit, limit);
		}

		#endregion
		#region sin

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInSin(float start, float end, float target, float limit)
		{
			float c = end - start;
			return -c * (float)Math.Cos(target / limit * MathHelper.PiOver2) + c + start;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutSin(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountOutSin(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInOutSin(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountInOutSin(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutInSin(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return (target < limit * 0.5f) ?
				lerpOutSin(start, start + hc, target * 2, limit) :
				lerpInSin(start + hc, end, target * 2 - limit, limit);
		}

		#endregion
		#region expo

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInExpo(float start, float end, float target, float limit)
		{
			float c = end - start;
			return (target == 0) ? start :
				c * (float)Math.Pow(2, 10 * (target / limit - 1)) + start - c * 0.001f;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutExpo(float start, float end, float target, float limit)
		{
			float c = end - start;
			return (target == limit) ? end :
				c * 1.001f * (1 - (float)Math.Pow(2, -10 * target / limit)) + start;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInOutExpo(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountInOutSin(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutInExpo(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return (target < limit * 0.5f) ?
				lerpOutExpo(start, start + hc, target * 2, limit) :
				lerpInExpo(start + hc, end, target * 2 - limit, limit);
		}

		#endregion
		#region circ

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInCirc(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountInCirc(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutCirc(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountOutCirc(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInOutCirc(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return ((target /= limit * 0.5f) < 1) ?
				-hc * ((float)Math.Sqrt(1 - target * target) - 1) + start :
				hc * ((float)Math.Sqrt(1 - (target -= 2) * target) + 1) + start;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutInCirc(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return (target < limit * 0.5f) ?
				lerpOutCirc(start, start + hc, target * 2, limit) :
				lerpInCirc(start + hc, end, target * 2 - limit, limit);
		}

		#endregion
		#region elastic

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInElastic(float start, float end, float target, float limit)
		{
			return lerpInElastic(start, end, target, limit, 0, limit * 0.3f);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="amp">振り切る大きさ。</param>
		/// <param name="period">周期。</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInElastic(
			float start, float end, float target, float limit, float amp, float period)
		{
			float result = start;
			if (target != 0)
			{
				if ((target /= limit) == 1)
				{
					result = end;
				}
				else
				{
					float s = calcElastic(end - start, ref amp, ref period);
					result = start - (amp * (float)Math.Pow(2, 10 * (target -= 1)) *
						(float)Math.Sin((target * limit - s) * MathHelper.TwoPi / period));
				}
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutElastic(float start, float end, float target, float limit)
		{
			return lerpOutElastic(start, end, target, limit, 0, limit * 0.3f);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="amp">振り切る大きさ。</param>
		/// <param name="period">周期。</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutElastic(
			float start, float end, float target, float limit, float amp, float period)
		{
			float result = start;
			if (target != 0)
			{
				if ((target /= limit) == 1)
				{
					result = end;
				}
				else
				{
					float c = end - start;
					float s = calcElastic(c, ref amp, ref period);
					result = amp * (float)Math.Pow(2, -10 * target) *
						(float)Math.Sin((target * limit - s) *
						MathHelper.TwoPi / period) + c + start;
				}
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInOutElastic(float start, float end, float target, float limit)
		{
			return lerpInOutElastic(start, end, target, limit, 0, limit * 0.3f);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="amp">振り切る大きさ。</param>
		/// <param name="period">周期。</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInOutElastic(
			float start, float end, float target, float limit, float amp, float period)
		{
			float result = start;
			if (target != 0)
			{
				if ((target /= limit * 0.5f) == 2)
				{
					result = end;
				}
				else
				{
					float c = end - start;
					float s = calcElastic(c, ref amp, ref period);
					result = (target < 1) ?
						start - 0.5f * (amp * (float)Math.Pow(2, 10 * (target -= 1)) *
							(float)Math.Sin((target * limit - s) * MathHelper.TwoPi / period)) :
						amp * (float)Math.Pow(2, -10 * (target -= 1)) *
							(float)Math.Sin((target * limit - s) * MathHelper.TwoPi / period) *
							0.5f + c + start;
				}
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutInElastic(float start, float end, float target, float limit)
		{
			return lerpOutInElastic(start, end, target, limit, 0, limit * 0.3f);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="amp">振り切る大きさ。</param>
		/// <param name="period">周期。</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutInElastic(
			float start, float end, float target, float limit, float amp, float period)
		{
			float hc = (end - start) * 0.5f;
			return (target < limit * 0.5f) ?
				lerpOutElastic(start, start + hc, target * 2, limit, amp, period) :
				lerpInElastic(start + hc, end, target * 2 - limit, limit, amp, period);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>線形補間に必要な値を計算します。</summary>
		/// 
		/// <param name="delta">差分値。</param>
		/// <param name="amp">振り切る大きさ。</param>
		/// <param name="period">周期。</param>
		/// <returns>値。</returns>
		private static float calcElastic(float delta, ref float amp, ref float period)
		{
			float result;
			if (amp != 0 || amp < Math.Abs(delta))
			{
				amp = delta;
				result = period * 0.25f;
			}
			else
			{
				result = period / MathHelper.TwoPi * (float)Math.Asin(delta / amp);
			}
			return result;
		}

		#endregion
		#region back

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInBack(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountInBack(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInBack(
			float start, float end, float target, float limit, float overshoot)
		{
			return MathHelper.Lerp(start, end, amountInBack(target, limit, overshoot));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutBack(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountOutBack(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutBack(
			float start, float end, float target, float limit, float overshoot)
		{
			return MathHelper.Lerp(start, end, amountOutBack(target, limit, overshoot));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInOutBack(float start, float end, float target, float limit)
		{
			return lerpInOutBack(start, end, target, limit, 1.70158f);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInOutBack(
			float start, float end, float target, float limit, float overshoot)
		{
			float hc = (end - start) * 0.5f;
			return ((target /= limit * 0.5f) < 1) ?
				hc * (target * target * (((overshoot *= 1.525f) + 1) * target - overshoot)) + start :
				hc * ((target -= 2) * target * (((overshoot *= 1.525f) + 1) * target + overshoot) + 2) * start;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutInBack(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return (target < limit * 0.5f) ?
				lerpOutBack(start, start + hc, target * 2, limit) :
				lerpInBack(start + hc, end, target * 2 - limit, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutInBack(
			float start, float end, float target, float limit, float overshoot)
		{
			float hc = (end - start) * 0.5f;
			return (target < limit * 0.5f) ?
				lerpOutBack(start, start + hc, target * 2, limit, overshoot) :
				lerpInBack(start + hc, end, target * 2 - limit, limit, overshoot);
		}

		#endregion
		#region bounce

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInBounce(float start, float end, float target, float limit)
		{
			float c = end - start;
			return c - lerpOutBounce(0, c, limit - target, limit) + start;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutBounce(float start, float end, float target, float limit)
		{
			return MathHelper.Lerp(start, end, amountOutBounce(target, limit));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpInOutBounce(float start, float end, float target, float limit)
		{
			float c = end - start;
			return (target < limit * 0.5f) ?
				lerpInBounce(0, c, target * 2, limit) * 0.5f + start :
				lerpOutBounce(0, c, target * 2 - limit, limit) * 0.5f + c * 0.5f + start;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2つの値の間を線形補間します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpOutInBounce(float start, float end, float target, float limit)
		{
			float hc = (end - start) * 0.5f;
			return (target < limit * 0.5f) ?
				lerpOutBounce(start, start + hc, target * 2, limit) :
				lerpInBounce(start + hc, end, target * 2 - limit, limit);
		}

		#endregion

		#endregion
		#region lerpClamp

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="expr1">ソース値。</param>
		/// <param name="expr2">ソース値。</param>
		/// <param name="amount">
		/// <paramref name="expr2"/>の重みを示す<c>0.0</c>から<c>1.0</c>までの値。
		/// </param>
		/// <returns>補完された値。</returns>
		public static float lerpClamp(float expr1, float expr2, float amount)
		{
			return MathHelper.Lerp(expr1, expr2, MathHelper.Clamp(amount, 0, 1));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampLinear(float start, float end, float target, float limit)
		{
			return lerpClamp(start, end, amountLinear(target, limit));
		}

		#region quad

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInQuad(float start, float end, float target, float limit)
		{
			return lerpInQuad(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutQuad(float start, float end, float target, float limit)
		{
			return lerpOutQuad(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInOutQuad(float start, float end, float target, float limit)
		{
			return lerpInOutQuad(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutInQuad(float start, float end, float target, float limit)
		{
			return lerpOutInQuad(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		#endregion
		#region cubic

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInCubic(float start, float end, float target, float limit)
		{
			return lerpInCubic(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutCubic(float start, float end, float target, float limit)
		{
			return lerpOutCubic(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInOutCubic(float start, float end, float target, float limit)
		{
			return lerpInOutCubic(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutInCubic(float start, float end, float target, float limit)
		{
			return lerpOutInCubic(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		#endregion
		#region quart

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInQuart(float start, float end, float target, float limit)
		{
			return lerpInQuart(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutQuart(float start, float end, float target, float limit)
		{
			return lerpOutQuart(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInOutQuart(float start, float end, float target, float limit)
		{
			return lerpInOutQuart(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutInQuart(float start, float end, float target, float limit)
		{
			return lerpOutInQuart(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		#endregion
		#region quint

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInQuint(float start, float end, float target, float limit)
		{
			return lerpInQuint(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutQuint(float start, float end, float target, float limit)
		{
			return lerpOutQuint(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInOutQuint(float start, float end, float target, float limit)
		{
			return lerpInOutQuint(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutInQuint(float start, float end, float target, float limit)
		{
			return lerpOutInQuint(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		#endregion
		#region sin

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInSin(float start, float end, float target, float limit)
		{
			return lerpInSin(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutSin(float start, float end, float target, float limit)
		{
			return lerpOutSin(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInOutSin(float start, float end, float target, float limit)
		{
			return lerpInOutSin(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutInSin(float start, float end, float target, float limit)
		{
			return lerpOutInSin(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		#endregion
		#region expo

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInExpo(float start, float end, float target, float limit)
		{
			return lerpInExpo(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutExpo(float start, float end, float target, float limit)
		{
			return lerpOutExpo(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInOutExpo(float start, float end, float target, float limit)
		{
			return lerpInOutExpo(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutInExpo(float start, float end, float target, float limit)
		{
			return lerpOutInExpo(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		#endregion
		#region circ

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInCirc(float start, float end, float target, float limit)
		{
			return lerpInCirc(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutCirc(float start, float end, float target, float limit)
		{
			return lerpOutCirc(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInOutCirc(float start, float end, float target, float limit)
		{
			return lerpInOutCirc(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutInCirc(float start, float end, float target, float limit)
		{
			return lerpOutInCirc(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		#endregion
		#region elastic

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInElastic(float start, float end, float target, float limit)
		{
			return lerpInElastic(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="amp">振り切る大きさ。</param>
		/// <param name="period">周期。</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInElastic(
			float start, float end, float target, float limit, float amp, float period)
		{
			return lerpInElastic(start, end, MathHelper.Clamp(target, 0, 1), limit, amp, period);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutElastic(float start, float end, float target, float limit)
		{
			return lerpOutElastic(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="amp">振り切る大きさ。</param>
		/// <param name="period">周期。</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutElastic(
			float start, float end, float target, float limit, float amp, float period)
		{
			return lerpOutElastic(start, end, MathHelper.Clamp(target, 0, 1), limit, amp, period);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInOutElastic(
			float start, float end, float target, float limit)
		{
			return lerpInOutElastic(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="amp">振り切る大きさ。</param>
		/// <param name="period">周期。</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInOutElastic(
			float start, float end, float target, float limit, float amp, float period)
		{
			return lerpInOutElastic(
				start, end, MathHelper.Clamp(target, 0, 1), limit, amp, period);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutInElastic(
			float start, float end, float target, float limit)
		{
			return lerpOutInElastic(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="amp">振り切る大きさ。</param>
		/// <param name="period">周期。</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutInElastic(
			float start, float end, float target, float limit, float amp, float period)
		{
			return lerpOutInElastic(
				start, end, MathHelper.Clamp(target, 0, 1), limit, amp, period);
		}

		#endregion
		#region back

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInBack(float start, float end, float target, float limit)
		{
			return lerpInBack(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInBack(
			float start, float end, float target, float limit, float overshoot)
		{
			return lerpInBack(start, end, MathHelper.Clamp(target, 0, 1), limit, overshoot);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutBack(float start, float end, float target, float limit)
		{
			return lerpOutBack(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutBack(
			float start, float end, float target, float limit, float overshoot)
		{
			return lerpOutBack(start, end, MathHelper.Clamp(target, 0, 1), limit, overshoot);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInOutBack(float start, float end, float target, float limit)
		{
			return lerpInOutBack(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInOutBack(
			float start, float end, float target, float limit, float overshoot)
		{
			return lerpInOutBack(start, end, MathHelper.Clamp(target, 0, 1), limit, overshoot);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutInBack(float start, float end, float target, float limit)
		{
			return lerpOutInBack(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutInBack(
			float start, float end, float target, float limit, float overshoot)
		{
			return lerpOutInBack(start, end, MathHelper.Clamp(target, 0, 1), limit, overshoot);
		}

		#endregion
		#region bounce

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInBounce(float start, float end, float target, float limit)
		{
			return lerpOutInBounce(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutBounce(float start, float end, float target, float limit)
		{
			return lerpOutInBounce(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampInOutBounce(float start, float end, float target, float limit)
		{
			return lerpOutInBounce(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpClampOutInBounce(float start, float end, float target, float limit)
		{
			return lerpOutInBounce(start, end, MathHelper.Clamp(target, 0, 1), limit);
		}

		#endregion
		#endregion
		#region lerpLoop

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="expr1">ソース値。</param>
		/// <param name="expr2">ソース値。</param>
		/// <param name="amount">
		/// <paramref name="expr2"/>の重みを示す<c>0.0</c>から<c>1.0</c>までの値。
		/// </param>
		/// <returns>補完された値。</returns>
		public static float lerpLoop(float expr1, float expr2, float amount)
		{
			return MathHelper.Lerp(expr1, expr2, CMisc.clampLoop(amount, 0, 1));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopLinear(float start, float end, float target, float limit)
		{
			return lerpLoop(start, end, amountLinear(target, limit));
		}

		#region quad

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInQuad(float start, float end, float target, float limit)
		{
			return lerpInQuad(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutQuad(float start, float end, float target, float limit)
		{
			return lerpOutQuad(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInOutQuad(float start, float end, float target, float limit)
		{
			return lerpInOutQuad(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutInQuad(float start, float end, float target, float limit)
		{
			return lerpOutInQuad(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		#endregion
		#region cubic

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInCubic(float start, float end, float target, float limit)
		{
			return lerpInCubic(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutCubic(float start, float end, float target, float limit)
		{
			return lerpOutCubic(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInOutCubic(float start, float end, float target, float limit)
		{
			return lerpInOutCubic(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutInCubic(float start, float end, float target, float limit)
		{
			return lerpOutInCubic(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		#endregion
		#region quart

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInQuart(float start, float end, float target, float limit)
		{
			return lerpInQuart(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutQuart(float start, float end, float target, float limit)
		{
			return lerpOutQuart(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInOutQuart(float start, float end, float target, float limit)
		{
			return lerpInOutQuart(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutInQuart(float start, float end, float target, float limit)
		{
			return lerpOutInQuart(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		#endregion
		#region quint

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInQuint(float start, float end, float target, float limit)
		{
			return lerpInQuint(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutQuint(float start, float end, float target, float limit)
		{
			return lerpOutQuint(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInOutQuint(float start, float end, float target, float limit)
		{
			return lerpInOutQuint(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutInQuint(float start, float end, float target, float limit)
		{
			return lerpOutInQuint(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		#endregion
		#region sin

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInSin(float start, float end, float target, float limit)
		{
			return lerpInSin(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutSin(float start, float end, float target, float limit)
		{
			return lerpOutSin(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInOutSin(float start, float end, float target, float limit)
		{
			return lerpInOutSin(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutInSin(float start, float end, float target, float limit)
		{
			return lerpOutInSin(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		#endregion
		#region expo

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInExpo(float start, float end, float target, float limit)
		{
			return lerpInExpo(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutExpo(float start, float end, float target, float limit)
		{
			return lerpOutExpo(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInOutExpo(float start, float end, float target, float limit)
		{
			return lerpInOutExpo(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutInExpo(float start, float end, float target, float limit)
		{
			return lerpOutInExpo(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		#endregion
		#region circ

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInCirc(float start, float end, float target, float limit)
		{
			return lerpInCirc(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutCirc(float start, float end, float target, float limit)
		{
			return lerpOutCirc(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInOutCirc(float start, float end, float target, float limit)
		{
			return lerpInOutCirc(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutInCirc(float start, float end, float target, float limit)
		{
			return lerpOutInCirc(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		#endregion
		#region elastic

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInElastic(float start, float end, float target, float limit)
		{
			return lerpInElastic(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="amp">振り切る大きさ。</param>
		/// <param name="period">周期。</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInElastic(
			float start, float end, float target, float limit, float amp, float period)
		{
			return lerpInElastic(start, end, CMisc.clampLoop(target, 0, 1), limit, amp, period);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutElastic(float start, float end, float target, float limit)
		{
			return lerpOutElastic(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="amp">振り切る大きさ。</param>
		/// <param name="period">周期。</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutElastic(
			float start, float end, float target, float limit, float amp, float period)
		{
			return lerpOutElastic(start, end, CMisc.clampLoop(target, 0, 1), limit, amp, period);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInOutElastic(float start, float end, float target, float limit)
		{
			return lerpInOutElastic(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="amp">振り切る大きさ。</param>
		/// <param name="period">周期。</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInOutElastic(
			float start, float end, float target, float limit, float amp, float period)
		{
			return lerpInOutElastic(
				start, end, CMisc.clampLoop(target, 0, 1), limit, amp, period);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutInElastic(float start, float end, float target, float limit)
		{
			return lerpOutInElastic(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="amp">振り切る大きさ。</param>
		/// <param name="period">周期。</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutInElastic(
			float start, float end, float target, float limit, float amp, float period)
		{
			return lerpOutInElastic(
				start, end, CMisc.clampLoop(target, 0, 1), limit, amp, period);
		}

		#endregion
		#region back

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInBack(float start, float end, float target, float limit)
		{
			return lerpInBack(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInBack(
			float start, float end, float target, float limit, float overshoot)
		{
			return lerpInBack(start, end, CMisc.clampLoop(target, 0, 1), limit, overshoot);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutBack(float start, float end, float target, float limit)
		{
			return lerpOutBack(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutBack(
			float start, float end, float target, float limit, float overshoot)
		{
			return lerpOutBack(start, end, CMisc.clampLoop(target, 0, 1), limit, overshoot);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInOutBack(float start, float end, float target, float limit)
		{
			return lerpInOutBack(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInOutBack(
			float start, float end, float target, float limit, float overshoot)
		{
			return lerpInOutBack(start, end, CMisc.clampLoop(target, 0, 1), limit, overshoot);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutInBack(float start, float end, float target, float limit)
		{
			return lerpOutInBack(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <param name="overshoot">
		/// <c>1.0</c>を突破する量(<c>1.70158</c>で10%突破します)。
		/// </param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutInBack(
			float start, float end, float target, float limit, float overshoot)
		{
			return lerpOutInBack(start, end, CMisc.clampLoop(target, 0, 1), limit, overshoot);
		}

		#endregion
		#region bounce

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInBounce(float start, float end, float target, float limit)
		{
			return lerpOutInBounce(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutBounce(float start, float end, float target, float limit)
		{
			return lerpOutInBounce(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopInOutBounce(float start, float end, float target, float limit)
		{
			return lerpOutInBounce(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="amount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内にループして丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="target"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		public static float lerpLoopOutInBounce(float start, float end, float target, float limit)
		{
			return lerpOutInBounce(start, end, CMisc.clampLoop(target, 0, 1), limit);
		}

		#endregion
		#endregion
	}
}
