////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.util.math;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>グラデーション用情報を格納する構造体。</summary>
	[Serializable]
	public struct SGradation
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>初期値。</summary>
		public float start;

		/// <summary>最終値。</summary>
		public float end;

		/// <summary>限界値1。</summary>
		public float limit1;

		/// <summary>限界値2。</summary>
		public float limit2;

		/// <summary>内分カウンタ。</summary>
		public EInterpolate interpolate;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>
		/// すべての項目に同一の値を設定します。(グラデーションしません)
		/// </para>
		/// </summary>
		/// 
		/// <param name="expr">値</param>
		public SGradation(float expr)
			: this(expr, expr, expr, expr)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>
		/// グラデーション値を設定します。
		/// </para>
		/// </summary>
		/// 
		/// <param name="fStart">初期値</param>
		/// <param name="fEnd">最終値</param>
		/// <param name="fLimit1">限界値1</param>
		/// <param name="fLimit2">限界値2</param>
		public SGradation(float fStart, float fEnd, float fLimit1, float fLimit2)
			: this(fStart, fEnd, fLimit1, fLimit2, EInterpolate.clampLinear)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>
		/// グラデーション値を設定します。
		/// </para>
		/// </summary>
		/// 
		/// <param name="fStart">初期値</param>
		/// <param name="fEnd">最終値</param>
		/// <param name="fLimit1">限界値1</param>
		/// <param name="fLimit2">限界値2</param>
		/// <param name="interpolate">内分カウンタ。</param>
		public SGradation(
			float fStart, float fEnd, float fLimit1, float fLimit2, EInterpolate interpolate)
		{
			start = fStart;
			end = fEnd;
			limit1 = fLimit1;
			limit2 = fLimit2;
			this.interpolate = interpolate;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>グラデーションするかどうかを取得します。</summary>
		/// 
		/// <value>グラデーションする場合、<c>true</c>。</value>
		public bool gradation
		{
			get
			{
				return start != end;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>値が常にゼロになるかどうかを取得します。</summary>
		/// 
		/// <value>常にゼロである場合、<c>true</c>。</value>
		public bool zero
		{
			get
			{
				return start == end && start == 0f;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// グラデーション計算をせず、デフォルトの値を取得します。
		/// </summary>
		/// 
		/// <param name="g">グラデーション用情報 オブジェクト</param>
		/// <returns>デフォルトの値</returns>
		public static implicit operator float(SGradation g)
		{
			return g.get();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// すべての項目に同一の値を設定します。(グラデーションしません)
		/// </summary>
		/// 
		/// <param name="f">値</param>
		/// <returns>グラデーション用情報 オブジェクト</returns>
		public static implicit operator SGradation(float f)
		{
			return new SGradation(f);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>グラデーション同士を加算します。</summary>
		/// 
		/// <param name="g1">グラデーション用情報 オブジェクト1</param>
		/// <param name="g2">グラデーション用情報 オブジェクト2</param>
		/// <returns>グラデーション用情報 オブジェクト</returns>
		public static SGradation operator +(SGradation g1, SGradation g2)
		{
			return new SGradation(
					g1.start + g2.start, g1.end + g2.end,
					g1.limit1 + g2.limit1, g1.limit2 + g2.limit2);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>グラデーション同士を減算します。</summary>
		/// 
		/// <param name="g1">グラデーション用情報 オブジェクト1</param>
		/// <param name="g2">グラデーション用情報 オブジェクト2</param>
		/// <returns>グラデーション用情報 オブジェクト</returns>
		public static SGradation operator -(SGradation g1, SGradation g2)
		{
			return new SGradation(
					g1.start - g2.start, g1.end - g2.end,
					g1.limit1 - g2.limit1, g1.limit2 - g2.limit2);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>グラデーションを乗算します。</summary>
		/// 
		/// <param name="g">グラデーション用情報 オブジェクト</param>
		/// <param name="f">値</param>
		/// <returns>グラデーション用情報 オブジェクト</returns>
		public static SGradation operator *(SGradation g, float f)
		{
			return new SGradation(g.start * f, g.end * f, g.limit1 * f, g.limit2 * f);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>グラデーションを除算します。</summary>
		/// 
		/// <param name="g">グラデーション用情報 オブジェクト</param>
		/// <param name="f">値</param>
		/// <returns>グラデーション用情報 オブジェクト</returns>
		public static SGradation operator /(SGradation g, float f)
		{
			return new SGradation(g.start / f, g.end / f, g.limit1 / f, g.limit2 / f);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>グラデーション値を設定します。</summary>
		/// 
		/// <param name="fStart">初期値</param>
		/// <param name="fEnd">最終値</param>
		/// <param name="fLimit1">限界値1</param>
		/// <param name="fLimit2">限界値2</param>
		public void set(float fStart, float fEnd, float fLimit1, float fLimit2)
		{
			start = fStart;
			end = fEnd;
			limit1 = fLimit1;
			limit2 = fLimit2;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// すべての項目に同一の値を設定します。(グラデーションしません)
		/// </summary>
		/// 
		/// <param name="fExpr">値</param>
		public void set(float fExpr)
		{
			set(fExpr, fExpr, fExpr, fExpr);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>グラデーション計算をします。</summary>
		/// 
		/// <param name="nNow">現在値</param>
		/// <param name="nSize">分割数</param>
		/// <returns>補完値</returns>
		public float smooth(int nNow, int nSize)
		{
			if(limit1 == limit2)
			{
				return limit1;
			}
			return MathHelper.Clamp(interpolate.interpolate(start, end, nNow, nSize),
				MathHelper.Min(limit1, limit2), MathHelper.Max(limit1, limit2));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>グラデーションの合計全長の計算をします。</summary>
		/// 
		/// <param name="nSize">分割数</param>
		/// <returns>全長</returns>
		public float length(int nSize)
		{
			float fResult = 0;
			for(int i = 0; i < nSize; fResult += smooth(i++, nSize))
				;
			return fResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// グラデーション計算をせず、デフォルトの値を取得します。
		/// </summary>
		/// 
		/// <returns>デフォルトの値</returns>
		public float get()
		{
			if(limit1 == limit2)
			{
				return limit1;
			}
			return MathHelper.Clamp(start,
				MathHelper.Min(limit1, limit2),
				MathHelper.Max(limit1, limit2));
		}
	}
}
