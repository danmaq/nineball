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

namespace danmaq.nineball.util.math
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>内分カウンタ機能の関数集クラス。</summary>
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
			CMisc.clampLoop(MathHelper.Clamp(target, 0, limit), 0, limit) / limit;

		/// <summary>加速変化ループする内分カウンタ。</summary>
		public static readonly Func<float, float, float> _amountLoopSlowdown = (target, limit) =>
			1 - (float)Math.Pow(1 - CMisc.clampLoop(target, 0, limit) / limit, 2);

		/// <summary>減速変化ループする内分カウンタ。</summary>
		public static readonly Func<float, float, float> _amountLoopAccelerate = (target, limit) =>
			(float)Math.Pow(CMisc.clampLoop(target, 0, limit) / limit, 2);

		/// <summary>範囲丸め込み付きの線形補完。</summary>
		public static readonly Func<float, float, float, float> _clampLerp =
			(start, end, fAmount) => MathHelper.Lerp(start, end, MathHelper.Clamp(fAmount, 0, 1));

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
		/// <summary>
		/// <para>2つの値の間を線形補間します。</para>
		/// <para>
		/// <paramref name="fAmount"/>が<c>0.0</c>から<c>1.0</c>
		/// までの範囲を超過した場合、範囲内に丸め込まれます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="expr1">ソース値。</param>
		/// <param name="expr2">ソース値。</param>
		/// <param name="fAmount">
		/// <paramref name="expr2"/>の重みを示す<c>0.0</c>から<c>1.0</c>までの値。
		/// </param>
		/// <returns>補完された値。</returns>
		public static float clampLerp(float expr1, float expr2, float fAmount)
		{
			return _clampLerp(expr1, expr2, fAmount);
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
		public static float amountOutInQuad(float target, float limit)
		{
			return target < limit * 0.5f ?
				amountOutQuad(target * 2, limit) : amountInQuad(target * 2 - limit, limit);
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
	}
}
