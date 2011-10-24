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
		lerpLinear,

		/// <summary>加速線形補完。</summary>
		lerpInQuad,

		/// <summary>減速線形補完。</summary>
		lerpOutQuad,

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
