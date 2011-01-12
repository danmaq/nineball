////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.ball.core;
using danmaq.ball.data;
using danmaq.ball.entity.font;
using danmaq.nineball.entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.cursor.view
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>カーソル点灯表示状態。</summary>
	sealed class CAIVisible
		: CAIBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CAIBase instance = new CAIVisible();

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
		private CAIVisible()
		{
			device.VertexDeclaration =
				new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
			effect = CONTENT.fxCursor;
			effect.Parameters["View"].SetValue(Matrix.CreateLookAt(
				Vector3.Backward, Vector3.Zero, Vector3.Up));
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
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(CEntity entity, CCursor privateMembers, GameTime gameTime)
		{
			Matrix world = privateMembers.world;
			effect.Parameters["World"].SetValue(world);
			effect.Begin();
			EffectPassCollection passes = effect.CurrentTechnique.Passes;
			for (int i = passes.Count; --i >= 0; )
			{
				EffectPass pass = passes[i];
				pass.Begin();
				device.DrawUserPrimitives<VertexPositionNormalTexture>(
					PrimitiveType.TriangleStrip, vertex, 0, 2);
				pass.End();
			}
			effect.End();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>カーソルの明滅を切り替えます。</summary>
		/// 
		/// <returns>消灯状態。</returns>
		protected override CAIBase onBlink()
		{
			return CAIHidden.instance;
		}
	}
}
