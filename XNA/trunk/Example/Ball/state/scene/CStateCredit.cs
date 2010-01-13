////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using danmaq.nineball.entity;
using danmaq.nineball.entity.manager;
using danmaq.nineball.misc;
using danmaq.nineball.state;
using danmaq.nineball.util;
using Microsoft.Xna.Framework;

namespace danmaq.ball.state.scene {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>クレジット画面シーン。</summary>
	public sealed class CStateCredit : IState {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateCredit instance = new CStateCredit();

		/// <summary>コルーチン管理 クラス。</summary>
		public readonly CCoRoutineManager coRoutineManager = new CCoRoutineManager();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>透明度。</summary>
		private float m_fAlpha = 0;

		/// <summary>終了したかどうか。</summary>
		private bool m_bExit = false;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateCredit() { }

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
		public void setup( IEntity entity, object privateMembers ) {
			CLogger.add( "クレジット画面シーンを開始します。" );
			coRoutineManager.initialize();
			coRoutineManager.add( coAlpha() );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public void update( IEntity entity, object privateMembers, GameTime gameTime ) {
			if( m_bExit ) { entity.nextState = CStateTitle.instance; }
			coRoutineManager.update( gameTime );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public void draw( IEntity entity, object privateMembers, GameTime gameTime ) {

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
		public void teardown( IEntity entity, object privateMembers, IState nextState ) {
			CLogger.add( "クレジット画面シーンを終了します。" );
			coRoutineManager.Dispose();
			m_fAlpha = 0;
			m_bExit = false;
			GC.Collect();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		private IEnumerator coAlpha() {
			const int FADETIME = 60;
			for(
				int i = 0; i < FADETIME;
				m_fAlpha = CInterpolate._clampAccelerate( 0, 1, ++i, FADETIME )
			) { yield return null; }
			for( int i = 0; i < 120; i++ ) { yield return null; }
			for(
				int i = 0; i < FADETIME;
				m_fAlpha = CInterpolate._clampAccelerate( 1, 0, ++i, FADETIME )
			) { yield return null; }
			m_bExit = true;
		}
	}
}
