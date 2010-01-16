////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using danmaq.ball.core;

namespace danmaq.ball.entity.font
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>カーソル。</summary>
	class CCursor : CEntity
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CCursor instance = new CCursor();

		private readonly VertexPositionNormalTexture[] vertex = new VertexPositionNormalTexture[3];

		private readonly GraphicsDevice device = CGame.instance.GraphicsDevice;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CCursor()
		{
			vertex[0] = new VertexPositionNormalTexture(new Vector3(0, 1, 0), Vector3.Zero, Vector2.Zero);
			vertex[1] = new VertexPositionNormalTexture(new Vector3(1, 0, 0), Vector3.Zero, Vector2.Zero);
			vertex[2] = new VertexPositionNormalTexture(new Vector3(0, 0, 1), Vector3.Zero, Vector2.Zero);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(GameTime gameTime)
		{
			base.draw(gameTime);
			device.VertexDeclaration = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
		}
	}
}
