////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2011 danmaq all rights reserved.
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
using danmaq.nineball.entity.fonts;
using danmaq.nineball.entity.manager;
using danmaq.nineball.state;
using danmaq.nineball.state.manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.scene
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲーム開始前カウントダウンのシーン。</summary>
	sealed class CSceneCountdown
		: CSceneBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState<CEntity, CGame> instance = new CSceneCountdown();

		/// <summary>コルーチン管理クラス。</summary>
		private readonly CCoRoutineManager mgrCo = new CCoRoutineManager();

		/// <summary>カウントダウン表示用フォント。</summary>
		private readonly CFont countdown = new CFont(CONTENT.texFont98);

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CSceneCountdown()
			: base(Resources.SCENE_COUNTDOWN)
		{
			countdown.pos = new Vector2(320, 200);
			countdown.isDrawShadow = false;
			countdown.sprite = CGame.sprite;
			countdown.scale = new Vector2(20);
			countdown.color = Color.Black;
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
			CGame.instance.bgColor = Color.Silver;
			countdown.gradationMode = false;
			mgrCo.nextState = CStateCoRoutineManager.instance;
			mgrCo.Add(coCountdown());
			taskManager.Add(countdown);
			taskManager.Add(mgrCo);
			print(new Point(40, 20), EAlign.Center, Color.DarkRed, Resources.ROLL_SPACE);
			CPresenceSender.sendAtGame();
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
			base.update(entity, privateMembers, gameTime);
			if (mgrCo.Count == 0)
			{
				entity.nextState = CSceneGame.instance;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>透明度を設定するコルーチンです。</summary>
		/// 
		/// <returns>コルーチン用オブジェクト。実行時は常時<c>null</c>。</returns>
		private IEnumerator coCountdown()
		{
			for (int i = 3; i > 0; i--)
			{
				countdown.text = i.ToString();
				for (int j = 60; --j >= 0; )
				{
					yield return null;
				}
			}
		}
	}
}
