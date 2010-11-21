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
		private readonly Func<GamePadState, Vector3>[] processorList;

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
			Func<GamePadState, Vector3>[] processorList =
				new Func<GamePadState, Vector3>[(int)EGamePadButtons.__reserved];
			processorList[(int)EGamePadButtons.none] = (state) => Vector3.Zero;
			processorList[(int)EGamePadButtons.start] = (state) =>
				Vector3.UnitZ * (float)state.Buttons.Start;
			processorList[(int)EGamePadButtons.back] = (state) =>
				Vector3.UnitZ * (float)state.Buttons.Back;
			processorList[(int)EGamePadButtons.bigButton] = (state) =>
				Vector3.UnitZ * (float)state.Buttons.BigButton;
			processorList[(int)EGamePadButtons.A] = (state) =>
				Vector3.UnitZ * (float)state.Buttons.A;
			processorList[(int)EGamePadButtons.B] = (state) =>
				Vector3.UnitZ * (float)state.Buttons.B;
			processorList[(int)EGamePadButtons.X] = (state) =>
				Vector3.UnitZ * (float)state.Buttons.X;
			processorList[(int)EGamePadButtons.Y] = (state) =>
				Vector3.UnitZ * (float)state.Buttons.Y;
			processorList[(int)EGamePadButtons.leftShoulder] = (state) =>
				Vector3.UnitZ * (float)state.Buttons.LeftShoulder;
			processorList[(int)EGamePadButtons.rightShoulder] = (state) =>
				Vector3.UnitZ * (float)state.Buttons.RightShoulder;
			processorList[(int)EGamePadButtons.leftTrigger] = (state) =>
				Vector3.UnitZ * state.Triggers.Left;
			processorList[(int)EGamePadButtons.rightTrigger] = (state) =>
				Vector3.UnitZ * state.Triggers.Right;
			processorList[(int)EGamePadButtons.dPad] = (state) =>
			{
				Vector3 v =
					-Vector3.UnitY * (float)state.DPad.Up +
					Vector3.UnitY * (float)state.DPad.Down +
					-Vector3.UnitX * (float)state.DPad.Left +
					Vector3.UnitX * (float)state.DPad.Right;
				if (v.Length() > 0)
				{
					v.Normalize();
				}
				return v;
			};
			processorList[(int)EGamePadButtons.leftThumb] = (state) =>
			    new Vector3(state.ThumbSticks.Left, (float)state.Buttons.LeftStick);
			processorList[(int)EGamePadButtons.rightThumb] = (state) => 
			    new Vector3(state.ThumbSticks.Right, (float)state.Buttons.RightStick);
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
			float threshold = entity.threshold;
			for (int i = assign.Count; --i >= 0; )
			{
				Vector3 v3 = processorList[assign[i]](nowState);
				Vector2 v2 = new Vector2(v3.X, v3.Y);
				if (v2.Length() < threshold)
				{
					v3.X = 0;
					v3.Y = 0;
				}
				if (v3.Z < threshold)
				{
					v3.Z = 0;
				}
				buttons[i] = buttons[i].updateVelocity(v3);
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
