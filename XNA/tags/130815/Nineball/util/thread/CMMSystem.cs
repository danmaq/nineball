////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

#if !WINDOWS
using System;
#endif

namespace danmaq.nineball.util.thread
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>高精度タイマ クラス。</summary>
	/// <remarks>Windows以外の環境では何もしません。</remarks>
	public static class CMMSystem
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>正常終了を示すステータスコード。</summary>
		public const uint TIMERR_NOERROR = 0;

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

#if WINDOWS

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// システム時刻をミリ秒単位で取得します。
		/// システム時刻は Windows が起動してから経過した時間です。
		/// </summary>
		/// 
		/// <returns>システム時刻。</returns>
		[DllImport("winmm.dll", EntryPoint = "timeGetTime")]
		public static extern uint timeGetTime();

		//* -----------------------------------------------------------------------*
		/// <summary>アプリケーションの最小タイマ分解能を設定します。</summary>
		/// <remarks>
		/// タイマサービスの使用直前にこの関数を呼び出し、タイマサービスの使用終了後
		/// ただちに timeEndPeriod 関数を呼び出してください。
		/// 両方の呼び出しで同じ最小分解能を指定し、timeBeginPeriod 関数の呼び出しと、
		/// timeEndPeriod 関数の呼び出しを一致させてください。アプリケーションは、
		/// timeEndPeriod 関数の呼び出しと一致している限り、何度でも timeBeginPeriod
		/// 関数を呼び出すことができます。
		/// </remarks>
		/// 
		/// <param name="uPeriod">最小タイマ分解能(ミリ秒単位)。</param>
		/// <returns>ステータス コード。</returns>
		[DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
		public static extern uint timeBeginPeriod(uint uPeriod);

		//* -----------------------------------------------------------------------*
		/// <summary>以前にセットされた最小タイマ分解能をクリアします。</summary>
		/// 
		/// <param name="uPeriod">以前の呼び出しで指定された最小タイマ分解能。</param>
		/// <returns>ステータス コード。</returns>
		[DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
		public static extern uint timeEndPeriod(uint uPeriod);

#else

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// システム時刻をミリ秒単位で取得します。
		/// システム時刻は Windows が起動してから経過した時間です。
		/// </summary>
		/// 
		/// <returns>システム時刻。</returns>
		public static uint timeGetTime()
		{
			return (uint)((long)(DateTime.Now.Ticks * 0000.1f) % uint.MaxValue);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アプリケーションの最小タイマ分解能を設定します。</summary>
		/// <remarks>
		/// タイマサービスの使用直前にこの関数を呼び出し、タイマサービスの使用終了後
		/// ただちに timeEndPeriod 関数を呼び出してください。
		/// 両方の呼び出しで同じ最小分解能を指定し、timeBeginPeriod 関数の呼び出しと、
		/// timeEndPeriod 関数の呼び出しを一致させてください。アプリケーションは、
		/// timeEndPeriod 関数の呼び出しと一致している限り、何度でも timeBeginPeriod
		/// 関数を呼び出すことができます。
		/// </remarks>
		/// 
		/// <param name="uPeriod">最小タイマ分解能(ミリ秒単位)。</param>
		/// <returns>ステータス コード。</returns>
		public static uint timeBeginPeriod(uint uPeriod)
		{
			return TIMERR_NOERROR;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>以前にセットされた最小タイマ分解能をクリアします。</summary>
		/// 
		/// <param name="uPeriod">以前の呼び出しで指定された最小タイマ分解能。</param>
		/// <returns>ステータス コード。</returns>
		public static uint timeEndPeriod(uint uPeriod)
		{
			return TIMERR_NOERROR;
		}

#endif
	}
}
