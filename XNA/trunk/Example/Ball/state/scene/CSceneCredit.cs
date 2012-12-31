////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using danmaq.ball.core;
using danmaq.ball.data;
using danmaq.ball.misc;
using danmaq.ball.Properties;
using danmaq.nineball.data;
using danmaq.nineball.entity;
using danmaq.nineball.entity.manager;
using danmaq.nineball.state;
using danmaq.nineball.util.math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.scene
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>クレジット画面シーン。</summary>
	sealed class CSceneCredit : CSceneBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState<CEntity, CGame> instance = new CSceneCredit();

		/// <summary>コルーチン管理クラス。</summary>
		private readonly CCoRoutineManager mgrCo = new CCoRoutineManager();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>透明度。</summary>
		private byte alpha = 0;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CSceneCredit()
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
		public override void setup(CEntity entity, CGame privateMembers)
		{
			base.setup(entity, privateMembers);
			alpha = 0;
			mgrCo.Add(coAlpha());
			CPresenceSender.sendAtCredit();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(CEntity entity, CGame privateMembers, GameTime gameTime)
		{
			mgrCo.update(gameTime);
			if (mgrCo.Count == 0)
			{
				entity.nextState = CSceneMenu.instance;
			}
			base.update(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(CEntity entity, CGame privateMembers, GameTime gameTime)
		{
			mgrCo.draw(gameTime);
			CGame.sprite.add(CONTENT.texLogo, new Vector2(320, 200), EAlign.Center, EAlign.Center,
				new Rectangle(0, 0, 384, 384), new Color(Color.White, alpha), 0f,
				SpriteBlendMode.AlphaBlend);
			base.draw(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>透明度を設定するコルーチンです。</summary>
		/// 
		/// <returns>コルーチン用オブジェクト。実行時は常時<c>null</c>。</returns>
		private IEnumerator coAlpha()
		{
			const int FADETIME = 30;
			for(
				int i = 0; i < FADETIME;
				alpha = interpolate(0, 255, ++i, FADETIME)
			)
			{
				yield return null;
			}
			for(int i = 0; i < 90; i++)
			{
				yield return null;
			}
			for(
				int i = 0; i < FADETIME;
				alpha = interpolate(255, 0, ++i, FADETIME)
			)
			{
				yield return null;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>加速変化する内分カウンタです。</summary>
		/// 
		/// <param name="start"><paramref name="now"/>が0と等しい場合の値</param>
		/// <param name="end">
		/// <paramref name="now"/>が<paramref name="limit"/>と等しい場合の値
		/// </param>
		/// <param name="now">現在時間</param>
		/// <param name="limit"><paramref name="end"/>に到達する時間</param>
		/// <returns>
		/// 0から<paramref name="limit"/>までの<paramref name="now"/>に相当する
		/// <paramref name="start"/>から<paramref name="end"/>までの値
		/// </returns>
		private byte interpolate(float start, float end, float now, float limit)
		{
			float result = CInterpolate._clampAccelerate(start, end, now, limit);
			return (byte)((int)(result / 16) * 16);
		}
	}
}
