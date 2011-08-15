////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Threading;

namespace danmaq.nineball.util
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>スレッド プールのためのラッパー クラスです。</summary>
	/// <remarks>
	/// 内部にカウンタを設け、すべてのスレッドがアイドル
	/// 状態になったかどうかを確認することができます。
	/// </remarks>
	public static class CThreadPool
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

		/// <summary>アクティブなスレッドの数。</summary>
		private static int m_activeCount = 0;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

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
		public static void add(WaitCallback callback)
		{
			lock (syncLock)
			{
				m_activeCount++;
				// TODO : ヒープ喰いを避けるためとはいえ、これだけのためにstateを潰すのは余り賢いやり方ではない。
				ThreadPool.QueueUserWorkItem(CThreadPool.callback, callback);
			}
		}
	}
}
