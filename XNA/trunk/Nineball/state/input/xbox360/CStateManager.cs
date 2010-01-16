////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Collections.ObjectModel;
using danmaq.nineball.data;
using danmaq.nineball.entity;
using danmaq.nineball.entity.input;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.state.input.xbox360
{

	// TODO : 自動キーアサイン

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360対応コントローラ専用の入力状態。</summary>
	public sealed class CStateManager : CState<CInput, List<SInputState>>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateManager instance =
			new CStateManager();

		/// <summary>追加動作用のオブジェクト。</summary>
		public readonly CEntity behavior = new CEntity(CStateDetector.instance);

		/// <summary>コントローラ 入力制御・管理クラス一覧。</summary>
		private readonly List<CInputXBOX360> inputList = new List<CInputXBOX360>(1);

		/// <summary>プレイヤー一覧。</summary>
		private readonly List<PlayerIndex> m_playerList = new List<PlayerIndex>(1);

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateManager()
		{
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>プレイヤー一覧を取得します。</summary>
		/// 
		/// <value>プレイヤー一覧。</value>
		public ReadOnlyCollection<PlayerIndex> playerList
		{
			get
			{
				return m_playerList.AsReadOnly();
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
		/// <param name="buttonsState">ボタンの入力状態一覧。</param>
		public override void setup(CInput entity, List<SInputState> buttonsState)
		{
			base.setup(entity, buttonsState);
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
		)
		{
			base.update(entity, buttonsState, gameTime);
			int nLength = entity.buttonStateList.Count;
			foreach(CInputXBOX360 input in inputList)
			{
				input.update(gameTime);
				if(input.currentState == CState.empty)
				{
					removePlayer(input);
				}
				for(int i = nLength - 1; i >= 0; i--)
				{
					buttonsState[i] |= input.buttonStateList[i];
				}
			}
			behavior.update(gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="buttonsState">ボタンの入力状態一覧。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(
			CInput entity, List<SInputState> buttonsState, GameTime gameTime
		)
		{
			base.draw(entity, buttonsState, gameTime);
			behavior.draw(gameTime);
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
		public override void teardown(IEntity entity, object privateMembers, IState nextState)
		{
			base.teardown(entity, privateMembers, nextState);
			behavior.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理するXBOX360コントローラ追加の予約をします。</summary>
		/// <remarks>
		/// 既に登録されているか、<paramref name="playerIndex"/>に対応する
		/// XBOX360コントローラが接続されていない場合、予約は失敗します。
		/// </remarks>
		/// 
		/// <param name="playerIndex">対応するプレイヤー番号。</param>
		/// <returns>予約が成功した場合、<c>true</c>。</returns>
		public bool addPlayer(PlayerIndex playerIndex)
		{
			bool bResult = !playerList.Contains(playerIndex);
			if(bResult)
			{
				m_playerList.Add(playerIndex);
				CInputXBOX360 input = new CInputXBOX360(playerIndex);
				input.initialize();
				bResult = input.currentState != CState.empty;
				if(bResult)
				{
					input.changedState += onChangedState;
					inputList.Add(input);
				}
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理するXBOX360コントローラ削除の予約をします。</summary>
		/// <remarks>既に登録されていない場合、予約は失敗します。</remarks>
		/// 
		/// <param name="playerIndex">対応するプレイヤー番号。</param>
		/// <returns>予約が成功した場合、<c>true</c>。</returns>
		public bool removePlayer(PlayerIndex playerIndex)
		{
			bool bResult = false;
			CInputXBOX360 input = inputList.Find(i => i.playerIndex == playerIndex);
			if(input != null)
			{
				bResult = removePlayer(input);
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理するXBOX360コントローラ削除の予約をします。</summary>
		/// <remarks>既に登録されていない場合、予約は失敗します。</remarks>
		/// 
		/// <param name="input">コントローラ 入力制御・管理クラス。</param>
		/// <returns>予約が成功した場合、<c>true</c>。</returns>
		public bool removePlayer(CInputXBOX360 input)
		{
			bool bResult = m_playerList.Remove(input.playerIndex);
			if(bResult)
			{
				input.changedState -= onChangedState;
				input.Dispose();
				inputList.Remove(input);
				if(behavior.currentState == CState.empty)
				{
					behavior.nextState = CStateDetector.instance;
				}
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>子オブジェクトの状態が変化したときに呼び出されるメソッドです。</summary>
		/// 
		/// <param name="sender">送信元のオブジェクト。</param>
		/// <param name="e">状態変化情報。</param>
		private void onChangedState(object sender, CEventChangedState e)
		{
			CInputXBOX360 input = (CInputXBOX360)sender;
			if(e.next == CState.empty)
			{
				removePlayer(input);
			}
		}
	}
}
