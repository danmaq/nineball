////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>アニメーション スプライト。</summary>
	public sealed class CAnimationSprite
		: CEntity
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>座標一覧。</summary>
		public readonly List<Rectangle> rectList = new List<Rectangle>();

		/// <summary>アニメーション プログラム。</summary>
		public readonly List<int> program = new List<int>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>テクスチャ。</summary>
		public Texture2D texture;

		/// <summary>インデックス ポインタ。</summary>
		public int index = 0;

		/// <summary>アニメーション速度。</summary>
		public int interval = 1;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>現在の切り出し位置を取得します。</summary>
		/// 
		/// <value>現在の切り出し位置。</value>
		public Rectangle now
		{
			get
			{
				return rectList[index];
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>既定のアニメーション プログラムをセットします。</summary>
		/// 
		/// <param name="loop">アニメーションをループするかどうか。</param>
		public void setDefaultProgram(bool loop)
		{
			program.Clear();
			rectList.ForEach(r => program.Add(1));
			program.Add(-program.Count);
		}
	}
}
