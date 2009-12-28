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
using danmaq.nineball.util.resolution;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.util {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>スプライト描画管理 クラス。</summary>
	public sealed class CSprite : IDisposable {

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>描画情報が格納された基本クラス。</summary>
		public abstract class CDrawInfoBase : IComparable<CDrawInfoBase> {

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>優先度比較の際のコールバック先。</summary>
			protected Func<CDrawInfoBase, int> onCompare = null;

			/// <summary>文字列かどうか。</summary>
			protected bool bString = false;

			/// <summary>合成モード。</summary>
			public SpriteBlendMode blendMode = SpriteBlendMode.None;

			/// <summary>乗算色。</summary>
			public Color color = Color.White;

			/// <summary>回転(ラジアン)。</summary>
			public float fRotation = 0.0f;

			/// <summary>原点座標。</summary>
			public Vector2 origin = Vector2.Zero;

			/// <summary>エフェクト種類。</summary>
			public SpriteEffects effects = SpriteEffects.None;

			/// <summary>レイヤ深度。</summary>
			public float fLayerDepth = 0.0f;

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
			public int CompareTo( CDrawInfoBase other ) {
				int nResult = Math.Sign( other.fLayerDepth - fLayerDepth );
				if( nResult == 0 ) {
					if( !bString && other.bString ) { nResult = -1; }
					else if( bString && !other.bString ) { nResult = 1; }
					else if( blendMode < other.blendMode ) { nResult = -1; }
					else if( blendMode > other.blendMode ) { nResult = 1; }
					else { nResult = onCompare( other ); }
				}
				return nResult;
			}
		}

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>画像用描画情報が格納されたクラス。</summary>
		public sealed class CDrawInfo : CDrawInfoBase {

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>テクスチャ リソース。</summary>
			public Texture2D texture;

			/// <summary>描画先の矩形情報。</summary>
			public Rectangle destinationRectangle;

			/// <summary>描画元の矩形情報。</summary>
			public Rectangle sourceRectangle;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			public CDrawInfo() {
				onCompare = info => Math.Sign(
					texture.GetHashCode() - ( ( CDrawInfo )info ).texture.GetHashCode() );
				bString = false;
			}
		}

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>文字用描画情報が格納されたクラス。</summary>
		public sealed class CDrawStringInfo : CDrawInfoBase {

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

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
			/// <summary>コンストラクタ。</summary>
			public CDrawStringInfo() {
				onCompare = info => text.CompareTo( ( ( CDrawStringInfo )info ).text );
				bString = true;
				blendMode = SpriteBlendMode.AlphaBlend;
			}
		}

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>現在の描画状態が格納された構造体。</summary>
		public struct SDrawMode {

			/// <summary>描画中かどうか。</summary>
			public bool isBegin;

			/// <summary>合成モード。</summary>
			public SpriteBlendMode blendMode;
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>描画情報リスト。</summary>
		private readonly List<CDrawInfoBase> drawList = new List<CDrawInfoBase>();

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
		/// 
		/// <param name="spriteBatch">スプライトバッチ</param>
		public CSprite( SpriteBatch spriteBatch ) { this.spriteBatch = spriteBatch; }

		//* -----------------------------------------------------------------------*
		/// <summary>デストラクタ。</summary>
		~CSprite() { Dispose(); }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>スプライトバッチ。</summary>
		public SpriteBatch spriteBatch { get; private set; }

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// アンマネージ リソースの解放およびリセットに関連付けられている
		/// アプリケーション定義のタスクを実行します。
		/// </summary>
		public void Dispose() {
			drawList.Clear();
			GC.SuppressFinalize( this );
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
		) { add( tex, rect, rect, color, 0, Vector2.Zero, SpriteEffects.None, fLayer, blend ); }

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
		) { add( tex, pos, halign, valign, srcRect, color, 0, Vector2.One, SpriteEffects.None, fLayer, blend ); }

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
		) {
			Vector2 origin = Vector2.Zero;
			switch( halign ) {
				case EAlign.Center:
					origin.X = ( float )( srcRect.Width ) * 0.5f;
					break;
				case EAlign.RightBottom:
					origin.X = srcRect.Width;
					break;
			}
			switch( valign ) {
				case EAlign.Center:
					origin.Y = ( float )( srcRect.Height ) * 0.5f;
					break;
				case EAlign.RightBottom:
					origin.Y = srcRect.Height;
					break;
			}
			add( tex, new Rectangle( ( int )( pos.X ), ( int )( pos.Y ),
					( int )( srcRect.Width * scale.X ), ( int )( srcRect.Height * scale.Y ) ),
				srcRect, color, fRotate, origin, effects, fLayer, blend );
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
		) {
			add( tex, dstRect, srcRect, color, 0,
				Vector2.Zero, SpriteEffects.None, fLayer, blend );
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
		) {
			CDrawInfo info = new CDrawInfo();
			info.texture = tex;
			info.destinationRectangle = resolution == null ?
				dstRect : resolution.resizeFromVGA( dstRect );
			info.sourceRectangle = srcRect;
			info.color = color;
			info.fRotation = fRotate -
				( resolution != null && resolution.vertical ? 0 : 0 );
			info.origin = origin;
			info.effects = effects;
			info.fLayerDepth = fLayer;
			info.blendMode = blend;
			drawList.Add( info );
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
		public void add(
			SpriteFont spriteFont, string text, Vector2 pos, Color color, float fRotate,
			Vector2 origin, Vector2 scale, SpriteEffects effects, float fLayer
		) {
			CDrawStringInfo info = new CDrawStringInfo();
			info.spriteFont = spriteFont;
			info.text = text;
			info.position = resolution == null ? pos : resolution.resizeFromVGA( pos );
			info.color = color;
			info.fRotation = fRotate;
			info.origin = origin;
			if( resolution == null ) { info.scale = scale; }
			else {
				Type typeResAF = typeof( CResolutionAspectFix );
				if( resolution.GetType() == typeResAF || resolution.GetType().IsSubclassOf( typeResAF ) ) {
					CResolutionAspectFix res = ( CResolutionAspectFix )resolution;
					info.scale = scale * res.scaleGapFromVGA;
				}
				else { info.scale = scale * resolution.scaleGapFromVGA; }
			}
			info.effects = effects;
			info.fLayerDepth = fLayer;
			drawList.Add( info );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>予約された描画処理を実行します。</summary>
		public void update() { update( Vector2.Zero ); }

		//* -----------------------------------------------------------------------*
		/// <summary>予約された描画処理を実行します。</summary>
		/// 
		/// <param name="gap">描画座標誤差</param>
		public void update( Vector2 gap ) {
			drawList.Sort();
			for( int i=0; i < drawList.Count; i++ ) {
				CDrawInfoBase __info = drawList[i];
				changeMode( __info );
				if( __info.GetType() == typeof( CDrawInfo ) ) {
					CDrawInfo info = ( CDrawInfo )__info;
					Rectangle rectDst = info.destinationRectangle;
					rectDst.X += ( int )gap.X;
					rectDst.Y += ( int )gap.Y;
					spriteBatch.Draw( info.texture, rectDst,
						info.sourceRectangle, info.color, info.fRotation,
						info.origin, info.effects, info.fLayerDepth );
				}
				else {
					CDrawStringInfo info = ( CDrawStringInfo )__info;
					spriteBatch.DrawString( info.spriteFont, info.text,
						info.position + gap, info.color, info.fRotation, info.origin,
						info.scale, info.effects, info.fLayerDepth );
				}
			}
			changeMode( null );
			drawList.Clear();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画モードを変更します。</summary>
		/// <remarks>引数に<c>null</c>を渡すことで描画処理を終了します。</remarks>
		/// 
		/// <param name="info">次に描画する情報</param>
		private void changeMode( CDrawInfoBase info ) {
			if( !spriteBatch.IsDisposed ) {
				if( m_drawMode.isBegin && ( info == null || m_drawMode.blendMode != info.blendMode ) ) {
					spriteBatch.End();
					m_drawMode.isBegin = false;
				}
				if( info != null && !m_drawMode.isBegin ) {
					spriteBatch.Begin( info.blendMode, SpriteSortMode.FrontToBack, SaveStateMode.None );
					m_drawMode.isBegin = true;
					m_drawMode.blendMode = info.blendMode;
					spriteBatch.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
					spriteBatch.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
				}
			}
		}
	}
}
