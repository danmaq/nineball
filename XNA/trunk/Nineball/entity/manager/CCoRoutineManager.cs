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
using danmaq.nineball.state.manager;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>コルーチン管理 クラス。</summary>
	public sealed class CCoRoutineManager
		: CEntity, ICollection<IEnumerator>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>コルーチン一覧。</summary>
		private readonly List<IEnumerator> coRoutines;

		/// <summary>コルーチン追加一覧。</summary>
		private readonly List<IEnumerator> addList = new List<IEnumerator>();

		/// <summary>コルーチン削除一覧。</summary>
		private readonly List<IEnumerator> removeList = new List<IEnumerator>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CCoRoutineManager()
			: this(CStateCoRoutineManager.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		public CCoRoutineManager(IState firstState)
			: base(firstState, new List<IEnumerator>())
		{
			coRoutines = (List<IEnumerator>)privateMembers;
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
				return coRoutines.Count;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>この管理クラスが読み取り専用かどうかを取得します。</summary>
		/// 
		/// <value><c>false</c>。</value>
		bool ICollection<IEnumerator>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>無限ループ用コルーチンです。</summary>
		/// 
		/// <returns>コルーチン。実行時は常時<c>null</c>。</returns>
		public static IEnumerator coEternalWait()
		{
			while (true)
			{
				yield return null;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチン追加・削除の予約を確定します。</summary>
		public void commit()
		{
			removeList.ForEach(co => coRoutines.Remove(co));
			removeList.Clear();
			coRoutines.AddRange(addList);
			addList.Clear();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチン管理に使用したメモリを切り詰めます。</summary>
		public void TrimExcess()
		{
			coRoutines.TrimExcess();
			addList.TrimExcess();
			removeList.TrimExcess();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチン追加の予約をします。</summary>
		/// 
		/// <param name="co">コルーチン。</param>
		/// <exception cref="System.ArgumentNullException">
		/// 引数が<c>null</c>の場合。
		/// </exception>
		public void Add(IEnumerator co)
		{
			if (co == null)
			{
				throw new ArgumentNullException("co");
			}
			addList.Add(co);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>タスク削除の予約をします。</summary>
		/// 
		/// <param name="co">コルーチン。</param>
		/// <returns><c>true</c>。</returns>
		/// <exception cref="System.ArgumentNullException">
		/// 引数が<c>null</c>の場合。
		/// </exception>
		public bool Remove(IEnumerator co)
		{
			if (co == null)
			{
				throw new ArgumentNullException("co");
			}
			removeList.Add(co);
			return true;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理しているコルーチンをすべて即時破棄します。</summary>
		public void Clear()
		{
			coRoutines.Clear();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>特定の値が格納されているかどうかを判断します。</summary>
		/// 
		/// <param name="co">検索するオブジェクト。</param>
		/// <returns>存在する場合、<c>true</c>。</returns>
		public bool Contains(IEnumerator co)
		{
			return coRoutines.Contains(co);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチン一覧を配列にコピーします。</summary>
		/// 
		/// <param name="array">
		/// コルーチン一覧がコピーされる1次元かつ0から始まるインデックス番号の配列。
		/// </param>
		/// <param name="arrayIndex">
		/// コピーの開始位置となる、配列の0から始まるインデックス番号。
		/// </param>
		public void CopyTo(IEnumerator[] array, int arrayIndex)
		{
			coRoutines.CopyTo(array, arrayIndex);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 指定した型のコレクションに対する単純な反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		public IEnumerator<IEnumerator> GetEnumerator()
		{
			return coRoutines.GetEnumerator();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 非ジェネリック コレクションに対する反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)coRoutines).GetEnumerator();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			coRoutines.Clear();
			addList.Clear();
			removeList.Clear();
			TrimExcess();
			base.Dispose();
		}
	}
}
