////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.state.input.low;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.util.collection.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ゲームパッド専用低位入力制御・管理クラスのコレクション。</summary>
	public sealed class CGamePadInputCollection
		: CXBOX360InputCollection<GamePadState>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CGamePadInputCollection instance = new CGamePadInputCollection();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>アナログ入力におけるハイパス値(0～1)。</summary>
		public float threshold = 0.25f;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CGamePadInputCollection()
			: base(CStateGamePadInput.instanceList)
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 前回の状態と現在の状態より、ボタン入力があったかどうかを取得します。
		/// </summary>
		/// 
		/// <param name="now">現在のキー入力状態。</param>
		/// <param name="prev">前回のキー入力状態。</param>
		/// <returns>ボタン入力があった場合、<c>true</c>。</returns>
		protected override bool isInput(GamePadState now, GamePadState prev)
		{
			bool result = now.IsConnected && prev.IsConnected;
			if (result)
			{
				GamePadButtons btns = now.Buttons;
				GamePadDPad dpad = now.DPad;
				GamePadThumbSticks nStks = now.ThumbSticks;
				GamePadThumbSticks pStks = prev.ThumbSticks;
				// TODO : トリガ忘れてね？
				result =
					btns != prev.Buttons && (
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
					dpad != prev.DPad && (
						dpad.Up == ButtonState.Pressed ||
						dpad.Down == ButtonState.Pressed ||
						dpad.Left == ButtonState.Pressed ||
						dpad.Right == ButtonState.Pressed) ||
					nStks != pStks && (
						(nStks.Left.Length() >= threshold && pStks.Left.Length() < threshold) ||
						(nStks.Right.Length() >= threshold && pStks.Right.Length() < threshold));
			}
			return result;
		}
	}
}
