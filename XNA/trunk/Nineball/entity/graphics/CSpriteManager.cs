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
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers
			: IDisposable
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>描画情報リスト。</summary>
			public readonly List<SSpriteDrawInfo> drawCache = new List<SSpriteDrawInfo>();

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
				m_resolution = value ?? CResolutionDummy.instance;
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
		/// <param name="layer">重ね合わせ深度。</param>
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
		/// <param name="layer">重ね合わせ深度。</param>
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
		/// <param name="rotate">回転(ラジアン)</param>
		/// <param name="scale">拡大率</param>
		/// <param name="effects">反転効果</param>
		/// <param name="layer">重ね合わせ深度。</param>
		/// <param name="blend">合成モード</param>
		public void add(
			Texture2D tex, Vector2 pos, EAlign halign, EAlign valign, Rectangle srcRect, Color color,
			float rotate, Vector2 scale, SpriteEffects effects, float layer, SpriteBlendMode blend)
		{
			add(tex, pos, halign, valign, srcRect, color, rotate, scale, TextureAddressMode.Clamp,
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
		/// <param name="rotate">回転(ラジアン)。</param>
		/// <param name="scale">拡大率。</param>
		/// <param name="addressMode">テクスチャ アドレッシング モード。</param>
		/// <param name="effects">反転効果。</param>
		/// <param name="layer">重ね合わせ深度。。</param>
		/// <param name="blend">合成モード。</param>
		public void add(
			Texture2D tex, Vector2 pos, EAlign halign, EAlign valign, Rectangle srcRect,
			Color color, float rotate, Vector2 scale, TextureAddressMode addressMode,
			SpriteEffects effects, float layer, SpriteBlendMode blend)
		{
			Vector2 origin = new Vector2(
				halign.origin(srcRect.Width), valign.origin(srcRect.Height));
			add(tex, new Rectangle((int)(pos.X), (int)(pos.Y),
					(int)(srcRect.Width * scale.X), (int)(srcRect.Height * scale.Y)),
				srcRect, color, rotate, origin, addressMode, effects, layer, blend);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画処理を予約します。</summary>
		/// 
		/// <param name="tex">テクスチャ</param>
		/// <param name="dstRect">描画先矩形</param>
		/// <param name="srcRect">描画元矩形</param>
		/// <param name="color">色</param>
		/// <param name="layer">重ね合わせ深度。</param>
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
		/// <param name="rotate">回転(ラジアン)</param>
		/// <param name="origin">原点座標</param>
		/// <param name="effects">反転効果</param>
		/// <param name="layer">重ね合わせ深度。</param>
		/// <param name="blend">合成モード</param>
		public void add(
			Texture2D tex, Rectangle dstRect, Rectangle srcRect, Color color, float rotate,
			Vector2 origin, SpriteEffects effects, float layer, SpriteBlendMode blend)
		{
			add(tex, dstRect, srcRect, color, rotate, origin, TextureAddressMode.Clamp,
				effects, layer, blend);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画処理を予約します。</summary>
		/// 
		/// <param name="tex">テクスチャ。</param>
		/// <param name="dstRect">描画先矩形。</param>
		/// <param name="srcRect">描画元矩形。</param>
		/// <param name="color">色。</param>
		/// <param name="rotate">回転(ラジアン)。</param>
		/// <param name="origin">原点座標。</param>
		/// <param name="addressMode">テクスチャ アドレッシング モード。</param>
		/// <param name="effects">反転効果。</param>
		/// <param name="layer">重ね合わせ深度。。</param>
		/// <param name="blend">合成モード。</param>
		public void add(
			Texture2D tex, Rectangle dstRect, Rectangle srcRect, Color color, float rotate,
			Vector2 origin, TextureAddressMode addressMode, SpriteEffects effects,
			float layer, SpriteBlendMode blend)
		{
			SSpriteDrawInfo info = SSpriteDrawInfo.initialized;
			info.texture = tex;
			info.destinationRectangle = resolution.convertRectangle(dstRect);
			info.sourceRectangle = srcRect;
			info.color = color;
			info.fRotation = rotate + resolution.rotate;
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
		/// <param name="rotate">回転(ラジアン)</param>
		/// <param name="origin">原点座標</param>
		/// <param name="scale">拡大率</param>
		/// <param name="effects">反転効果</param>
		/// <param name="layer">重ね合わせ深度。</param>
		/// <param name="blend">合成モード</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="text"/>が<c>null</c>と等しい場合。
		/// </exception>
		public void add(
			SpriteFont spriteFont, string text, Vector2 pos, Color color, float rotate,
			Vector2 origin, Vector2 scale, SpriteEffects effects, float layer,
			SpriteBlendMode blend)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			SSpriteDrawInfo info = SSpriteDrawInfo.initialized;
			info.spriteFont = spriteFont;
			info.text = text;
			info.position = resolution.convertPosition(pos);
			info.color = color;
			info.fRotation = rotate + resolution.rotate;
			info.origin = origin;
			info.blendMode = blend;
			info.scale = scale * resolution.scale;
			info.effects = effects;
			info.fLayerDepth = layer;
			_private.drawCache.Add(info);
		}
	}
}
