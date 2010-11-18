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
using danmaq.nineball.data.input;
using danmaq.nineball.entity.input;
using danmaq.nineball.entity.input.low;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.input
{

	using CAdapter = CInputAdapter<CXNAInput<GamePadState>, GamePadState>;

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>高位入力制御・管理クラスのXBOX360 ゲームパッド用の状態。</summary>
	public sealed class CStateGamePadInput
		: CState<CAdapter, CAdapter.CPrivateMembers>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState<CAdapter, CAdapter.CPrivateMembers> instance =
			new CStateGamePadInput();

		/// <summary>プロセッサ一覧。</summary>
		private readonly Func<SInputInfo, GamePadState, SInputInfo>[] processorList;

		/// <summary>ヌル低位入力制御・管理クラス。</summary>
		private readonly CXNAInput<GamePadState> nullDevice = new CXNAInput<GamePadState>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateGamePadInput()
		{
			Vector3 up = -Vector3.UnitY;
			Vector3 down = Vector3.UnitY;
			Vector3 left = -Vector3.UnitX;
			Vector3 right = Vector3.UnitX;
			Func<SInputInfo, GamePadState, SInputInfo>[] processorList =
				new Func<SInputInfo, GamePadState, SInputInfo>[(int)EGamePadButtons.__reserved];
			processorList[(int)EGamePadButtons.start] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.Buttons.Start));
			processorList[(int)EGamePadButtons.back] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.Buttons.Back));
			processorList[(int)EGamePadButtons.bigButton] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.Buttons.BigButton));
			processorList[(int)EGamePadButtons.A] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.Buttons.A));
			processorList[(int)EGamePadButtons.B] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.Buttons.B));
			processorList[(int)EGamePadButtons.X] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.Buttons.X));
			processorList[(int)EGamePadButtons.Y] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.Buttons.Y));
			processorList[(int)EGamePadButtons.leftShoulder] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.Buttons.LeftShoulder));
			processorList[(int)EGamePadButtons.rightShoulder] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.Buttons.RightShoulder));
			processorList[(int)EGamePadButtons.leftTrigger] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, state.Triggers.Left));
			processorList[(int)EGamePadButtons.rightTrigger] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, state.Triggers.Right));
			processorList[(int)EGamePadButtons.dPad] = (info, state) => info.updateVelocity(
				up * (float)state.DPad.Up +
				down * (float)state.DPad.Down +
				left * (float)state.DPad.Left +
				right * (float)state.DPad.Right);
			processorList[(int)EGamePadButtons.leftThumb] = (info, state) => info.updateVelocity(
				new Vector3(state.ThumbSticks.Left, (float)state.Buttons.LeftStick));
			processorList[(int)EGamePadButtons.rightThumb] = (info, state) => info.updateVelocity(
				new Vector3(state.ThumbSticks.Right, (float)state.Buttons.RightStick));
			this.processorList = processorList;
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
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		public override void setup(CAdapter entity, CAdapter.CPrivateMembers privateMembers)
		{
			if (entity.lowerInput == null)
			{
				entity.lowerInput = nullDevice;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(
			CAdapter entity, CAdapter.CPrivateMembers privateMembers, GameTime gameTime)
		{
			entity.lowerInput.update(gameTime);
			IList<int> assign = entity.assignList;
			List<SInputInfo> buttons = privateMembers.buttonList;
			GamePadState nowState = entity.lowerInput.nowInputState;
			for (int i = assign.Count; --i >= 0; )
			{
				buttons[i] = processorList[assign[i]](buttons[i], nowState);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(
			CAdapter entity, CAdapter.CPrivateMembers privateMembers, GameTime gameTime)
		{
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
		public override void teardown(
			CAdapter entity, CAdapter.CPrivateMembers privateMembers, IState nextState)
		{
		}
	}
}
