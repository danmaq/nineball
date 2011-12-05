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
	/// <summary>擬似乱数ジェネレータのラッパー。</summary>
	/// <remarks>乱数生成アルゴリズムとしてxorshift法を使用しています。</remarks>
	public class CRandom
		: Random
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>擬似乱数系列の開始値を計算するために使用する数値。</summary>
		public readonly int seed;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>擬似乱数を計算するために使用する数値。</summary>
		private int m_x;

		/// <summary>擬似乱数を計算するために使用する数値。</summary>
		private int m_y;

		/// <summary>擬似乱数を計算するために使用する数値。</summary>
		private int m_z;

		/// <summary>擬似乱数を計算するために使用する数値。</summary>
		private int m_w;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CRandom()
			: this((int)(DateTime.Now.Ticks % int.MaxValue))
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="seed">
		/// 擬似乱数系列の開始値を計算するために使用する数値。
		/// </param>
		public CRandom(int seed)
			: base(seed)
		{
			this.seed = seed;
			m_w = seed;
			m_x = seed << 16 + seed >> 16;
			m_y = seed + m_x;
			m_z = m_x ^ m_y;
			counter = 0;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>乱数を使用した回数を取得します。</summary>
		/// 
		/// <value>乱数を使用した回数。</value>
		public ulong counter
		{
			get;
			private set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>0 以上の乱数を返します。</summary>
		/// 
		/// <returns>
		/// 0 以上で<c>System.Int32.MaxValue</c> より小さい 32 ビット符号付整数。
		/// </returns>
		public override int Next()
		{
			int t = (m_x ^ (m_x << 11));
			m_x = m_y;
			m_y = m_z;
			m_z = m_w;
			int result = (m_w = (m_w ^ (m_w >> 19)) ^ (t ^ (t >> 8)));
			counter++;
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>0.0 と 1.0 の間の乱数を返します。</summary>
		/// <remarks>
		/// 乱数生成アルゴリズムとして高速・高周期なxorshift法を使用しています。
		/// ただしスーパークラスの作法に従うため1回除算が入るので、そこで足引っ張っています。
		/// </remarks>
		/// 
		/// <returns>0.0 以上 1.0 未満の倍精度浮動小数点数。</returns>
		protected override double Sample()
		{
			return Next() / (double)int.MaxValue;
		}
	}
}
