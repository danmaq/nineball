////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.entity.input {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360用コントローラ 入力制御・管理クラス。</summary>
	public sealed class CInputXBOX360 : CInput {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>プレイヤー番号一覧。</summary>
		public static readonly ReadOnlyCollection<PlayerIndex> allPlayers = new List<PlayerIndex> {
			PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four
		}.AsReadOnly();

		/// <summary>ボタン割り当て値の一覧。</summary>
		private readonly List<Buttons> m_assignList = new List<Buttons>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="playerIndex">プレイヤー番号。</param>
		public CInputXBOX360( PlayerIndex playerIndex ) : base( CState.empty ) {
			this.playerIndex = playerIndex;
			nextState = state.input.xbox360.CStateDefault.instance;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// 状態として、nullを設定しようとした場合。
		/// </exception>
		public new IState<CInputXBOX360, List<SInputState>> nextState {
			set { nextStateBase = value; }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>プレイヤー番号を取得します。</summary>
		/// 
		/// <value>プレイヤー番号。</value>
		public PlayerIndex playerIndex { get; private set; }

		//* -----------------------------------------------------------------------*
		/// <summary>ボタン割り当て値の一覧を設定/取得します。</summary>
		/// 
		/// <value>ボタン割り当て値の一覧。</value>
		public IList<Buttons> assignList {
			get { return m_assignList; }
			set {
				if( value.Count != m_assignList.Count ) {
					throw new ArgumentOutOfRangeException( "value" );
				}
				m_assignList.Clear();
				m_assignList.AddRange( value );
				ReadOnlyCollection<SInputState> stateList = buttonStateList;
				while( m_assignList.Count > stateList.Count ) {
					m_assignList.RemoveAt( m_assignList.Count - 1 );
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在押下されているボタン情報を取得します。</summary>
		/// 
		/// <value>押下されたボタン情報。</value>
		public Buttons press {
			get {
				Buttons result = 0;
				checkConnect();
				result = GamePad.GetState( playerIndex ).getPress();
				return result;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理を実行します。</summary>
		public override void initialize() {
			base.initialize();
			checkConnect();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 接続されているかどうかを調べて、接続されていない場合は自殺します。
		/// </summary>
		/// 
		/// <returns>接続されている場合、<c>true</c>。</returns>
		public bool checkConnect() {
			bool bResult = GamePad.GetState( playerIndex ).IsConnected;
			if( !bResult ) { Dispose(); }
			return bResult;
		}
	}
}
