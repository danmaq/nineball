////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.state.input.low;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.util.collection.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360チャットパッド専用低位入力制御・管理クラスのコレクション。</summary>
	public sealed class CChatPadInputCollection
		: CXBOX360InputCollection<KeyboardState>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CChatPadInputCollection instance = new CChatPadInputCollection();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CChatPadInputCollection()
			: base(CStateKeyboardInput.chatPadInstanceList)
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
		protected override bool isInput(KeyboardState now, KeyboardState prev)
		{
			Keys[] nowKeys = now.GetPressedKeys();
			Keys[] prevKeys = prev.GetPressedKeys();
			return nowKeys.Length > prevKeys.Length;
		}
	}
}
