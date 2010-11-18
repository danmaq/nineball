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
using danmaq.nineball.entity.input;
using danmaq.nineball.entity.input.low;
using danmaq.nineball.util.collection.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.input
{

	using CAdapter = CInputAdapter<CXNAInput<MouseState>, MouseState>;

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>高位入力制御・管理クラスのマウス用の状態。</summary>
	public sealed class CStateMouseInput
		: CState<CAdapter, CAdapter.CPrivateMembers>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState<CAdapter, CAdapter.CPrivateMembers> instance =
			new CStateMouseInput();

		/// <summary>プロセッサ一覧。</summary>
		private readonly Func<SInputInfo, MouseState, SInputInfo>[] processorList;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateMouseInput()
		{
			Func<SInputInfo, MouseState, SInputInfo>[] processorList =
				new Func<SInputInfo, MouseState, SInputInfo>[(int)EMouseButtons.__reserved];
			processorList[(int)EMouseButtons.None] = (info, state) => info;
			processorList[(int)EMouseButtons.position] = (info, state) =>
				info.updatePosition(new Vector3(state.X, state.Y, 0));
			processorList[(int)EMouseButtons.scrollWheel] = (info, state) =>
				info.updatePosition(new Vector3(0, 0, state.ScrollWheelValue));
			processorList[(int)EMouseButtons.leftButton] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.LeftButton));
			processorList[(int)EMouseButtons.middleButton] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.MiddleButton));
			processorList[(int)EMouseButtons.rightButton] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.RightButton));
			processorList[(int)EMouseButtons.xButton1] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.XButton1));
			processorList[(int)EMouseButtons.xButton2] = (info, state) =>
				info.updateVelocity(new Vector3(0, 0, (float)state.XButton2));
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
				entity.lowerInput = CMouseInputCollection.instance.input;
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
			for (int i = assign.Count; --i >= 0; )
			{
				buttons[i] = processorList[assign[i]](buttons[i], entity.lowerInput.nowInputState);
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
