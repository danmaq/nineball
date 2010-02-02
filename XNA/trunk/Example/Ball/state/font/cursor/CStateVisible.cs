////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.ball.core;
using danmaq.ball.entity.font;
using danmaq.ball.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.font.cursor
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>カーソル点灯状態。</summary>
	public sealed class CStateVisible : CStateBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateVisible instance = new CStateVisible();

		/// <summary>カーソル ポリゴンのための頂点情報。</summary>
		private readonly VertexPositionNormalTexture[] vertex = {
			new VertexPositionNormalTexture(new Vector3(16, 0, 0), Vector3.Zero, Vector2.Zero),
			new VertexPositionNormalTexture(new Vector3(0, 0, 0), Vector3.Zero, Vector2.Zero),
			new VertexPositionNormalTexture(new Vector3(16, 16, 0), Vector3.Zero, Vector2.Zero),
			new VertexPositionNormalTexture(new Vector3(0, 16, 0), Vector3.Zero, Vector2.Zero),
		};

		/// <summary>グラフィック デバイス。</summary>
		private readonly GraphicsDevice device = CGame.instance.GraphicsDevice;

		/// <summary>シェーダ プログラム読み込みのためのコンテンツ管理クラス。</summary>
		private readonly ContentManager contentManager = CGame.instance.Content;

		/// <summary>カーソル表示のためのシェーダ。</summary>
		private readonly Effect effect;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateVisible()
		{
			device.VertexDeclaration =
				new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
			effect = contentManager.Load<Effect>(Resources.FX_CURSOR);
			effect.Parameters["View"].SetValue(Matrix.CreateLookAt(
				new Vector3(0, 0, 1), Vector3.Zero, new Vector3(0, 1, 0)));
			effect.Parameters["Projection"].SetValue(Matrix.CreateOrthographic(
				device.Viewport.Width, device.Viewport.Height, 0.1f, 1000f));
			effect.CurrentTechnique = effect.Techniques["XORTechnique"];
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="world">カーソルの3D位置を示すワールド行列。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(CCursor entity, Matrix world, GameTime gameTime)
		{
			effect.Parameters["World"].SetValue(world);
			effect.Begin();
			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Begin();
				device.DrawUserPrimitives<VertexPositionNormalTexture>(
					PrimitiveType.TriangleStrip, vertex, 0, 2);
				pass.End();
			}
			effect.End();
			base.draw(entity, world, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>カーソルの明滅を切り替えます。</summary>
		/// 
		/// <returns>消灯状態。</returns>
		protected override CStateBase onBlink()
		{
			return CStateHidden.instance;
		}
	}
}
