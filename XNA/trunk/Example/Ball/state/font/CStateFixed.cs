////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.ball.misc;
using danmaq.nineball.data;
using danmaq.nineball.entity.fonts;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.font {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>よゆ風固定ピッチフォント用の状態。</summary>
	public sealed class CStateFixed : CState<CFont, object> {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateFixed instance = new CStateFixed();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateFixed() { }

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
				Vector2 pos = entity.pos;
				pos.X += getOriginX( entity );
				entity.sprite.add( entity.font, entity.text, CMisc.Cursor2VGA( pos ),
					new Color(
						( byte )entity.colorRed, ( byte )entity.colorGreen,
						( byte )entity.colorBlue, ( byte )entity.colorAlpha ),
					0.0f, Vector2.Zero, entity.scale, SpriteEffects.None, entity.layer );
			}
			base.draw( entity, privateMembers, gameTime );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>フォント情報より原点X座標を算出します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <returns>原点X座標</returns>
		private int getOriginX( CFont entity ) {
			int nResult = 0;
			int nWidth = ( int )( entity.font.MeasureString( entity.text ) / 8 ).X;
			switch( entity.alignHorizontal ) {
				case EAlign.LeftTop:
					nResult = 0;
					break;
				case EAlign.Center:
					nResult = nWidth / 2;
					break;
				case EAlign.RightBottom:
					nResult = nWidth;
					break;
			}
			return nResult;
		}
	}
}
