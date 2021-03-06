﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
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
		loopInSin,

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

		/// <summary>範囲丸め込み付きの等速線形補完。</summary>
		[Obsolete("この機能は今後サポートされません。clampLinearを使用してください。")]
		clampSmooth,

		/// <summary>範囲丸め込み付きの減速線形補完。</summary>
		[Obsolete("この機能は今後サポートされません。clampOutQuadを使用してください。")]
		clampSlowdown,

		/// <summary>範囲丸め込み付きの加速線形補完。</summary>
		[Obsolete("この機能は今後サポートされません。clampInQuadを使用してください。")]
		clampAccelerate,

		/// <summary>範囲丸め込み付きの加速→減速線形補完。</summary>
		[Obsolete("この機能は今後サポートされません。clampInOutQuadを使用してください。")]
		clampSlowFastSlow,

		/// <summary>範囲丸め込み付きの減速→加速線形補完。</summary>
		[Obsolete("この機能は今後サポートされません。clampOutInQuadを使用してください。")]
		clampFastSlowFast,

		/// <summary>範囲ループ付きの等速線形補完。</summary>
		[Obsolete("この機能は今後サポートされません。loopLinearを使用してください。")]
		loopSmooth,

		/// <summary>範囲ループ付きの減速線形補完。</summary>
		[Obsolete("この機能は今後サポートされません。loopOutQuadを使用してください。")]
		loopSlowdown,

		/// <summary>範囲ループ付きの加速線形補完。</summary>
		[Obsolete("この機能は今後サポートされません。loopInQuadを使用してください。")]
		loopAccelerate,

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
		private static Func<float, float, float>[] amountList =
			new Func<float, float, float>[(int)EInterpolate.__reserved];

		/// <summary>線形補完一覧。</summary>
		private static Func<float, float, float, float, float>[] interpolateList =
			new Func<float, float, float, float, float>[(int)EInterpolate.__reserved];

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static EInterpolateExtention()
		{
#pragma warning disable 618
			amountList[(int)EInterpolate.clampSmooth] = CInterpolate.amountLinearClamp;
			amountList[(int)EInterpolate.clampSlowdown] = CInterpolate.amountOutQuadClamp;
			amountList[(int)EInterpolate.clampAccelerate] = CInterpolate.amountInQuadClamp;
			amountList[(int)EInterpolate.loopSmooth] = CInterpolate.amountLinearLoop;
			amountList[(int)EInterpolate.loopSlowdown] = CInterpolate.amountOutQuadLoop;
			amountList[(int)EInterpolate.loopAccelerate] = CInterpolate.amountInQuadLoop;
			interpolateList[(int)EInterpolate.clampSmooth] = CInterpolate.lerpClampLinear;
			interpolateList[(int)EInterpolate.clampSlowdown] = CInterpolate.lerpClampOutQuad;
			interpolateList[(int)EInterpolate.clampAccelerate] = CInterpolate.lerpClampInQuad;
			interpolateList[(int)EInterpolate.clampSlowFastSlow] = CInterpolate.lerpClampInOutQuad;
			interpolateList[(int)EInterpolate.clampFastSlowFast] = CInterpolate.lerpClampOutInQuad;
			interpolateList[(int)EInterpolate.loopSmooth] = CInterpolate.lerpLoopLinear;
			interpolateList[(int)EInterpolate.loopSlowdown] = CInterpolate.lerpLoopOutQuad;
			interpolateList[(int)EInterpolate.loopAccelerate] = CInterpolate.lerpLoopInQuad;
#pragma warning restore 618

			amountList[(int)EInterpolate.linear] = CInterpolate.amountLinear;
			amountList[(int)EInterpolate.inQuad] = CInterpolate.amountInQuad;
			amountList[(int)EInterpolate.outQuad] = CInterpolate.amountOutQuad;
			amountList[(int)EInterpolate.inCubic] = CInterpolate.amountInCubic;
			amountList[(int)EInterpolate.outCubic] = CInterpolate.amountOutCubic;
			amountList[(int)EInterpolate.inQuart] = CInterpolate.amountInQuart;
			amountList[(int)EInterpolate.outQuart] = CInterpolate.amountOutQuart;
			amountList[(int)EInterpolate.inQuint] = CInterpolate.amountInQuint;
			amountList[(int)EInterpolate.outQuint] = CInterpolate.amountOutQuint;
			amountList[(int)EInterpolate.outSin] = CInterpolate.amountOutSin;
			amountList[(int)EInterpolate.inOutSin] = CInterpolate.amountInOutSin;
			amountList[(int)EInterpolate.inCirc] = CInterpolate.amountInCirc;
			amountList[(int)EInterpolate.outCirc] = CInterpolate.amountOutCirc;
			amountList[(int)EInterpolate.inBack] = CInterpolate.amountInBack;
			amountList[(int)EInterpolate.outBack] = CInterpolate.amountOutBack;
			amountList[(int)EInterpolate.outBounce] = CInterpolate.amountOutBounce;
			amountList[(int)EInterpolate.clampLinear] = CInterpolate.amountLinearClamp;
			amountList[(int)EInterpolate.clampInQuad] = CInterpolate.amountInQuadClamp;
			amountList[(int)EInterpolate.clampOutQuad] = CInterpolate.amountOutQuadClamp;
			amountList[(int)EInterpolate.clampInCubic] = CInterpolate.amountInCubicClamp;
			amountList[(int)EInterpolate.clampOutCubic] = CInterpolate.amountOutCubicClamp;
			amountList[(int)EInterpolate.clampInQuart] = CInterpolate.amountInQuartClamp;
			amountList[(int)EInterpolate.clampOutQuart] = CInterpolate.amountOutQuartClamp;
			amountList[(int)EInterpolate.clampInQuint] = CInterpolate.amountInQuintClamp;
			amountList[(int)EInterpolate.clampOutQuint] = CInterpolate.amountOutQuintClamp;
			amountList[(int)EInterpolate.clampOutSin] = CInterpolate.amountOutSinClamp;
			amountList[(int)EInterpolate.clampInOutSin] = CInterpolate.amountInOutSinClamp;
			amountList[(int)EInterpolate.clampInCirc] = CInterpolate.amountInCircClamp;
			amountList[(int)EInterpolate.clampOutCirc] = CInterpolate.amountOutCircClamp;
			amountList[(int)EInterpolate.clampInBack] = CInterpolate.amountInBackClamp;
			amountList[(int)EInterpolate.clampOutBack] = CInterpolate.amountOutBackClamp;
			amountList[(int)EInterpolate.clampOutBounce] = CInterpolate.amountOutBounceClamp;
			amountList[(int)EInterpolate.loopLinear] = CInterpolate.amountLinearLoop;
			amountList[(int)EInterpolate.loopInQuad] = CInterpolate.amountInQuadLoop;
			amountList[(int)EInterpolate.loopOutQuad] = CInterpolate.amountOutQuadLoop;
			amountList[(int)EInterpolate.loopInCubic] = CInterpolate.amountInCubicLoop;
			amountList[(int)EInterpolate.loopOutCubic] = CInterpolate.amountOutCubicLoop;
			amountList[(int)EInterpolate.loopInQuart] = CInterpolate.amountInQuartLoop;
			amountList[(int)EInterpolate.loopOutQuart] = CInterpolate.amountOutQuartLoop;
			amountList[(int)EInterpolate.loopInQuint] = CInterpolate.amountInQuintLoop;
			amountList[(int)EInterpolate.loopOutQuint] = CInterpolate.amountOutQuintLoop;
			amountList[(int)EInterpolate.loopOutSin] = CInterpolate.amountOutSinLoop;
			amountList[(int)EInterpolate.loopInOutSin] = CInterpolate.amountInOutSinLoop;
			amountList[(int)EInterpolate.loopInCirc] = CInterpolate.amountInCircLoop;
			amountList[(int)EInterpolate.loopOutCirc] = CInterpolate.amountOutCircLoop;
			amountList[(int)EInterpolate.loopInBack] = CInterpolate.amountInBackLoop;
			amountList[(int)EInterpolate.loopOutBack] = CInterpolate.amountOutBackLoop;

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
			interpolateList[(int)EInterpolate.loopLinear] = CInterpolate.lerpLoopLinear;
			interpolateList[(int)EInterpolate.loopInQuad] = CInterpolate.lerpLoopInQuad;
			interpolateList[(int)EInterpolate.loopOutQuad] = CInterpolate.lerpLoopOutQuad;
			interpolateList[(int)EInterpolate.loopInOutQuad] = CInterpolate.lerpLoopInOutQuad;
			interpolateList[(int)EInterpolate.loopOutInQuad] = CInterpolate.lerpLoopOutInQuad;
			interpolateList[(int)EInterpolate.loopInCubic] = CInterpolate.lerpLoopInCubic;
			interpolateList[(int)EInterpolate.loopOutCubic] = CInterpolate.lerpLoopOutCubic;
			interpolateList[(int)EInterpolate.loopInOutCubic] = CInterpolate.lerpLoopInOutCubic;
			interpolateList[(int)EInterpolate.loopOutInCubic] = CInterpolate.lerpLoopOutInCubic;
			interpolateList[(int)EInterpolate.loopInQuart] = CInterpolate.lerpLoopInQuart;
			interpolateList[(int)EInterpolate.loopOutQuart] = CInterpolate.lerpLoopOutQuart;
			interpolateList[(int)EInterpolate.loopInOutQuart] = CInterpolate.lerpLoopInOutQuart;
			interpolateList[(int)EInterpolate.loopOutInQuart] = CInterpolate.lerpLoopOutInQuart;
			interpolateList[(int)EInterpolate.loopInQuint] = CInterpolate.lerpLoopInQuint;
			interpolateList[(int)EInterpolate.loopOutQuint] = CInterpolate.lerpLoopOutQuint;
			interpolateList[(int)EInterpolate.loopInOutQuint] = CInterpolate.lerpLoopInOutQuint;
			interpolateList[(int)EInterpolate.loopOutInQuint] = CInterpolate.lerpLoopOutInQuint;
			interpolateList[(int)EInterpolate.loopInSin] = CInterpolate.lerpLoopInSin;
			interpolateList[(int)EInterpolate.loopOutSin] = CInterpolate.lerpLoopOutSin;
			interpolateList[(int)EInterpolate.loopInOutSin] = CInterpolate.lerpLoopInOutSin;
			interpolateList[(int)EInterpolate.loopOutInSin] = CInterpolate.lerpLoopOutInSin;
			interpolateList[(int)EInterpolate.loopInExpo] = CInterpolate.lerpLoopInExpo;
			interpolateList[(int)EInterpolate.loopOutExpo] = CInterpolate.lerpLoopOutExpo;
			interpolateList[(int)EInterpolate.loopInOutExpo] = CInterpolate.lerpLoopInOutExpo;
			interpolateList[(int)EInterpolate.loopOutInExpo] = CInterpolate.lerpLoopOutInExpo;
			interpolateList[(int)EInterpolate.loopInCirc] = CInterpolate.lerpLoopInCirc;
			interpolateList[(int)EInterpolate.loopOutCirc] = CInterpolate.lerpLoopOutCirc;
			interpolateList[(int)EInterpolate.loopInOutCirc] = CInterpolate.lerpLoopInOutCirc;
			interpolateList[(int)EInterpolate.loopOutInCirc] = CInterpolate.lerpLoopOutInCirc;
			interpolateList[(int)EInterpolate.loopInElastic] = CInterpolate.lerpLoopInElastic;
			interpolateList[(int)EInterpolate.loopOutElastic] = CInterpolate.lerpLoopOutElastic;
			interpolateList[(int)EInterpolate.loopInOutElastic] = CInterpolate.lerpLoopInOutElastic;
			interpolateList[(int)EInterpolate.loopOutInElastic] = CInterpolate.lerpLoopOutInElastic;
			interpolateList[(int)EInterpolate.loopInBack] = CInterpolate.lerpLoopInBack;
			interpolateList[(int)EInterpolate.loopOutBack] = CInterpolate.lerpLoopOutBack;
			interpolateList[(int)EInterpolate.loopInOutBack] = CInterpolate.lerpLoopInOutBack;
			interpolateList[(int)EInterpolate.loopOutInBack] = CInterpolate.lerpLoopOutInBack;
			interpolateList[(int)EInterpolate.loopInBounce] = CInterpolate.lerpLoopInBounce;
			interpolateList[(int)EInterpolate.loopOutBounce] = CInterpolate.lerpLoopOutBounce;
			interpolateList[(int)EInterpolate.loopInOutBounce] = CInterpolate.lerpLoopInOutBounce;
			interpolateList[(int)EInterpolate.loopOutInBounce] = CInterpolate.lerpLoopOutInBounce;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>線形補完列挙体に対応する関数を取得します。</summary>
		/// <remarks>
		/// 下記の列挙体に対してはこの関数に対応していません。(<c>null</c>が戻ります)
		/// </remarks>
		/// 
		/// <param name="interpolate">線形補完列挙体。</param>
		/// <returns>対応する関数へのデリゲート。</returns>
		/// <exception cref="System.IndexOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static Func<float, float, float> getAmountFunction(
			this EInterpolate interpolate)
		{
			return amountList[(int)interpolate];
		}

		//* -----------------------------------------------------------------------*
		/// <summary>線形補完列挙体に対応する関数を取得します。</summary>
		/// 
		/// <param name="interpolate">線形補完列挙体。</param>
		/// <returns>対応する関数へのデリゲート。</returns>
		/// <exception cref="System.IndexOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static Func<float, float, float, float, float> getLerpFunction(
			this EInterpolate interpolate)
		{
			return interpolateList[(int)interpolate];
		}

		//* -----------------------------------------------------------------------*
		/// <summary>線形補完を計算します。</summary>
		/// 
		/// <param name="interpolate">線形補完列挙体。</param>
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end"><paramref name="target"/>が<paramref name="limit"/>と等しい場合の値</param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="target"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		/// <exception cref="System.IndexOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static float interpolate(
			this EInterpolate interpolate, float start, float end, float target, float limit)
		{
			return interpolate.getLerpFunction()(start, end, target, limit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>線形補完の比較ログを生成します。</summary>
		/// 
		/// <param name="start"><paramref name="target"/>が0と等しい場合の値</param>
		/// <param name="end"><paramref name="target"/>が<paramref name="limit"/>と等しい場合の値</param>
		/// <param name="target">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>各種線形補完の比較用文字列。</returns>
		public static string interpolate(float start, float end, float target, float limit)
		{
			string result = string.Format("[{0}/{1}]", target, limit);
			for (int i = (int)EInterpolate.__reserved; --i >= 0; )
			{
				result += string.Format("{0:F1}\t", interpolateList[i](start, end, target, limit));
			}
			return result;
		}
	}
}
