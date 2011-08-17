////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using danmaq.nineball.state;
using danmaq.nineball.state.manager;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>再利用を前提としたタスク管理クラス。</summary>
	public class CFlyweightFactory
		: CEntity, IEnumerable<IEntity>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>墓場タスク一覧。</summary>
		protected readonly List<IEntity> grave = new List<IEntity>();

		/// <summary>登録されているタスク一覧。</summary>
		private readonly List<IEntity> tasks;

		/// <summary>ゾンビ検索用のラムダ式。</summary>
		private readonly Predicate<IEntity> findZombie =
			e => e.currentState == CState.empty && e.nextState == null;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>
		/// <para>インスタンスを作成する関数のデリゲート。</para>
		/// <para><c>Add()</c>メソッドを呼び出す際に内部から呼び出されます。</para>
		/// </summary>
		public Func<IEntity> createInstance = () => new CEntity();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CFlyweightFactory()
			: this(CStateFlyweightFactory.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		public CFlyweightFactory(IState firstState)
			: base(firstState, new List<IEntity>())
		{
			tasks = (List<IEntity>)privateMembers;
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
				return tasks.Count;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定したインデックスにある要素を取得します。</summary>
		/// 
		/// <param name="index">
		/// 取得する要素の、<c>0</c>から始まるインデックス番号。
		/// </param>
		/// <value>指定したインデックスにある要素。</value>
		public IEntity this[int index]
		{
			get
			{
				IEntity result;
				lock (((ICollection)tasks).SyncRoot)
				{
					result = tasks[index];
				}
				return result;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ゾンビタスクを墓場に送り込みます。</summary>
		/// 
		/// <returns>墓場送りにした数。</returns>
		public int cleanup()
		{
			int result = 0;
			lock (((ICollection)tasks).SyncRoot)
			{
				for (int i = tasks.Count; --i >= 0; )
				{
					if (findZombie(tasks[i]))
					{
						grave.Add(tasks[i]);
						tasks.RemoveAt(i);
						result++;
					}
				}
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			Clear();
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>タスクを追加します。</para>
		/// <para>
		/// また、このメソッドに<c>CState.Empty</c>を指定することで、
		/// 空のタスクを備蓄することができます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="state">追加する状態。</param>
		/// <returns>実際に追加されたタスク オブジェクト。</returns>
		public IEntity Add(IState state)
		{
			IEntity task = null;
			lock (((ICollection)tasks).SyncRoot)
			{
				if (grave.Count > 0)
				{
					task = grave[0];
					grave.RemoveAt(0);
					tasks.Add(task);
				}
				else
				{
					task = tasks.Find(findZombie);
					if (task == null)
					{
						task = createInstance();
						tasks.Add(task);
					}
				}
			}
			task.nextState = state;
			return task;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>タスクをまとめて追加します。</summary>
		/// <remarks>このメソッドではFlyweightな再利用はできません。</remarks>
		/// 
		/// <param name="collection">追加するタスク一覧。</param>
		/// <exception cref="System.ArgumentNullException">
		/// 引数が<c>null</c>の場合。
		/// </exception>
		public void AddRange(IEnumerable<IEntity> collection)
		{
			lock (((ICollection)tasks).SyncRoot)
			{
				tasks.AddRange(collection);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>特定の値が格納されているかどうかを判断します。</summary>
		/// 
		/// <param name="task">検索するオブジェクト。</param>
		/// <returns>存在する場合、<c>true</c>。</returns>
		public bool Contains(IEntity task)
		{
			return tasks.Contains(task);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理しているタスクを全て削除します。</summary>
		public void Clear()
		{
			lock (((ICollection)tasks).SyncRoot)
			{
				for (int i = tasks.Count; --i >= 0; )
				{
					tasks[i].Dispose();
				}
				tasks.Clear();
				grave.Clear();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>登録されたタスクに対して、指定の処理を実行します。</summary>
		/// 
		/// <param name="action">登録されたタスクに対して実行されるデリゲート。</param>
		/// <exception cref="System.ArgumentNullException">
		/// 引数が<c>null</c>の場合。
		/// </exception>
		public void ForEach(Action<IEntity> action)
		{
			lock (((ICollection)tasks).SyncRoot)
			{
				for (int i = tasks.Count; --i >= 0; )
				{
					action(tasks[i]);
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 指定した型のコレクションに対する単純な反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		public IEnumerator<IEntity> GetEnumerator()
		{
			return tasks.GetEnumerator();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 非ジェネリック コレクションに対する反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)tasks).GetEnumerator();
		}
	}
}
