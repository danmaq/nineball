////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
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
			return peekLast(list, false);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>リストの末尾を削除します。</summary>
		/// 
		/// <param name="list">対象のリスト。</param>
		/// <returns>削除された末尾の要素。</returns>
		public static _T pop<_T>(this IList<_T> list)
		{
			return pop(list, false);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>リストの末尾を置換します。</summary>
		/// 
		/// <param name="list">対象のリスト。</param>
		/// <param name="value">置換する要素。</param>
		/// <returns>置換された要素。</returns>
		public static _T replaceLast<_T>(this IList<_T> list, _T value)
		{
			return replaceLast(list, value, false);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>リストに要素が存在する場合、末尾の要素を取得します。</summary>
		/// <remarks>
		/// <paramref name="validateEmpty"/>が<c>true</c>かつ、要素が存在しない場合、
		/// 既定値を取得します。
		/// </remarks>
		/// 
		/// <param name="list">対象のリスト。</param>
		/// <param name="validateEmpty">要素の有無を確認するかどうか。</param>
		/// <returns>末尾の要素。</returns>
		public static _T peekLast<_T>(this IList<_T> list, bool validateEmpty)
		{
			int index = list.Count - 1;
			return validateEmpty && index < 0 ? default(_T) : list[index];
		}

		//* -----------------------------------------------------------------------*
		/// <summary>リストの末尾を削除します。</summary>
		/// <remarks>
		/// <paramref name="validateEmpty"/>が<c>true</c>かつ、要素が存在しない場合、
		/// リストを操作せずに、既定値を取得します。
		/// </remarks>
		/// 
		/// <param name="list">対象のリスト。</param>
		/// <param name="validateEmpty">要素の有無を確認するかどうか。</param>
		/// <returns>削除された末尾の要素。</returns>
		public static _T pop<_T>(this IList<_T> list, bool validateEmpty)
		{
			int index = list.Count - 1;
			_T result = default(_T);
			if (!(validateEmpty && index < 0))
			{
				result = list[index];
				list.RemoveAt(index);
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>リストの末尾を置換します。</summary>
		/// <remarks>
		/// <paramref name="validateEmpty"/>が<c>true</c>かつ、要素が存在しない場合、
		/// リスト末尾に追加し、既定値を取得します。
		/// </remarks>
		/// 
		/// <param name="list">対象のリスト。</param>
		/// <param name="value">置換する要素。</param>
		/// <param name="validateEmpty">要素の有無を確認するかどうか。</param>
		/// <returns>置換された要素。</returns>
		public static _T replaceLast<_T>(this IList<_T> list, _T value, bool validateEmpty)
		{
			int index = list.Count - 1;
			_T result = default(_T);
			if (validateEmpty && index < 0)
			{
				list.Add(value);
			}
			else
			{
				result = list[index];
				list[index] = value;
			}
			return result;
		}
	}
}
