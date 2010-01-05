////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.state;
using danmaq.nineball.state.manager;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.manager {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>メインループのゲームコンポーネント クラス。</summary>
	public sealed class CMainLoop : CEntity {

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>Nineball終了時にアプリケーションも終了するかどうか。</summary>
		public bool exitOnDispose = true;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="game">ゲーム コンポーネントをアタッチするゲーム。</param>
		public CMainLoop( Game game ) { this.game = game; }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// このコンポーネントにアタッチされたゲームを取得します。
		/// </summary>
		/// 
		/// <value>このコンポーネントにアタッチされたゲーム。</value>
		public Game game { get; private set; }

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// 状態として、nullを設定しようとした場合。
		/// </exception>
		public new IState<CMainLoop, Game> nextState {
			set { base.nextState = value; }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// オブジェクトと状態クラスのみがアクセス可能なフィールドを取得します。
		/// </summary>
		/// 
		/// <value>オブジェクトと状態クラスのみがアクセス可能なフィールド。</value>
		protected override object privateMembers {
			get { return game; }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理を実行します。</summary>
		public override void initialize() {
			nextState = CStateMainLoopDefault.instance;
			base.initialize();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose() {
			base.Dispose();
			if( exitOnDispose ) { game.Exit(); }
		}
	}
}
