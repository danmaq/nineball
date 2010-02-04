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
using danmaq.nineball.old.core.data;
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
	[Obsolete("このクラスは今後サポートされません。CInputManagerを使用してください。")]
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
					CLogger.add("ゲームパッドのオート・センター機能のOFFに出来ませんでした。");
					CLogger.add("このゲームパッドではフォース フィードバックは使用できません。");
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
						CLogger.add("アプリケーションによるレガシ ゲームパッドの独占に失敗しました。");
						CLogger.add("(共有モードで再設定を試みます。このモードではフォースフィードバックの使用は出来ません。)");
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
				CLogger.add("レガシ ゲームパッドの初期化に失敗しました。");
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
			strResult += ToString(device.DeviceInformation);
			strResult += ToString(device.Caps);
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

		//* -----------------------------------------------------------------------*
		/// <summary>デバイスの識別情報レポートを作成します。</summary>
		/// 
		/// <param name="info">デバイスの識別情報 列挙オブジェクト</param>
		/// <returns>デバイスの識別情報レポート 文字列</returns>
		private string ToString(DeviceInstance info)
		{
			string strResult = "▽ レガシ ゲームパッド デバイスの識別情報一覧\r\n";
			strResult += "  デバイス名・説明    : " + info.ProductName + Environment.NewLine;
			strResult += "  デバイスGUID        : " + info.ProductGuid.ToString() + Environment.NewLine;
			strResult += "  インスタンス登録名  : " + info.InstanceName + Environment.NewLine;
			strResult += "  インスタンスGUID    : " + info.InstanceGuid.ToString() + Environment.NewLine;
			strResult += "  ForceFeedback GUID  : " + info.FFDriverGuid.ToString() + Environment.NewLine;
			strResult += "  デバイス タイプ     : " + info.DeviceType.ToString() + Environment.NewLine;
			strResult += "  デバイス サブタイプ : " + info.DeviceSubType.ToString() + Environment.NewLine;
			strResult += "  HID 使用コード      : " + info.Usage + Environment.NewLine;
			strResult += "  HID 使用ページ      : " + info.UsagePage + Environment.NewLine;
			return strResult;
		}


		//* -----------------------------------------------------------------------*
		/// <summary>デバイスの性能レポートを作成します。</summary>
		/// 
		/// <param name="caps">デバイスの性能 列挙オブジェクト</param>
		/// <returns>デバイスの性能レポート 文字列</returns>
		private string ToString(DeviceCaps caps)
		{
			string strResult = "▽ レガシ ゲームパッド デバイスの能力一覧" + Environment.NewLine;
			strResult += "  ドライバ バージョン       : " + caps.FFDriverVersion + Environment.NewLine;
			strResult += "  ファームウェア バージョン : " + caps.FirmwareRevision + Environment.NewLine;
			strResult += "  ハードウェア バージョン   : " + caps.HardwareRevision + Environment.NewLine;
			strResult += "  デバイスの最小分解能      : " + caps.FFMinTimeResolution + " ミリ秒" + Environment.NewLine;
			strResult += "  フォース命令送信最小間隔  : " + caps.FFSamplePeriod + " ミリ秒" + Environment.NewLine;
			strResult += "  使用可能な軸の数          : " + caps.NumberAxes + Environment.NewLine;
			strResult += "  使用可能なボタンの数      : " + caps.NumberButtons + Environment.NewLine;
			strResult += "  使用可能なPOVの数         : " + caps.NumberPointOfViews + Environment.NewLine;
			strResult += "  別デバイスのエイリアス    : " + caps.Alias.ToStringOX() + Environment.NewLine;
			strResult += "  物理的にアタッチされた    : " + caps.Attatched.ToStringOX() + Environment.NewLine;
			strResult += "  Emulateされた仮想デバイス : " + caps.Hidden.ToStringOX() + Environment.NewLine;
			strResult += "  ユーザー モード デバイス  : " + caps.Emulated.ToStringOX() + Environment.NewLine;
			strResult += "  デッドバンド              : " + caps.DeadBand.ToStringOX() + Environment.NewLine;
			strResult += "  フォース フィードバック   : " + caps.ForceFeedback.ToStringOX() + Environment.NewLine;
			strResult += "  フェード エフェクト       : " + caps.Fade.ToStringOX() + Environment.NewLine;
			strResult += "  遅延フォース エフェクト   : " + caps.StartDelay.ToStringOX() + Environment.NewLine;
			strResult += "  条件エフェクトの飽和      : " + caps.Saturation.ToStringOX() + Environment.NewLine;
			strResult += "  PosNegCoefficients        : " + caps.PosNegCoefficients.ToStringOX() + Environment.NewLine;
			strResult += "  PosNegSaturation          : " + caps.PosNegSaturation.ToStringOX() + Environment.NewLine;
			strResult += "  プレース ホルダ           : " + caps.Phantom.ToStringOX() + Environment.NewLine;
			strResult += "  Attack                    : " + caps.Attack.ToStringOX() + Environment.NewLine;
			strResult += "  PolledDataFormat          : " + caps.PolledDataFormat.ToStringOX() + Environment.NewLine;
			strResult += "  PolledDevice              : " + caps.PolledDevice.ToStringOX() + Environment.NewLine;
			return strResult;
		}
	}
}

#endif
