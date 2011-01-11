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
using danmaq.ball.entity.font;
using danmaq.ball.Properties;
using danmaq.ball.state.font.cursor;
using danmaq.nineball.data;
using danmaq.nineball.entity;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.scene
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>難易度選択シーン。</summary>
	sealed class CStateMenu
		: CSceneBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState<CEntity, CGame> instance = new CStateMenu();

		/// <summary>難易度メニュー。</summary>
		private readonly string menu =
			string.Format("１{0}２{0}３{0}４{0}５{0}６{0}７{0}８{0}９", "      ");

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateMenu()
			: base(Resources.SCENE_TITLE)
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
			print(new Point(40, 7), EAlign.Center, Color.Aqua, CGame.name);
			print(new Point(40, 9), EAlign.Center, Color.Aqua, Resources.CREDIT);
			print(new Point(6, 14), EAlign.LeftTop, Color.White, Resources.DESC_LEVEL);
			print(new Point(6, 16), EAlign.LeftTop, Color.White, menu);
			CCursor.instance.nextState = CStateCursor.instance;
			CCursor.instance.changedState += onCursorChanged;
			taskManager.Add(CCursor.instance);
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
		}

		//* -----------------------------------------------------------------------*
		/// <summary>初期化の状態が変化したときに呼び出されるメソッドです。</summary>
		/// 
		/// <param name="sender">送信元。</param>
		/// <param name="e">イベントの情報。</param>
		private void onCursorChanged(object sender, CEventChangedState e)
		{
			if (e.next == CState.empty)
			{
				CGame.instance.scene.nextState = CSceneCountdown.instance;
			}
		}
	}
}
