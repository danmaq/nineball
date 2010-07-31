////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>マンマシンI/F入力制御・管理クラスのコレクションの拡張機能。</summary>
	public static class CInputCollectionExtension
	{

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <typeparamref name="_T"/>型の入力制御・管理クラスが登録されていたら取得します。
		/// </summary>
		/// <remarks>自動認識状態のコレクションから取得する時に便利です。</remarks>
		/// 
		/// <typeparam name="_T">取得したい入力制御・管理クラスの型。</typeparam>
		/// <param name="collection">入力制御・管理コレクション</param>
		/// <returns>
		/// <typeparamref name="_T"/>型の入力制御・管理クラス。存在しない場合、<c>null</c>。
		/// </returns>
		public static _T getInstance<_T>(this CInputCollection collection) where _T : CInput
		{
			_T input = null;
			if(collection != null)
			{
				int nTotal = collection.Count;
				if(nTotal > 0)
				{
					Type typeExpect = typeof(_T);
					if(nTotal == 1)
					{
						CInput _input = collection.childList[0];
						Type typeGot = _input.GetType();
						if(typeExpect == typeGot || typeExpect.IsSubclassOf(typeGot))
						{
							input = (_T)_input;
						}
					}
					else
					{
						input = (_T)collection.FirstOrDefault(_input =>
							_input.GetType() == typeExpect ||
							typeExpect.IsSubclassOf(_input.GetType()));
					}
				}
			}
			return input;
		}
	}
}
