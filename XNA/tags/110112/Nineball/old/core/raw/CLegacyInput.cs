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
using System.Collections.Generic;
using danmaq.nineball.old.core.data;
using danmaq.nineball.Properties;
using danmaq.nineball.util;
using danmaq.nineball.util.caps;
using Microsoft.DirectX.DirectInput;

namespace danmaq.nineball.old.core.raw
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ デバイス クラス。</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	public sealed class CLegacyInput : IDisposable
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>フォース フィードバック エフェクト一覧。</summary>
		private readonly Dictionary<EForcePreset, EffectObject> FORCE_PATTERNS =
			new Dictionary<EForcePreset, EffectObject>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>レガシ ゲームパッド デバイス。</summary>
		private Device m_device = null;

		/// <summary>解放済みかどうか。</summary>
		private Exception m_exception = null;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="guid">デバイスのインスタンスGUID</param>
		/// <param name="hWnd">ウィンドウ ハンドル</param>
		public CLegacyInput(Guid guid, IntPtr hWnd)
		{
			try
			{
				device = new Device(guid);
				CLogger.add("◎◎ レガシ ゲームパッド情報\r\n" + ToString());
				CooperativeLevelFlags coLevel =
					CooperativeLevelFlags.NoWindowsKey | CooperativeLevelFlags.Background;
				device.SetDataFormat(DeviceDataFormat.Joystick);
				try
				{
					device.Properties.AutoCenter = false;
				}
				catch(Exception e)
				{
					CLogger.add(Resources.WARN_LEGACY_CENTER);
					CLogger.add(e);
					hWnd = IntPtr.Zero;
				}
				if(hWnd == IntPtr.Zero)
				{
					device.SetCooperativeLevel(null, CooperativeLevelFlags.NonExclusive | coLevel);
				}
				else
				{
					try
					{
						device.SetCooperativeLevel(hWnd, CooperativeLevelFlags.Exclusive | coLevel);
					}
					catch(Exception e)
					{
						CLogger.add(Resources.WARN_LEGACY_EXCLUSIVE);
						CLogger.add(Resources.WARN_LEGACY_COOP);
						CLogger.add(e);
						hWnd = IntPtr.Zero;
						device.SetCooperativeLevel(null, CooperativeLevelFlags.NonExclusive | coLevel);
					}
				}
				device.Properties.AxisModeAbsolute = true;
				int[] anAxis = null;
				foreach(DeviceObjectInstance doi in device.Objects)
				{
					if((doi.ObjectId & (int)DeviceObjectTypeFlags.Axis) != 0)
					{
						device.Properties.SetRange(
							ParameterHow.ById, doi.ObjectId, new InputRange(-1000, 1000));
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
				try
				{
					if(hWnd == IntPtr.Zero)
					{
						throw new UnsupportedException("共有モードではフォースフィードバックは使用は出来ません。");
					}
					FORCE_PATTERNS.Add(EForcePreset.Square, createEffect(anAxis, 5000, 583333));
					FORCE_PATTERNS.Add(EForcePreset.Short, createEffect(anAxis, 5000, 0, 100000));
					FORCE_PATTERNS.Add(EForcePreset.Mild, createEffect(anAxis, 5000, 0, 1000000));
					FORCE_PATTERNS.Add(EForcePreset.Hard, createEffect(anAxis, 10000, 0, 2000000));
				}
				catch(Exception e)
				{
					CLogger.add("フォース フィードバックの作成に失敗しました。");
					CLogger.add(e);
				}
				device.Acquire();
			}
			catch(Exception e)
			{
				CLogger.add(Resources.ERR_LEGACY_INIT_FAILED);
				CLogger.add(e);
				exception = e;
				Dispose();
			}
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>解放済みかどうか。</summary>
		public bool isDisposed
		{
			get
			{
				return m_exception != null;
			}
		}

		/// <summary>エラー情報。</summary>
		public Exception exception
		{
			get
			{
				return m_exception;
			}
			private set
			{
				if(m_exception == null)
				{
					m_exception = value;
				}
			}
		}

		/// <summary>デバイスの情報。</summary>
		/// <exception cref="ObjectDisposedException">
		/// 解放後に呼ばれた場合。
		/// </exception>
		public DeviceInstance infomation
		{
			get
			{
				if(isDisposed)
				{
					throw exception;
				}
				return device.DeviceInformation;
			}
		}

		/// <summary>ゲームパッドの入力状態を取得します。</summary>
		/// <exception cref="ObjectDisposedException">
		/// 解放後に呼ばれた場合。
		/// </exception>
		public JoystickState state
		{
			get
			{
				if(isDisposed)
				{
					throw exception;
				}
				try
				{
					device.Poll();
				}
				catch(NotAcquiredException)
				{
					device.Acquire();
					device.Poll();
				}
				return device.CurrentJoystickState;
			}
		}

		/// <summary>使用するフォース フィードバックのプリセット。</summary>
		/// <exception cref="ObjectDisposedException">
		/// 解放後に呼ばれた場合。
		/// </exception>
		public EForcePreset force
		{
			set
			{
				if(isCanUseForce)
				{
					if(isDisposed)
					{
						throw exception;
					}
					if(value == EForcePreset.None)
					{
						foreach(EffectObject eo in FORCE_PATTERNS.Values)
						{
							eo.Stop();
						}
					}
					else
					{
						EffectObject eo;
						if(FORCE_PATTERNS.TryGetValue(value, out eo))
						{
							eo.Start(1);
						}
					}
				}
			}
		}

		/// <summary>フォース フィードバックに対応しているかどうか。</summary>
		public bool isCanUseForce
		{
			get
			{
				return FORCE_PATTERNS.Count > 0;
			}
		}

		/// <summary>レガシ ゲームパッド デバイス。</summary>
		private Device device
		{
			get
			{
				return m_device;
			}
			set
			{
				m_device = value;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// アンマネージ リソースの解放およびリセットに関連付けられている
		/// アプリケーション定義のタスクを実行します。
		/// </summary>
		public void Dispose()
		{
			try
			{
				foreach(EffectObject eo in FORCE_PATTERNS.Values)
				{
					eo.Dispose();
				}
				FORCE_PATTERNS.Clear();
			}
			catch(Exception e)
			{
				CLogger.add("レガシ ゲームパッドの解放に失敗しました。");
				CLogger.add(e);
			}
			if(device != null)
			{
				try
				{
					device.Unacquire();
					device.Dispose();
				}
				catch(Exception e)
				{
					CLogger.add("レガシ ゲームパッドの解放に失敗しました。");
					CLogger.add(e);
				}
				device = null;
			}
			exception = new ObjectDisposedException(GetType().ToString());
		}

		//* -----------------------------------------------------------------------*
		/// <summary>デバイスの環境レポートを作成します。</summary>
		/// 
		/// <returns>デバイスの環境レポート 文字列</returns>
		public override string ToString()
		{
			string strResult = "◎◎ レガシ ゲームパッド情報\r\n";
			strResult += device.DeviceInformation.createCapsReport();
			strResult += device.Caps.createCapsReport();
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// フォース フィードバックのプリセットデータを作成します。
		/// </summary>
		/// 
		/// <param name="anAxis">軸データ</param>
		/// <param name="nMagnitude">フォースの量</param>
		/// <param name="nDuration">フォースがかかる時間</param>
		/// <returns>フォース フィードバック管理クラス</returns>
		/// <exception cref="NotSupportedException"></exception>
		private EffectObject createEffect(int[] anAxis, int nMagnitude, int nDuration)
		{
			foreach(EffectInformation ei in device.GetEffects(EffectType.All))
			{
				if(DInputHelper.GetTypeCode(ei.EffectType) == (int)EffectType.ConstantForce)
				{
					Effect effect = createEffect(anAxis);
					effect.EffectType = EffectType.ConstantForce;
					effect.Constant = new ConstantForce();
					effect.Constant.Magnitude = nMagnitude;
					// HACK : ここで例外もなしにコケることがある。なんでだ？
					return new EffectObject(ei.EffectGuid, effect, device);
				}
			}
			throw new NotSupportedException();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// フォース フィードバックのプリセットデータを作成します。
		/// </summary>
		/// 
		/// <param name="anAxis">軸データ</param>
		/// <param name="nStartMagnitude">フォースが始まる時の値</param>
		/// <param name="nLastMagnitude">フォースが終わる時の値</param>
		/// <param name="nDuration">フォースがかかる時間</param>
		/// <returns>フォース フィードバック管理クラス</returns>
		/// <exception cref="NotSupportedException"></exception>
		private EffectObject createEffect(
			int[] anAxis, int nStartMagnitude, int nLastMagnitude, int nDuration
		)
		{
			foreach(EffectInformation ei in device.GetEffects(EffectType.All))
			{
				if(DInputHelper.GetTypeCode(ei.EffectType) == (int)EffectType.RampForce)
				{
					Effect effect = createEffect(anAxis);
					effect.EffectType = EffectType.RampForce;
					effect.RampStruct = new RampForce();
					effect.RampStruct.Start = nStartMagnitude;
					effect.RampStruct.End = nLastMagnitude;
					return new EffectObject(ei.EffectGuid, effect, device);
				}
			}
			throw new NotSupportedException();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// フォース フィードバックのための共通データを作成します。
		/// </summary>
		/// 
		/// <param name="anAxis">軸データ</param>
		/// <returns>フォース フィードバック設定用構造体</returns>
		private Effect createEffect(int[] anAxis)
		{
			Effect effect = new Effect();
			effect.SetDirection(new int[anAxis.Length]);
			effect.SetAxes(new int[1]);
			effect.Gain = 10000;
			effect.SamplePeriod = 0;
			effect.TriggerButton = (int)Button.NoTrigger;
			effect.TriggerRepeatInterval = (int)DI.Infinite;
			effect.Flags = EffectFlags.ObjectOffsets | EffectFlags.Spherical;
			effect.UsesEnvelope = false;
			return effect;
		}
	}
}

#endif
