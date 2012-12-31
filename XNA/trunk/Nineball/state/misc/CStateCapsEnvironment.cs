////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;
using danmaq.nineball.util.caps;

namespace danmaq.nineball.state.misc
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>システム環境検証クラス。</summary>
	public sealed class CStateCapsEnvironment
		: CStateCapsBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>デバッグ バージョンかどうか。</summary>
		public const bool DEBUG =
#if DEBUG
			true;
#else
			false;
#endif

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateCapsBase instance = new CStateCapsEnvironment();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateCapsEnvironment()
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>環境レポートを生成します。</summary>
		/// 
		/// <returns>レポート文字列。</returns>
		protected override string createReport()
		{
			string strResult = "◆◆◆ 基本環境情報" + Environment.NewLine;
			strResult += "  DNL デバッグ版    : " + DEBUG.ToStringOX() + Environment.NewLine;
#if WINDOWS
			strResult += "  ユーザ対話モード  : " + Environment.UserInteractive.ToStringOX() + Environment.NewLine;
			strResult += "  コマンドライン    : " + Environment.CommandLine + Environment.NewLine;
#endif
			strResult += "  演算プロセッサ数  : " + Environment.ProcessorCount + " 個" + Environment.NewLine;
			int workerThreads, completionPortThreads;
			ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
			strResult += "  ワーカー スレッド : " + workerThreads + " 個" + Environment.NewLine;
			strResult += "  非同期I/Oスレッド : " + completionPortThreads + " 個" + Environment.NewLine;
			return strResult;
		}
	}
}
