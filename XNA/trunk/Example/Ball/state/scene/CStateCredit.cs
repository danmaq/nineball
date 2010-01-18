////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using danmaq.ball.Properties;
using danmaq.nineball.data;
using danmaq.nineball.entity;
using danmaq.nineball.misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.scene
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>クレジット画面シーン。</summary>
	public sealed class CStateCredit : CSceneBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateCredit instance = new CStateCredit();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>透明度。</summary>
		private float m_fAlpha = 0;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateCredit()
			: base(Resources.SCENE_CREDIT)
		{
		}

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
		public override void setup(IEntity entity, object privateMembers)
		{
			base.setup(entity, privateMembers);
			m_fAlpha = 0;
			localCoRoutineManager.add(coAlpha());
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(IEntity entity, object privateMembers, GameTime gameTime)
		{
			base.update(entity, privateMembers, gameTime);
			if(localPhaseManager.phase == 1)
			{
				entity.nextState = CStateTitle.instance;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(IEntity entity, object privateMembers, GameTime gameTime)
		{
			base.draw(entity, privateMembers, gameTime);
			systemSpriteManager.add(contentManager.Load<Texture2D>(Resources.IMAGE_LOGO),
				new Vector2(320, 240), EAlign.Center, EAlign.Center,
				new Rectangle(0, 0, 384, 384), new Color(Color.White, m_fAlpha), 0f,
				SpriteBlendMode.AlphaBlend);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		private IEnumerator coAlpha()
		{
			const int FADETIME = 60;
			for(
				int i = 0; i < FADETIME;
				m_fAlpha = CInterpolate._clampAccelerate(0, 1, ++i, FADETIME)
			)
			{
				yield return null;
			}
			for(int i = 0; i < 120; i++)
			{
				yield return null;
			}
			for(
				int i = 0; i < FADETIME;
				m_fAlpha = CInterpolate._clampAccelerate(1, 0, ++i, FADETIME)
			)
			{
				yield return null;
			}
			localPhaseManager.reserveNextPhase = true;
		}
	}
}
