////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace danmaq.nineball.util.collection
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>部分責任コレクション。</para>
	/// <para>
	/// このクラスを通じてコレクションに格納された要素は、
	/// このクラスを破棄することで道連れにされます。
	/// </para>
	/// </summary>
	/// 
	/// <typeparam name="_T">コレクション内の要素の基本型。</typeparam>
	/// <typeparam name="_P">コレクション内の要素の型。</typeparam>
	public class CDisposablePartialCollection<_T, _P> :
		CPartialCollection<_T, _P> where _P : _T, IDisposable
	{

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="collection">部分的に責任を持つ対象のリスト。</param>
		public CDisposablePartialCollection(ICollection<_T> collection) :
			base(collection)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理している要素を全て解放します。</summary>
		/// 
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		public override void Clear()
		{
			throwAtReadOnly();
			for (int i = m_partial.Count; --i >= 0; )
			{
				m_partial[i].Dispose();
			}
			base.Clear();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理している要素を解放します。</summary>
		/// 
		/// <param name="item">要素。</param>
		/// <returns>解放できた場合、<c>true</c>。</returns>
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		public override bool Remove(_P item)
		{
			bool bResult = base.Remove(item);
			if(bResult)
			{
				item.Dispose();
			}
			return bResult;
		}
	}
}
