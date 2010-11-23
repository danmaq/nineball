////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.data.input;
using danmaq.nineball.entity.input;
using danmaq.nineball.state;

namespace danmaq.nineball.util.collection.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>入力管理クラスのヘルパ クラス。</summary>
	public class CInputHelper
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>アクティブな入力管理クラスのコレクション。</summary>
		public readonly CInputAdapterAdapter collection = new CInputAdapterAdapter();

		/// <summary>キーボード用入力管理クラス。</summary>
		public readonly CKeyboard keyboard = new CKeyboard();

		/// <summary>マウス用入力管理クラス。</summary>
		public readonly CMouse mouse = new CMouse();

		/// <summary>XBOX360ゲームパッド用入力管理クラス。</summary>
		public readonly CGamePad gamePad = new CGamePad();

		/// <summary>レガシ ゲームパッド用入力管理クラス。</summary>
		public readonly CLegacyGamePad legacy = new CLegacyGamePad();

		/// <summary>デバイス フラグに対応するデバイス一覧。</summary>
		private readonly List<KeyValuePair<EDevice, IInputAdapter>> devices =
			new List<KeyValuePair<EDevice,IInputAdapter>>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>アクティブなデバイス一覧。</summary>
		private EDevice m_devices;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CInputHelper()
		{
			keyboard.lowerInput = CKeyboardInputCollection.instance.input;
			gamePad.lowerInput = CGamePadInputCollection.instance.inputList[0];
#if WINDOWS
			if (CLegacyInputCollection.instance.inputList.Count > 0)
			{
				legacy.lowerInput = CLegacyInputCollection.instance.inputList[0];
			}
#endif
			EDevice dev = EDevice.keyboard | EDevice.mouse | EDevice.gamePad;
			if (legacy.lowerInput != null)
			{
				dev |= EDevice.legacyGamePad;
			}
			else
			{
				legacy.nextState = CState.empty;
			}
			devices.Add(new KeyValuePair<EDevice, IInputAdapter>(EDevice.keyboard, keyboard));
			devices.Add(new KeyValuePair<EDevice, IInputAdapter>(EDevice.mouse, mouse));
			devices.Add(new KeyValuePair<EDevice, IInputAdapter>(EDevice.gamePad, gamePad));
			devices.Add(new KeyValuePair<EDevice, IInputAdapter>(EDevice.legacyGamePad, legacy));
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>アクティブなデバイス一覧を取得します。</summary>
		/// 
		/// <value>アクティブなデバイス一覧。</value>
		public EDevice activeDevice
		{
			get
			{
				return m_devices;
			}
			set
			{
				m_devices = value;
				collection.lowerInput.Clear();
				for (int i = devices.Count; --i >= 0; )
				{
					if ((value & devices[i].Key) > 0)
					{
						collection.lowerInput.Add(devices[i].Value);
					}
				}
			}
		}
	}
}
