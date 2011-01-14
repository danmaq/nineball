////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Reflection;
using danmaq.ball.Properties;
using danmaq.ball.state.scene;
using danmaq.nineball.entity;
using danmaq.nineball.entity.component;
using danmaq.nineball.entity.graphics;
using danmaq.nineball.util;
using danmaq.nineball.util.resolution;
using danmaq.nineball.util.storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.core
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲーム クラス。</summary>
	sealed class CGame : Game
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>このアプリケーションのバージョン情報。</summary>
		public static readonly string version = string.Format("[VER-{0}]",
			Assembly.GetExecutingAssembly().GetName().Version.ToString());

		/// <summary>このアプリケーションの名前。</summary>
		public static readonly string name =
			((AssemblyProductAttribute)Attribute.GetCustomAttribute(
				Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute))).Product;

		/// <summary>グラフィック デバイスの構成・管理クラス。</summary>
		public readonly GraphicsDeviceManager graphicDeviceManager;

		/// <summary>シーン管理用オブジェクト。</summary>
		public readonly CEntity scene;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>スプライト描画管理クラス。</summary>
		public static CSpriteManager sprite;

		/// <summary>背景色。</summary>
		public Color bgColor = Color.Black;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CGame()
		{
			if (instance != null)
			{
				throw new InvalidOperationException(
					string.Format(nineball.Properties.Resources.ERR_SINGLETON,
					this.GetType().FullName));
			}
			instance = this;
			graphicDeviceManager = new GraphicsDeviceManager(this);
			Rectangle rect = EResolution.VGA.toRect();
			graphicDeviceManager.PreferredBackBufferWidth = rect.Width;
			graphicDeviceManager.PreferredBackBufferHeight = rect.Height;
			new CGuideWrapper(this);
			scene = new CEntity(CSceneInitialize.instance, this);
			CDrawableGameComponent gcScene = new CDrawableGameComponent(this, scene, true);
			gcScene.DrawOrder = int.MaxValue;
			gcScene.UpdateOrder = int.MaxValue;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>クラス オブジェクトを取得します。</summary>
		/// 
		/// <value>クラス オブジェクト。</value>
		public static CGame instance
		{
			get;
			private set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ここからプログラムが開始されます。</summary>
		/// 
		/// <param name="args">プログラムへ渡される引数。</param>
		private static void Main(string[] args)
		{
			CLogger.outFile = Resources.FILE_BOOTLOG;
			CLogger.add(string.Format("{0} - {1}", name, version));

#if !DEBUG
			try
#endif
			{
				using (CMutexObject mutex = new CMutexObject())
				using (CGame game = new CGame())
				{
					game.Run();
				}
			}
#if !DEBUG
			catch (Exception e)
			{
				CLogger.add(e);
				throw e;
			}
#endif
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ゲームを初期化します。</summary>
		protected override void Initialize()
		{
			try
			{
				// ここで初めてゲーマー サービスの初期化をするっぽい
				base.Initialize();
			}
			catch (GamerServicesNotAvailableException e)
			{
				CLogger.add(nineball.Properties.Resources.WARN_GAMER_SERVICE);
				CLogger.add(e);
				CGuideWrapper.instance.removeGamerServiceComponent();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>使用されているリソースを解放します。</summary>
		/// 
		/// <param name="disposing">
		/// マネージ リソースに加えて、アンマネージ リソースも同時に解放するかどうか。
		/// </param>
		protected override void Dispose(bool disposing)
		{
			scene.Dispose();
			CMisc.safeDispose(ref sprite);
			instance = null;
			base.Dispose(disposing);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(bgColor);
			base.Draw(gameTime);
		}
	}
}
