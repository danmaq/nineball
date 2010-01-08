////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.entity;
using danmaq.nineball.entity.input;
using Microsoft.Xna.Framework;
using danmaq.nineball.data;

namespace danmaq.nineball.state.input {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>既定の入力状態。</summary>
	public sealed class CStateDefault : CState<CInput, List<SInputState>> {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateDefault instance = new CStateDefault();

		/// <summary>コントローラ 入力制御・管理クラス一覧。</summary>
		private readonly List<CInput> inputList = new List<CInput>( 1 );

		/// <summary>入力デバイス追加用キュー。</summary>
		private readonly Queue<CInput> addQueue = new Queue<CInput>();

		/// <summary>入力デバイス削除用キュー。</summary>
		private readonly Queue<CInput> removeQueue = new Queue<CInput>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>入力デバイス定数。</summary>
		private EInputDevice m_inputDevice = EInputDevice.None;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateDefault() { }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>入力デバイスを設定/取得します。</summary>
		/// 
		/// <value>入力デバイス。</value>
		public EInputDevice inputDevice {
			get { return m_inputDevice; }
			set {
				value &= EInputDevice.All;
				if( value != m_inputDevice ) {
					if( value == EInputDevice.None ) {	// ないなら全部消してしまう
						inputList.ForEach( item => item.Dispose() );
						inputList.Clear();
					}
					else {	// 一部だけある場合はちょっとややこしい
						List<IState<CInput, List<SInputState>>> enabled, disabled;
						value.getState( out enabled, out disabled );
						foreach( IState<CInput, List<SInputState>> state in enabled ) {
							if( inputList.Find( input => input.currentState == state ) == null ) {
								addQueue.Enqueue( new CInput( state ) );
							}
						}
						foreach( IState<CInput, List<SInputState>> state in disabled ) {
							CInput input = inputList.Find( i => i.currentState == state );
							if( input != null ) {
								input.Dispose();
								removeQueue.Enqueue( input );
							}
						}
					}
					m_inputDevice = value;
				}
			}
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
		/// <param name="buttonsState">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		public override void setup( CInput entity, List<SInputState> buttonsState ) {
			base.setup( entity, buttonsState );
			inputDevice |= EInputDevice.Keyboard;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="buttonsState">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(
			CInput entity, List<SInputState> buttonsState, GameTime gameTime
		) {
			base.update( entity, buttonsState, gameTime );
			while( removeQueue.Count > 0 ) {	// デバイス削除の予約を実行
				CInput input = removeQueue.Dequeue();
				entity.changedButtonsNum -= input.onChangedButtonsNum;
				inputList.Remove( input );
			}
			while( addQueue.Count > 0 ) {	// デバイス追加の予約を実行
				CInput input = addQueue.Dequeue();
				entity.changedButtonsNum += input.onChangedButtonsNum;
				input.changedState += onChangeState;
				inputList.Add( input );
			}
			int nLength = buttonsState.Count;
			foreach( CInput input in inputList ) {
				input.update( gameTime );
				for( int i = nLength - 1; i >= 0; i-- ) {
					buttonsState[i] |= input.buttonStateList[i];
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="buttonsState">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(
			CInput entity, List<SInputState> buttonsState, GameTime gameTime
		) {
			base.draw( entity, buttonsState, gameTime );
			inputList.ForEach( input => input.draw( gameTime ) );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>オブジェクトが別の状態へ移行する時に呼び出されます。</para>
		/// <para>このメソッドは、遷移先の<c>setup</c>よりも先に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を終了したオブジェクト。</param>
		/// <param name="buttonsState">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="nextState">オブジェクトが次に適用する状態。</param>
		public override void teardown( IEntity entity, object buttonsState, IState nextState ) {
			inputList.ForEach( input => input.Dispose() );
			inputList.Clear();
			base.teardown( entity, buttonsState, nextState );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>状態が変化した際に呼び出されるメソッドです。</summary>
		/// 
		/// <param name="sender">送信元のオブジェクト。</param>
		/// <param name="e">変化後の状態。</param>
		private void onChangeState( object sender, CEventChangedState e ) {
			CInput input = ( CInput )sender;
			if( input.currentState == CState.empty ) {
				input.changedState -= onChangeState;
				removeQueue.Enqueue( ( CInput )sender );
			}
		}
	}
}
