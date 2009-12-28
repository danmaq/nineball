////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.state {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>メインループクラス用の既定の状態です。</summary>
	public sealed class CMainLoopDefaultState : CState<CMainLoop, Game> {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CMainLoopDefaultState instance = new CMainLoopDefaultState();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>背景色。</summary>
		public Color colorBack = Color.CornflowerBlue;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CMainLoopDefaultState() { }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>グラフィック デバイスの構成・管理クラスを取得します。</summary>
		/// 
		/// <value>グラフィック デバイスの構成・管理クラス。</value>
		public GraphicsDeviceManager graphicsDeviceManager { get; set; }

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
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="game">オブジェクトにアタッチされたゲーム。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update( CMainLoop entity, Game game, GameTime gameTime ) {
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="game">オブジェクトにアタッチされたゲーム。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw( CMainLoop entity, Game game, GameTime gameTime ) {
			GraphicsDevice device = game.GraphicsDevice;
			device.Clear( colorBack );
			device.RenderState.DepthBufferEnable = true;
			device.RenderState.DepthBufferWriteEnable = true;
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
			CMainLoop entity, Game game, CState<CMainLoop, Game> nextState
		) { }

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
		/// <param name="nextState">オブジェクトが次に適用する状態。</param>
		public override void teardown( CMainLoop entity, Game game, CState nextState ) {
		}
	}
}
