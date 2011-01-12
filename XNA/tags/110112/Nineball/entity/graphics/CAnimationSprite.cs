////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.data;
using danmaq.nineball.state;
using danmaq.nineball.state.graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.entity.graphics
{

	// TODO : アニメスプライトはまだしも、カメラパスとフォグアニメ、統合できるんじゃね？

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>アニメーション スプライト。</summary>
	public class CAnimationSprite
		: CEntity
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>アニメーション定義構造体。</summary>
		public struct SData
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>切り出し座標。</summary>
			public Rectangle srcRect;

			/// <summary>乗算色。</summary>
			public Color color;

			/// <summary>合成モード。</summary>
			public SpriteBlendMode blendMode;

			/// <summary>次に移動するフレーム(現在位置からの相対指定)。</summary>
			public int next;

		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>アニメーション定義一覧。</summary>
		public readonly List<SData> data = new List<SData>();

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

		/// <summary>インデックス ポインタ。</summary>
		public int index = 0;

		/// <summary>アニメーション速度。</summary>
		public int interval = 1;

		/// <summary>描画レイヤ。</summary>
		public float layer = 0f;

		/// <summary>回転。</summary>
		public float rotate = 0f;

		/// <summary>拡大率。</summary>
		public Vector2 scale = Vector2.One;

		/// <summary>反転効果。</summary>
		public SpriteEffects effect = SpriteEffects.None;

		/// <summary>スプライト管理クラス。</summary>
		public CSpriteManager sprite;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CAnimationSprite()
			: base(CStateAnimationSprite.instance)
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
		public SData now
		{
			get
			{
				return data[index];
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			data.Clear();
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>既定のアニメーション プログラムをセットします。</summary>
		/// 
		/// <param name="loop">アニメーションをループするかどうか。</param>
		public void setDefaultProgram(bool loop)
		{
			int length = data.Count;
			SData _data;
			for (int i = length; --i >= 0; )
			{
				_data = data[i];
				_data.next = 1;
				data[i] = _data;
			}
			_data = data[length - 1];
			_data.next = loop ? 0 : -length;
			data[length - 1] = _data;
		}
	}
}
