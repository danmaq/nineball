////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;
using danmaq.nineball.entity.input.low;
using danmaq.nineball.state.input.low;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.util.collection.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>キーボード専用低位入力制御・管理クラスのコレクション。</summary>
	public sealed class CKeyboardInputCollection
		: IInputCollection
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CKeyboardInputCollection instance = new CKeyboardInputCollection();

		/// <summary>キーボード専用低位入力制御・管理クラス。</summary>
		public readonly CXNAInput<KeyboardState> input =
			new CXNAInput<KeyboardState>(CStateKeyboardInput.keyboard);

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CKeyboardInputCollection()
		{
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
		/// 検出されたキー一覧。検出しなかった場合、<c>null</c>。
		/// </returns>
		public IEnumerable<Keys> detectInput(GameTime gameTime)
		{
			input.update(gameTime);
			Keys[] now = input.nowInputState.GetPressedKeys();
			Keys[] prev = input.prevInputState.GetPressedKeys();
			return now.Length > prev.Length ? now.Except(prev) : null;
		}

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
		/// ボタン入力が検出された場合、<c>true</c>。
		/// </returns>
		bool IInputCollection.detectInput(GameTime gameTime)
		{
			return detectInput(gameTime) != null;
		}
	}
}
