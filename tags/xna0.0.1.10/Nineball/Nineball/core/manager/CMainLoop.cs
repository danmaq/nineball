////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2009 danmaq all rights reserved.
//
//		──メインループ クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using Microsoft.Xna.Framework;
using danmaq.Nineball.core.data;
using danmaq.Nineball.core.raw;
using Microsoft.Xna.Framework.Graphics;
using System;

#if XBOX360
using Microsoft.Xna.Framework.GamerServices;
#endif

namespace danmaq.Nineball.core.manager {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>メインループ クラス。</summary>
	public class CMainLoop<_T> : Game where _T : new() {
		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>DNL動作初期設定用構造体。</summary>
		public readonly SStarter<_T> INITIALIZE_DATA;

		/// <summary>グラフィックデバイス管理クラス。</summary>
		public readonly GraphicsDeviceManager GRAPHICS_DEVICE_MANAGER;

		/// <summary>テクスチャキャッシュ。</summary>
		public readonly CResourceManager<Texture2D> RESOURCE_MANAGER_TEXTURE = new CResourceManager<Texture2D>();

		/// <summary>モデルキャッシュ。</summary>
		public readonly CResourceManager<Model> RESOURCE_MANAGER_MODEL = new CResourceManager<Model>();

		/// <summary>フォントキャッシュ。</summary>
		public readonly CResourceManager<SpriteFont> RESOURCE_MANAGER_FONT = new CResourceManager<SpriteFont>();

		/// <summary>シーン管理クラス。</summary>
		public readonly CSceneManager SCENE_MANAGER = new CSceneManager();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>背景色。</summary>
		public Color colorBack = Color.Black;

		/// <summary>終了処理を開始したかどうか。</summary>
		private bool isExit = false;

		/// <summary>入力管理クラス。</summary>
		private CInput m_input = null;

		/// <summary>音響制御・管理クラス。</summary>
		private CAudio m_audio = null;

		/// <summary>ゲームデータ管理クラス。</summary>
		private CDataIOManager<_T> m_mgrDataIO = null;

		/// <summary>スプライト描画管理クラス。</summary>
		private CSprite m_mgrDraw = null;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="initializeData">DNL動作初期設定用構造体</param>
		public CMainLoop( SStarter<_T> initializeData ) {
			INITIALIZE_DATA = initializeData;
			GRAPHICS_DEVICE_MANAGER = new GraphicsDeviceManager( this );
			GRAPHICS_DEVICE_MANAGER.PreferredBackBufferWidth = 640;
			GRAPHICS_DEVICE_MANAGER.PreferredBackBufferHeight = 480;
			base.Content.RootDirectory = initializeData.fileIOConfigure.dirContent;
#if XBOX360
			Components.Add( new GamerServicesComponent( this ) );
#endif
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>入力管理クラス。</summary>
		public CInput input {
			get { return m_input; }
			private set { m_input = value; }
		}

		/// <summary>音響制御・管理クラス。</summary>
		public CAudio audio {
			get { return m_audio; }
			private set { m_audio = value; }
		}

		/// <summary>ゲームデータ管理クラス。</summary>
		public CDataIOManager<_T> gamedata {
			get { return m_mgrDataIO; }
			private set { m_mgrDataIO = value; }
		}

		/// <summary>スプライト描画管理クラス。</summary>
		public CSprite spriteDraw {
			get { return m_mgrDraw; }
			private set { m_mgrDraw = value; }
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理</summary>
		protected override void Initialize() {
			base.Initialize();
			TargetElapsedTime = TimeSpan.FromSeconds( 1.0 / ( double )( INITIALIZE_DATA.fps ) );
			IsFixedTimeStep = true;
			SCENE_MANAGER.nowScene = INITIALIZE_DATA.sceneFirst;
			gamedata = new CDataIOManager<_T>(
				INITIALIZE_DATA.codename, INITIALIZE_DATA.fileIOConfigure.fileConfigure );
			SStarter<_T>.SInputInitializeData iniInput = INITIALIZE_DATA.inputConfigure;
			input = new CInput( Window.Handle, iniInput.buttons, iniInput.keyLoopStart, iniInput.keyLoopInterval );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ロード処理</summary>
		protected override void LoadContent() {
			CLogger.add( "ゲームリソースを読込しています..." );
			SStarter<_T>.SXACTInitializeData xact = INITIALIZE_DATA.XACTConfigure;
			if( xact ) {
				audio = new CAudio( xact.index2assert, xact.loopSEInterval,
					xact.fileXGS, xact.fileXSB, xact.fileXWBSE, xact.fileXWBBGM );
			}
			RESOURCE_MANAGER_TEXTURE.reload( true, Content );
			RESOURCE_MANAGER_FONT.reload( true, Content );
			if( spriteDraw != null ) { spriteDraw.Dispose(); }
			spriteDraw = new CSprite( GraphicsDevice );
			GC.Collect();
			base.LoadContent();
			CLogger.add( "ゲームリソースの読込完了。" );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アンロード処理</summary>
		protected override void UnloadContent() {
			CLogger.add( "ゲームリソースを解放しています..." );
			base.UnloadContent();
			CLogger.add( "ゲームリソースの解放完了。" );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理をします。</summary>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		protected override void Update( GameTime gameTime ) {
#if WINDOWS
			bool bUpdate = IsActive;
#else
			CGuideManager.update();
			bool bUpdate = IsActive && !Guide.IsVisible;
#endif
			if( bUpdate ) {
				if( input != null ) { input.update(); }
				if( !SCENE_MANAGER.update( gameTime ) ) {
					CLogger.add( "ゲームの終了処理を開始します。" );
					isExit = true;
					Content.Unload();
					if( input != null ) {
						input.Dispose();
						input = null;
					}
					if( audio != null ) {
						audio.Dispose();
						audio = null;
					}
					if( spriteDraw != null ) {
						spriteDraw.Dispose();
						spriteDraw = null;
					}
					Exit();
				}
			}
			if( audio != null ) { audio.update(); }
			base.Update( gameTime );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理をします。</summary>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		protected override void Draw( GameTime gameTime ) {
			if( !isExit ) {
				GraphicsDevice.Clear( colorBack );
				GraphicsDevice.RenderState.DepthBufferEnable = true;
				GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
				SCENE_MANAGER.draw( gameTime, spriteDraw );
				spriteDraw.update();
			}
			base.Draw( gameTime );
		}
	}
}
