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
using System.Collections.Generic;
using danmaq.nineball.state;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>タスク管理クラス。</para>
	/// <para>
	/// タスクの追加削除が少なく、総数も少ない場合は<c>CGameComponentManager</c>が
	/// 便利ですが、追加削除のオーバーヘッドが大きいため、
	/// それを回避したい場合はこのクラスを使うとよいでしょう。
	/// </para>
	/// </summary>
	public sealed class CTaskManager : CEntity, ICollection<ITask>
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>タスク登録情報。</summary>
		public struct SRemoveInfo
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>タスク本体。</summary>
			public readonly ITask task;

			/// <summary>削除時に呼び出される関数。</summary>
			public readonly Action<ITask> callback;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="task">タスク本体。</param>
			/// <param name="callback">削除時に呼び出される関数。</param>
			public SRemoveInfo(ITask task, Action<ITask> callback)
			{
				this.task = task;
				this.callback = callback;
			}
		}

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers : IDisposable
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>登録されているタスク一覧。</summary>
			public readonly List<ITask> tasks = new List<ITask>();

			/// <summary>追加予約されているタスク一覧。</summary>
			public readonly List<ITask> add = new List<ITask>();

			/// <summary>削除予約されているタスク一覧。</summary>
			public readonly List<SRemoveInfo> remove = new List<SRemoveInfo>();

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>フィールドのオブジェクトを解放します。</summary>
			public void Dispose()
			{
				tasks.Clear();
				add.Clear();
				remove.Clear();
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _private;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>全削除フラグ。</summary>
		private bool m_allClear = false;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CTaskManager()
			: this(null)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		public CTaskManager(IState firstState)
			: base(firstState, new CPrivateMembers())
		{
			_private = (CPrivateMembers)privateMembers;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>登録されているタスクの総数を取得します。</summary>
		/// 
		/// <value>登録されているタスクの総数。</value>
		public int Count
		{
			get
			{
				return _private.tasks.Count;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>この管理クラスが読み取り専用かどうかを取得します。</summary>
		/// 
		/// <value>この管理クラスが読み取り専用である場合、<c>true</c>。</value>
		public bool IsReadOnly
		{
			get
			{
				return ((ICollection<ITask>)_private.tasks).IsReadOnly;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>タスク追加の予約をします。</summary>
		/// 
		/// <param name="item">タスク。</param>
		public void Add(ITask item)
		{
			_private.add.Add(item);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>タスク削除の予約をします。</summary>
		/// 
		/// <param name="task">タスク。</param>
		/// <returns><c>true</c>。</returns>
		public bool Remove(ITask task)
		{
			return Remove(task, null);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>タスク削除の予約をします。</summary>
		/// 
		/// <param name="task">タスク。</param>
		/// <param name="callback">削除直前に実行されるアクション。</param>
		/// <returns><c>true</c>。</returns>
		public bool Remove(ITask task, Action<ITask> callback)
		{
			_private.remove.Add(new SRemoveInfo(task, callback));
			return true;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理しているタスクを全て解放するための予約を入れます。</summary>
		public void Clear()
		{
			m_allClear = true;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>特定の値が格納されているかどうかを判断します。</summary>
		/// 
		/// <param name="item">検索するオブジェクト。</param>
		/// <returns>存在する場合、<c>true</c>。</returns>
		public bool Contains(ITask item)
		{
			throw new NotImplementedException();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>要素を配列にコピーします。</summary>
		/// 
		/// <param name="array">
		/// 要素がコピーされる1次元かつ0から始まるインデックス番号の配列。
		/// </param>
		/// <param name="arrayIndex">
		/// コピーの開始位置となる、配列の0から始まるインデックス番号。
		/// </param>
		public void CopyTo(ITask[] array, int arrayIndex)
		{
			_private.tasks.CopyTo(array, arrayIndex);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 指定した型のコレクションに対する単純な反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		public IEnumerator<ITask> GetEnumerator()
		{
			return _private.tasks.GetEnumerator();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 非ジェネリック コレクションに対する反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_private.tasks).GetEnumerator();
		}
	}
}
