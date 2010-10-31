////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.data.phase;
using danmaq.nineball.entity;
using danmaq.nineball.entity.component;
using danmaq.nineball.entity.manager;
using danmaq.nineball.util;
using danmaq.nineball.util.collection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#if XBOX360
using Microsoft.Xna.Framework.GamerServices;
#endif

namespace danmaq.nineball.state.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>メインループクラス用の既定の状態です。</summary>
	public sealed class CStateMainLoopDefault : CState<CMainLoop, CMainLoop.CPrivateMembers>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateMainLoopDefault instance = new CStateMainLoopDefault();

		/// <summary>シーン オブジェクト。</summary>
		public readonly CEntity scene = new CEntity();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>背景色。</summary>
		public Color colorBack = Color.CornflowerBlue;

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private CMainLoop.CPrivateMembers _private = null;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateMainLoopDefault()
		{
			isUseDepthBuffer = true;
			isWriteDepthBuffer = true;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>グラフィック デバイスの構成・管理クラスを取得します。</summary>
		/// 
		/// <value>グラフィック デバイスの構成・管理クラス。</value>
		public GraphicsDeviceManager graphicsDeviceManager
		{
			get
			{
				return _private.graphicsDeviceManager;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>スプライト描画管理クラスを取得します。</summary>
		/// 
		/// <value>スプライト描画管理クラス。</value>
		public CSprite sprite
		{
			get
			{
				return _private.sprite;
			}
			set
			{
				_private.sprite = value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>オブジェクトにアタッチされたゲームを取得します。</summary>
		/// 
		/// <value>オブジェクトにアタッチされたゲーム。</value>
		public Game game
		{
			get
			{
				return _private.game;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>登録されているゲーム コンポーネント一覧を取得します。</summary>
		/// 
		/// <value>登録されているゲーム コンポーネント一覧。</value>
		public CGameComponentManager registedGameComponentList
		{
			get
			{
				return _private.registedGameComponentList;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>深度バッファを使用するかどうかを設定/取得します。</summary>
		/// 
		/// <value>深度バッファを使用する場合、<c>true</c>。</value>
		public bool isUseDepthBuffer
		{
			get;
			set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>深度バッファへ書き込みを行うかどうかを設定/取得します。</summary>
		/// 
		/// <value>深度バッファへ書き込みを行う場合、<c>true</c>。</value>
		public bool isWriteDepthBuffer
		{
			get;
			set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>初期化されたかどうかを取得します。</summary>
		/// 
		/// <value>初期化された場合、<c>true</c>。</value>
		public bool isSetupped
		{
			get;
			private set;
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
		public override void setup(CMainLoop entity, CMainLoop.CPrivateMembers privateMembers)
		{
			_private = privateMembers;
#if XBOX360
			registedGameComponentList.Add( new GamerServicesComponent( game ) );
			registedGameComponentList.Add(
				new CGameComponent<CEntity>( game, new CEntity( CStateGuideHelper.instance ), false ) );
#endif
			registedGameComponentList.Add(new CDrawableGameComponent(
				game, new CEntity(CStateFPSCalculator.instance), false));
			sprite = new CSprite(new SpriteBatch(game.GraphicsDevice));
			game.Content.RootDirectory = "Content";
			scene.initialize();
			isSetupped = true;
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
			CMainLoop entity, CMainLoop.CPrivateMembers privateMembers, GameTime gameTime
		)
		{
			scene.update(gameTime);
			entity.phase.count++;
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
			CMainLoop entity, CMainLoop.CPrivateMembers privateMembers, GameTime gameTime
		)
		{
			GraphicsDevice device = game.GraphicsDevice;
			device.Clear(colorBack);
			device.RenderState.DepthBufferEnable = isUseDepthBuffer;
			device.RenderState.DepthBufferWriteEnable = isWriteDepthBuffer;
			scene.draw(gameTime);
			sprite.draw();
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
		public override void teardown(CMainLoop entity, CMainLoop.CPrivateMembers privateMembers, IState nextState)
		{
			scene.Dispose();
			isSetupped = false;
		}
	}
}
