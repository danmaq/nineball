////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.ball.core;
using danmaq.ball.data;
using danmaq.ball.entity;
using danmaq.ball.Properties;
using danmaq.ball.state.ball;
using danmaq.nineball.data;
using danmaq.nineball.entity;
using danmaq.nineball.entity.input;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.scene
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲーム画面シーン。</summary>
	sealed class CSceneGame
		: CSceneBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState<CEntity, CGame> instance = new CSceneGame();

		/// <summary>入力状態。</summary>
		private readonly IList<SInputInfo> inputData = CInput.instance.collection.buttonList;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CSceneGame()
			: base(Resources.SCENE_GAME)
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
			CBall.enemy.nextState = CStateEnemy.instance;
			CBall.player.nextState = CStatePlayer.instance;
			taskManager.Add(CBall.enemy);
			taskManager.Add(CBall.player);
			print(new Point(40, 20), EAlign.Center, Color.DarkRed, Resources.ROLL_SPACE);
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
			if (CBall.enemy.goal || CBall.player.goal)
			{
				entity.nextState = CBall.player.position.X > CBall.enemy.position.X ?
					CSceneJudge.won : CSceneJudge.lose;
			}
			if (inputData[(int)EInputActionMap.cancel].push)
			{
				entity.nextState = CSceneMenu.instance;
			}
		}
	}
}
