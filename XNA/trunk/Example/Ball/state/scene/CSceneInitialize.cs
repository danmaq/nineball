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
using danmaq.ball.Properties;
using danmaq.ball.state.scene.initialize;
using danmaq.nineball.data;
using danmaq.nineball.entity;
using danmaq.nineball.state;
using danmaq.nineball.state.misc;
using danmaq.nineball.util;
using Microsoft.Xna.Framework;

namespace danmaq.ball.state.scene
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>初期化シーン。</summary>
	sealed class CSceneInitialize
		: CState<CEntity, CGame>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState instance = new CSceneInitialize();

		/// <summary>初期化用AI。</summary>
		private CEntity aiInitializer = new CEntity();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CSceneInitialize()
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
			CLogger.add(Resources.SCENE_INITIALIZE);
			entity.allowSameState = true;
			CGame.instance.Content.RootDirectory = Resources.DIR_CONTENT;
			CStateCapsXNA xnastate = CStateCapsXNA.instance;
			xnastate.nextState = CAIInupt.instance;
			aiInitializer.nextState = xnastate;
			aiInitializer.changedState += onInitializerChanged;
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
			aiInitializer.update(gameTime);
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
			aiInitializer.draw(gameTime);
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
		public override void teardown(CEntity entity, CGame privateMembers, IState nextState)
		{
			CGame.instance.Window.Title += " " + CGame.version;
			CLogger.add(CStateCapsXNA.instance.report);
			CLogger.add("全ての初期化が完了しました。ゲームを起動します...");
			aiInitializer.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>初期化の状態が変化したときに呼び出されるメソッドです。</summary>
		/// 
		/// <param name="sender">送信元。</param>
		/// <param name="e">イベントの情報。</param>
		private void onInitializerChanged(object sender, CEventChangedState e)
		{
			if (e.next == CState.empty)
			{
				CGame.instance.scene.nextState = CSceneCredit.instance;
			}
		}
	}
}
