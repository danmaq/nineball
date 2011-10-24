////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.nineball.util.math
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>線形補完の列挙体。</summary>
	public enum EInterpolate
	{

		/// <summary>範囲丸め込み付きの等速線形補完。</summary>
		clampSmooth,

		/// <summary>範囲丸め込み付きの減速線形補完。</summary>
		clampSlowdown,

		/// <summary>範囲丸め込み付きの加速線形補完。</summary>
		clampAccelerate,

		/// <summary>範囲丸め込み付きの加速→減速線形補完。</summary>
		clampSlowFastSlow,

		/// <summary>範囲丸め込み付きの減速→加速線形補完。</summary>
		clampFastSlowFast,

		/// <summary>範囲ループ付きの等速線形補完。</summary>
		loopSmooth,

		/// <summary>範囲ループ付きの減速線形補完。</summary>
		loopSlowdown,

		/// <summary>範囲ループ付きの加速線形補完。</summary>
		loopAccelerate,

		/// <summary>等速線形補完。</summary>
		linear,

		/// <summary>加速線形補完。</summary>
		inQuad,

		/// <summary>減速線形補完。</summary>
		outQuad,

		/// <summary>加速→減速線形補完。</summary>
		inOutQuad,

		/// <summary>減速→加速線形補完。</summary>
		outInQuad,

		/// <summary>加速線形補完。</summary>
		inCubic,

		/// <summary>減速線形補完。</summary>
		outCubic,

		/// <summary>加速→減速線形補完。</summary>
		inOutCubic,

		/// <summary>減速→加速線形補完。</summary>
		outInCubic,

		/// <summary>加速線形補完。</summary>
		inQuart,

		/// <summary>減速線形補完。</summary>
		outQuart,

		/// <summary>加速→減速線形補完。</summary>
		inOutQuart,

		/// <summary>減速→加速線形補完。</summary>
		outInQuart,

		/// <summary>加速線形補完。</summary>
		inQuint,

		/// <summary>減速線形補完。</summary>
		outQuint,

		/// <summary>加速→減速線形補完。</summary>
		inOutQuint,

		/// <summary>減速→加速線形補完。</summary>
		outInQuint,

		/// <summary>加速線形補完。</summary>
		inSin,

		/// <summary>減速線形補完。</summary>
		outSin,

		/// <summary>加速→減速線形補完。</summary>
		inOutSin,

		/// <summary>減速→加速線形補完。</summary>
		outInSin,

		/// <summary>加速線形補完。</summary>
		inExpo,

		/// <summary>減速線形補完。</summary>
		outExpo,

		/// <summary>加速→減速線形補完。</summary>
		inOutExpo,

		/// <summary>減速→加速線形補完。</summary>
		outInExpo,

		/// <summary>加速線形補完。</summary>
		inCirc,

		/// <summary>減速線形補完。</summary>
		outCirc,

		/// <summary>加速→減速線形補完。</summary>
		inOutCirc,

		/// <summary>減速→加速線形補完。</summary>
		outInCirc,

		/// <summary>加速線形補完。</summary>
		inElastic,

		/// <summary>減速線形補完。</summary>
		outElastic,

		/// <summary>加速→減速線形補完。</summary>
		inOutElastic,

		/// <summary>減速→加速線形補完。</summary>
		outInElastic,

		/// <summary>加速線形補完。</summary>
		inBack,

		/// <summary>減速線形補完。</summary>
		outBack,

		/// <summary>加速→減速線形補完。</summary>
		inOutBack,

		/// <summary>減速→加速線形補完。</summary>
		outInBack,

		/// <summary>加速線形補完。</summary>
		inBounce,

		/// <summary>減速線形補完。</summary>
		outBounce,

		/// <summary>加速→減速線形補完。</summary>
		inOutBounce,

		/// <summary>減速→加速線形補完。</summary>
		outInBounce,

		/// <summary>等速線形補完。</summary>
		clampLinear,

		/// <summary>加速線形補完。</summary>
		clampInQuad,

		/// <summary>減速線形補完。</summary>
		clampOutQuad,

		/// <summary>加速→減速線形補完。</summary>
		clampInOutQuad,

		/// <summary>減速→加速線形補完。</summary>
		clampOutInQuad,

		/// <summary>加速線形補完。</summary>
		clampInCubic,

		/// <summary>減速線形補完。</summary>
		clampOutCubic,

		/// <summary>加速→減速線形補完。</summary>
		clampInOutCubic,

		/// <summary>減速→加速線形補完。</summary>
		clampOutInCubic,

		/// <summary>加速線形補完。</summary>
		clampInQuart,

		/// <summary>減速線形補完。</summary>
		clampOutQuart,

		/// <summary>加速→減速線形補完。</summary>
		clampInOutQuart,

		/// <summary>減速→加速線形補完。</summary>
		clampOutInQuart,

		/// <summary>加速線形補完。</summary>
		clampInQuint,

		/// <summary>減速線形補完。</summary>
		clampOutQuint,

		/// <summary>加速→減速線形補完。</summary>
		clampInOutQuint,

		/// <summary>減速→加速線形補完。</summary>
		clampOutInQuint,

		/// <summary>加速線形補完。</summary>
		clampInSin,

		/// <summary>減速線形補完。</summary>
		clampOutSin,

		/// <summary>加速→減速線形補完。</summary>
		clampInOutSin,

		/// <summary>減速→加速線形補完。</summary>
		clampOutInSin,

		/// <summary>加速線形補完。</summary>
		clampInExpo,

		/// <summary>減速線形補完。</summary>
		clampOutExpo,

		/// <summary>加速→減速線形補完。</summary>
		clampInOutExpo,

		/// <summary>減速→加速線形補完。</summary>
		clampOutInExpo,

		/// <summary>加速線形補完。</summary>
		clampInCirc,

		/// <summary>減速線形補完。</summary>
		clampOutCirc,

		/// <summary>加速→減速線形補完。</summary>
		clampInOutCirc,

		/// <summary>減速→加速線形補完。</summary>
		clampOutInCirc,

		/// <summary>加速線形補完。</summary>
		clampInElastic,

		/// <summary>減速線形補完。</summary>
		clampOutElastic,

		/// <summary>加速→減速線形補完。</summary>
		clampInOutElastic,

		/// <summary>減速→加速線形補完。</summary>
		clampOutInElastic,

		/// <summary>加速線形補完。</summary>
		clampInBack,

		/// <summary>減速線形補完。</summary>
		clampOutBack,

		/// <summary>加速→減速線形補完。</summary>
		clampInOutBack,

		/// <summary>減速→加速線形補完。</summary>
		clampOutInBack,

		/// <summary>加速線形補完。</summary>
		clampInBounce,

		/// <summary>減速線形補完。</summary>
		clampOutBounce,

		/// <summary>加速→減速線形補完。</summary>
		clampInOutBounce,

		/// <summary>減速→加速線形補完。</summary>
		clampOutInBounce,

		/// <summary>等速線形補完。</summary>
		loopLinear,

		/// <summary>加速線形補完。</summary>
		loopInQuad,

		/// <summary>減速線形補完。</summary>
		loopOutQuad,

		/// <summary>加速→減速線形補完。</summary>
		loopInOutQuad,

		/// <summary>減速→加速線形補完。</summary>
		loopOutInQuad,

		/// <summary>加速線形補完。</summary>
		loopInCubic,

		/// <summary>減速線形補完。</summary>
		loopOutCubic,

		/// <summary>加速→減速線形補完。</summary>
		loopInOutCubic,

		/// <summary>減速→加速線形補完。</summary>
		loopOutInCubic,

		/// <summary>加速線形補完。</summary>
		loopInQuart,

		/// <summary>減速線形補完。</summary>
		loopOutQuart,

		/// <summary>加速→減速線形補完。</summary>
		loopInOutQuart,

		/// <summary>減速→加速線形補完。</summary>
		loopOutInQuart,

		/// <summary>加速線形補完。</summary>
		loopInQuint,

		/// <summary>減速線形補完。</summary>
		loopOutQuint,

		/// <summary>加速→減速線形補完。</summary>
		loopInOutQuint,

		/// <summary>減速→加速線形補完。</summary>
		loopOutInQuint,

		/// <summary>加速線形補完。</summary>
		loopIinSin,

		/// <summary>減速線形補完。</summary>
		loopOutSin,

		/// <summary>加速→減速線形補完。</summary>
		loopInOutSin,

		/// <summary>減速→加速線形補完。</summary>
		loopOutInSin,

		/// <summary>加速線形補完。</summary>
		loopInExpo,

		/// <summary>減速線形補完。</summary>
		loopOutExpo,

		/// <summary>加速→減速線形補完。</summary>
		loopInOutExpo,

		/// <summary>減速→加速線形補完。</summary>
		loopOutInExpo,

		/// <summary>加速線形補完。</summary>
		loopInCirc,

		/// <summary>減速線形補完。</summary>
		loopOutCirc,

		/// <summary>加速→減速線形補完。</summary>
		loopInOutCirc,

		/// <summary>減速→加速線形補完。</summary>
		loopOutInCirc,

		/// <summary>加速線形補完。</summary>
		loopInElastic,

		/// <summary>減速線形補完。</summary>
		loopOutElastic,

		/// <summary>加速→減速線形補完。</summary>
		loopInOutElastic,

		/// <summary>減速→加速線形補完。</summary>
		loopOutInElastic,

		/// <summary>加速線形補完。</summary>
		loopInBack,

		/// <summary>減速線形補完。</summary>
		loopOutBack,

		/// <summary>加速→減速線形補完。</summary>
		loopInOutBack,

		/// <summary>減速→加速線形補完。</summary>
		loopOutInBack,

		/// <summary>加速線形補完。</summary>
		loopInBounce,

		/// <summary>減速線形補完。</summary>
		loopOutBounce,

		/// <summary>加速→減速線形補完。</summary>
		loopInOutBounce,

		/// <summary>減速→加速線形補完。</summary>
		loopOutInBounce,

		/// <summary>予約(使用してはいけません)。</summary>
		__reserved,
	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>線形補完列挙体の拡張機能。</summary>
	public static class EInterpolateExtention
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>線形補完一覧。</summary>
		private static Func<float, float, float, float, float>[] interpolateList =
			new Func<float, float, float, float, float>[(int)EInterpolate.__reserved];

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static EInterpolateExtention()
		{
			interpolateList[(int)EInterpolate.clampSmooth] = CInterpolate._clampSmooth;
			interpolateList[(int)EInterpolate.clampSlowdown] = CInterpolate._clampSlowdown;
			interpolateList[(int)EInterpolate.clampAccelerate] = CInterpolate._clampAccelerate;
			interpolateList[(int)EInterpolate.clampSlowFastSlow] = CInterpolate._clampSlowFastSlow;
			interpolateList[(int)EInterpolate.clampFastSlowFast] = CInterpolate._clampFastSlowFast;
			interpolateList[(int)EInterpolate.loopSmooth] = CInterpolate._loopSmooth;
			interpolateList[(int)EInterpolate.loopSlowdown] = CInterpolate._loopSlowdown;
			interpolateList[(int)EInterpolate.loopAccelerate] = CInterpolate._loopAccelerate;

			interpolateList[(int)EInterpolate.linear] = CInterpolate.lerpLinear;
			interpolateList[(int)EInterpolate.inQuad] = CInterpolate.lerpInQuad;
			interpolateList[(int)EInterpolate.outQuad] = CInterpolate.lerpOutQuad;
			interpolateList[(int)EInterpolate.inOutQuad] = CInterpolate.lerpInOutQuad;
			interpolateList[(int)EInterpolate.outInQuad] = CInterpolate.lerpOutInQuad;
			interpolateList[(int)EInterpolate.inCubic] = CInterpolate.lerpInCubic;
			interpolateList[(int)EInterpolate.outCubic] = CInterpolate.lerpOutCubic;
			interpolateList[(int)EInterpolate.inOutCubic] = CInterpolate.lerpInOutCubic;
			interpolateList[(int)EInterpolate.outInCubic] = CInterpolate.lerpOutInCubic;
			interpolateList[(int)EInterpolate.inQuart] = CInterpolate.lerpInQuart;
			interpolateList[(int)EInterpolate.outQuart] = CInterpolate.lerpOutQuart;
			interpolateList[(int)EInterpolate.inOutQuart] = CInterpolate.lerpInOutQuart;
			interpolateList[(int)EInterpolate.outInQuart] = CInterpolate.lerpOutInQuart;
			interpolateList[(int)EInterpolate.inQuint] = CInterpolate.lerpInQuint;
			interpolateList[(int)EInterpolate.outQuint] = CInterpolate.lerpOutQuint;
			interpolateList[(int)EInterpolate.inOutQuint] = CInterpolate.lerpInOutQuint;
			interpolateList[(int)EInterpolate.outInQuint] = CInterpolate.lerpOutInQuint;
			interpolateList[(int)EInterpolate.inSin] = CInterpolate.lerpInSin;
			interpolateList[(int)EInterpolate.outSin] = CInterpolate.lerpOutSin;
			interpolateList[(int)EInterpolate.inOutSin] = CInterpolate.lerpInOutSin;
			interpolateList[(int)EInterpolate.outInSin] = CInterpolate.lerpOutInSin;
			interpolateList[(int)EInterpolate.inExpo] = CInterpolate.lerpInExpo;
			interpolateList[(int)EInterpolate.outExpo] = CInterpolate.lerpOutExpo;
			interpolateList[(int)EInterpolate.inOutExpo] = CInterpolate.lerpInOutExpo;
			interpolateList[(int)EInterpolate.outInExpo] = CInterpolate.lerpOutInExpo;
			interpolateList[(int)EInterpolate.inCirc] = CInterpolate.lerpInCirc;
			interpolateList[(int)EInterpolate.outCirc] = CInterpolate.lerpOutCirc;
			interpolateList[(int)EInterpolate.inOutCirc] = CInterpolate.lerpInOutCirc;
			interpolateList[(int)EInterpolate.outInCirc] = CInterpolate.lerpOutInCirc;
			interpolateList[(int)EInterpolate.inElastic] = CInterpolate.lerpInElastic;
			interpolateList[(int)EInterpolate.outElastic] = CInterpolate.lerpOutElastic;
			interpolateList[(int)EInterpolate.inOutElastic] = CInterpolate.lerpInOutElastic;
			interpolateList[(int)EInterpolate.outInElastic] = CInterpolate.lerpOutInElastic;
			interpolateList[(int)EInterpolate.inBack] = CInterpolate.lerpInBack;
			interpolateList[(int)EInterpolate.outBack] = CInterpolate.lerpOutBack;
			interpolateList[(int)EInterpolate.inOutBack] = CInterpolate.lerpInOutBack;
			interpolateList[(int)EInterpolate.outInBack] = CInterpolate.lerpOutInBack;
			interpolateList[(int)EInterpolate.inBounce] = CInterpolate.lerpInBounce;
			interpolateList[(int)EInterpolate.outBounce] = CInterpolate.lerpOutBounce;
			interpolateList[(int)EInterpolate.inOutBounce] = CInterpolate.lerpInOutBounce;
			interpolateList[(int)EInterpolate.outInBounce] = CInterpolate.lerpOutInBounce;
			interpolateList[(int)EInterpolate.clampLinear] = CInterpolate.lerpClampLinear;
			interpolateList[(int)EInterpolate.clampInQuad] = CInterpolate.lerpClampInQuad;
			interpolateList[(int)EInterpolate.clampOutQuad] = CInterpolate.lerpClampOutQuad;
			interpolateList[(int)EInterpolate.clampInOutQuad] = CInterpolate.lerpClampInOutQuad;
			interpolateList[(int)EInterpolate.clampOutInQuad] = CInterpolate.lerpClampOutInQuad;
			interpolateList[(int)EInterpolate.clampInCubic] = CInterpolate.lerpClampInCubic;
			interpolateList[(int)EInterpolate.clampOutCubic] = CInterpolate.lerpClampOutCubic;
			interpolateList[(int)EInterpolate.clampInOutCubic] = CInterpolate.lerpClampInOutCubic;
			interpolateList[(int)EInterpolate.clampOutInCubic] = CInterpolate.lerpClampOutInCubic;
			interpolateList[(int)EInterpolate.clampInQuart] = CInterpolate.lerpClampInQuart;
			interpolateList[(int)EInterpolate.clampOutQuart] = CInterpolate.lerpClampOutQuart;
			interpolateList[(int)EInterpolate.clampInOutQuart] = CInterpolate.lerpClampInOutQuart;
			interpolateList[(int)EInterpolate.clampOutInQuart] = CInterpolate.lerpClampOutInQuart;
			interpolateList[(int)EInterpolate.clampInQuint] = CInterpolate.lerpClampInQuint;
			interpolateList[(int)EInterpolate.clampOutQuint] = CInterpolate.lerpClampOutQuint;
			interpolateList[(int)EInterpolate.clampInOutQuint] = CInterpolate.lerpClampInOutQuint;
			interpolateList[(int)EInterpolate.clampOutInQuint] = CInterpolate.lerpClampOutInQuint;
			interpolateList[(int)EInterpolate.clampInSin] = CInterpolate.lerpClampInSin;
			interpolateList[(int)EInterpolate.clampOutSin] = CInterpolate.lerpClampOutSin;
			interpolateList[(int)EInterpolate.clampInOutSin] = CInterpolate.lerpClampInOutSin;
			interpolateList[(int)EInterpolate.clampOutInSin] = CInterpolate.lerpClampOutInSin;
			interpolateList[(int)EInterpolate.clampInExpo] = CInterpolate.lerpClampInExpo;
			interpolateList[(int)EInterpolate.clampOutExpo] = CInterpolate.lerpClampOutExpo;
			interpolateList[(int)EInterpolate.clampInOutExpo] = CInterpolate.lerpClampInOutExpo;
			interpolateList[(int)EInterpolate.clampOutInExpo] = CInterpolate.lerpClampOutInExpo;
			interpolateList[(int)EInterpolate.clampInCirc] = CInterpolate.lerpClampInCirc;
			interpolateList[(int)EInterpolate.clampOutCirc] = CInterpolate.lerpClampOutCirc;
			interpolateList[(int)EInterpolate.clampInOutCirc] = CInterpolate.lerpClampInOutCirc;
			interpolateList[(int)EInterpolate.clampOutInCirc] = CInterpolate.lerpClampOutInCirc;
			interpolateList[(int)EInterpolate.clampInElastic] = CInterpolate.lerpClampInElastic;
			interpolateList[(int)EInterpolate.clampOutElastic] = CInterpolate.lerpClampOutElastic;
			interpolateList[(int)EInterpolate.clampInOutElastic] = CInterpolate.lerpClampInOutElastic;
			interpolateList[(int)EInterpolate.clampOutInElastic] = CInterpolate.lerpClampOutInElastic;
			interpolateList[(int)EInterpolate.clampInBack] = CInterpolate.lerpClampInBack;
			interpolateList[(int)EInterpolate.clampOutBack] = CInterpolate.lerpClampOutBack;
			interpolateList[(int)EInterpolate.clampInOutBack] = CInterpolate.lerpClampInOutBack;
			interpolateList[(int)EInterpolate.clampOutInBack] = CInterpolate.lerpClampOutInBack;
			interpolateList[(int)EInterpolate.clampInBounce] = CInterpolate.lerpClampInBounce;
			interpolateList[(int)EInterpolate.clampOutBounce] = CInterpolate.lerpClampOutBounce;
			interpolateList[(int)EInterpolate.clampInOutBounce] = CInterpolate.lerpClampInOutBounce;
			interpolateList[(int)EInterpolate.clampOutInBounce] = CInterpolate.lerpClampOutInBounce;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>線形補完列挙体に対応する関数を取得します。</summary>
		/// 
		/// <param name="interpolate">線形補完列挙体。</param>
		/// <returns>対応する関数へのデリゲート。</returns>
		/// <exception cref="System.IndexOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static Func<float, float, float, float, float> getFunction(
			this EInterpolate interpolate)
		{
			return interpolateList[(int)interpolate];
		}

		//* -----------------------------------------------------------------------*
		/// <summary>線形補完を計算します。</summary>
		/// 
		/// <param name="interpolate">線形補完列挙体。</param>
		/// <param name="start"><paramref name="now"/>が0と等しい場合の値</param>
		/// <param name="end"><paramref name="now"/>が<paramref name="limit"/>と等しい場合の値</param>
		/// <param name="now">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="now"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		/// <exception cref="System.IndexOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static float interpolate(
			this EInterpolate interpolate, float start, float end, float now, float limit)
		{
			return interpolate.getFunction()(start, end, now, limit);
		}
	}
}
