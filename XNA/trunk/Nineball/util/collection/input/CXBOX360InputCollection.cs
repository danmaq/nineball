////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.ObjectModel;
using danmaq.nineball.entity.input.low;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.util.collection.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360デバイス共通低位入力制御・管理クラスのコレクション。</summary>
	/// 
	/// <typeparam name="_T">入力状態の型。</typeparam>
	public abstract class CXBOX360InputCollection<_T>
		: IInputCollection
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>ゲームパッド専用低位入力制御・管理クラス一覧。</summary>
		public readonly ReadOnlyCollection<CXNAInput<_T>> inputList;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="stateList">状態のコレクション。</param>
		public CXBOX360InputCollection(IList stateList)
		{
			int length = stateList.Count;
			CXNAInput<_T>[] array = new CXNAInput<_T>[length];
			for (int i = length; --i >= 0; )
			{
				array[i] = new CXNAInput<_T>((IState)stateList[i]);
			}
			inputList = Array.AsReadOnly<CXNAInput<_T>>(array);
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
		protected abstract bool isInput(_T now, _T prev);

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
		/// ボタン入力が検出されたデバイスの管理クラス。検出しなかった場合、<c>null</c>。
		/// </returns>
		public CXNAInput<_T> detectInput(GameTime gameTime)
		{
			CXNAInput<_T> result = null;
			for (int i = inputList.Count; --i >= 0 && result == null; )
			{
				CXNAInput<_T> input = inputList[i];
				input.update(gameTime);
				if (isInput(input.nowInputState, input.prevInputState))
				{
					result = input;
				}
			}
			return result;
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
