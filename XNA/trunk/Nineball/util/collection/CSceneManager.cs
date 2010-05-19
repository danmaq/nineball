////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;

namespace danmaq.nineball.util.collection
{
	public sealed class CSceneManager : DrawableGameComponent
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		private readonly CDisposablePartialCollection<IGameComponent, DrawableGameComponent> collection;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="collection">部分的に責任を持つ対象のリスト。</param>
		public CSceneManager(Game game)
			: base(game)
		{
			game.Components.Add(this);
			collection = new CDisposablePartialCollection<IGameComponent, DrawableGameComponent>(game.Components);
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		public DrawableGameComponent now
		{
			get
			{
				ReadOnlyCollection<DrawableGameComponent> partial = collection.partial;
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
		protected override void Dispose(bool disposing)
		{
			collection.Dispose();
			base.Dispose(disposing);
		}

		public void Push(DrawableGameComponent component)
		{
			collection.Add(component);
		}

		public DrawableGameComponent Pop()
		{
			DrawableGameComponent now = this.now;
			if (now != null)
			{
				collection.Remove(now);
			}
			return now;
		}
	}
}
