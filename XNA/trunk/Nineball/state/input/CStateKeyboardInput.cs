////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.data.input;
using danmaq.nineball.entity.input;
using danmaq.nineball.entity.input.low;
using danmaq.nineball.util.collection.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.input
{

	using CAdapter = CInputAdapter<CXNAInput<KeyboardState>, KeyboardState>;

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// 高位入力制御・管理クラスのキーボードまたはXBOX360チャットパッド用の状態。
	/// </summary>
	public sealed class CStateKeyboardInput
		: CState<CAdapter, CAdapter.CPrivateMembers>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState<CAdapter, CAdapter.CPrivateMembers> instance =
			new CStateKeyboardInput();

		/// <summary>プロセッサ一覧。</summary>
		private readonly Func<SInputInfo, KeyboardState, SInputInfo>[] processorList;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateKeyboardInput()
		{
			Func<SInputInfo, KeyboardState, SInputInfo>[] processorList =
				new Func<SInputInfo, KeyboardState, SInputInfo>[-(int)EKeyboardAxisButtons.__reserved];
			processorList[0] = (info, state) => info;
			processorList[-(int)EKeyboardAxisButtons.wsad] = (info, state) =>
				updateAxis(info, state, Keys.W, Keys.S, Keys.A, Keys.D);
			processorList[-(int)EKeyboardAxisButtons.ijkl] = (info, state) =>
				updateAxis(info, state, Keys.I, Keys.J, Keys.K, Keys.L);
			processorList[-(int)EKeyboardAxisButtons.arrow] = (info, state) =>
				updateAxis(info, state, Keys.Up, Keys.Down, Keys.Left, Keys.Right);
			processorList[-(int)EKeyboardAxisButtons.numpad] = (info, state) =>
				updateAxis(info, state, Keys.NumPad8, Keys.NumPad2, Keys.NumPad4, Keys.NumPad6);
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
				entity.lowerInput = CKeyboardInputCollection.instance.input;
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
			KeyboardState nowState = entity.lowerInput.nowInputState;
			for (int i = assign.Count; --i >= 0; )
			{
				int id = assign[i];
				if (id >= 0)
				{
					buttons[i] = buttons[i].updateVelocity(
						Vector3.UnitZ * Convert.ToInt32(nowState.IsKeyDown((Keys)id)));
				}
				else
				{
					buttons[i] = processorList[-id](buttons[i], nowState);
				}
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

		//* -----------------------------------------------------------------------*
		/// <summary>方向キーの情報を取得します。</summary>
		/// 
		/// <param name="prev">前回の入力情報。</param>
		/// <param name="state">低位入力クラスから取得した最新の情報。</param>
		/// <param name="up">上ボタンとして割り当てられたキー。</param>
		/// <param name="down">下ボタンとして割り当てられたキー。</param>
		/// <param name="left">左ボタンとして割り当てられたキー。</param>
		/// <param name="right">右ボタンとして割り当てられたキー。</param>
		/// <returns>最新の入力情報。</returns>
		private SInputInfo updateAxis(SInputInfo prev, KeyboardState state,
			Keys up, Keys down, Keys left, Keys right)
		{
			Vector3 v =
				Convert.ToInt32(state.IsKeyDown(up)) * -Vector3.UnitY +
				Convert.ToInt32(state.IsKeyDown(down)) * Vector3.UnitY +
				Convert.ToInt32(state.IsKeyDown(left)) * -Vector3.UnitX +
				Convert.ToInt32(state.IsKeyDown(right)) * Vector3.UnitX;
			if (v.Length() > 0)
			{
				v.Normalize();
			}
			return prev.updateVelocity(v);
		}
	}
}
