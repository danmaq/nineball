////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;

namespace danmaq.nineball.util.thread
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>スレッド プールのためのラッパー クラスです。</summary>
	/// <remarks>
	/// 内部にカウンタを設け、すべてのスレッドがアイドル
	/// 状態になったかどうかを確認することができます。
	/// </remarks>
	public static class CThreadPoolWrapper
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>排他制御のために使用するオブジェクト。</summary>
		private static readonly object syncLock = new object();

		/// <summary>コールバック。</summary>
		private static readonly WaitCallback callback = (o) =>
		{
			((WaitCallback)o)(o);
			lock (syncLock)
			{
				m_activeCount--;
			}
		};

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>アクティブなスレッド プール追加メソッド。</summary>
		public static Func<WaitCallback, object, bool> activeThreadPool =
			ThreadPool.QueueUserWorkItem;

		/// <summary>アクティブなスレッドの数。</summary>
		private static int m_activeCount = 0;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>現在の積みタスク数(実行中も含む)を取得します。</summary>
		/// 
		/// <value>積みタスク数。</value>
		public static int tasks
		{
			get
			{
				int result;
				lock (syncLock)
				{
					result = m_activeCount;
				}
				return result;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>スレッドがアイドル状態となったかどうかを取得します。</summary>
		/// 
		/// <value>スレッドがアイドル状態となった場合、<c>true</c>。</value>
		public static bool idle
		{
			get
			{
				bool result;
				lock (syncLock)
				{
					result = m_activeCount <= 0;
				}
				return result;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>実行の予約をします。</summary>
		/// 
		/// <param name="callback">実行されるデリゲート。</param>
		/// <returns>現在の残タスク数。</returns>
		public static int add(WaitCallback callback)
		{
			int result;
			lock (syncLock)
			{
				result = ++m_activeCount;
				// TODO : ヒープ喰いを避けるためとはいえ、これだけのためにstateを潰すのは余り賢いやり方ではない。
				activeThreadPool(CThreadPoolWrapper.callback, callback);
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アイドル状態になるまで現在のスレッドを待機し続けます。</summary>
		/// 
		/// <returns>スレッドを明け渡した回数。</returns>
		public static int waitUntilIdle()
		{
			return waitUntilIdle(0);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アイドル状態になるまで現在のスレッドを待機し続けます。</summary>
		/// 
		/// <param name="ms">
		/// スレッドの状態を確認する間隔(ミリ秒単位)。
		/// <c>0</c>を設定した場合、スレッドを一時明け渡すのみで休止はしません。
		/// </param>
		/// <returns>待機した単位時間数。</returns>
		public static int waitUntilIdle(int ms)
		{
			int result = 0;
			while (!idle)
			{
				Thread.Sleep(ms);
				result++;
			}
			return result;
		}
	}
}
