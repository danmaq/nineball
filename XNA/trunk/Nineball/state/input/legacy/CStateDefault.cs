////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if WINDOWS && !DISABLE_LEGACY

using System;
using danmaq.nineball.entity.input;
using Microsoft.DirectX.DirectInput;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.state.input.legacy {

	// TODO : AXISまでキーカスタマイズするのは難しいので何とかする

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ専用の入力状態。</summary>
	public class CStateDefault : CState<CInputLegacy, CInputLegacy.CPrivateMembers> {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateDefault instance = new CStateDefault();

		/// <summary>入力値の幅。</summary>
		private const int INPUTRANGE = 1000;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateDefault() { }

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
			CInputLegacy entity, CInputLegacy.CPrivateMembers privateMembers
		) {
			base.setup( entity, privateMembers );
			try {
				privateMembers.enableForceFeedback = initializeForceFeedback( privateMembers );
				initializeAxis( privateMembers );
			}
			catch( Exception e ) {
				privateMembers.errorReport += Environment.NewLine +
					"レガシ ゲーム コントローラは使用できません。";
				privateMembers.errorReport += Environment.NewLine + e.ToString();
				entity.Dispose();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>フォース フィードバックの初期化をします。</summary>
		/// 
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <returns>フォース フィードバックの初期化に成功した場合、<c>true</c>。</returns>
		private bool initializeForceFeedback(
			CInputLegacy.CPrivateMembers privateMembers
		) {
			bool bResult = true;
			try { privateMembers.device.Properties.AutoCenter = false; }
			catch( Exception e ) {
				bResult = false;
				privateMembers.errorReport +=
					"ゲームパッドのオート・センター機能のOFFに出来ませんでした。" +
					Environment.NewLine;
				privateMembers.errorReport +=
					"このゲームパッドではフォース フィードバックの使用はできません。" +
					Environment.NewLine;
				privateMembers.errorReport += e.ToString();
			}
			bool ZeroHandle = CInputLegacy.hWnd == IntPtr.Zero;
			CooperativeLevelFlags coLevel =
				CooperativeLevelFlags.NoWindowsKey | CooperativeLevelFlags.Background;
			try {
				if( !bResult || ZeroHandle ) {
					throw new ApplicationException(
						"ウィンドウ ハンドルが指定されていないため、ゲームパッドを独占出来ません。" );
				}
				privateMembers.device.SetCooperativeLevel(
					CInputLegacy.hWnd, CooperativeLevelFlags.Exclusive | coLevel );
			}
			catch( Exception e ) {
				bResult = false;
				privateMembers.errorReport +=
					"ゲームパッドの独占に失敗しました。共有モードで再設定を試みます。" +
					Environment.NewLine;
				privateMembers.errorReport +=
					"このモードではフォースフィードバックの使用は出来ません。" +
					Environment.NewLine;
				privateMembers.errorReport += e.ToString();
				privateMembers.device.SetCooperativeLevel(
					null, CooperativeLevelFlags.NonExclusive | coLevel );
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コントローラの軸の初期化をします。</summary>
		/// 
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		private void initializeAxis( CInputLegacy.CPrivateMembers privateMembers ) {
			privateMembers.device.Properties.AxisModeAbsolute = true;
			int[] anAxis = null;
			foreach( DeviceObjectInstance doi in privateMembers.device.Objects ) {
				if( ( doi.ObjectId & ( int )DeviceObjectTypeFlags.Axis ) != 0 ) {
					privateMembers.device.Properties.SetRange( ParameterHow.ById,
						doi.ObjectId, new InputRange( -INPUTRANGE, INPUTRANGE ) );
				}
				if( ( doi.Flags & ( int )ObjectInstanceFlags.Actuator ) != 0 ) {
					int[] __anAxis;
					if( anAxis == null ) { anAxis = new int[1]; }
					else {
						__anAxis = new int[anAxis.Length + 1];
						anAxis.CopyTo( __anAxis, 0 );
						anAxis = __anAxis;
					}
					anAxis[anAxis.Length - 1] = doi.Offset;
					if( anAxis.Length == 2 ) { break; }
				}
			}
		}
	}
}

#endif
