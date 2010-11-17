////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.ObjectModel;
using danmaq.nineball.entity.input.low;
using danmaq.nineball.state.input.low;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.util.collection.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ゲームパッド専用低位入力制御・管理クラスのコレクション。</summary>
	public sealed class CGamePadInputCollection
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CGamePadInputCollection instance = new CGamePadInputCollection();

		/// <summary>ゲームパッド専用低位入力制御・管理クラス一覧。</summary>
		public readonly ReadOnlyCollection<CXNAInput<GamePadState>> inputList;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>アナログ入力におけるハイパス値(0～1)。</summary>
		public float threshold = 0.25f;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CGamePadInputCollection()
		{
			ReadOnlyCollection<CStateGamePadInput> stateList = CStateGamePadInput.instanceList;
			int length = stateList.Count;
			CXNAInput<GamePadState>[] array = new CXNAInput<GamePadState>[length];
			for (int i = length; --i >= 0; )
			{
				array[i] = new CXNAInput<GamePadState>(stateList[i]);
			}
			inputList = Array.AsReadOnly<CXNAInput<GamePadState>>(array);
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ボタン入力を検出します。</summary>
		/// <remarks>
		/// 注意: このメソッドを呼び出すと、自動的に登録されているクラスに対して
		/// <c>update()</c>が実行されます。レガシ ゲームパッドが高位入力管理クラスにて
		/// アクティブの状態でこのメソッドを呼び出すと、高位入力側の判定が
		/// 1フレーム分欠落します。
		/// </remarks>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		/// <returns>
		/// ボタン入力を検出が検出されたデバイスの管理クラス。
		/// 検出しなかった場合、<c>null</c>。
		/// </returns>
		public CXNAInput<GamePadState> detectInput(GameTime gameTime)
		{
			CXNAInput<GamePadState> result = null;
			for (int i = inputList.Count; --i >= 0 && result == null; )
			{
				CXNAInput<GamePadState> input = inputList[i];
				input.update(gameTime);
				GamePadState state = input.nowInputState;
				if (state.IsConnected)
				{
					GamePadButtons btns = state.Buttons;
					GamePadDPad dpad = state.DPad;
					GamePadThumbSticks sticks = state.ThumbSticks;
					if(
						btns != input.prevInputState.Buttons && (
							btns.A == ButtonState.Pressed ||
							btns.B == ButtonState.Pressed ||
							btns.Back == ButtonState.Pressed ||
							btns.BigButton == ButtonState.Pressed ||
							btns.LeftShoulder == ButtonState.Pressed ||
							btns.LeftStick == ButtonState.Pressed ||
							btns.RightShoulder == ButtonState.Pressed ||
							btns.RightStick == ButtonState.Pressed ||
							btns.Start == ButtonState.Pressed ||
							btns.X == ButtonState.Pressed ||
							btns.Y == ButtonState.Pressed) ||
						dpad != input.prevInputState.DPad && (
							dpad.Up == ButtonState.Pressed ||
							dpad.Down == ButtonState.Pressed ||
							dpad.Left == ButtonState.Pressed ||
							dpad.Right == ButtonState.Pressed) ||
						sticks != input.prevInputState.ThumbSticks && (
							sticks.Left.Length() > threshold ||
							sticks.Right.Length() > threshold))
					{
						result = input;
					}
				}
			}
			return result;
		}
	}
}
