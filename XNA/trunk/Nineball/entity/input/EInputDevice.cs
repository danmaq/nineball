////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.state;
using System.Collections.Generic;
using danmaq.nineball.state.input;

namespace danmaq.nineball.entity.input {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>入力デバイス列挙体。</summary>
	[Flags]
	public enum EInputDevice : ushort {

		/// <summary>入力デバイスなし。</summary>
		None = 0,

		/// <summary>キーボード。</summary>
		Keyboard = 1 << 0,

		/// <summary>XBOX360コントローラ。</summary>
		XBOX360 = 1 << 1,

		/// <summary>レガシ コントローラ。</summary>
		Legacy = 1 << 2,

		/// <summary>対応する全てのコントローラ。</summary>
		All = Keyboard | XBOX360 | Legacy,

	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>入力デバイス列挙体の拡張機能。</summary>
	public static class EInputDeviceExtension {

		//* -----------------------------------------------------------------------*
		/// <summary>対応する状態を取得します。</summary>
		/// 
		/// <param name="device">入力デバイス定数。</param>
		/// <returns>対応する状態。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 複数の定数を定義した場合。
		/// </exception>
		public static IState<CInput, List<SInputState>> getState( this EInputDevice device ) {
			IState<CInput, List<SInputState>> result = null;
			switch( device ) {
				case EInputDevice.None:
					break;
				case EInputDevice.Keyboard:
					result = CStateKeyboard.instance;
					break;
				case EInputDevice.XBOX360:
					result = CStateXBOX360Controller.instance;
					break;
				case EInputDevice.Legacy:
					result = CStateLegacyController.instance;
					break;
				default:
					throw new ArgumentOutOfRangeException( "device" );
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
			out List<IState<CInput, List<SInputState>>> enabled,
			out List<IState<CInput, List<SInputState>>> disabled
		) {
			EInputDevice[] list = {
				EInputDevice.Keyboard, EInputDevice.XBOX360, EInputDevice.Legacy
			};
			EInputDevice _device = ( device & EInputDevice.All );
			enabled = new List<IState<CInput,List<SInputState>>>();
			disabled = new List<IState<CInput,List<SInputState>>>();
			IState<CInput, List<SInputState>> state;
			foreach( EInputDevice target in list ) {
				state = ( _device & target ).getState();
				if( state == null ) { disabled.Add( target.getState() ); }
				else { enabled.Add( state ); }
			}
		}
	}
}
