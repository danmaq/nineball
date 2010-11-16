////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.Properties;

namespace danmaq.nineball.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>参照可能な値ラッパー。</summary>
	/// <remarks>ポインタ配列を作る時などに役立ちます。</remarks>
	/// 
	/// <typeparam name="_T">値の型。</typeparam>
	[Serializable]
	public class CValue<_T>
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>値。</summary>
		public _T value;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CValue()
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="v">値。</param>
		public CValue(_T v)
			: this()
		{
			this.value = v;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>値から直接オブジェクトを作成します。</summary>
		/// 
		/// <param name="r">値。</param>
		/// <returns>値オブジェクト。</returns>
		public static implicit operator CValue<_T>(_T r)
		{
			return new CValue<_T>(r);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>値を取得します。</summary>
		/// 
		/// <param name="r">値オブジェクト。</param>
		/// <returns>値。</returns>
		public static implicit operator _T(CValue<_T> r)
		{
			return r.value;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>値の文字列表現を取得します。</summary>
		/// 
		/// <returns>値の文字列表現。</returns>
		public override string ToString()
		{
			return value == null ? Resources.NULL : value.ToString();
		}
	}
}
