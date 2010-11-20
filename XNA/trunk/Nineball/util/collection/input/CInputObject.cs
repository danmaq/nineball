////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.entity.input;
using danmaq.nineball.entity.input.low;
using danmaq.nineball.state.input;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.util.collection.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>高位入力制御・管理クラスのヘルパ クラス。</summary>
	public sealed class CInputObject
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>キーボード用入力管理クラス。</summary>
		public readonly CInputAdapter<CXNAInput<KeyboardState>, KeyboardState> keyboard
			= new CInputAdapter<CXNAInput<KeyboardState>, KeyboardState>(
				CStateKeyboardInput.instance);

		/// <summary>マウス用入力管理クラス。</summary>
		public readonly CInputAdapter<CXNAInput<MouseState>, MouseState> mouse
			= new CInputAdapter<CXNAInput<MouseState>, MouseState>(
				CStateMouseInput.instance);

		/// <summary>XBOX360ゲームパッド用入力管理クラス。</summary>
		public readonly CInputAdapter<CXNAInput<GamePadState>, GamePadState> gamePad
			= new CInputAdapter<CXNAInput<GamePadState>, GamePadState>(
				CStateGamePadInput.instance);

		/// <summary>レガシ ゲームパッド用入力管理クラス。</summary>
#if WINDOWS
		public readonly CInputAdapter<CLegacyInput, Microsoft.DirectX.DirectInput.JoystickState> legacy
			= new CInputAdapter<CLegacyInput, Microsoft.DirectX.DirectInput.JoystickState>(
				CStateLegacyInput.instance);
#else
		public readonly CInputEmptyAdapter legacy = new CInputEmptyAdapter();
#endif


		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CInputObject()
		{
			keyboard.lowerInput = CKeyboardInputCollection.instance.input;
			mouse.lowerInput = CMouseInputCollection.instance.input;
			gamePad.lowerInput = CGamePadInputCollection.instance.inputList[0];
#if WINDOWS
			if (CLegacyInputCollection.instance.inputList.Count > 0)
			{
				legacy.lowerInput = CLegacyInputCollection.instance.inputList[0];
			}
#endif
		}
	}
}
