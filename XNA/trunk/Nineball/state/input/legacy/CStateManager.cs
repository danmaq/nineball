////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if WINDOWS && !DISABLE_LEGACY

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using danmaq.nineball.entity;
using danmaq.nineball.entity.input;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.state.input.legacy {

	// TODO : 作りかけ
	// 実質マルチプレイヤー実装が先に必要

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ専用の入力状態。</summary>
	public sealed class CStateManager : CState<CInput, List<SInputState>> {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateManager instance = new CStateManager();

		/// <summary>追加動作用のオブジェクト。</summary>
		public readonly CEntity behavior = new CEntity();

		/// <summary>コントローラ 入力制御・管理クラス一覧。</summary>
		private readonly List<CInput> inputList = new List<CInput>( 1 );

		/// <summary>プレイヤー一覧。</summary>
		private readonly List<int> m_playerList = new List<int>( 1 );

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>ウィンドウ ハンドル。</summary>
		public IntPtr hWnd = IntPtr.Zero;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateManager() { }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>プレイヤー一覧を取得します。</summary>
		/// 
		/// <value>プレイヤー一覧。</value>
		public ReadOnlyCollection<int> playerList {
			get { return m_playerList.AsReadOnly(); }
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
		/// <param name="buttonsState">ボタンの入力状態一覧。</param>
		public override void setup( CInput entity, List<SInputState> buttonsState ) {
			base.setup( entity, buttonsState );
			behavior.initialize();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="buttonsState">ボタンの入力状態一覧。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(
			CInput entity, List<SInputState> buttonsState, GameTime gameTime
		) {
			base.update( entity, buttonsState, gameTime );
			int nLength = entity.buttonStateList.Count;
			foreach( CInput input in inputList ) {
				input.update( gameTime );
				if( input.currentState == CState.empty ) { }
				for( int i = nLength - 1; i >= 0; i-- ) {
					buttonsState[i] |= input.buttonStateList[i];
				}
			}
			behavior.update( gameTime );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="buttonsState">ボタンの入力状態一覧。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(
			CInput entity, List<SInputState> buttonsState, GameTime gameTime
		) {
			base.draw( entity, buttonsState, gameTime );
			behavior.draw( gameTime );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>オブジェクトが別の状態へ移行する時に呼び出されます。</para>
		/// <para>このメソッドは、遷移先の<c>setup</c>よりも先に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を終了したオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="nextState">オブジェクトが次に適用する状態。</param>
		public override void teardown( IEntity entity, object privateMembers, IState nextState ) {
			base.teardown( entity, privateMembers, nextState );
			behavior.Dispose();
		}
	}
}

#endif
