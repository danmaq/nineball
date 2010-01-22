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
using danmaq.nineball.entity;
using danmaq.nineball.state.manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.scene
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>初期化シーン。</summary>
	public sealed class CStateInitialize : CSceneBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateInitialize instance = new CStateInitialize();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateInitialize()
			: base(Resources.SCENE_INITIALIZE)
		{
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
		public override void setup(IEntity entity, object privateMembers)
		{
			base.setup(entity, privateMembers);
			CStateMainLoopDefault stateMain = CStateMainLoopDefault.instance;
			stateMain.colorBack = Color.Black;
			stateMain.isUseDepthBuffer = false;
			stateMain.isWriteDepthBuffer = false;
			systemSpriteManager.resolution = game.resolution;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(IEntity entity, object privateMembers, GameTime gameTime)
		{
			entity.nextState = CStateCredit.instance;
			base.update(entity, privateMembers, gameTime);
		}
	}
}
