////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.data;
using danmaq.nineball.state;
using danmaq.nineball.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>アニメーション スプライト。</summary>
	public class CAnimationSprite
		: CEntity
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>切り出し座標一覧。</summary>
		public readonly List<Rectangle> rectList = new List<Rectangle>();

		/// <summary>アニメーション プログラム。</summary>
		public readonly List<int> program = new List<int>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>テクスチャ。</summary>
		public Texture2D texture;

		/// <summary>描画先座標。</summary>
		public Vector2 position;

		/// <summary>水平位置揃え情報。</summary>
		public EAlign alignHorizontal = EAlign.Center;

		/// <summary>垂直位置揃え情報。</summary>
		public EAlign alignVertical = EAlign.Center;

		/// <summary>乗算色。</summary>
		public Color color = Color.White;

		/// <summary>合成モード。</summary>
		public SpriteBlendMode blendMode = SpriteBlendMode.AlphaBlend;

		/// <summary>インデックス ポインタ。</summary>
		public int index = 0;

		/// <summary>アニメーション速度。</summary>
		public int interval = 1;

		/// <summary>スプライト管理クラス。</summary>
		public CSprite sprite;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CAnimationSprite()
			: base(null)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		public CAnimationSprite(IState firstState)
			: base(firstState, null)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		/// <param name="privateMembers">
		///	オブジェクトと状態クラスのみがアクセス可能なフィールド。
		///	</param>
		public CAnimationSprite(IState firstState, object privateMembers)
			: base(firstState, privateMembers)
		{
		}

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
