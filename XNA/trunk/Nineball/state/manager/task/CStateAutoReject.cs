////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.entity;
using danmaq.nineball.entity.manager;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.state.manager.task
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>特定条件においてタスクを自動排除する管理クラス用状態です。</summary>
	public class CStateAutoReject
		 : CState<CTaskManager, CTaskManager.CPrivateMembers>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>CState.emptyを検出して排除するクラス オブジェクト。</summary>
		public static readonly IState<CTaskManager, CTaskManager.CPrivateMembers> emptyState =
			new CStateAutoReject(task => ((IEntity)task).currentState == CState.empty);

		/// <summary>排除条件。</summary>
		protected readonly Predicate<ITask> predicate;
	
		/// <summary>接続先。</summary>
		private readonly IState<CTaskManager, CTaskManager.CPrivateMembers> adaptee =
			CStateDefault.instance;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="predicate">排除条件。</param>
		protected CStateAutoReject(Predicate<ITask> predicate)
		{
			this.predicate = predicate;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>状態が開始された時に呼び出されます。</para>
		/// <para>このメソッドは、遷移元の<c>teardown</c>よりも後に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されたオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		public override void setup(
			CTaskManager entity, CTaskManager.CPrivateMembers privateMembers)
		{
			adaptee.setup(entity, privateMembers);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(
			CTaskManager entity, CTaskManager.CPrivateMembers privateMembers, GameTime gameTime)
		{
			adaptee.update(entity, privateMembers, gameTime);
			// NOTE : GC対策のため、List<T>.FindAllは使用しない
			IList<ITask> tasks = privateMembers.tasks;
			for (int i = tasks.Count; --i >= 0; )
			{
				ITask task = tasks[i];
				if (predicate(task))
				{
					entity.Remove(task);
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(
			CTaskManager entity, CTaskManager.CPrivateMembers privateMembers, GameTime gameTime)
		{
			adaptee.draw(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>オブジェクトが別の状態へ移行する時に呼び出されます。</para>
		/// <para>このメソッドは、遷移先の<c>setup</c>よりも先に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を終了したオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="nextState">オブジェクトが次に適用する状態。</param>
		public override void teardown(
			CTaskManager entity, CTaskManager.CPrivateMembers privateMembers, IState nextState)
		{
			adaptee.teardown(entity, privateMembers, nextState);
		}
	}
}
