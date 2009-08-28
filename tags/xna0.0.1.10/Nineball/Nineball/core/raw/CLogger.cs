////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2009 danmaq all rights reserved.
//
//		──ログ出力クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;

namespace danmaq.Nineball.core.raw {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ログ出力 クラス。</summary>
	public static class CLogger {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>標準出力の解説。</summary>
		private const string DESCRIPTION_STDOUT = "標準出力";

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
		static CLogger() { logStart(); }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>現在時間文字列。</summary>
		public static string now {
			get {
				DateTime time = DateTime.Now;
				return string.Format(
					"{0}:{1}:{2}.{3}", time.Hour, time.Minute, time.Second, time.Millisecond );
			}
		}

		/// <summary>
		/// <para>出力先ファイル名。</para>
		/// <para>空文字やNULLを設定するとデフォルトの標準出力先に戻ります。</para>
		/// </summary>
		/// <remarks>
		/// 現在のバージョンではXBOX360は標準出力以外は無視されます。
		/// </remarks>
		public static string outFile {
			get { return m_strOutFile; }
			set {
				if( m_strOutFile != value ) {
					bool bCompleted = false;
					try {
#if WINDOWS
						if( value != null && value.Length > 0 ) {
							StreamWriter sw = File.CreateText( value );
							sw.AutoFlush = true;
							Console.SetOut( sw );
						}
						else { Console.SetOut( DEFAULT_OUT ); }
						bCompleted = true;
#else
						throw new PlatformNotSupportedException(
						"現行のバージョンのXBOX360版では、ログ出力先を標準出力以外に設定することができません。" );
#endif
					}
					catch( Exception e ) {
						Console.WriteLine( "出力先を切り替えることが出来ませんでした。" +
							Environment.NewLine + e.ToString() );
					}
					logStart();
					if( bCompleted ) {
						string strPrevOutFile = m_strOutFile;
						m_strOutFile = value;
						Console.WriteLine( "出力先が切り替わりました。" + Environment.NewLine +
							"以前の出力先は {0} で、現在の変更された出力先は {1} です。",
							getDestDescription( strPrevOutFile ), getDestDescription( value ) );
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
		public static void add( object obj ) {
			string strText = obj.ToString();
			Console.WriteLine(
				now + ( strText.Contains( Environment.NewLine ) ?
				( " > " + Environment.NewLine ) : " > " ) + strText );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ログの末尾に改行を追加します。</summary>
		public static void add() { Console.WriteLine(); }

		//* -----------------------------------------------------------------------*
		/// <summary>ログ出力開始メッセージを出力します。</summary>
		private static void logStart() {
			Console.WriteLine( DateTime.Now.ToString() +
				Environment.NewLine + Environment.NewLine + now + " > ログ出力開始" );
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
		private static string getDestDescription( string strDest ) {
			return ( strDest == null ? DESCRIPTION_STDOUT : strDest );
		}
	}
}
