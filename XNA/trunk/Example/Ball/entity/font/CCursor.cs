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
	/// <summary>カーソル オブジェクト。</summary>
	class CCursor : CEntity
	{

		// TODO : 表示だけでなく、中身も実装する。

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CCursor instance = new CCursor();

		private readonly VertexPositionNormalTexture[] vertex = new VertexPositionNormalTexture[4];

		private readonly GraphicsDevice device = CGame.instance.GraphicsDevice;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CCursor()
		{
			vertex[0] = new VertexPositionNormalTexture(new Vector3(1, 0, 0), Vector3.Zero, Vector2.Zero);
			vertex[1] = new VertexPositionNormalTexture(new Vector3(0, 0, 0), Vector3.Zero, Vector2.Zero);
			vertex[2] = new VertexPositionNormalTexture(new Vector3(1, 1, 0), Vector3.Zero, Vector2.Zero);
			vertex[3] = new VertexPositionNormalTexture(new Vector3(0, 1, 0), Vector3.Zero, Vector2.Zero);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(GameTime gameTime)
		{
			base.draw(gameTime);
			device.VertexDeclaration =
				new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
			BasicEffect effect = new BasicEffect(device, null);
			Matrix world = Matrix.Identity;
			Matrix view = Matrix.CreateLookAt(new Vector3(8, 0, 8), Vector3.Zero, new Vector3(0, 1, 0));
			Matrix projection = Matrix.CreatePerspectiveFieldOfView(
				MathHelper.PiOver4, device.Viewport.Width / device.Viewport.Height, 0.1f, 1000f);
			effect.World = world;
			effect.View = view;
			effect.Projection = projection;
			effect.Begin();
			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Begin();
				device.DrawUserPrimitives<VertexPositionNormalTexture>(
					PrimitiveType.TriangleStrip, vertex, 0, 2);
				pass.End();
			}
			effect.End();
		}
	}
}
