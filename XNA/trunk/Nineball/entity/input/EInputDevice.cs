////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.state;
using danmaq.nineball.state.input;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>入力デバイス列挙体。</summary>
	[Flags]
	public enum EInputDevice : byte
	{

		/// <summary>入力デバイスなし。</summary>
		None = 0,

		/// <summary>キーボード。</summary>
		Keyboard = 1 << 0,

		/// <summary>XBOX360コントローラ。</summary>
		XBOX360 = 1 << 1,

#if WINDOWS && !DISABLE_LEGACY
		/// <summary>レガシ コントローラ。</summary>
		Legacy = 1 << 2,
#endif

		/// <summary>対応する全てのコントローラ。</summary>
		All =
#if WINDOWS && !DISABLE_LEGACY
			Keyboard | XBOX360 | Legacy,
#else
 Keyboard | XBOX360,
#endif

	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>入力デバイス列挙体の拡張機能。</summary>
	public static class EInputDeviceExtension
	{

		//* -----------------------------------------------------------------------*
		/// <summary>対応する状態を取得します。</summary>
		/// 
		/// <param name="device">入力デバイス定数。</param>
		/// <returns>対応する状態。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 複数の定数を定義した場合。
		/// </exception>
		public static IState<CInputParent, List<SInputState>> getState(this EInputDevice device)
		{
			IState<CInputParent, List<SInputState>> result = null;
			switch(device)
			{
				case EInputDevice.None:
					break;
				case EInputDevice.Keyboard:
					result = CStateKeyboard.instance;
					break;
				case EInputDevice.XBOX360:
					result = state.input.xbox360.CStateManager.instance;
					break;
#if WINDOWS && !DISABLE_LEGACY
				case EInputDevice.Legacy:
					result = state.input.legacy.CStateManager.instance;
					break;
#endif
				default:
					throw new ArgumentOutOfRangeException("device");
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>対応する状態を取得します。</summary>
		/// 
		/// <param name="device">入力デバイス定数。</param>
		/// <param name="enabled">有効なデバイスに対応する状態一覧。</param>
		/// <param name="disabled">無効なデバイスに対応する状態一覧。</param>
		public static void getState(
			this EInputDevice device,
			out List<IState<CInputParent, List<SInputState>>> enabled,
			out List<IState<CInputParent, List<SInputState>>> disabled
		)
		{
			EInputDevice[] list = {
				EInputDevice.Keyboard, EInputDevice.XBOX360,
#if WINDOWS && !DISABLE_LEGACY
				EInputDevice.Legacy
#endif
			};
			EInputDevice _device = (device & EInputDevice.All);
			enabled = new List<IState<CInputParent, List<SInputState>>>();
			disabled = new List<IState<CInputParent, List<SInputState>>>();
			IState<CInputParent, List<SInputState>> state;
			foreach(EInputDevice target in list)
			{
				state = (_device & target).getState();
				if(state == null)
				{
					disabled.Add(target.getState());
				}
				else
				{
					enabled.Add(state);
				}
			}
		}
	}
}
