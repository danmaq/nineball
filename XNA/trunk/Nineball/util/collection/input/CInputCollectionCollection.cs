////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.util.collection.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>低位入力制御・管理クラスのコレクションのコレクション。</summary>
	public sealed class CInputCollectionCollection
		: List<IInputCollection>, IInputCollection
	{

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CInputCollectionCollection()
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="capacity">新しいリストに格納できる要素の数。</param>
		private CInputCollectionCollection(int capacity)
			: base(capacity)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="collection">
		/// 新しいリストに要素がコピーされたコレクション。
		/// </param>
		private CInputCollectionCollection(IEnumerable<IInputCollection> collection)
			: base(collection)
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
		/// <returns>検出されたボタン。</returns>
		public IInputCollection detectInput(GameTime gameTime)
		{
			return Find(c => c.detectInput(gameTime));
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
