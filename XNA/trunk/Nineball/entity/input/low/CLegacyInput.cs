////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if WINDOWS

using System;
using danmaq.nineball.Properties;
using danmaq.nineball.state.input.low;
using danmaq.nineball.util.caps;
using Microsoft.DirectX.DirectInput;

namespace danmaq.nineball.entity.input.low
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲームパッド専用低位入力制御・管理クラス。</summary>
	public class CLegacyInput
		: CEntity, ILowerInput<JoystickState>
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers : IDisposable
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>最新の入力状態。</summary>
			public JoystickState nowState;

			/// <summary>前回の入力状態。</summary>
			public JoystickState prevState;

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>このオブジェクトの終了処理を行います。</summary>
			public void Dispose()
			{
				nowState = new JoystickState();
				prevState = new JoystickState();
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>入力値の幅。</summary>
		public const int RANGE = 1000;

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _privateMembers;

		/// <summary>ウィンドウ ハンドル。</summary>
		public readonly IntPtr hWnd;

		/// <summary>レガシ ゲームパッド デバイス。</summary>
		public readonly Device device;

		/// <summary>デバイスの性能レポート。</summary>
		public readonly string capsReport;

		/// <summary>デバイスの初期化エラー レポート。</summary>
		public readonly string errorReport;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="guid">デバイスのインスタンスGUID</param>
		/// <param name="hWnd">ウィンドウ ハンドル</param>
		public CLegacyInput(Guid guid, IntPtr hWnd)
			: base(CStateLegacyInput.instance, new CPrivateMembers())
		{
			_privateMembers = (CPrivateMembers)privateMembers;
			device = new Device(guid);
			capsReport =
				device.DeviceInformation.createCapsReport() + device.Caps.createCapsReport();
			device.SetDataFormat(DeviceDataFormat.Joystick);
			string errorReport = string.Empty;
			try
			{
				initializeForceFeedBack(ref errorReport);
				initializeAxis();
				device.Acquire();
			}
			catch (Exception e)
			{
				errorReport +=
					Resources.ERR_LEGACY_INIT_FAILED + Environment.NewLine + e.ToString();
			}
			this.errorReport = errorReport;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>最新の入力状態を取得します。</summary>
		/// 
		/// <value>最新の入力状態。</value>
		public JoystickState nowInputState
		{
			get
			{
				return _privateMembers.nowState;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>前回の入力状態を取得します。</summary>
		/// 
		/// <value>前回の入力状態。</value>
		public JoystickState prevInputState
		{
			get
			{
				return _privateMembers.prevState;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			_privateMembers.Dispose();
			device.Unacquire();
			device.Dispose();
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>フォース フィードバックの初期化をします。</summary>
		/// 
		/// <param name="errorReport">追記されるエラー レポート。</param>
		/// <returns>フォース フィードバックの初期化に成功した場合、<c>true</c>。</returns>
		private bool initializeForceFeedBack(ref string errorReport)
		{
			bool result = true;
			string enter = Environment.NewLine;
			CooperativeLevelFlags coLevel =
				CooperativeLevelFlags.NoWindowsKey | CooperativeLevelFlags.Background;
			try
			{
				try
				{
					device.Properties.AutoCenter = false;
				}
				catch (Exception e)
				{
					throw new ApplicationException(Resources.WARN_LEGACY_CENTER, e);
				}
				if (hWnd == IntPtr.Zero)
				{
					throw new ApplicationException(Resources.WARN_LEGACY_EXCLUSIVE);
				}
				device.SetCooperativeLevel(hWnd, CooperativeLevelFlags.Exclusive | coLevel);
			}
			catch (Exception e)
			{
				result = false;
				errorReport += Resources.WARN_LEGACY_COOP + enter + e.ToString();
				device.SetCooperativeLevel(null, CooperativeLevelFlags.NonExclusive | coLevel);
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コントローラの軸の初期化をします。</summary>
		private void initializeAxis()
		{
			device.Properties.AxisModeAbsolute = true;
			int[] anAxis = null;
			foreach (DeviceObjectInstance doi in device.Objects)
			{
				if ((doi.ObjectId & (int)DeviceObjectTypeFlags.Axis) != 0)
				{
					device.Properties.SetRange(ParameterHow.ById,
						doi.ObjectId, new InputRange(-RANGE, RANGE));
				}
				if ((doi.Flags & (int)ObjectInstanceFlags.Actuator) != 0)
				{
					if (anAxis == null)
					{
						anAxis = new int[1];
					}
					else
					{
						int[] __anAxis = new int[anAxis.Length + 1];
						anAxis.CopyTo(__anAxis, 0);
						anAxis = __anAxis;
					}
					anAxis[anAxis.Length - 1] = doi.Offset;
					if (anAxis.Length == 2)
					{
						break;
					}
				}
			}
		}
	}
}

#endif
