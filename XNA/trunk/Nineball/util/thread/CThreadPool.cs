////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading;

namespace danmaq.nineball.util.thread
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>スレッド プール クラスです。</summary>
	/// <remarks>
	/// 基本的にはThreadPoolクラスの車輪の再発明(縮小版)です。
	/// XBOX360で使うとどうもスレッドが毎回落ちるようなので、独自に作りました。
	/// </remarks>
	public static class CThreadPool
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>スレッド情報。</summary>
		private class CThreadInfo
			: IDisposable
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>排他制御のために使用するオブジェクト。</summary>
			private readonly object syncLock = new object();

			/// <summary>スレッド。</summary>
			public readonly Thread thread;

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>スレッドの停止を予約するかどうか。</summary>
			private bool m_terminate = false;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="priority">スレッドの優先度。</param>
			/// <param name="sleep">スレッドが動作するたびに休眠する時間(ミリ秒)。</param>
			/// <param name="interval">休眠までのタスク消化回数。</param>
			public CThreadInfo(ThreadPriority priority, int sleep, int interval)
			{
				ThreadStart method = () =>
				{
					bool loop = true;
					int counter = 0;
					do
					{
						KeyValuePair<WaitCallback, object> info = CThreadPool.pop();
						if (info.Key != null)
						{
							info.Key(info.Value);
						}
						Thread.Sleep(++counter % interval == 0 ? sleep : 0);
						lock (syncLock)
						{
							loop = !m_terminate;
						}
					}
					while (loop);
				};
				thread = new Thread(method);
				thread.Name = "Nineball:CThreadPool";
				thread.Priority = priority;
				thread.Start();
			}

			//* -----------------------------------------------------------------------*
			/// <summary>スレッドをキリのよいところで停止させます。</summary>
			public void Dispose()
			{
				lock (syncLock)
				{
					m_terminate = true;
				}
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>排他制御のために使用するオブジェクト。</summary>
		private static readonly object syncLock = new object();

		/// <summary>実行キュー。</summary>
		private static readonly List<KeyValuePair<WaitCallback, object>> queue =
			new List<KeyValuePair<WaitCallback, object>>();

		/// <summary>スレッド一覧。</summary>
		private static readonly List<CThreadInfo> threads = new List<CThreadInfo>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>優先度。</summary>
		private static ThreadPriority m_priority = ThreadPriority.Normal;

		/// <summary>休眠時間。</summary>
		private static int m_wait = 0;

		/// <summary>休眠する間隔。</summary>
		private static int m_interval = 1;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>スレッドの数を取得/設定します。</summary>
		/// <remarks>設定された数に応じて、スレッドの本数が増減します。</remarks>
		/// 
		/// <value>スレッドの数。</value>
		public static int count
		{
			get
			{
				int result;
				lock (syncLock)
				{
					result = threads.Count;
				}
				return result;
			}
			set
			{
				lock (syncLock)
				{
					int gap = value - threads.Count;
					for (int g = gap; --g >= 0;
						threads.Add(new CThreadInfo(priority, wait, interval)))
						;
					for (int g = gap; ++g <= 0; )
					{
						threads[0].Dispose();
						threads.RemoveAt(0);
					}
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>スレッドの優先度を取得/設定します。</summary>
		/// <remarks>
		/// 注意：優先度を変更すると、いったん全てのスレッドがリセットされます。
		/// </remarks>
		/// 
		/// <value>スレッドの数。</value>
		public static ThreadPriority priority
		{
			get
			{
				return m_priority;
			}
			set
			{
				if (value != m_priority)
				{
					int count = CThreadPool.count;
					reboot();
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>スレッドの1タスク消化ごとの休眠時間を取得/設定します。</summary>
		/// <remarks>
		/// 注意：優先度を変更すると、いったん全てのスレッドがリセットされます。
		/// また、<c>CMMSystem.timeBeginPeriod</c>メソッドを実行するまでは、
		/// 実際の時間以上に休眠してしまうことがあります。
		/// </remarks>
		/// 
		/// <value>休眠時間(ミリ秒)。</value>
		public static int wait
		{
			get
			{
				return m_wait;
			}
			set
			{
				int v = Math.Max(0, value);
				if (v != m_wait)
				{
					m_wait = v;
					reboot();
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>スレッド休眠が発生するタスク消化回数を取得/設定します。</summary>
		/// <remarks>
		/// 注意：優先度を変更すると、いったん全てのスレッドがリセットされます。
		/// </remarks>
		/// 
		/// <value>タスク消化回数。</value>
		public static int interval
		{
			get
			{
				return m_interval;
			}
			set
			{
				int v = Math.Max(1, value);
				if (v != m_interval)
				{
					m_interval = v;
					reboot();
				}
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>タスクを追加します。</summary>
		/// <remarks>
		/// プールが空の場合、現在のスレッドでタスクが即時実行されます。
		/// </remarks>
		/// 
		/// <param name="callback">タスク。</param>
		/// <param name="state">タスクに渡す値。</param>
		/// <returns>
		/// 予約に成功した場合、<c>true</c>。
		/// プールが空で、現在のスレッドを使い即時実行した場合、<c>false</c>。
		/// </returns>
		public static bool pushTask(WaitCallback callback, object state)
		{
			bool result;
			lock (syncLock)
			{
				result = threads.Count > 0;
				if (result)
				{
					queue.Add(new KeyValuePair<WaitCallback, object>(callback, state));
				}
			}
			if (!result)
			{
				callback(state);
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>スレッドを再起動します。</summary>
		public static void reboot()
		{
			int count = CThreadPool.count;
			CThreadPool.count = 0;
			CThreadPool.count = count;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>タスクを取得します。</summary>
		/// 
		/// <returns>タスク。</returns>
		private static KeyValuePair<WaitCallback, object> pop()
		{
			KeyValuePair<WaitCallback, object> result =
				new KeyValuePair<WaitCallback, object>(null, null);
			lock (syncLock)
			{
				if (queue.Count > 0)
				{
					result = queue[0];
					queue.RemoveAt(0);
				}
			}
			return result;
		}
	}
}
