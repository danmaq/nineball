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
using danmaq.nineball.data;
using danmaq.nineball.entity;
using danmaq.nineball.entity.input;
using danmaq.nineball.state;
using danmaq.nineball.state.manager;
using danmaq.nineball.util;
using danmaq.nineball.util.collection;
using Microsoft.Xna.Framework;

namespace danmaq.ball.state.scene {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>シーン基底クラス。</summary>
	public abstract class CSceneBase : CState {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>シーンの名前。</summary>
		public readonly string sceneName;

		/// <summary>ゲーム オブジェクト。</summary>
		protected readonly CGame game = CGame.instance;

		/// <summary>フェーズ・カウンタ管理クラス。</summary>
		protected readonly CPhase localPhaseManager = new CPhase();

		/// <summary>ゲーム コンポーネント管理クラス。</summary>
		protected readonly CGameComponentManager localGameComponentManager;

		/// <summary>グラフィック デバイスの構成・管理クラス。</summary>
		protected readonly GraphicsDeviceManager graphicDeviceManager;

		/// <summary>入力管理クラス。</summary>
		protected readonly CInput inputManager;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="sceneName">シーン名称。</param>
		protected CSceneBase( string sceneName ) {
			this.sceneName = sceneName;
			inputManager = game.inputManager;
			graphicDeviceManager = game.graphicDeviceManager;
			localGameComponentManager = new CGameComponentManager( game.Components );
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>ゲーム共通のフェーズ・カウンタ管理クラスを取得します。</summary>
		/// 
		/// <value>ゲーム共通のフェーズ・カウンタ管理クラス。</value>
		protected CPhase systemPhaseManager {
			get { return CStateMainLoopDefault.instance.phase; }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ゲーム共通のスプライト バッチ管理クラスを取得します。</summary>
		/// 
		/// <value>ゲーム共通のスプライト バッチ管理クラス。</value>
		protected CSprite systemSpriteManager {
			get { return CStateMainLoopDefault.instance.sprite; }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ゲーム共通のゲーム コンポーネント管理クラスを取得します。</summary>
		/// 
		/// <value>ゲーム共通のゲーム コンポーネント管理クラス。</value>
		protected CGameComponentManager systemGameComponentManager {
			get { return CStateMainLoopDefault.instance.registedGameComponentList; }
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
		public override void setup( IEntity entity, object privateMembers ) {
			CLogger.add( sceneName + "シーンを開始します。" );
			base.setup( entity, privateMembers );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update( IEntity entity, object privateMembers, GameTime gameTime ) {
			localPhaseManager.count++;
			base.update( entity, privateMembers, gameTime );
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
		public override void teardown( IEntity entity, object privateMembers, IState nextState ) {
			CLogger.add( sceneName + "シーンを終了します。" );
			localPhaseManager.reset();
			localGameComponentManager.Dispose();
			base.teardown( entity, privateMembers, nextState );
		}
	}
}
