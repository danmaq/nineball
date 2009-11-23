////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2009 danmaq all rights reserved.
//
//		──コルーチン管理 クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace danmaq.Nineball.core.manager {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>コルーチン管理 クラス。</summary>
	public sealed class CCoRoutineManager {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>コルーチンを動かすためのイテレータ。</summary>
		private readonly LinkedList<IEnumerator<object>> coRoutines =
			new LinkedList<IEnumerator<object>>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>次回update()呼び出し時にコルーチンをすべて破壊するかどうかを設定します。</summary>
		/// <remarks>update()を呼びだすたびにfalseに書き換えられます。</remarks>
		public bool reserveAllRemove = false;

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンの件数を取得します。</summary>
		/// 
		/// <param name="m">コルーチン管理クラス</param>
		/// <returns>スレッドの件数</returns>
		public static implicit operator int( CCoRoutineManager m ) { return m.coRoutines.Count; }

		//* -----------------------------------------------------------------------*
		/// <summary>無限ループ用スレッドです。</summary>
		/// 
		/// <returns>スレッドが実行される間、<c>null</c></returns>
		public static IEnumerator<object> threadEternalWait() {
			while( true ) { yield return null; }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンを1ループ分実行します。</summary>
		/// 
		/// <returns>まだ全てのスレッドが完了していない場合、<c>true</c></returns>
		public bool update() {
			LinkedListNode<IEnumerator<object>> nodeNext;
			for(
				LinkedListNode<IEnumerator<object>> node = coRoutines.First; node != null; node = nodeNext
			){
				nodeNext = node.Next;
				if( node.Value == null || !node.Value.MoveNext() ) { coRoutines.Remove( node ); }
			}
			if( reserveAllRemove ) {
				remove();
				reserveAllRemove = false;
			}
			return ( coRoutines.Count > 0 );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンを全て削除します。</summary>
		public void remove() { coRoutines.Clear(); }

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンを削除します。</summary>
		/// 
		/// <param name="thread">コルーチン</param>
		/// <returns>コルーチンを削除できた場合、<c>true</c></returns>
		public bool remove( IEnumerator<object> thread ) { return coRoutines.Remove( thread ); }

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンを登録します。</summary>
		/// 
		/// <param name="thread">コルーチン</param>
		/// <returns>コルーチンを登録できた場合、<c>true</c></returns>
		public bool add( IEnumerator<object> thread ) {
			bool bResult = ( thread != null && coRoutines.Find( thread ) == null );
			if( bResult ) { coRoutines.AddLast( thread ); }
			return bResult;
		}
	}
}
