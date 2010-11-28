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
using danmaq.nineball.data;
using danmaq.nineball.old.core.raw;
using danmaq.nineball.state;
using danmaq.nineball.state.graphics;
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
		private CResolution m_resolution = null;

		/// <summary>アスペクト比固定かどうか。</summary>
		private bool m_aspectFixed;

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
			resolution = new CResolution();
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>解像度管理クラスを取得/設定します。</summary>
		/// 
		/// <value>解像度管理クラス。</value>
		public CResolution resolution
		{
			get
			{
				return m_resolution;
			}
			set
			{
				m_resolution = value;
				Type expect = typeof(CResolutionAspectFix);
				Type got = value.GetType();
				m_aspectFixed = got == expect || got.IsSubclassOf(expect);
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
		/// <param name="fLayer">レイヤ番号</param>
		/// <param name="blend">合成モード</param>
		public void add(
			Texture2D tex, Rectangle rect, Color color, float fLayer, SpriteBlendMode blend
		)
		{
			add(tex, rect, rect, color, 0, Vector2.Zero, SpriteEffects.None, fLayer, blend);
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
		/// <param name="fLayer">レイヤ番号</param>
		/// <param name="blend">合成モード</param>
		public void add(
			Texture2D tex, Vector2 pos, EAlign halign, EAlign valign, Rectangle srcRect,
			Color color, float fLayer, SpriteBlendMode blend
		)
		{
			add(tex, pos, halign, valign, srcRect, color, 0, Vector2.One, SpriteEffects.None, fLayer, blend);
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
		/// <param name="fLayer">レイヤ番号</param>
		/// <param name="blend">合成モード</param>
		public void add(
			Texture2D tex, Vector2 pos, EAlign halign, EAlign valign, Rectangle srcRect, Color color,
			float fRotate, Vector2 scale, SpriteEffects effects, float fLayer, SpriteBlendMode blend
		)
		{
			Vector2 origin = Vector2.Zero;
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
				srcRect, color, fRotate, origin, effects, fLayer, blend);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画処理を予約します。</summary>
		/// 
		/// <param name="tex">テクスチャ</param>
		/// <param name="dstRect">描画先矩形</param>
		/// <param name="srcRect">描画元矩形</param>
		/// <param name="color">色</param>
		/// <param name="fLayer">レイヤ番号</param>
		/// <param name="blend">合成モード</param>
		public void add(
			Texture2D tex, Rectangle dstRect, Rectangle srcRect,
			Color color, float fLayer, SpriteBlendMode blend
		)
		{
			add(tex, dstRect, srcRect, color, 0,
				Vector2.Zero, SpriteEffects.None, fLayer, blend);
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
		/// <param name="fLayer">レイヤ番号</param>
		/// <param name="blend">合成モード</param>
		public void add(
			Texture2D tex, Rectangle dstRect, Rectangle srcRect, Color color, float fRotate,
			Vector2 origin, SpriteEffects effects, float fLayer, SpriteBlendMode blend
		)
		{
			SSpriteDrawInfo info = new SSpriteDrawInfo();
			info.initialize();
			info.texture = tex;
			info.destinationRectangle = resolution == null ?
				dstRect : resolution.resizeFromVGA(dstRect);
			info.sourceRectangle = srcRect;
			info.color = color;
			info.fRotation = fRotate -
				(resolution != null && resolution.vertical ? 0 : 0);
			info.origin = origin;
			info.effects = effects;
			info.fLayerDepth = fLayer;
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
		/// <param name="fLayer">レイヤ番号</param>
		/// <param name="blend">合成モード</param>
		public void add(
			SpriteFont spriteFont, string text, Vector2 pos, Color color, float fRotate,
			Vector2 origin, Vector2 scale, SpriteEffects effects, float fLayer,
			SpriteBlendMode blend
		)
		{
			SSpriteDrawInfo info = new SSpriteDrawInfo();
			info.initialize();
			info.spriteFont = spriteFont;
			info.text = text;
			info.position = resolution == null ? pos : resolution.resizeFromVGA(pos);
			info.color = color;
			info.fRotation = fRotate;
			info.origin = origin;
			info.blendMode = blend;
			if (resolution == null)
			{
				info.scale = scale;
			}
			else
			{
				if (m_aspectFixed)
				{
					CResolutionAspectFix res = (CResolutionAspectFix)resolution;
					info.scale = scale * res.scaleGapFromVGA;
				}
				else
				{
					info.scale = scale * resolution.scaleGapFromVGA;
				}
			}
			info.effects = effects;
			info.fLayerDepth = fLayer;
			_private.drawCache.Add(info);
		}
	}
}
