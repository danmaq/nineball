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
using danmaq.nineball.data.input;
using danmaq.nineball.entity.input;
using danmaq.nineball.entity.input.low;
using danmaq.nineball.util.math;
using Microsoft.DirectX.DirectInput;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.state.input
{

	using CAdapter = CInputAdapter<CLegacyInput, JoystickState>;

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>高位入力制御・管理クラスのレガシ ゲームパッド用の状態。</summary>
	public sealed class CStateLegacyInput
		: CState<CAdapter, CAdapter.CPrivateMembers>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState<CAdapter, CAdapter.CPrivateMembers> instance =
			new CStateLegacyInput();

		/// <summary>プロセッサ一覧。</summary>
		private readonly Func<SInputInfo, JoystickState, CAdapter, SInputInfo>[] processorList;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateLegacyInput()
		{
			Func<SInputInfo, JoystickState, CAdapter, SInputInfo>[] processorList =
				new Func<SInputInfo, JoystickState, CAdapter, SInputInfo>
					[-(int)ELegacyGamePadAxisButtons.__reserved];
			processorList[0] = (info, state, entity) => info;
			processorList[-(int)ELegacyGamePadAxisButtons.analog] =
				(info, state, entity) => info.updateVelocityWithAxisHPF(
					new Vector3(state.X, state.Y, state.Z) / (float)CLegacyInput.RANGE,
					entity.threshold);
			processorList[-(int)ELegacyGamePadAxisButtons.pov] =
				(info, state, entity) => info.updateVelocityWithAxisHPF(
					getVelocityFromPOV(state), entity.threshold);
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
			JoystickState nowState = entity.lowerInput.nowInputState;
			float threshold = entity.threshold;
			byte[] buffer = nowState.GetButtons();
			for (int i = assign.Count; --i >= 0; )
			{
				int id = assign[i];
				if (id >= 0)
				{
					buttons[i] = buttons[i].updateVelocity(Vector3.UnitZ *
						CInterpolate._amountSmooth(buffer[id], byte.MaxValue));
				}
				else
				{
					buttons[i] = processorList[-id](buttons[i], nowState, entity);
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
		/// <summary>ハット スイッチの角度情報をベクトルに変換します。。</summary>
		/// 
		/// <param name="state">最新の入力状態。</param>
		/// <returns>ベクトル。Z軸はダミー値。</returns>
		private Vector3 getVelocityFromPOV(JoystickState state)
		{
			Vector3 result = Vector3.Zero;
			int[] povList = state.GetPointOfView();
			if (povList.Length > 0)
			{
				int pov = povList[0];
				if (pov >= 0)
				{
					float angle = MathHelper.ToRadians(pov * 0.01f);
					Quaternion q = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, angle);
					result = Vector3.Transform(-Vector3.UnitY, q);
				}
			}
			return result;
		}
	}
}

#endif
