////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using danmaq.nineball.Properties;
#if XBOX360
using System.Diagnostics;
#endif

namespace danmaq.nineball.util
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ログ出力 クラス。</summary>
	public static class CLogger
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>デフォルトの標準出力先。</summary>
		private static readonly TextWriter DEFAULT_OUT = Console.Out;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>出力先ファイル名。</summary>
		private static string m_strOutFile;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		static CLogger()
		{
			logStart();
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>現在時間文字列を取得します。</summary>
		/// 
		/// <value>現在時間文字列。</value>
		public static string now
		{
			get
			{
				DateTime time = DateTime.Now;
				return string.Format(
					"{0}:{1}:{2}.{3}",
					time.Hour.ToString(), time.Minute.ToString(),
					time.Second.ToString(), time.Millisecond.ToString());
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>出力先ファイル名を設定/取得します。</para>
		/// <para>空文字やNULLを設定するとデフォルトの標準出力先に戻ります。</para>
		/// </summary>
		/// <remarks>
		/// 現在のバージョンではXBOX360は標準出力以外は無視されます。
		/// </remarks>
		/// 
		/// <value>出力先ファイル名。</value>
		public static string outFile
		{
			get
			{
				return m_strOutFile;
			}
			set
			{
				if(m_strOutFile != value)
				{
					bool bCompleted = false;
					try
					{
#if WINDOWS
						if(value != null && value.Length > 0)
						{
							StreamWriter sw = File.CreateText(value);
							sw.AutoFlush = true;
							Console.SetOut(sw);
						}
						else
						{
							Console.SetOut(DEFAULT_OUT);
						}
						bCompleted = true;
#else
						throw new PlatformNotSupportedException(Resources.TRACE_ERR_CHANGE_FAILED);
#endif
					}
					catch(Exception e)
					{
						add(Resources.TRACE_INFO_CHANGE_FAILED +
							Environment.NewLine + e.ToString());
					}
					logStart();
					if(bCompleted)
					{
						string strPrevOutFile = m_strOutFile;
						m_strOutFile = value;
						Console.WriteLine(Resources.TRACE_INFO_CHANGE_SUCCEEDED,
							getDestDescription(strPrevOutFile), getDestDescription(value));
					}
				}
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>オブジェクトを強制的に文字列化してログに残します。</summary>
		/// <remarks>自動的に現在時刻が追加されます。</remarks>
		/// 
		/// <param name="obj">オブジェクト</param>
		public static void add(object obj)
		{
			// TODO : 可変引数に対応する
			string strText = obj == null ? Resources.NULL : obj.ToString();
			strText = now + (strText.Contains(Environment.NewLine) ?
				(" > " + Environment.NewLine) : " > ") + strText;
#if WINDOWS
			Console.WriteLine(strText);
#else
			Trace.WriteLine(strText);
#endif
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ログの末尾に改行を追加します。</summary>
		public static void add()
		{
			Console.WriteLine();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ログ出力開始メッセージを出力します。</summary>
		private static void logStart()
		{
			string strText = DateTime.Now.ToString() +
				Environment.NewLine + Environment.NewLine + now + " > " + Resources.TRACE_INFO_START;
#if WINDOWS
			Console.WriteLine(strText);
#else
			Trace.WriteLine(strText);
#endif
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>出力先の説明を取得します。</para>
		/// <para>
		/// ファイル名を指定した場合はファイル名、
		/// そうでない場合は「標準出力」の文字を取得します。
		/// </para>
		/// </summary>
		/// 
		/// <param name="strDest">出力先ファイル名</param>
		private static string getDestDescription(string strDest)
		{
			return strDest ?? Resources.STDOUT;
		}
	}
}
