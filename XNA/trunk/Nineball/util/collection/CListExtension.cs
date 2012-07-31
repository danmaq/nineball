////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace danmaq.nineball.util.collection
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>汎用リスト用拡張機能。</summary>
	public static class CListExtension
	{

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>リストの末尾を取得します。</summary>
		/// 
		/// <param name="list">対象のリスト。</param>
		/// <returns>末尾の要素。</returns>
		public static _T peekLast<_T>(this IList<_T> list)
		{
			return list[list.Count - 1];
		}

		//* -----------------------------------------------------------------------*
		/// <summary>リストの末尾を削除します。</summary>
		/// 
		/// <param name="list">対象のリスト。</param>
		/// <returns>削除された末尾の要素。</returns>
		public static _T pop<_T>(this IList<_T> list)
		{
			int index = list.Count - 1;
			_T result = list[index];
			list.RemoveAt(index);
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>リストの末尾を置換します。</summary>
		/// 
		/// <param name="list">対象のリスト。</param>
		/// <param name="value">置換する要素。</param>
		/// <returns>置換された要素。</returns>
		public static _T replaceLast<_T>(this IList<_T> list, _T value)
		{
			int index = list.Count - 1;
			_T result = list[index];
			list[index] = value;
			return result;
		}
	}
}
