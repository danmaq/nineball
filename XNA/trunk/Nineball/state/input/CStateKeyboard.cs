////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.entity.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>キーボード専用の入力状態。</summary>
	public sealed class CStateKeyboard : CState<CInput, List<SInputState>>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateKeyboard instance = new CStateKeyboard();

		/// <summary>キー割り当て値の一覧。</summary>
		public readonly List<Keys> assignList = new List<Keys>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateKeyboard()
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="buttonsState">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// キー割り当てがボタンの数よりも少ない場合。
		/// </exception>
		public override void update(
			CInput entity, List<SInputState> buttonsState, GameTime gameTime
		)
		{
			KeyboardState state = Keyboard.GetState();
			while(buttonsState.Count > assignList.Count)
			{
				assignList.Add(Keys.None);
			}
			for(int i = buttonsState.Count - 1; i >= 0; i--)
			{
				buttonsState[i].refresh(state.IsKeyDown(assignList[i]));
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 現在のキー割り当て一覧を破棄して、新しい割り当てを設定します。
		/// </summary>
		/// 
		/// <param name="collection">キー割り当て一覧。</param>
		public void setAssignList(IEnumerable<Keys> collection)
		{
			assignList.Clear();
			assignList.AddRange(collection);
		}
	}
}
