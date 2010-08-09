////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.data.content;
using danmaq.nineball.state;
using danmaq.nineball.state.manager;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ローダータスク クラス。</summary>
	public class CContentLoader : CEntity
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers : IDisposable
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>読込対象のコンテンツ一覧。</summary>
			public Queue<ICache> contents;

			/// <summary>読込対象のコンテンツ合計数。</summary>
			public int total;

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>フィールドのオブジェクトを解放します。</summary>
			public void Dispose()
			{
				contents = null;
				total = 0;
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _private = new CPrivateMembers();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>読み込みターンの待機フレーム数。</summary>
		public ushort interval = 1;

		/// <summary>1読み込みターン毎に読み込むファイル数。</summary>
		public ushort loadPerFrame = 1;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>進捗率を取得します。</summary>
		/// 
		/// <value>進捗率。最低値は0で、最大値は1。</value>
		public float progress
		{
			get
			{
				int total = _private.total;
				return (float)(total - _private.contents.Count) / (float)total;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// 状態として、nullを設定しようとした場合。
		/// </exception>
		public new IState<CContentLoader, CPrivateMembers> nextState
		{
			set
			{
				base.nextState = value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// オブジェクトと状態クラスのみがアクセス可能なフィールドを取得します。
		/// </summary>
		/// 
		/// <value>オブジェクトと状態クラスのみがアクセス可能なフィールド。</value>
		protected override object privateMembers
		{
			get
			{
				return _private;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>読込対象のコンテンツ一覧をセットします。</summary>
		/// 
		/// <param name="contents">読込対象のコンテンツ一覧。</param>
		public void setTarget(IEnumerable<ICache> contents)
		{
			_private.contents = new Queue<ICache>(contents);
			_private.total = _private.contents.Count;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>読込を開始します。</summary>
		public void start()
		{
			nextState = CStateContentLoader.instance;
		}
	}
}
