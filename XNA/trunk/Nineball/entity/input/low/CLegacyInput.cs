////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if WINDOWS

using System;
using System.Collections.Generic;
using danmaq.nineball.data.input;
using danmaq.nineball.Properties;
using danmaq.nineball.state.input.low;
using danmaq.nineball.util.caps;
using Microsoft.DirectX.DirectInput;
using Microsoft.Xna.Framework;

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

		/// <summary>フォースの最大値。</summary>
		public const int FORCE_GAIN = 10000;

		/// <summary>ウィンドウ ハンドル。</summary>
		public readonly IntPtr hWnd;

		/// <summary>レガシ ゲームパッド デバイス。</summary>
		public readonly Device device;

		/// <summary>デバイスの性能レポート。</summary>
		public readonly string capsReport;

		/// <summary>デバイスの初期化エラー レポート。</summary>
		public readonly string errorReport;

		/// <summary>フォース フィードバックが有効かどうか。</summary>
		public readonly bool availableForceFeedback;

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _privateMembers;

		/// <summary>レガシ ゲームパッド用に変換されたフォース情報一覧。</summary>
		private readonly Dictionary<SForceData, EffectObject> effectList =
			new Dictionary<SForceData, EffectObject>();

		/// <summary>フォース フィードバックを開始するための関数。</summary>
		private readonly System.Action startForceFeedback = () =>
		{
		};

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>フォース情報。</summary>
		private SForceData m_force;

		/// <summary>軸情報。</summary>
		private int[] m_axis;

		/// <summary>現在アクティブなフォース。</summary>
		private EffectObject m_currentEffect;

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
			string errorReport = string.Empty;
			try
			{
				device = new Device(guid);
				device.SetDataFormat(DeviceDataFormat.Joystick);
				capsReport =
					device.DeviceInformation.createCapsReport() + device.Caps.createCapsReport();
				availableForceFeedback = initializeForceFeedBack(ref errorReport);
				initializeAxis();
				device.Acquire();
				startForceFeedback = _startForceFeedback;
			}
			catch (Exception e)
			{
				string err = device == null ?
					Resources.INPUT_ERR_LEGACY_INIT_FAILED :
					Resources.INPUT_ERR_LEGACY_INIT_FAILED_MAYBE;
				errorReport += err + Environment.NewLine + e.ToString();
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

		//* -----------------------------------------------------------------------*
		/// <summary>フォース情報を取得/設定します。</summary>
		/// 
		/// <value>フォース情報。</value>
		public SForceData force
		{
			get
			{
				return m_force;
			}
			set
			{
				m_force = value;
				startForceFeedback();
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			_privateMembers.Dispose();
			effectList.Clear();
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
					throw new ApplicationException(Resources.INPUT_WARN_LEGACY_CENTER, e);
				}
				if (hWnd == IntPtr.Zero)
				{
					throw new ApplicationException(Resources.INPUT_WARN_LEGACY_EXCLUSIVE);
				}
				device.SetCooperativeLevel(hWnd, CooperativeLevelFlags.Exclusive | coLevel);
			}
			catch (Exception e)
			{
				result = false;
				errorReport += Resources.INPUT_WARN_LEGACY_COOP + enter + e.ToString();
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
			m_axis = anAxis;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>フォース フィードバックを開始します。</summary>
		private void _startForceFeedback()
		{
			if (m_force.zero)
			{
				if (m_currentEffect != null)
				{
					m_currentEffect.Stop();
					m_currentEffect = null;
				}
			}
			else
			{
				EffectObject eo = null;
				if (!effectList.TryGetValue(force, out eo))
				{
					int start = (int)(FORCE_GAIN * MathHelper.Max(
						force.strengthL.smooth(0, force.durationL),
						force.strengthS.smooth(0, force.durationS)));
					int end = (int)(FORCE_GAIN * MathHelper.Max(
						force.strengthL.smooth(force.duration, force.durationL),
						force.strengthS.smooth(force.duration, force.durationS)));
					Effect effect = new Effect();
					EffectType effectType;
					if (start == end)
					{
						effectType = EffectType.ConstantForce;
						ConstantForce constant = new ConstantForce();
						constant.Magnitude = start;
						effect.Constant = constant;
					}
					else
					{
						effectType = EffectType.RampForce;
						RampForce ramp = new RampForce();
						ramp.Start = start;
						ramp.End = end;
						effect.RampStruct = ramp;
					}
					effect.EffectType = effectType;
					Guid guid = Guid.Empty;
					foreach (EffectInformation ei in device.GetEffects(effectType))
					{
						guid = ei.EffectGuid;
					}
					if (guid != Guid.Empty)
					{
						effect.SetDirection(new int[m_axis.Length]);
						effect.SetAxes(new int[1]);
						effect.Gain = FORCE_GAIN;
						effect.SamplePeriod = 0;
						effect.TriggerButton = (int)Button.NoTrigger;
						effect.TriggerRepeatInterval = (int)DI.Infinite;
						effect.Flags = EffectFlags.ObjectOffsets | EffectFlags.Spherical;
						effect.UsesEnvelope = false;
						effect.Duration = force.duration * 16666;
						eo = new EffectObject(guid, effect, device);
					}
				}
				// nullである場合、GUIDの取得に失敗している
				if (eo != null)
				{
					if (m_currentEffect != null)
					{
						m_currentEffect.Stop();
					}
					try
					{
						eo.Start(1);	// コケる場合がある
						m_currentEffect = eo;
					}
					catch (Exception)
					{
						// TODO: 握りつぶしてしまうのもなんか宜しくないが……とりあえず応急処置。
					}
				}
			}
		}
	}
}

#endif
