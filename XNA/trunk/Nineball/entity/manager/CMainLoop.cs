////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.data.phase;
using danmaq.nineball.state;
using danmaq.nineball.state.manager;
using danmaq.nineball.util;
using danmaq.nineball.util.collection;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>メインループのゲームコンポーネント クラス。</summary>
	public sealed class CMainLoop : CEntity
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers : IDisposable
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>このコンポーネントにアタッチされたゲーム。</summary>
			public Game game = null;

			/// <summary>グラフィック デバイスの構成・管理クラス。</summary>
			public GraphicsDeviceManager graphicsDeviceManager = null;

			/// <summary>スプライト描画管理クラス。</summary>
			public CSprite sprite = null;

			/// <summary>登録されているゲーム コンポーネント一覧。</summary>
			public CGameComponentManager registedGameComponentList = null;

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>フィールドのオブジェクトを解放します。</summary>
			public void Dispose()
			{
				registedGameComponentList.Dispose();
				registedGameComponentList = null;
				if(sprite != null)
				{
					sprite.Dispose();
					sprite = null;
				}
				graphicsDeviceManager = null;
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _private;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>フェーズ・カウンタ進行管理クラス。</summary>
		public SPhase phase = SPhase.initialized;

		/// <summary>Nineball終了時にアプリケーションも終了するかどうか。</summary>
		public bool exitOnDispose = true;

		/// <summary>最初に設定される状態。</summary>
		private IState<CMainLoop, CPrivateMembers> m_firstState = null;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="game">ゲーム コンポーネントをアタッチするゲーム。</param>
		/// <param name="graphicsDeviceManager">
		/// グラフィック デバイスの構成・管理クラス。
		/// </param>
		public CMainLoop(Game game, GraphicsDeviceManager graphicsDeviceManager)
			: base(null, new CPrivateMembers())
		{
			_private = (CPrivateMembers)privateMembers;
			_private.game = game;
			_private.graphicsDeviceManager = graphicsDeviceManager;
			_private.registedGameComponentList = new CGameComponentManager(game);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="game">ゲーム コンポーネントをアタッチするゲーム。</param>
		/// <param name="graphicsDeviceManager">
		/// グラフィック デバイスの構成・管理クラス。
		/// </param>
		/// <param name="firstState">最初に設定される状態。</param>
		public CMainLoop(Game game, GraphicsDeviceManager graphicsDeviceManager,
			IState<CMainLoop, CPrivateMembers> firstState)
			: this(game, graphicsDeviceManager)
		{
			m_firstState = firstState;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理を実行します。</summary>
		public override void initialize()
		{
			nextState = m_firstState ?? CStateMainLoopDefault.instance;
			base.initialize();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			base.Dispose();
			_private.Dispose();
			if(exitOnDispose)
			{
				_private.game.Exit();
			}
		}
	}
}
