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
using System.Linq;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using danmaq.nineball.state.input.xbox360;
using System;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ゲーム コントローラ入力制御・管理クラス。</summary>
	public sealed class CInputXBOX360 : CInput
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト一覧。</summary>
		private static readonly ReadOnlyCollection<CInputXBOX360> instanceList;

		/// <summary>XBOX360 プレイヤー番号。</summary>
		public readonly PlayerIndex playerIndex;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		static CInputXBOX360()
		{
			PlayerIndex[] all =
			{
				PlayerIndex.One,
				PlayerIndex.Two,
				PlayerIndex.Three,
				PlayerIndex.Four,
			};
			List<CInputXBOX360> _instanceList = new List<CInputXBOX360>(all.Length);
			foreach(PlayerIndex playerIndex in all)
			{
				_instanceList.Add(new CInputXBOX360(playerIndex));
			}
			instanceList = _instanceList.AsReadOnly();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CInputXBOX360(PlayerIndex playerIndex)
			: base(-1, CStateDefault.empty)
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
			{
				throw new ArgumentException("playerIndex");
			}
			return instance;
		}
	}
}
