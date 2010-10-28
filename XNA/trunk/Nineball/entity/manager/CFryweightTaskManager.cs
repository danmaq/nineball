////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.state;
using danmaq.nineball.state.manager;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>再利用を前提としたタスク管理クラス。</summary>
	/// 
	/// <typeparam name="_T">再利用する型。</typeparam>
	public sealed class CFryweightTaskManager<_T>
		: CEntity where _T : IEntity, new()
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>登録されているタスク一覧。</summary>
		public readonly List<_T> tasks = new List<_T>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CFryweightTaskManager()
			: this(CStateFryweightTaskManager<_T>.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		public CFryweightTaskManager(IState firstState)
			: base(firstState, null)
		{
		}


		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		/// <param name="privateMembers">
		///	オブジェクトと状態クラスのみがアクセス可能なフィールド。
		///	</param>
		public CFryweightTaskManager(IState firstState, object privateMembers)
			: base(firstState, privateMembers)
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			tasks.ForEach(task => task.Dispose());
			tasks.Clear();
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>タスクを追加します。</summary>
		/// 
		/// <param name="state">追加する状態。</param>
		/// <returns>実際に追加されたタスク オブジェクト。</returns>
		public _T Add(IState state)
		{
			_T task;
			task = tasks.Find(item => item.currentState == CState.empty);
			if (task == null)
			{
				task = new _T();
				task.nextState = state;
				tasks.Add(task);
			}
			else
			{
				task.nextState = state;
			}
			return task;
		}
	}
}
