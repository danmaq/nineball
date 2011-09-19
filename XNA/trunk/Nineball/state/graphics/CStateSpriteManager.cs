////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.data;
using danmaq.nineball.entity.graphics;
using danmaq.nineball.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.state.graphics
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>スプライト描画管理クラスの既定の状態。</summary>
	public sealed class CStateSpriteManager
		: CState<CSpriteManager, CSpriteManager.CPrivateMembers>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState<CSpriteManager, CSpriteManager.CPrivateMembers> instance =
			new CStateSpriteManager();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateSpriteManager()
		{
		}

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>現在の描画状態。</summary>
		private CSpriteManager.SDrawMode m_drawMode;

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>状態が開始された時に呼び出されます。</para>
		/// <para>このメソッドは、遷移元の<c>teardown</c>よりも後に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されたオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		public override void setup(
			CSpriteManager entity, CSpriteManager.CPrivateMembers privateMembers)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(
			CSpriteManager entity, CSpriteManager.CPrivateMembers privateMembers, GameTime gameTime)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(
			CSpriteManager entity, CSpriteManager.CPrivateMembers privateMembers, GameTime gameTime)
		{
			SpriteBatch spriteBatch = entity.spriteBatch;
			if (spriteBatch == null)
			{
				throw new InvalidOperationException(Resources.DRAW_ERR_SPRITEBATCH);
			}
			List<SSpriteDrawInfo> drawCache = privateMembers.drawCache;
			int length = drawCache.Count;
			privateMembers.drawCache.Sort();

			for (int i = length; --i >= 0; )
			{
				SSpriteDrawInfo info = drawCache[i];
				changeMode(spriteBatch, info);
				if (info.spriteFont == null)
				{
					spriteBatch.Draw(info.texture, info.destinationRectangle,
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
			privateMembers.maxReserved =
				Math.Max(privateMembers.maxReserved, length);
			resetMode(spriteBatch);
			privateMembers.drawCache.Clear();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>オブジェクトが別の状態へ移行する時に呼び出されます。</para>
		/// <para>このメソッドは、遷移先の<c>setup</c>よりも先に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を終了したオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="nextState">オブジェクトが次に適用する状態。</param>
		public override void teardown(
			CSpriteManager entity, CSpriteManager.CPrivateMembers privateMembers, IState nextState)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画モードを変更します。</summary>
		/// 
		/// <param name="spriteBatch">スプライト バッチ。</param>
		/// <param name="info">次に描画する情報</param>
		private void changeMode(SpriteBatch spriteBatch, SSpriteDrawInfo info)
		{
			if (!spriteBatch.IsDisposed)
			{
				if (m_drawMode.isBegin && (m_drawMode.blendMode != info.blendMode))
				{
					spriteBatch.End();
					m_drawMode.isBegin = false;
				}
				if (!m_drawMode.isBegin)
				{
					spriteBatch.Begin(info.blendMode, SpriteSortMode.Immediate, SaveStateMode.None);
					m_drawMode.isBegin = true;
					m_drawMode.blendMode = info.blendMode;
					spriteBatch.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
					spriteBatch.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画モードを終了します。</summary>
		/// 
		/// <param name="spriteBatch">スプライト バッチ。</param>
		private void resetMode(SpriteBatch spriteBatch)
		{
			if (!spriteBatch.IsDisposed && m_drawMode.isBegin)
			{
				spriteBatch.End();
				m_drawMode.isBegin = false;
			}
		}
	}
}
