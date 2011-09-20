////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.data;
using danmaq.nineball.state;
using danmaq.nineball.state.graphics;
using danmaq.nineball.util.resolution;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.entity.graphics
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>スプライト描画管理クラス。</summary>
	public sealed class CSpriteManager
		: CEntity
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>現在の描画状態が格納された構造体。</summary>
		public struct SDrawMode
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>描画中かどうか。</summary>
			public bool isBegin;

			/// <summary>合成モード。</summary>
			public SpriteBlendMode blendMode;

			/// <summary>テクスチャ・アドレッシング。</summary>
			public TextureAddressMode addressMode;

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>描画状態を変更すべきかどうかを判定します。</summary>
			/// 
			/// <param name="info">描画情報。</param>
			/// <returns>描画状態を変更すべき場合、<c>true</c>。</returns>
			public bool changeDrawMode(SSpriteDrawInfo info)
			{
				return !(
					blendMode == info.blendMode &&
					addressMode == info.addressMode);
			}
		}

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers
			: IDisposable
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>描画情報リスト。</summary>
			public readonly List<SSpriteDrawInfo> drawCache = new List<SSpriteDrawInfo>(1);

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>スプライトの最大予約数。</summary>
			public int maxReserved;

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>フィールドのオブジェクトを解放します。</summary>
			public void Dispose()
			{
				drawCache.Clear();
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _private;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>予約可能な最大数。</summary>
		public int reserveLimit = 10000;

		/// <summary>スプライトバッチ。</summary>
		public SpriteBatch spriteBatch;

		/// <summary>解像度管理クラス。</summary>
		private CResolutionBase m_resolution = CResolutionDummy.instance;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CSpriteManager()
			: this(CStateSpriteManager.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		public CSpriteManager(IState firstState)
			: base(firstState, new CPrivateMembers())
		{
			_private = (CPrivateMembers)privateMembers;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>解像度管理クラスを取得/設定します。</summary>
		/// 
		/// <value>解像度管理クラス。</value>
		public CResolutionBase resolution
		{
			get
			{
				return m_resolution;
			}
			set
			{
				m_resolution = value == null ? CResolutionDummy.instance : value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>スプライトの予約数を取得します。</summary>
		/// 
		/// <value>スプライトの予約数</value>
		public int reservedCount
		{
			get
			{
				return _private.drawCache.Count;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>スプライトの最大予約数を取得します。</summary>
		/// 
		/// <value>スプライトの最大予約数</value>
		public int maxReserved
		{
			get
			{
				return _private.maxReserved;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// アンマネージ リソースの解放およびリセットに関連付けられている
		/// アプリケーション定義のタスクを実行します。
		/// </summary>
		public override void Dispose()
		{
			_private.Dispose();
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画処理を予約します。</summary>
		/// 
		/// <param name="tex">テクスチャ</param>
		/// <param name="rect">描画元・先矩形</param>
		/// <param name="color">色</param>
		/// <param name="layer">レイヤ番号</param>
		/// <param name="blend">合成モード</param>
		public void add(
			Texture2D tex, Rectangle rect, Color color, float layer, SpriteBlendMode blend)
		{
			add(tex, rect, rect, color, 0, Vector2.Zero, SpriteEffects.None, layer, blend);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画処理を予約します。</summary>
		/// 
		/// <param name="tex">テクスチャ</param>
		/// <param name="pos">描画先座標</param>
		/// <param name="halign">位置揃え(横)</param>
		/// <param name="valign">位置揃え(縦)</param>
		/// <param name="srcRect">描画元矩形</param>
		/// <param name="color">色</param>
		/// <param name="layer">レイヤ番号</param>
		/// <param name="blend">合成モード</param>
		public void add(
			Texture2D tex, Vector2 pos, EAlign halign, EAlign valign, Rectangle srcRect,
			Color color, float layer, SpriteBlendMode blend)
		{
			add(tex, pos, halign, valign, srcRect, color, 0, Vector2.One, SpriteEffects.None,
				layer, blend);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画処理を予約します。</summary>
		/// 
		/// <param name="tex">テクスチャ</param>
		/// <param name="pos">描画先座標</param>
		/// <param name="halign">位置揃え(横)</param>
		/// <param name="valign">位置揃え(縦)</param>
		/// <param name="srcRect">描画元矩形</param>
		/// <param name="color">色</param>
		/// <param name="fRotate">回転(ラジアン)</param>
		/// <param name="scale">拡大率</param>
		/// <param name="effects">反転効果</param>
		/// <param name="layer">レイヤ番号</param>
		/// <param name="blend">合成モード</param>
		public void add(
			Texture2D tex, Vector2 pos, EAlign halign, EAlign valign, Rectangle srcRect, Color color,
			float fRotate, Vector2 scale, SpriteEffects effects, float layer, SpriteBlendMode blend)
		{
			add(tex, pos, halign, valign, srcRect, color, fRotate, scale, TextureAddressMode.Clamp,
				SpriteEffects.None, layer, blend);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画処理を予約します。</summary>
		/// 
		/// <param name="tex">テクスチャ。</param>
		/// <param name="pos">描画先座標。</param>
		/// <param name="halign">位置揃え(横)。</param>
		/// <param name="valign">位置揃え(縦)。</param>
		/// <param name="srcRect">描画元矩形。</param>
		/// <param name="color">色。</param>
		/// <param name="fRotate">回転(ラジアン)。</param>
		/// <param name="scale">拡大率。</param>
		/// <param name="addressMode">テクスチャ アドレッシング モード。</param>
		/// <param name="effects">反転効果。</param>
		/// <param name="layer">レイヤ番号。</param>
		/// <param name="blend">合成モード。</param>
		public void add(
			Texture2D tex, Vector2 pos, EAlign halign, EAlign valign, Rectangle srcRect,
			Color color, float fRotate, Vector2 scale, TextureAddressMode addressMode,
			SpriteEffects effects, float layer, SpriteBlendMode blend)
		{
			Vector2 origin = Vector2.Zero;
			// TODO : これ変換なしにSpriteBatch.Drawに渡した方が処理早いんじゃね？
			switch (halign)
			{
				case EAlign.Center:
					origin.X = (float)(srcRect.Width) * 0.5f;
					break;
				case EAlign.RightBottom:
					origin.X = srcRect.Width;
					break;
			}
			switch (valign)
			{
				case EAlign.Center:
					origin.Y = (float)(srcRect.Height) * 0.5f;
					break;
				case EAlign.RightBottom:
					origin.Y = srcRect.Height;
					break;
			}
			add(tex, new Rectangle((int)(pos.X), (int)(pos.Y),
					(int)(srcRect.Width * scale.X), (int)(srcRect.Height * scale.Y)),
				srcRect, color, fRotate, origin, addressMode, effects, layer, blend);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画処理を予約します。</summary>
		/// 
		/// <param name="tex">テクスチャ</param>
		/// <param name="dstRect">描画先矩形</param>
		/// <param name="srcRect">描画元矩形</param>
		/// <param name="color">色</param>
		/// <param name="layer">レイヤ番号</param>
		/// <param name="blend">合成モード</param>
		public void add(
			Texture2D tex, Rectangle dstRect, Rectangle srcRect,
			Color color, float layer, SpriteBlendMode blend)
		{
			add(tex, dstRect, srcRect, color, 0,
				Vector2.Zero, SpriteEffects.None, layer, blend);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画処理を予約します。</summary>
		/// 
		/// <param name="tex">テクスチャ</param>
		/// <param name="dstRect">描画先矩形</param>
		/// <param name="srcRect">描画元矩形</param>
		/// <param name="color">色</param>
		/// <param name="fRotate">回転(ラジアン)</param>
		/// <param name="origin">原点座標</param>
		/// <param name="effects">反転効果</param>
		/// <param name="layer">レイヤ番号</param>
		/// <param name="blend">合成モード</param>
		public void add(
			Texture2D tex, Rectangle dstRect, Rectangle srcRect, Color color, float fRotate,
			Vector2 origin, SpriteEffects effects, float layer, SpriteBlendMode blend)
		{
			add(tex, dstRect, srcRect, color, fRotate, origin, TextureAddressMode.Clamp,
				effects, layer, blend);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画処理を予約します。</summary>
		/// 
		/// <param name="tex">テクスチャ。</param>
		/// <param name="dstRect">描画先矩形。</param>
		/// <param name="srcRect">描画元矩形。</param>
		/// <param name="color">色。</param>
		/// <param name="fRotate">回転(ラジアン)。</param>
		/// <param name="origin">原点座標。</param>
		/// <param name="addressMode">テクスチャ アドレッシング モード。</param>
		/// <param name="effects">反転効果。</param>
		/// <param name="layer">レイヤ番号。</param>
		/// <param name="blend">合成モード。</param>
		public void add(
			Texture2D tex, Rectangle dstRect, Rectangle srcRect, Color color, float fRotate,
			Vector2 origin, TextureAddressMode addressMode, SpriteEffects effects,
			float layer, SpriteBlendMode blend)
		{
			SSpriteDrawInfo info = SSpriteDrawInfo.initialized;
			info.texture = tex;
			info.destinationRectangle = resolution.convertRectangle(dstRect);
			info.sourceRectangle = srcRect;
			info.color = color;
			info.fRotation = fRotate + resolution.rotate;
			info.origin = origin;
			info.addressMode = addressMode;
			info.effects = effects;
			info.fLayerDepth = layer;
			info.blendMode = blend;
			_private.drawCache.Add(info);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画処理を予約します。</summary>
		/// 
		/// <param name="spriteFont">スプライトフォント テクスチャ</param>
		/// <param name="text">描画したいテキスト</param>
		/// <param name="pos">座標</param>
		/// <param name="color">色</param>
		/// <param name="fRotate">回転(ラジアン)</param>
		/// <param name="origin">原点座標</param>
		/// <param name="scale">拡大率</param>
		/// <param name="effects">反転効果</param>
		/// <param name="layer">レイヤ番号</param>
		/// <param name="blend">合成モード</param>
		public void add(
			SpriteFont spriteFont, string text, Vector2 pos, Color color, float fRotate,
			Vector2 origin, Vector2 scale, SpriteEffects effects, float layer,
			SpriteBlendMode blend)
		{
			SSpriteDrawInfo info = SSpriteDrawInfo.initialized;
			info.spriteFont = spriteFont;
			info.text = text;
			info.position = resolution.convertPosition(pos);
			info.color = color;
			info.fRotation = fRotate + resolution.rotate;
			info.origin = origin;
			info.blendMode = blend;
			info.scale = scale * resolution.scale;
			info.effects = effects;
			info.fLayerDepth = layer;
			_private.drawCache.Add(info);
		}
	}
}
