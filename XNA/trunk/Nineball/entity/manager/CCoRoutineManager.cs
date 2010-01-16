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
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.manager
{

	// TODO : コルーチン ハンドル作った方がいいかも。removeの引数設定が紛らわしい
	// 特に引数を持つコルーチンの場合どうすんの、とか
	// TODO : 制御をステート側へ移行する

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>コルーチン管理 クラス。</summary>
	public sealed class CCoRoutineManager : CEntity
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>コルーチン追加/削除用キューのデータ。</summary>
		private struct SQueue
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>コルーチンの全削除のためのキュー。</summary>
			public static readonly SQueue removeAll = new SQueue(false);

			/// <summary>追加するかどうか。</summary>
			public readonly bool add;

			/// <summary>コルーチン本体。</summary>
			public readonly IEnumerator coRoutine;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="add">追加するかどうか。</param>
			/// <param name="coRoutine">コルーチン本体。</param>
			/// <exception cref="System.ArgumentNullException">
			/// コルーチン本体にnullを設定しようとした場合。
			/// </exception>
			public SQueue(bool add, IEnumerator coRoutine)
			{
				if(coRoutine == null)
				{
					throw new ArgumentNullException("coRoutine");
				}
				this.add = add;
				this.coRoutine = coRoutine;
			}

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="add">追加するかどうか。</param>
			private SQueue(bool add)
			{
				this.add = add;
				this.coRoutine = null;
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>登録されているコルーチン一覧。</summary>
		private readonly LinkedList<IEnumerator> coRoutines =
			new LinkedList<IEnumerator>();

		/// <summary>一覧操作用のキュー。</summary>
		private readonly Queue<SQueue> operationQueue = new Queue<SQueue>();

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>無限ループ用コルーチンです。</summary>
		/// 
		/// <returns>null</returns>
		public static IEnumerator coEternalWait
		{
			get
			{
				while(true)
				{
					yield return null;
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンの件数を取得します。</summary>
		/// 
		/// <value>コルーチンの件数</value>
		public int count
		{
			get
			{
				int nResult = coRoutines.Count;
				foreach(SQueue item in operationQueue)
				{
					if(item.add)
					{
						nResult++;
					}
					else if(item.coRoutine == null)
					{
						nResult = 0;
					}
					else
					{
						nResult--;
					}
				}
				return Math.Max(0, nResult);
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンの件数を取得します。</summary>
		/// 
		/// <param name="m">コルーチン管理クラス</param>
		/// <returns>コルーチンの件数</returns>
		public static implicit operator int(CCoRoutineManager m)
		{
			return m.count;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンの全削除を予約します。</summary>
		public override void Dispose()
		{
			remove();
			commitQueue();
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コルーチンを1ループ分実行します。</para>
		/// <para>実行の前後にて、登録/削除の予約を実行します。</para>
		/// </summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(GameTime gameTime)
		{
			commitQueue();
			LinkedListNode<IEnumerator> nodeNext;
			for(
				LinkedListNode<IEnumerator> node = coRoutines.First; node != null; node = nodeNext
			)
			{
				nodeNext = node.Next;
				if(node.Value == null || !node.Value.MoveNext())
				{
					remove(node.Value);
				}
			}
			commitQueue();
			base.update(gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンの全削除を予約します。</summary>
		public void remove()
		{
			operationQueue.Enqueue(SQueue.removeAll);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンの削除を予約します。</summary>
		/// 
		/// <param name="coRoutine">コルーチン</param>
		public void remove(IEnumerator coRoutine)
		{
			operationQueue.Enqueue(new SQueue(false, coRoutine));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンの登録を予約します。</summary>
		/// 
		/// <param name="coRoutine">コルーチン</param>
		public void add(IEnumerator coRoutine)
		{
			operationQueue.Enqueue(new SQueue(true, coRoutine));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチン操作キューをリストへ反映します。</summary>
		private void commitQueue()
		{
			while(operationQueue.Count > 0)
			{
				SQueue data = operationQueue.Dequeue();
				if(data.add)
				{
					coRoutines.AddLast(data.coRoutine);
				}
				else if(data.coRoutine == null)
				{
					coRoutines.Clear();
				}
				else
				{
					coRoutines.Remove(data.coRoutine);
				}
			}
		}
	}
}
