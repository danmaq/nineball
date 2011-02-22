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
using System.Text.RegularExpressions;
using danmaq.nineball.util;
using Microsoft.DirectX.DirectInput;

namespace danmaq.nineball.old.core.raw
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ デバイス用ゲームパッド入力管理クラス。</summary>
	/// <remarks>
	/// <para>
	/// XNAではXBOX360コントローラしか扱えないので、XNAの前身である
	/// Managed DirectX1.0を使用してゲームパッドの制御をします。
	/// </para>
	/// <para>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </para>
	/// </remarks>
	[Obsolete("このクラスは今後サポートされません。danmaq.nineball.util.collection.input.CInputHelper使用してください。")]
	public sealed class CLegacyInputManager : IDisposable
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>レガシ デバイスのリスト。</summary>
		public readonly List<CLegacyInput> DEVICES = new List<CLegacyInput>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// <remarks>
		/// 接続されているデバイスを認識し、初期化します。XBOX360コントローラを
		/// 自動的に識別し、初期化の対象から外します。
		/// </remarks>
		/// 
		/// <param name="__hWnd">ウィンドウハンドル</param>
		public CLegacyInputManager(IntPtr __hWnd)
		{
			CLogger.add("レガシ ゲームパッドの初期化をしています...");
			bool bFailed = false;
			DeviceList pads =
				Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
			foreach(DeviceInstance pad in pads)
			{
				if(!(Regex.IsMatch(pad.ProductName, "Xbox ?360", RegexOptions.IgnoreCase)))
				{
					CLegacyInput device = new CLegacyInput(pad.InstanceGuid, __hWnd);
					if(device.isDisposed)
					{
						bFailed = true;
					}
					else
					{
						DEVICES.Add(device);
					}
					DEVICES.Add(device);
				}
			}
			if(bFailed)
			{
				CLogger.add(
					"いくつかのレガシ ゲームパッドにおいて初期化が失敗しました。\r\n" +
					"最新のDirectXがインストールされていない可能性があります。\r\n" +
					"このランタイムは、下記のWebサイトで入手することが出来ます。\r\n\r\n" +
					"(日本語) http://www.microsoft.com/japan/windows/DirectX/");
			}
			CLogger.add("レガシ ゲームパッドの初期化完了。");
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>デバイスの名前一覧。</summary>
		public List<string> names
		{
			get
			{
				List<string> listResult = new List<string>();
				foreach(CLegacyInput device in DEVICES)
				{
					listResult.Add(device.infomation.ProductName);
				}
				return listResult;
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
			CLogger.add("レガシ ゲームパッドの解放をしています...");
			foreach(CLegacyInput device in DEVICES)
			{
				device.Dispose();
			}
			DEVICES.Clear();
			CLogger.add("レガシ ゲームパッドの解放完了。");
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ゲームパッドの入力状態を取得します。</summary>
		/// <remarks>
		/// XBOX360コントローラ以外で最も若い内部IDを
		/// 持つゲームパッドの状態を取得します。
		/// </remarks>
		/// 
		/// <returns>ジョイスティック デバイスの状態構造体</returns>
		/// <exception cref="DeviceNotRegisteredException">
		/// デバイスまたはデバイス インスタンスがDirectInputに登録されていない場合
		/// </exception>
		public CLegacyInput getDevice()
		{
			if(DEVICES.Count == 0)
			{
				throw new DeviceNotRegisteredException();
			}
			return DEVICES[0];
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ゲームパッドの入力状態を取得します。</summary>
		/// <remarks>
		/// <para>
		/// GUIDに対応したゲームパッドを探し、対象の入力状態を取得します。
		/// </para>
		/// <para>
		/// 見つからなかった場合、XBOX360コントローラ以外で
		/// 最も若い内部IDを持つゲームパッドの状態を取得します。
		/// </para>
		/// </remarks>
		/// 
		/// <param name="guid">プロダクトGUID</param>
		/// <returns>ジョイスティック デバイスの状態構造体</returns>
		/// <exception cref="DeviceNotRegisteredException">
		/// デバイスまたはデバイス インスタンスがDirectInputに登録されていない場合
		/// </exception>
		public CLegacyInput getDevice(Guid guid)
		{
			foreach(CLegacyInput device in DEVICES)
			{
				if(device.infomation.ProductGuid.Equals(guid))
				{
					return device;
				}
			}
			return getDevice();
		}

	}
}

#endif
