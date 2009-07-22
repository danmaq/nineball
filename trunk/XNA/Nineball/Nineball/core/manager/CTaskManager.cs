////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library / Copyright (c) 2008 danmaq all rights reserved.
//		──タスクを管理するクラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.Nineball.core.raw;
using Microsoft.Xna.Framework;

namespace danmaq.Nineball.core.manager {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>タスク進行を管理するクラス。</para>
	/// <para>このクラスにタスクを登録し、そしてこのクラスを通じ実行させます。</para>
	/// </summary>
	/// <remarks>複数のタスクを擬似的なマルチタスクで並列実行します。</remarks>
	public sealed class CTaskManager {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>タスク本体のリスト</summary>
		public readonly LinkedList<ITask> tasks = new LinkedList<ITask>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>一時停止中かどうか。</summary>
		public bool pause = false;

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>タスクを追加します。</summary>
		/// <remarks>
		/// タスクを登録した瞬間からレイヤ番号の変更が出来なくなりますので
		/// レイヤ設定はそれまでに済ませて、必ずロックをかけてください。
		/// </remarks>
		/// 
		/// <param name="task">タスク オブジェクト</param>
		public void add( ITask task ) {
			task.manager = this;
			task.initialize();
			for( LinkedListNode<ITask> n = tasks.First; n != null; n = n.Next ) {
				if( n.Value.layer < task.layer ) {
					tasks.AddBefore( n, task );
					return;
				}
			}
			tasks.AddLast( task );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>登録されているタスクを全て抹消します。</summary>
		public void erase() {
			foreach( ITask task in tasks ) { task.Dispose(); }
			tasks.Clear();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定のタスクを検索し、抹消します。</summary>
		/// 
		/// <param name="task">抹消させるタスク オブジェクト</param>
		/// <returns>正常に抹消できた場合、<c>true</c></returns>
		public bool erase( ITask task ) {
			task.Dispose();
			return tasks.Remove( task );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 登録されているタスクのうち、指定のレイヤの属するものを抹消します。
		/// </summary>
		/// 
		/// <param name="byLayer">抹消させるレイヤ番号</param>
		public void erase( byte byLayer ) {
			LinkedListNode<ITask> nNext;
			for( LinkedListNode<ITask> n = tasks.First; n != null; n = nNext ) {
				// ! TODO : 指定レイヤを通過したら脱出するようにする
				nNext = n.Next;
				if( byLayer == n.Value.layer ) { erase( n.Value ); }
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 登録されているタスクのうち、指定のレイヤの範囲に属するものを抹消します。
		/// </summary>
		/// 
		/// <param name="byLayerLimit1">抹消させるレイヤ番号の範囲1</param>
		/// <param name="byLayerLimit2">抹消させるレイヤ番号の範囲2</param>
		public void erase( byte byLayerLimit1, byte byLayerLimit2 ) {
			int nEnd = Math.Max( byLayerLimit1, byLayerLimit2 );
			for( byte i = Math.Min( byLayerLimit1, byLayerLimit2 ); i <= nEnd; erase( i++ ) )
				;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 登録されているタスクのうち、指定のレイヤに属するものの件数を取得します。
		/// </summary>
		/// 
		/// <param name="byLayer">レイヤ番号</param>
		/// <returns>指定のレイヤに属するタスクの件数</returns>
		public LinkedList<ITask> find( byte byLayer ) {
			LinkedList<ITask> result = new LinkedList<ITask>();
			foreach( ITask task in tasks ) {
				if( task.layer == byLayer ) { result.AddLast( task ); }
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 登録されている全タスクに1フレーム分の更新処理をさせます。
		/// </summary>
		/// <remarks>
		/// その結果タスクより<c>false</c>が返ってきた
		/// 場合、そのタスクは終了・抹消されます。
		/// </remarks>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		public void update( GameTime gameTime ) {
			LinkedListNode<ITask> nNext;
			for( LinkedListNode<ITask> n = tasks.First; n != null; n = nNext ) {
				nNext = n.Next;
				if( !( ( pause && n.Value.isAvailablePause ) || n.Value.update( gameTime ) ) ) {
					erase( n.Value );
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 登録されている全タスクに1フレーム分の描画処理をさせます。
		/// </summary>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		/// <param name="sprite">スプライト描画管理クラス</param>
		public void draw( GameTime gameTime, CSprite sprite ) {
			foreach( ITask task in tasks ) { task.draw( gameTime, sprite ); }
		}
	}
}
