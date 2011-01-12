////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.ball.core;
using danmaq.ball.data;
using danmaq.nineball.data.input;
using danmaq.nineball.entity.component;
using danmaq.nineball.state;
using danmaq.nineball.util.collection.input;
using Microsoft.Xna.Framework.Input;

namespace danmaq.ball.state.initialize
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>入力初期化の状態。</summary>
	sealed class CAIInupt
		: CAIBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CAIInupt instance = new CAIInupt();

		/// <summary>キーボードにおけるボタン割り当て。</summary>
		private readonly int[] assignKeyboard;

		/// <summary>XBOX360 ゲームパッドにおけるボタン割り当て。</summary>
		private readonly int[] assignGamePad;

		/// <summary>レガシ ゲームパッドにおけるボタン割り当て。</summary>
		private readonly int[] assignLegacyGamePad;

		/// <summary>現在有効なデバイス。</summary>
		private readonly EDevice activeDevice;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CAIInupt()
			: base("入力機能の初期化")
		{
			assignKeyboard = new int[(int)EInputActionMap.__reserved];
			assignGamePad = new int[(int)EInputActionMap.__reserved];
			assignLegacyGamePad = new int[(int)EInputActionMap.__reserved];
			assignKeyboard[(int)EInputActionMap.cursor] = (int)EKeyboardAxisButtons.arrow;
			assignKeyboard[(int)EInputActionMap.enter] = (int)Keys.Space;
			assignKeyboard[(int)EInputActionMap.cancel] = (int)Keys.Escape;
			assignGamePad[(int)EInputActionMap.cursor] = (int)EGamePadButtons.dPad;
			assignGamePad[(int)EInputActionMap.enter] = (int)EGamePadButtons.A;
			assignGamePad[(int)EInputActionMap.cancel] = (int)EGamePadButtons.back;
			assignLegacyGamePad[(int)EInputActionMap.cursor] = (int)ELegacyGamePadAxisButtons.analog;
			assignLegacyGamePad[(int)EInputActionMap.enter] = 0;
			assignLegacyGamePad[(int)EInputActionMap.cancel] = 1;
			activeDevice = EDevice.keyboard | EDevice.gamePad;
#if WINDOWS
			if (CLegacyInputCollection.instance.inputList.Count > 0)
			{
				activeDevice |= EDevice.legacyGamePad;
			}
#endif
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>次に遷移すべき状態を取得します。</summary>
		/// 
		/// <value>次に遷移すべき状態。</value>
		public override IState nextState
		{
			get
			{
				return CAIScreen.instance;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理を実行します。</summary>
		protected override void initialize()
		{
			CInputHelper input = CInput.instance;
			input.activeDevice = activeDevice;
			input.keyboard.assignList = new List<int>(assignKeyboard).AsReadOnly();
			input.gamePad.assignList = new List<int>(assignGamePad).AsReadOnly();
			input.legacy.assignList = new List<int>(assignLegacyGamePad).AsReadOnly();
			new CGameComponent(CGame.instance, input.collection, true);
			initializeSensitivity();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アナログ入力感度を初期化します。</summary>
		public void initializeSensitivity()
		{
			CInputHelper input = CInput.instance;
			float t = 0.8f;
			input.collection.threshold = t;
			input.keyboard.threshold = t;
			input.gamePad.threshold = t;
			input.legacy.threshold = t;
		}
	}
}
