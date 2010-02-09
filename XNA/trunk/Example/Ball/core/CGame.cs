////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.ball.Properties;
using danmaq.ball.state.scene;
using danmaq.nineball;
using danmaq.nineball.entity.component;
using danmaq.nineball.entity.input;
using danmaq.nineball.state.misc;
using danmaq.nineball.util.resolution;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#if !DEBUG
using danmaq.nineball.util;
#endif

namespace danmaq.ball.core
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲーム クラス。</summary>
	public sealed class CGame : Game
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CGame instance = new CGame();

		/// <summary>グラフィック デバイスの構成・管理クラス。</summary>
		public readonly GraphicsDeviceManager graphicDeviceManager;

		/// <summary>入力管理クラス。</summary>
		public readonly CInputKeyboard inputManager = new CInputKeyboard(0);

		/// <summary>解像度管理クラス。</summary>
		public readonly CResolution resolution = new CResolution(EResolution.VGA);

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CGame()
		{
			graphicDeviceManager = new GraphicsDeviceManager(this);
			Rectangle rc = resolution;
			graphicDeviceManager.PreferredBackBufferWidth = rc.Width;
			graphicDeviceManager.PreferredBackBufferHeight = rc.Height;
			IsMouseVisible = true;
			Content.RootDirectory = Resources.DIR_CONTENT;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ゲームを初期化します。</summary>
		protected override void Initialize()
		{
			CStateCapsXNA xnastate = CStateCapsXNA.instance;
			xnastate.nextState = CStateInitialize.instance;
			CStarter.scene.nextState = xnastate;
#if !DEBUG
			CLogger.outFile = Resources.FILE_BOOTLOG;
			CLogger.add(xnastate.report);
#endif
			CStarter.startNineball(this, graphicDeviceManager);
			new CGameComponent(this, inputManager, true);
			inputManager.ButtonsNum = 1;
			inputManager.assignList = new Keys[] { Keys.Space };
			base.Initialize();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ここからプログラムが開始されます。</summary>
		/// 
		/// <param name="args">プログラムへ渡される引数</param>
		private static void Main(string[] args)
		{
			instance.Run();
			instance.Dispose();
		}
	}
}
