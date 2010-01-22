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
using System.Linq;
using danmaq.nineball.state;
using danmaq.nineball.state.input.xbox360;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ゲーム コントローラ入力制御・管理クラス。</summary>
	public sealed class CInputXBOX360 : CInput
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>プレイヤー番号一覧。</summary>
		public static readonly ReadOnlyCollection<PlayerIndex> allPlayerIndex = new List<PlayerIndex>
		{
			PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four
		}.AsReadOnly();

		/// <summary>クラス オブジェクト一覧。</summary>
		private static readonly ReadOnlyCollection<CInputXBOX360> instanceList;

		/// <summary>XBOX360 プレイヤー番号。</summary>
		public readonly PlayerIndex playerIndex;

		/// <summary>ボタン割り当て値の一覧。</summary>
		private readonly List<Buttons> m_assignList = new List<Buttons>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static CInputXBOX360()
		{
			List<CInputXBOX360> _instanceList = new List<CInputXBOX360>(allPlayerIndex.Count);
			foreach(PlayerIndex playerIndex in allPlayerIndex)
			{
				_instanceList.Add(new CInputXBOX360(playerIndex));
			}
			instanceList = _instanceList.AsReadOnly();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CInputXBOX360(PlayerIndex playerIndex)
			: base(-1, CStateDefault.instance)
		{
			this.playerIndex = playerIndex;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>接続されているかどうかを取得します。</summary>
		/// 
		/// <value>接続されている場合、<c>true</c>。</value>
		public override bool connect
		{
			get
			{
				return playerNumber >= 0 && GamePad.GetState(playerIndex).IsConnected;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// 状態として、nullを設定しようとした場合。
		/// </exception>
		public new IState<CInputXBOX360, List<SInputState>> nextState
		{
			set
			{
				nextStateBase = value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ボタン割り当て値の一覧を設定/取得します。</summary>
		/// 
		/// <value>ボタン割り当て値の一覧。</value>
		public IList<Buttons> assignList
		{
			get
			{
				return m_assignList;
			}
			set
			{
				m_assignList.Clear();
				m_assignList.AddRange(value);
				int buttonsNum = ButtonsNum;
				while(m_assignList.Count > buttonsNum)
				{
					m_assignList.RemoveAt(m_assignList.Count - 1);
				}
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>クラス オブジェクトを取得します。</summary>
		/// 
		/// <param name="playerIndex">XBOX360 プレイヤー番号。</param>
		/// <param name="playerNumber">設定したいプレイヤー番号。</param>
		/// <returns>プレイヤー番号に対応したクラス オブジェクト。</returns>
		/// <exception cref="System.ArgumentException">
		/// 該当XBOX360プレイヤー番号のクラス オブジェクトが既に使用中である場合。
		/// </exception>
		/// <exception cref="System.InvalidOperationException">
		/// XBOX360プレイヤー番号に対応したクラス オブジェクトが見つからなかった場合。
		/// </exception>
		public static CInputXBOX360 getInstance(PlayerIndex playerIndex, short playerNumber)
		{
			CInputXBOX360 instance = instanceList.First(input => input.playerIndex == playerIndex);
			if(instance.connect)
			{	// クラス オブジェクトが既に使用中である場合
				throw new ArgumentException("playerIndex");
			}
			return instance;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// XBOX360ゲーム コントローラを自動認識する入力制御・管理クラスを作成します。
		/// </summary>
		/// 
		/// <param name="playerNumber">設定したいプレイヤー番号。</param>
		/// <returns>XBOX360ゲーム コントローラ入力制御・管理クラスコレクション。</returns>
		public static CInputCollection createXBOX360Detector(short playerNumber)
		{
			return new CInputCollection(
				playerNumber, state.input.collection.CStateXBOX360Detect.instance);
		}
	}
}
