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
using danmaq.nineball.state;
using danmaq.nineball.util.caps;
using Microsoft.DirectX.DirectInput;

namespace danmaq.nineball.entity.input {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ用 入力制御・管理クラス。</summary>
	public sealed class CInputLegacy : CInput {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>レガシ ゲームパッド デバイス。</summary>
		public readonly Device device;

		/// <summary>デバイスの性能レポート。</summary>
		public readonly string capsReport;

		/// <summary>フォース フィードバックをサポートするかどうか。</summary>
		public readonly bool enableForceFeedback;

		/// <summary>エラー レポート。</summary>
		public readonly string errorReport;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="guid">デバイスのインスタンスGUID。</param>
		/// <param name="hWnd">ウィンドウ ハンドル。</param>
		public CInputLegacy( Guid guid, IntPtr hWnd ) : base( CState.empty ) {
			string strErrReport = "";
			device = new Device( guid );
			capsReport =
				device.DeviceInformation.createCapsReport() + device.Caps.createCapsReport();
			device.SetDataFormat( DeviceDataFormat.Joystick );
			enableForceFeedback = initializeForceFeedback( hWnd, ref strErrReport );
			initializeAxis();
			errorReport = strErrReport;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose() {
			if( device != null ) {
				try {
					device.Unacquire();
					device.Dispose();
				}
				catch( Exception ) { }
			}
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>フォース フィードバックの初期化をします。</summary>
		/// 
		/// <param name="hWnd">ウィンドウ ハンドル。</param>
		/// <param name="strErrReport">エラー レポート。</param>
		/// <returns>フォース フィードバックの初期化に成功した場合、<c>true</c>。</returns>
		private bool initializeForceFeedback( IntPtr hWnd, ref string strErrReport ) {
			bool bResult = true;
			CooperativeLevelFlags coLevel =
				CooperativeLevelFlags.NoWindowsKey | CooperativeLevelFlags.Background;
			try { device.Properties.AutoCenter = false; }
			catch( Exception e ) {
				bResult = false;
				strErrReport += "ゲームパッドのオート・センター機能のOFFに出来ませんでした。" + Environment.NewLine;
				strErrReport += "このゲームパッドではフォース フィードバックの使用はできません。" + Environment.NewLine;
				strErrReport += e.ToString();
				hWnd = IntPtr.Zero;
			}
			if( hWnd == IntPtr.Zero ) {
				device.SetCooperativeLevel( null, CooperativeLevelFlags.NonExclusive | coLevel );
			}
			else {
				try {
					device.SetCooperativeLevel( hWnd, CooperativeLevelFlags.Exclusive | coLevel );
				}
				catch( Exception e ) {
					bResult = false;
					strErrReport += "ゲームパッドの独占に失敗しました。共有モードで再設定を試みます。" + Environment.NewLine;
					strErrReport += "このモードではフォースフィードバックの使用は出来ません。" + Environment.NewLine;
					strErrReport += e.ToString();
					hWnd = IntPtr.Zero;
					device.SetCooperativeLevel(
						null, CooperativeLevelFlags.NonExclusive | coLevel );
				}
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コントローラの軸の初期化をします。</summary>
		private void initializeAxis() {
			device.Properties.AxisModeAbsolute = true;
			int[] anAxis = null;
			foreach( DeviceObjectInstance doi in device.Objects ) {
				if( ( doi.ObjectId & ( int )DeviceObjectTypeFlags.Axis ) != 0 ) {
					device.Properties.SetRange(
						ParameterHow.ById, doi.ObjectId, new InputRange( -1000, 1000 ) );
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
