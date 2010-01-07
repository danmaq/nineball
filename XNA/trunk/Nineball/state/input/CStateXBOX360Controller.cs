﻿////////////////////////////////////////////////////////////////////////////////
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

namespace danmaq.nineball.state.input {

	// TODO : マルチプレイヤーにも対応させる
	// TODO : 自動キーアサイン・自動プレイヤー決定

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360対応コントローラ専用の入力状態。</summary>
	public sealed class CStateXBOX360Controller : CState<CInput, List<SInputState>> {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateXBOX360Controller instance = new CStateXBOX360Controller();

		/// <summary>ボタン一覧。</summary>
		private readonly List<Buttons> allButtons = new List<Buttons> {
			Buttons.DPadUp, Buttons.DPadDown, Buttons.DPadLeft, Buttons.DPadRight,
			Buttons.Start, Buttons.Back,
			Buttons.LeftStick, Buttons.RightStick, Buttons.LeftShoulder, Buttons.RightShoulder,
			Buttons.BigButton, Buttons.A, Buttons.B, Buttons.X, Buttons.Y,
			Buttons.RightTrigger, Buttons.LeftTrigger,
			Buttons.RightThumbstickUp, Buttons.RightThumbstickDown,
			Buttons.RightThumbstickRight, Buttons.RightThumbstickLeft,
			Buttons.LeftThumbstickUp, Buttons.LeftThumbstickDown,
			Buttons.LeftThumbstickRight, Buttons.LeftThumbstickLeft,
		};

		/// <summary>プレイヤー番号一覧。</summary>
		private readonly List<PlayerIndex> allPlayers = new List<PlayerIndex> {
			PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four
		};

		/// <summary>ボタン割り当て値の一覧。</summary>
		private readonly List<Buttons> assignList = new List<Buttons>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>現在アクティブなプレイヤー。</summary>
		public PlayerIndex? primaryPlayer = null;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateXBOX360Controller() { }

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(
			CInput entity, List<SInputState> privateMembers, GameTime gameTime
		) {
			base.update( entity, privateMembers, gameTime );
			if( primaryPlayer.HasValue ){
				GamePadState state = GamePad.GetState( primaryPlayer.Value );
				if( state.IsConnected ) {
					while( privateMembers.Count > assignList.Count ) { assignList.Add( 0 ); }
					for( int i = privateMembers.Count - 1; i >= 0; i-- ) {
						privateMembers[i].refresh( state.IsButtonDown( assignList[i] ) );
					}
				}
				else { primaryPlayer = null; }
			}
			if( !primaryPlayer.HasValue ) {
				Buttons buttons;
				getButton( out primaryPlayer, out buttons );
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 現在のボタン割り当て一覧を破棄して、新しい割り当てを設定します。
		/// </summary>
		/// 
		/// <param name="collection">ボタン割り当て一覧。</param>
		public void setAssignList( IEnumerable<Buttons> collection ) {
			assignList.Clear();
			assignList.AddRange( collection );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ボタンが押下されたかどうかを取得します。</summary>
		/// 
		/// <param name="playerIndex">プレイヤー番号。</param>
		/// <returns>押下されたボタン情報。</returns>
		private Buttons getButton( PlayerIndex playerIndex ) {
			Buttons result = 0;
			GamePadState state = GamePad.GetState( playerIndex );
			if( state.IsConnected ) {
				foreach( Buttons button in allButtons ) {
					if( state.IsButtonDown( button ) ) { result |= button; }
				}
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 全てのコントローラを検索して、ボタンが押下されたかどうかを取得します。
		/// </summary>
		/// 
		/// <param name="playerIndex">プレイヤー番号。</param>
		/// <param name="buttons">押下されたボタン情報。</param>
		private void getButton( out PlayerIndex? playerIndex, out Buttons buttons ) {
			playerIndex = null;
			buttons = 0;
			foreach( PlayerIndex player in allPlayers ) {
				buttons = getButton( player );
				if( buttons != 0 ) {
					playerIndex = player;
					break;
				}
			}
		}
	}
}
