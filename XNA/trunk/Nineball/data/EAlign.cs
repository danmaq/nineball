////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

namespace danmaq.nineball.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>位置揃え定義の列挙体。</summary>
	public enum EAlign
	{

		/// <summary>左端、または上端揃え</summary>
		LeftTop,

		/// <summary>中央揃え</summary>
		Center,

		/// <summary>右端、または下端揃え</summary>
		RightBottom,

		/// <summary>予約。(使用できません)</summary>
		__reserved
	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>位置揃え定義の列挙体の拡張機能。</summary>
	public static class EAlignExtension
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>原点を算出するための係数。</summary>
		private static readonly float[] coefficient = new float[(int)EAlign.__reserved];

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static EAlignExtension()
		{
			coefficient[(int)EAlign.LeftTop] = 0;
			coefficient[(int)EAlign.Center] = 0.5f;
			coefficient[(int)EAlign.RightBottom] = 1f;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>原点を計算します。</summary>
		/// 
		/// <param name="index">位置揃え定義の列挙体。</param>
		/// <param name="width">幅。</param>
		/// <returns>原点。</returns>
		public static float origin(this EAlign index, float width)
		{
			return coefficient[(int)index] * width;
		}
	}
}
