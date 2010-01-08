////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.entity.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.ObjectModel;

namespace danmaq.nineball.state.input.xbox360 {

	// TODO : マルチプレイヤーにも対応させる
	// TODO : 自動キーアサイン・自動プレイヤー決定
	// TODO : CInputXBOX360の管理クラスにする

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360対応コントローラ専用の入力状態。</summary>
	public sealed class CStateManager : CState<CInput, List<SInputState>> {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateManager instance =
			new CStateManager();

		/// <summary>コントローラ 入力制御・管理クラス一覧。</summary>
		private readonly List<CInputXBOX360> inputList = new List<CInputXBOX360>( 1 );

		/// <summary>プレイヤー一覧。</summary>
		private readonly List<PlayerIndex> m_playerList = new List<PlayerIndex>( 1 );

		/// <summary>入力デバイス追加用キュー。</summary>
		private readonly Queue<CInput> addQueue = new Queue<CInput>();

		/// <summary>入力デバイス削除用キュー。</summary>
		private readonly Queue<CInput> removeQueue = new Queue<CInput>();

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
		public ReadOnlyCollection<PlayerIndex> playerList {
			get { return m_playerList.AsReadOnly(); }
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

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
			int nLength = entity.buttonStateList.Count;
			foreach( CInput input in inputList ) {
				input.update( gameTime );
				for( int i = nLength - 1; i >= 0; i-- ) {
					buttonsState[i] |= input.buttonStateList[i];
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理するXBOX360コントローラ追加の予約します。</summary>
		/// <remarks>
		/// 既に登録されているか、<paramref name="playerIndex"/>に対応する
		/// XBOX360コントローラが接続されていない場合、予約は失敗します。
		/// </remarks>
		/// 
		/// <param name="playerIndex">対応するプレイヤー番号。</param>
		/// <returns>予約が成功した場合、<c>true</c>。</returns>
		public bool addPlayer( PlayerIndex playerIndex ) {
			bool bResult = !playerList.Contains( playerIndex );
			if( bResult ) {
				m_playerList.Add( playerIndex );
				CInputXBOX360 input = new CInputXBOX360( playerIndex );
				input.initialize();
				bResult = input.currentState != CState.empty;
				if( bResult ) { addQueue.Enqueue( input ); }
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 全てのコントローラを検索して、ボタンが押下されたかどうかを取得します。
		/// </summary>
		/// 
		/// <param name="playerIndex">プレイヤー番号。</param>
		/// <param name="buttons">押下されたボタン情報。</param>
		private static void getButton( out PlayerIndex? playerIndex, out Buttons buttons ) {
			playerIndex = null;
			buttons = 0;
			foreach( PlayerIndex player in CInputXBOX360.allPlayers ) {
				buttons = GamePad.GetState( player ).getPress();
				if( buttons != 0 ) {
					playerIndex = player;
					break;
				}
			}
		}
	}
}
