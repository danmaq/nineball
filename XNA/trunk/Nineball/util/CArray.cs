////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.nineball.util
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>配列操作周りの関数集クラス。</summary>
	public static class CArray
	{

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>配列の値をランダムに入れ替えます。</summary>
		/// 
		/// <typeparam name="T">配列の型。</typeparam>
		/// <param name="array">配列。</param>
		/// <param name="random">乱数ジェネレータ。</param>
		public static void sortRandom<T>(T[] array, Random random)
		{
			int length = array.Length;
			if (length >= 2)
			{
				for (int i = length; --i >= 0; )
				{
					int j = random.Next(i + 1);
					T tmp = array[j];
					array[j] = array[i];
					array[i] = tmp;
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>配列の値をインデックス値で埋めます。</summary>
		/// 
		/// <param name="array">配列。</param>
		public static void fillIndex(int[] array)
		{
			for (int i = array.Length; --i >= 0; array[i] = i)
				;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>配列の値をインデックス値で埋め、ランダムで並び替えます。</summary>
		/// 
		/// <param name="array">配列。</param>
		/// <param name="random">乱数ジェネレータ。</param>
		public static void fillIndexRandom(int[] array, Random random)
		{
			fillIndex(array);
			sortRandom(array, random);
		}
	}
}
