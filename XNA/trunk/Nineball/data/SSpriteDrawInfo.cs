////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>画像/文字用スプライトバッチ描画情報が格納された構造体。</summary>
	public struct SSpriteDrawInfo : IComparable<SSpriteDrawInfo>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>初期化された構造体。</summary>
		public static readonly SSpriteDrawInfo initialized;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>合成モード。</summary>
		public SpriteBlendMode blendMode;

		/// <summary>乗算色。</summary>
		public Color color;

		/// <summary>回転(ラジアン)。</summary>
		public float fRotation;

		/// <summary>原点座標。</summary>
		public Vector2 origin;

		/// <summary>エフェクト種類。</summary>
		public SpriteEffects effects;

		/// <summary>テクスチャ・アドレッシング。</summary>
		public TextureAddressMode addressMode;

		/// <summary>レイヤ深度。</summary>
		public float fLayerDepth;

		/// <summary>テクスチャ リソース。</summary>
		public Texture2D texture;

		/// <summary>描画先の矩形情報。</summary>
		public Rectangle destinationRectangle;

		/// <summary>描画元の矩形情報。</summary>
		public Rectangle sourceRectangle;

		/// <summary>スプライトフォント リソース。</summary>
		public SpriteFont spriteFont;

		/// <summary>描画する文字列。</summary>
		public string text;

		/// <summary>描画先座標。</summary>
		public Vector2 position;

		/// <summary>拡大率。</summary>
		public Vector2 scale;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static SSpriteDrawInfo()
		{
			initialized.initialize();
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 現在のオブジェクトを同じ型の別のオブジェクトと比較します。
		/// </summary>
		/// 
		/// <param name="other">このオブジェクトと比較するオブジェクト。</param>
		/// <returns>
		/// 比較対象オブジェクトの相対順序を示す 32 ビット符号付き整数。
		/// </returns>
		public int CompareTo(SSpriteDrawInfo other)
		{
			int nResult = Math.Sign(other.fLayerDepth - fLayerDepth);
			if (nResult == 0)
			{
				nResult = (int)other.blendMode - (int)blendMode;
				if (nResult == 0)
				{
					nResult = (int)other.effects - (int)effects;
					if (nResult == 0)
					{
						if (spriteFont == null && other.texture == null)
						{
							nResult = -1;
						}
						else if (texture == null && other.spriteFont == null)
						{
							nResult = 1;
						}
					}
				}
			}
			return nResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// テクスチャ向けに初期化します。
		/// </summary>
		public void initialize()
		{
			blendMode = SpriteBlendMode.None;
			color = Color.White;
			fRotation = 0f;
			origin = Vector2.Zero;
			effects = SpriteEffects.None;
			fLayerDepth = 0f;
			texture = null;
			addressMode = TextureAddressMode.Clamp;
			destinationRectangle = Rectangle.Empty;
			sourceRectangle = Rectangle.Empty;
			spriteFont = null;
			text = null;
			position = Vector2.Zero;
			scale = Vector2.One;
		}
	}
}
