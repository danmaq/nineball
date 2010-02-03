////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.old.core.data;
using danmaq.nineball.old.core.raw;
using danmaq.nineball.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#if XBOX360
using danmaq.nineball.old.core.inner;
using Microsoft.Xna.Framework.GamerServices;
#endif

namespace danmaq.nineball.old.core.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>メインループ クラス。</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete]
	public class CMainLoop<_T> : Game where _T : new()
	{
		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>DNL動作初期設定用構造体。</summary>
		public readonly SStarter<_T> initializeData;

		/// <summary>グラフィックデバイス管理クラス。</summary>
		public readonly GraphicsDeviceManager graphicsDeviceManager;

		/// <summary>テクスチャキャッシュ。</summary>
		public readonly CResourceManager<Texture2D> textureResourceManager = new CResourceManager<Texture2D>();

		/// <summary>モデルキャッシュ。</summary>
		public readonly CResourceManager<Model> modelResourceManager = new CResourceManager<Model>();

		/// <summary>フォントキャッシュ。</summary>
		public readonly CResourceManager<SpriteFont> fontResourceManager = new CResourceManager<SpriteFont>();

		/// <summary>シーン管理クラス。</summary>
		public readonly CTaskSceneManager sceneManager = new CTaskSceneManager();

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
		public CMainLoop(SStarter<_T> initializeData)
		{
			this.initializeData = initializeData;
			graphicsDeviceManager = new GraphicsDeviceManager(this);
			graphicsDeviceManager.PreferredBackBufferWidth = 640;
			graphicsDeviceManager.PreferredBackBufferHeight = 480;
			base.Content.RootDirectory = initializeData.fileIOConfigure.dirContent;
#if XBOX360
			Components.Add( new GamerServicesComponent( this ) );
#endif
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>入力管理クラス。</summary>
		public CInput input
		{
			get
			{
				return m_input;
			}
			private set
			{
				m_input = value;
			}
		}

		/// <summary>音響制御・管理クラス。</summary>
		public CAudio audio
		{
			get
			{
				return m_audio;
			}
			private set
			{
				m_audio = value;
			}
		}

		/// <summary>ゲームデータ管理クラス。</summary>
		public CDataIOManager<_T> gamedata
		{
			get
			{
				return m_mgrDataIO;
			}
			private set
			{
				m_mgrDataIO = value;
			}
		}

		/// <summary>スプライト描画管理クラス。</summary>
		public CSprite spriteDraw
		{
			get
			{
				return m_mgrDraw;
			}
			private set
			{
				m_mgrDraw = value;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理</summary>
		protected override void Initialize()
		{
			base.Initialize();
			TargetElapsedTime = TimeSpan.FromSeconds(1.0 / (double)(initializeData.fps));
			IsFixedTimeStep = true;
			sceneManager.nowScene = initializeData.sceneFirst;
			gamedata = new CDataIOManager<_T>(
				initializeData.codename, initializeData.fileIOConfigure.fileConfigure);
			SStarter<_T>.SInputInitializeData iniInput = initializeData.inputConfigure;
			input = new CInput(Window.Handle, iniInput.buttons, iniInput.keyLoopStart, iniInput.keyLoopInterval);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ロード処理</summary>
		protected override void LoadContent()
		{
			CLogger.add("ゲームリソースを読込しています...");
			SStarter<_T>.SXACTInitializeData xact = initializeData.XACTConfigure;
			if(xact)
			{
				audio = new CAudio(xact.index2assert, xact.loopSEInterval,
					xact.fileXGS, xact.fileXSB, xact.fileXWBSE, xact.fileXWBBGM);
			}
			textureResourceManager.reload(true, Content);
			fontResourceManager.reload(true, Content);
			if(spriteDraw != null)
			{
				spriteDraw.Dispose();
			}
			spriteDraw = new CSprite(new SpriteBatch(GraphicsDevice));
			GC.Collect();
			base.LoadContent();
			CLogger.add("ゲームリソースの読込完了。");
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アンロード処理</summary>
		protected override void UnloadContent()
		{
			CLogger.add("ゲームリソースを解放しています...");
			base.UnloadContent();
			CLogger.add("ゲームリソースの解放完了。");
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理をします。</summary>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		protected override void Update(GameTime gameTime)
		{
#if WINDOWS
			bool bUpdate = IsActive;
#else
			CGuideManager.update();
			bool bUpdate = IsActive && !Guide.IsVisible;
#endif
			if(bUpdate)
			{
				if(input != null)
				{
					input.update();
				}
				if(!sceneManager.update(gameTime))
				{
					CLogger.add("ゲームの終了処理を開始します。");
					isExit = true;
					Content.Unload();
					if(input != null)
					{
						input.Dispose();
						input = null;
					}
					if(audio != null)
					{
						audio.Dispose();
						audio = null;
					}
					if(spriteDraw != null)
					{
						spriteDraw.Dispose();
						spriteDraw = null;
					}
					Exit();
				}
			}
			if(audio != null)
			{
				audio.update();
			}
			base.Update(gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理をします。</summary>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		protected override void Draw(GameTime gameTime)
		{
			if(!isExit)
			{
				GraphicsDevice.Clear(colorBack);
				GraphicsDevice.RenderState.DepthBufferEnable = true;
				GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
				sceneManager.draw(gameTime, spriteDraw);
				spriteDraw.draw();
			}
			base.Draw(gameTime);
		}
	}
}
