////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.data;
using danmaq.nineball.entity;
using danmaq.nineball.entity.component;
using danmaq.nineball.entity.manager;
using danmaq.nineball.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using danmaq.nineball.util.collection;
using System;

#if XBOX360
using Microsoft.Xna.Framework.GamerServices;
#endif

namespace danmaq.nineball.state.manager {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>メインループクラス用の既定の状態です。</summary>
	public sealed class CStateMainLoopDefault : CState<CMainLoop, Game> {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateMainLoopDefault instance = new CStateMainLoopDefault();

		/// <summary>フェーズ・カウンタ進行管理クラス。</summary>
		public readonly CPhase phase = new CPhase();

		/// <summary>シーン オブジェクト。</summary>
		public readonly CEntity scene = new CEntity();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>背景色。</summary>
		public Color colorBack = Color.CornflowerBlue;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateMainLoopDefault() { }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>グラフィック デバイスの構成・管理クラスを取得します。</summary>
		/// 
		/// <value>グラフィック デバイスの構成・管理クラス。</value>
		public GraphicsDeviceManager graphicsDeviceManager { get; set; }

		//* -----------------------------------------------------------------------*
		/// <summary>スプライト描画管理クラスを取得します。</summary>
		/// 
		/// <value>スプライト描画管理クラス。</value>
		public CSprite sprite { get; private set; }

		//* -----------------------------------------------------------------------*
		/// <summary>オブジェクトにアタッチされたゲームを取得します。</summary>
		/// 
		/// <value>オブジェクトにアタッチされたゲーム。</value>
		public Game game { get; private set; }

		//* -----------------------------------------------------------------------*
		/// <summary>登録されているゲーム コンポーネント一覧を取得します。</summary>
		/// 
		/// <value>登録されているゲーム コンポーネント一覧。</value>
		public CDisposablePartialCollection<IGameComponent, GameComponent> registedGameComponentList {
			get; private set;
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
		/// <param name="game">オブジェクトにアタッチされたゲーム。</param>
		public override void setup( CMainLoop entity, Game game ) {
			registedGameComponentList =
				new CDisposablePartialCollection<IGameComponent, GameComponent>( game.Components );
#if XBOX360
			registedGameComponentList.Add( new GamerServicesComponent( game ) );
			registedGameComponentList.Add(
				new CGameComponent<CEntity>( game, new CEntity( CStateGuideHelper.instance ), false ) );
#endif
			registedGameComponentList.Add( new CDrawableGameComponent<CEntity>(
				game, new CEntity( CStateFPSCalculator.instance ), false ) );
			sprite = new CSprite( new SpriteBatch( game.GraphicsDevice ) );
			this.game = game;
			game.Content.RootDirectory = "Content";
			scene.initialize();
			base.setup( entity, game );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="game">オブジェクトにアタッチされたゲーム。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update( CMainLoop entity, Game game, GameTime gameTime ) {
			scene.update( gameTime );
			phase.count++;
			base.update( entity, game, gameTime );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="game">オブジェクトにアタッチされたゲーム。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw( CMainLoop entity, Game game, GameTime gameTime ) {
			scene.draw( gameTime );
			GraphicsDevice device = game.GraphicsDevice;
			device.Clear( colorBack );
			device.RenderState.DepthBufferEnable = true;
			device.RenderState.DepthBufferWriteEnable = true;
			sprite.update();
			base.draw( entity, game, gameTime );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>オブジェクトが別の状態へ移行する時に呼び出されます。</para>
		/// <para>このメソッドは、遷移先の<c>setup</c>よりも先に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を終了したオブジェクト。</param>
		/// <param name="game">オブジェクトにアタッチされたゲーム。</param>
		/// <param name="nextState">オブジェクトが次に適用する状態。</param>
		public override void teardown(
			CMainLoop entity, Game game, IState<CMainLoop, Game> nextState
		) {
			teardown( game );
			base.teardown( entity, game, nextState );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>オブジェクトが<c>CState.empty</c>へ移行する時に呼び出されます。</para>
		/// <para>このメソッドは、遷移先の<c>setup</c>よりも先に呼び出されます。</para>
		/// </summary>
		/// <remarks>
		/// このメソッドが呼び出された時は、通常オブジェクトが終了したことを意味します。
		/// </remarks>
		/// 
		/// <param name="entity">この状態を終了したオブジェクト。</param>
		/// <param name="game">オブジェクトにアタッチされたゲーム。</param>
		public override void teardown( CMainLoop entity, Game game ) {
			teardown( game );
			base.teardown( entity, game );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>オブジェクトが別の状態へ移行する時に呼び出されます。</summary>
		/// 
		/// <param name="game">オブジェクトにアタッチされたゲーム。</param>
		private void teardown( Game game ) {
			scene.Dispose();
			sprite.Dispose();
			registedGameComponentList.Dispose();
		}
	}
}
