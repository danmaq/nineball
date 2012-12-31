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
using System.Collections.ObjectModel;
using danmaq.nineball.util.collection;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.util
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>シーン管理クラス。</summary>
	/// <remarks>
	/// このクラスを使って、ゲーム コンポーネントをシーンのように使うことができます。
	/// </remarks>
	public sealed class SceneManager : IDisposable
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>部分責任コレクション。</summary>
		private readonly CDisposablePartialCollection<IGameComponent, GameComponent> collection;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="collection">ゲームコンポーネント管理コレクション。</param>
		public SceneManager(ICollection<IGameComponent> collection)
		{
			this.collection = new CDisposablePartialCollection<IGameComponent, GameComponent>(collection);
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>現在のシーン。</summary>
		public GameComponent now
		{
			get
			{
				ReadOnlyCollection<GameComponent> partial = collection.partial;
				int nLength = partial.Count;
				return nLength == 0 ? null : partial[nLength - 1];
			}
			set
			{
				if (collection.Count > 0)
				{
					collection.Remove(now);
				}
				collection.Add(value);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>格納されている要素の数を取得します。</summary>
		/// 
		/// <value>格納されている要素の数。</value>
		public int Count
		{
			get
			{
				return collection.Count;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>管理している要素を全て解放します。</summary>
		/// 
		public void Dispose()
		{
			collection.Dispose();
		}
	}
}
