////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.entity.fonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.state.fonts {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>グラデーション付きのフォント状態。</summary>
	public sealed class CStateGradation : CState<CFont, object> {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateGradation instance = new CStateGradation();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateGradation() { }

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw( CFont entity, object privateMembers, GameTime gameTime ) {
			if( entity.sprite != null && entity.font != null ) {
				SFontGradationInfo[] _gradation = createGradation( entity );
				Vector2 origin = getOrigin( entity, _gradation );
				Vector2 _pos;
				float fLayer;
				float fShadowLayer;
				entity.getShadowLayer( out fLayer, out fShadowLayer );
				foreach( SFontGradationInfo info in _gradation ) {
					_pos = entity.pos + info.pos - origin;
					if( entity.isDrawShadow ) {
						entity.sprite.add( entity.font, info.strByte, _pos + entity.gapShadow,
							info.argbShadow, info.rotate, Vector2.Zero,
							info.scale, SpriteEffects.None, fShadowLayer );
					}
					entity.sprite.add( entity.font, info.strByte, _pos, info.argbText,
						info.rotate, Vector2.Zero, info.scale, SpriteEffects.None, fLayer );
				}
			}
			base.draw( entity, privateMembers, gameTime );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// レンダリングされたグラデーション情報より原点を算出します。
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="gradation">レンダリングされたグラデーション情報</param>
		/// <returns>原点座標</returns>
		private Vector2 getOrigin( CFont entity, IEnumerable<SFontGradationInfo> gradation ) {
			Vector2 min = new Vector2( float.MaxValue, float.MaxValue );
			Vector2 max = new Vector2( float.MinValue, float.MinValue );
			foreach( SFontGradationInfo info in gradation ) {
				min = Vector2.Min( min, info.pos );
				max = Vector2.Max( max, info.pos + info.charSize * info.scale );
			}
			return entity.getOrigin( max - min );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// レンダリングされたグラデーション情報を作成します。
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <returns>レンダリングされたグラデーション情報。</returns>
		private SFontGradationInfo[] createGradation( CFont entity ) {
			int nSize = entity.text.Length;
			char[] szText = entity.text.ToCharArray();
			SFontGradationInfo[] result = new SFontGradationInfo[nSize];
			float fNowX = 0;
			for( int i = 0; i < nSize; i++ ) {
				float fAlpha = entity.colorAlpha.smooth( i, nSize );
				result[i].pos = new Vector2( entity.gapX.smooth( i, nSize ) + fNowX, entity.gapY.smooth( i, nSize ) );
				result[i].scale = new Vector2( entity.scaleX.smooth( i, nSize ), entity.scaleY.smooth( i, nSize ) );
				result[i].rotate = entity.rotate.smooth( i, nSize );
				result[i].strByte = szText[i].ToString();
				result[i].charSize = entity.font.MeasureString( result[i].strByte );
				result[i].argbText = new Color(
					( byte )entity.colorRed.smooth( i, nSize ), ( byte )entity.colorGreen.smooth( i, nSize ),
					( byte )entity.colorBlue.smooth( i, nSize ), ( byte )fAlpha );
				if( entity.isDrawShadow ) {
					result[i].argbShadow = new Color( 0, 0, 0, ( byte )( fAlpha / 1.5f ) );
				}
				fNowX += result[i].charSize.X * result[i].scale.X * entity.pitch.smooth( i, nSize );
			}
			return result;
		}
	}
}
