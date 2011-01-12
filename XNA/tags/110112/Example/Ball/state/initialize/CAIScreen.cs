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
using danmaq.nineball.data;
using danmaq.nineball.entity.component;
using danmaq.nineball.entity.graphics;
using danmaq.nineball.state;
using danmaq.nineball.util.resolution;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.initialize
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>画面初期化の状態。</summary>
	sealed class CAIScreen
		: CAIBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState instance = new CAIScreen();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CAIScreen()
			: base("画面表示の初期化")
		{
		}


		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>次に遷移すべき状態を取得します。</summary>
		/// 
		/// <value>次に遷移すべき状態。</value>
		public override IState nextState
		{
			get
			{
				return CAIComponents.instance;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理を実行します。</summary>
		protected override void initialize()
		{
			CGame game = CGame.instance;
			GraphicsDeviceManager gdm = game.graphicDeviceManager;
			game.IsMouseVisible = true;
			game.IsFixedTimeStep = false;
			gdm.IsFullScreen = false;
			gdm.SynchronizeWithVerticalRetrace = true;
			gdm.PreferMultiSampling = false;
			gdm.ApplyChanges();
			CSpriteManager sprite = new CSpriteManager();
			sprite.spriteBatch = new SpriteBatch(gdm.GraphicsDevice);
			sprite.resolution = new CResolutionLetterBox(EResolution.DCGA, EResolution.VGA, false);
			CGame.sprite = sprite;
			initializeViewport();
			game.GraphicsDevice.RenderState.PointSpriteEnable = true;
			game.GraphicsDevice.RenderState.MultiSampleAntiAlias = false;
			game.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.None;
			new CDrawableGameComponent(game, sprite, true).DrawOrder = 0;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ビューポートを初期化します。</summary>
		private void initializeViewport()
		{
			CResolutionLetterBox res = (CResolutionLetterBox)CGame.sprite.resolution;
			GraphicsDevice device = CGame.instance.GraphicsDevice;
			Rectangle viewRect = res.convertRectangle(EResolution.DCGA.toRect());
			Viewport viewPort = device.Viewport;
			viewPort.X = viewRect.X;
			viewPort.Y = viewRect.Y;
			viewPort.Width = viewRect.Width;
			viewPort.Height = viewRect.Height;
			device.Viewport = viewPort;
			res.alignHorizontal = EAlign.LeftTop;
			res.alignVertical = EAlign.LeftTop;
		}
	}
}
