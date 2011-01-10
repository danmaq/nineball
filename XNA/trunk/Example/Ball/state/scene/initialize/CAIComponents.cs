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
using danmaq.ball.data;
using danmaq.nineball.entity;
using danmaq.nineball.entity.component;
using danmaq.nineball.entity.fonts;
using danmaq.nineball.entity.graphics;
using danmaq.nineball.state;
using danmaq.nineball.state.fonts;
using danmaq.nineball.state.manager;
using danmaq.nineball.util.storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.scene.initialize
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>その他コンポーネントの初期化の状態。</summary>
	sealed class CAIComponents
		: CAIBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState instance = new CAIComponents();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CAIComponents()
			: base("その他コンポーネントの初期化")
		{
			CStateFPSViewer.instance.text = "FPS: {0}/{1}";
			CStateHeapViewer.instance.text = "mem: {0}/ delta: {1}({2})";
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
				return CState.empty;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理を実行します。</summary>
		public override void initialize()
		{
			CGame game = CGame.instance;
			CSpriteManager sprite = CGame.sprite;
			CONTENT.setContentManager(game.Content);
			SpriteFont font = CONTENT.texFont98;
			IState[] hudStates =
			{
				CStateFPSViewer.instance,
#if DEBUG
				CStateHeapViewer.instance,
#endif
			};
			for (int i = hudStates.Length; --i >= 0; )
			{
				CFont hud = new CFont(font);
				hud.nextState = hudStates[i];
				hud.sprite = sprite;
				hud.layer = 0;
				hud.commitNextState(true);
				new CDrawableGameComponent(game, hud, true);
			}
			new CDrawableGameComponent(game, new CEntity(CStateFPSCalculator.instance), true);
			CGuideWrapper.instance.NotificationPosition = NotificationPosition.TopRight;
		}
	}
}
