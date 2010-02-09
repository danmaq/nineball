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
using danmaq.nineball.entity.input;
using danmaq.nineball.entity.input.data;
using Microsoft.DirectX.DirectInput;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.state.input.legacy
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ既定の入力状態。</summary>
	public abstract class CStateDefaultBase : CState<CInputLegacy, CInputLegacy.CPrivateMembers>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>使用する方向ボタン。</summary>
		public readonly EAxisLegacy axisType;

		/// <summary>入力値の幅。</summary>
		protected const int INPUTRANGE = 1000;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="axisType">使用する方向ボタン。</param>
		protected CStateDefaultBase(EAxisLegacy axisType)
		{
			this.axisType = axisType;
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
		public override void setup(
			CInputLegacy entity, CInputLegacy.CPrivateMembers privateMembers)
		{
			try
			{
				privateMembers.enableForceFeedback = initializeForceFeedback(privateMembers);
				initializeAxis(privateMembers);
			}
			catch(Exception e)
			{
				privateMembers.errorReport += Environment.NewLine +
					"レガシ ゲーム コントローラは使用できません。";
				privateMembers.errorReport += Environment.NewLine + e.ToString();
				entity.Dispose();
			}
			base.setup(entity, privateMembers);
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
			CInputLegacy entity, CInputLegacy.CPrivateMembers privateMembers, GameTime gameTime)
		{
			JoystickState state = privateMembers.poll();
			byte[] buttons = state.GetButtons();
			int totalButtons = buttons.Length;
			for(int i = entity.assignList.Count; --i >= 0; )
			{
				short sButtonID = entity.assignList[i];
				if(sButtonID < 0)
				{
					privateMembers.buttonStateList[i].refresh(
						state.getInputState(sButtonID, INPUTRANGE));
				}
				else if(sButtonID < totalButtons)
				{
					privateMembers.buttonStateList[i].refresh(buttons[sButtonID] != 0);
				}
			}
			refleshAxis(state, entity, privateMembers, gameTime);
			base.update(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンの状態を更新します。</summary>
		/// 
		/// <param name="state">最新の入力情報。</param>
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		protected abstract void refleshAxis(
			JoystickState state, CInputLegacy entity,
			CInputLegacy.CPrivateMembers privateMembers, GameTime gameTime);

		//* -----------------------------------------------------------------------*
		/// <summary>フォース フィードバックの初期化をします。</summary>
		/// 
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <returns>フォース フィードバックの初期化に成功した場合、<c>true</c>。</returns>
		private bool initializeForceFeedback(
			CInputLegacy.CPrivateMembers privateMembers
		)
		{
			bool bResult = true;
			try
			{
				privateMembers.device.Properties.AutoCenter = false;
			}
			catch(Exception e)
			{
				bResult = false;
				privateMembers.errorReport +=
					"ゲームパッドのオート・センター機能のOFFに出来ませんでした。" +
					Environment.NewLine;
				privateMembers.errorReport +=
					"このゲームパッドではフォース フィードバックの使用はできません。" +
					Environment.NewLine;
				privateMembers.errorReport += e.ToString();
			}
			bool ZeroHandle = privateMembers.hWnd == IntPtr.Zero;
			CooperativeLevelFlags coLevel =
				CooperativeLevelFlags.NoWindowsKey | CooperativeLevelFlags.Background;
			try
			{
				if(!bResult || ZeroHandle)
				{
					throw new ApplicationException(
						"ウィンドウ ハンドルが指定されていないため、ゲームパッドを独占出来ません。");
				}
				privateMembers.device.SetCooperativeLevel(
					privateMembers.hWnd, CooperativeLevelFlags.Exclusive | coLevel);
			}
			catch(Exception e)
			{
				bResult = false;
				privateMembers.errorReport +=
					"ゲームパッドの独占に失敗しました。共有モードで再設定を試みます。" +
					Environment.NewLine;
				privateMembers.errorReport +=
					"このモードではフォースフィードバックの使用は出来ません。" +
					Environment.NewLine;
				privateMembers.errorReport += e.ToString();
				privateMembers.device.SetCooperativeLevel(
					null, CooperativeLevelFlags.NonExclusive | coLevel);
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コントローラの軸の初期化をします。</summary>
		/// 
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		private void initializeAxis(CInputLegacy.CPrivateMembers privateMembers)
		{
			privateMembers.device.Properties.AxisModeAbsolute = true;
			int[] anAxis = null;
			foreach(DeviceObjectInstance doi in privateMembers.device.Objects)
			{
				if((doi.ObjectId & (int)DeviceObjectTypeFlags.Axis) != 0)
				{
					privateMembers.device.Properties.SetRange(ParameterHow.ById,
						doi.ObjectId, new InputRange(-INPUTRANGE, INPUTRANGE));
				}
				if((doi.Flags & (int)ObjectInstanceFlags.Actuator) != 0)
				{
					int[] __anAxis;
					if(anAxis == null)
					{
						anAxis = new int[1];
					}
					else
					{
						__anAxis = new int[anAxis.Length + 1];
						anAxis.CopyTo(__anAxis, 0);
						anAxis = __anAxis;
					}
					anAxis[anAxis.Length - 1] = doi.Offset;
					if(anAxis.Length == 2)
					{
						break;
					}
				}
			}
		}
	}
}

#endif
