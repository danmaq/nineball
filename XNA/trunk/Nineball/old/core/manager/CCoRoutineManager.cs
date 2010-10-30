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

namespace danmaq.nineball.old.core.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>コルーチン管理 クラス。</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete("このクラスは今後サポートされません。danmaq.nineball.entity.manager.CCoRoutineManagerを使用してください。")]
	public sealed class CCoRoutineManager
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>接続先。</summary>
		private readonly entity.manager.CCoRoutineManager bridge =
			new entity.manager.CCoRoutineManager();

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
		public static implicit operator int(CCoRoutineManager m)
		{
			return m.bridge.Count;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンを1ループ分実行します。</summary>
		/// 
		/// <returns>まだ全てのスレッドが完了していない場合、<c>true</c></returns>
		public bool update()
		{
			bridge.update(null);
			if (reserveAllRemove)
			{
				bridge.Clear();
				reserveAllRemove = false;
			}
			return bridge.Count > 0;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンを全て削除します。</summary>
		public void remove()
		{
			bridge.Clear();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンを削除します。</summary>
		/// 
		/// <param name="co">コルーチン</param>
		/// <returns>コルーチンを削除できた場合、<c>true</c></returns>
		public bool remove(IEnumerator co)
		{
			return bridge.Remove(co);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コルーチンを登録します。</summary>
		/// 
		/// <param name="co">コルーチン</param>
		/// <returns>コルーチンを登録できた場合、<c>true</c></returns>
		public bool add(IEnumerator co)
		{
			bool result = true;
			try
			{
				bridge.Add(co);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
	}
}
