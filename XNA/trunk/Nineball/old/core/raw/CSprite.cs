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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.old.core.raw
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>スプライト描画管理 クラス。</summary>
	[Obsolete("このクラスは今後サポートされません。danmaq.nineball.entity.manager.CSpriteManagerを使用してください。")]
	public sealed class CSprite : IDisposable
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>現在の描画状態が格納された構造体。</summary>
		public struct SDrawMode
		{

			/// <summary>描画中かどうか。</summary>
			public bool isBegin;

			/// <summary>合成モード。</summary>
			public SpriteBlendMode blendMode;
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>予約可能な最大数。</summary>
		public int reserveLimit = 10000;

		/// <summary>描画情報リスト。</summary>
		private readonly List<SSpriteDrawInfo> drawCache = new List<SSpriteDrawInfo>(1);

		/// <summary>CResolutionAspectFixの型情報。</summary>
		private readonly Type typeResAF = typeof(CResolutionAspectFix);

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>解像度管理クラス。</summary>
		public CResolution resolution = null;

		/// <summary>現在の描画状態。</summary>
		private SDrawMode m_drawMode;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CSprite()
		{
			resolution = new CResolution();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="spriteBatch">スプライトバッチ</param>
		public CSprite(SpriteBatch spriteBatch)
			: this()
		{
			this.spriteBatch = spriteBatch;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>デストラクタ。</summary>
		~CSprite()
		{
			Dispose();
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>スプライトバッチを取得します。</summary>
		/// 
		/// <value>スプライトバッチ。</value>
		public SpriteBatch spriteBatch
		{
			get;
			set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>スプライトの予約数を取得します。</summary>
		/// 
		/// <value>スプライトの予約数</value>
		public int reservedCount
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>スプライトの最大予約数を取得します。</summary>
		/// 
		/// <value>スプライトの最大予約数</value>
		public int maxReserved
		{
			get;
			private set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// アンマネージ リソースの解放およびリセットに関連付けられている
		/// アプリケーション定義のタスクを実行します。
		/// </summary>
		public void Dispose()
		{
			drawCache.Clear();
			reservedCount = 0;
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
			switch(halign)
			{
				case EAlign.Center:
					origin.X = (float)(srcRect.Width) * 0.5f;
					break;
				case EAlign.RightBottom:
					origin.X = srcRect.Width;
					break;
			}
			switch(valign)
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
			setReserve(info);
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
			if(resolution == null)
			{
				info.scale = scale;
			}
			else
			{
				if(resolution.GetType() == typeResAF || resolution.GetType().IsSubclassOf(typeResAF))
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
			setReserve(info);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>予約された描画処理を実行します。</summary>
		public void draw()
		{
			if(spriteBatch == null)
			{
				throw new InvalidOperationException("描画に使用するSpriteBatchがありません。");
			}
			drawCache.Sort(0, reservedCount, null);
			for(int i=0; i < reservedCount; i++)
			{
				SSpriteDrawInfo info = drawCache[i];
				changeMode(info);
				if(info.spriteFont == null)
				{
					Rectangle rectDst = info.destinationRectangle;
					spriteBatch.Draw(info.texture, rectDst,
						info.sourceRectangle, info.color, info.fRotation,
						info.origin, info.effects, info.fLayerDepth);
				}
				else
				{
					spriteBatch.DrawString(info.spriteFont, info.text,
						info.position, info.color, info.fRotation, info.origin,
						info.scale, info.effects, info.fLayerDepth);
				}
			}
			maxReserved = Math.Max(maxReserved, reservedCount);
			resetMode();
			reservedCount = 0;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画モードを変更します。</summary>
		/// 
		/// <param name="info">次に描画する情報</param>
		private void changeMode(SSpriteDrawInfo info)
		{
			if(!spriteBatch.IsDisposed)
			{
				if(m_drawMode.isBegin && (m_drawMode.blendMode != info.blendMode))
				{
					spriteBatch.End();
					m_drawMode.isBegin = false;
				}
				if(!m_drawMode.isBegin)
				{
					spriteBatch.Begin(info.blendMode, SpriteSortMode.FrontToBack, SaveStateMode.None);
					m_drawMode.isBegin = true;
					m_drawMode.blendMode = info.blendMode;
					spriteBatch.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
					spriteBatch.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画モードを終了します。</summary>
		private void resetMode()
		{
			if(!spriteBatch.IsDisposed && m_drawMode.isBegin)
			{
				spriteBatch.End();
				m_drawMode.isBegin = false;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画予約を設定します。</summary>
		/// 
		/// <param name="info">描画する情報</param>
		private void setReserve(SSpriteDrawInfo info)
		{
			if(reservedCount >= drawCache.Count)
			{
				if(reservedCount < reserveLimit)
				{
					drawCache.Add(info);
				}
			}
			else
			{
				drawCache[reservedCount] = info;
			}
			reservedCount++;
		}
	}
}
