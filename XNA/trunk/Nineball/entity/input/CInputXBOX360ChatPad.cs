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
using danmaq.nineball.entity.input.data;
using danmaq.nineball.Properties;
using danmaq.nineball.state;
using danmaq.nineball.state.input.xbox360;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360チャットパッド入力制御・管理クラス。</summary>
	public sealed class CInputXBOX360ChatPad : CInputKeyboard
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト一覧。</summary>
		private static readonly ReadOnlyCollection<CInputXBOX360ChatPad> instanceList;

		/// <summary>XBOX360 プレイヤー番号。</summary>
		public readonly PlayerIndex playerIndex;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>XBOX360 ゲーム コントローラ入力制御・管理クラス。</summary>
		private CInputXBOX360 inputController = null;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static CInputXBOX360ChatPad()
		{
			List<CInputXBOX360ChatPad> _instanceList =
				new List<CInputXBOX360ChatPad>(CInputXBOX360.allPlayerIndex.Count);
			foreach(PlayerIndex playerIndex in CInputXBOX360.allPlayerIndex)
			{
				_instanceList.Add(new CInputXBOX360ChatPad(playerIndex));
			}
			instanceList = _instanceList.AsReadOnly();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="playerIndex">XBOX360 プレイヤー番号。</param>
		private CInputXBOX360ChatPad(PlayerIndex playerIndex)
			: base(-1, CStateChatPad.instance)
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
		/// <summary>方向ボタンの状態をベクトルで取得します。</summary>
		/// <remarks>
		/// 取得時は、XBOX360 ゲーム コントローラ入力制御・管理クラスの
		/// エイリアスとして機能します。設定はサポートしません。
		/// </remarks>
		/// 
		/// <value>方向ボタンの状態。</value>
		/// <exception cref="System.NotSupportedException">
		/// 設定を試みた場合。(このクラスでは、設定はサポートしません。)
		/// </exception>
		public override Vector2 axisVector
		{
			get
			{
				return inputController.axisVector;
			}
			protected set
			{
				throw new NotSupportedException(Resources.ERR_READONLY);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンの状態をフラグで取得します。</summary>
		/// <remarks>
		/// 取得時は、XBOX360 ゲーム コントローラ入力制御・管理クラスの
		/// エイリアスとして機能します。設定はサポートしません。
		/// </remarks>
		/// <example>
		/// bool bDown = (obj.axisFlag &amp; EDirectionFlags.down) != 0;
		/// </example>
		/// 
		/// <value>方向ボタンの状態。</value>
		/// <exception cref="System.NotSupportedException">
		/// 設定を試みた場合。(このクラスでは、設定はサポートしません。)
		/// </exception>
		public override EDirectionFlags axisFlag
		{
			get
			{
				return inputController.axisFlag;
			}
			protected set
			{
				throw new NotSupportedException(Resources.ERR_READONLY);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// 状態として、nullを設定しようとした場合。
		/// </exception>
		public new IState<CInputXBOX360ChatPad, List<SInputState>> nextState
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
		/// <param name="inputController">
		/// XBOX360ゲーム コントローラ入力制御・管理クラス。
		/// </param>
		/// <param name="playerNumber">設定したいプレイヤー番号。</param>
		/// <returns>プレイヤー番号に対応したクラス オブジェクト。</returns>
		/// <exception cref="System.ArgumentException">
		/// 該当XBOX360プレイヤー番号のゲーム コントローラが未接続である場合、または
		/// 該当XBOX360プレイヤー番号のクラス オブジェクトが既に使用中である場合。
		/// </exception>
		/// <exception cref="System.InvalidOperationException">
		/// XBOX360プレイヤー番号に対応したクラス オブジェクトが見つからなかった場合。
		/// </exception>
		public static CInputXBOX360ChatPad getInstance(
			CInputXBOX360 inputController, short playerNumber
		)
		{
			if(!inputController.connect)
			{	// XBOX360ゲーム コントローラが未接続である場合
				throw new ArgumentException("inputController");
			}
			CInputXBOX360ChatPad instance =
				instanceList.First(input => input.playerIndex == inputController.playerIndex);
			if(instance.connect)
			{	// クラス オブジェクトが既に使用中である場合
				throw new ArgumentException("inputController");
			}
			instance.playerNumber = playerNumber;
			instance.inputController = inputController;
			return instance;
		}
	}
}
