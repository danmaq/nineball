////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.state;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>メインループのゲームコンポーネント クラス。</summary>
	public sealed class CMainLoop : CEntity {

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
		/// <para>状態として、nullを設定しようとした場合。</para>
		/// <para>
		/// 何もしない状態を設定したい場合、<c>CState.empty</c>を使用します。
		/// </para>
		/// </exception>
		public new CState<CMainLoop, Game> nextState {
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
			nextState = danmaq.nineball.state.CMainLoopDefaultState.instance;
			base.initialize();
		}
	}
}
