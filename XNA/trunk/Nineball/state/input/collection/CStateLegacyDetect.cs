////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if WINDOWS

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using danmaq.nineball.entity;
using danmaq.nineball.entity.input;
using danmaq.nineball.Properties;
using Microsoft.DirectX.DirectInput;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.state.input.collection
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// マンマシンI/F入力制御・管理クラスコレクション用レガシ ゲーム コントローラ自動認識状態。
	/// </summary>
	public sealed class CStateLegacyDetect : CState<CInputCollection, List<SInputState>>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateLegacyDetect instance = new CStateLegacyDetect();

		/// <summary>既定の入力状態。</summary>
		private readonly CStateDefault defaultState = CStateDefault.instance;

		/// <summary>
		/// レガシ ゲーム コントローラ入力制御・管理クラス オブジェクト一覧。
		/// </summary>
		private readonly ReadOnlyCollection<CInputLegacy> instanceList;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateLegacyDetect()
		{
			List<CInputLegacy> inputList = new List<CInputLegacy>();
			DeviceList controllers =
				Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
			foreach(DeviceInstance controller in controllers)
			{
				if(!(Regex.IsMatch(controller.ProductName, "Xbox ?360", RegexOptions.IgnoreCase)))
				{
					inputList.Add(new CInputLegacy(
						-1, controller.InstanceGuid, Process.GetCurrentProcess().Handle));
				}
			}
			instanceList = inputList.AsReadOnly();
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
		/// <param name="buttonsState">ボタン押下情報一覧。</param>
		public override void setup(CInputCollection entity, List<SInputState> buttonsState)
		{
			if(instanceList.Count == 0)
			{
				entity.Dispose();
			}
			setCapacity(entity);
			entity.releaseAwayController = true;
			defaultState.setup(entity, buttonsState);
			base.setup(entity, buttonsState);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="buttonsState">ボタン押下情報一覧。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(
			CInputCollection entity, List<SInputState> buttonsState, GameTime gameTime
		)
		{
			if(entity.Count == 0)
			{
				foreach(CInputLegacy input in instanceList)
				{
					// TODO : 作りかけ
				}
			}
			if(entity.Count != 0)
			{
				setCapacity(entity);
				entity.nextState = CStateWaitDetect.instance;
			}
			defaultState.update(entity, buttonsState, gameTime);
			base.update(entity, buttonsState, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="buttonsState">ボタン押下情報一覧。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(
			CInputCollection entity, List<SInputState> buttonsState, GameTime gameTime
		)
		{
			defaultState.draw(entity, buttonsState, gameTime);
			base.draw(entity, buttonsState, gameTime);
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
			defaultState.teardown(entity, privateMembers, nextState);
			base.teardown(entity, privateMembers, nextState);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>子入力クラスとして受け入れる最大値を初期化します。</summary>
		/// 
		/// <param name="entity">この状態を終了したオブジェクト。</param>
		/// <exception cref="System.InvalidOperationException">
		/// 現在の保有個数が2個以上の場合。
		/// </exception>
		private void setCapacity(CInputCollection entity)
		{
			try
			{
				entity.capacity = 1;
			}
			catch(Exception e)
			{
				throw new InvalidOperationException(Resources.ERR_INPUT_DETECT_DUPLICATION, e);
			}
		}
	}
}

#endif
