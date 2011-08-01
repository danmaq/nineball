////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2009 danmaq all rights reserved.
//
//		──メッセージボックス 補助クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////


using System;
using danmaq.Nineball.Properties;

#if WINDOWS
using System.Windows.Forms;
#else
using Microsoft.Xna.Framework.GamerServices;
#endif

namespace danmaq.Nineball.core.raw {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>メッセージボックス 補助クラス。</summary>
	public static class CMessageBox {

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>タイトルバーに表示するアプリケーション タイトル。</summary>
		private static string m_strTitleBar = Resources.NAME;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>タイトルバーに表示するアプリケーション タイトル。</summary>
		public static string titleBar {
			get { return m_strTitleBar; }
			set {
				if( value != null ) { m_strTitleBar = value; }
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ランタイム不足のメッセージを表示します。</summary>
		/// 
		/// <param name="strRuntime">ランタイムの名前</param>
		/// <param name="strVersion">バージョン</param>
		/// <param name="strURI">入手先</param>
		public static void showLibNotFound( String strRuntime, String strVersion, String strURI ) {
			show( strRuntime + " がインストールされていないか、またはバージョンが古い可能性があります。"		+ Environment.NewLine +
				"このゲームを起動するためには " + strRuntime + " バージョン " + strVersion + " が必要です。"	+ Environment.NewLine +
				"このランタイム ライブラリは、下記のWebサイトで入手することが出来ます。"						+ Environment.NewLine + Environment.NewLine + strURI );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ランタイム不足のメッセージを表示します。</summary>
		public static void showLibNotFound() {
			show(
				"いくつかのランタイム ライブラリがインストールされていない可能性があります。"	+ Environment.NewLine +
				"このゲームを起動するためには下記が全てインストールされている必要があります。"	+ Environment.NewLine +
				"(かつ、ハードウェアが下記ソフトウェアに準拠している必要があります)"			+ Environment.NewLine + Environment.NewLine +
				"・Microsoft Windows XP SP2以降"												+ Environment.NewLine +
				"・Microsoft .NET Framework 2.0 SP1以降"										+ Environment.NewLine +
				"・Microsoft DirectX 9.0c Update(March 2008)以降"								+ Environment.NewLine +
				"・Microsoft XNA Framework 2.0"													+ Environment.NewLine + Environment.NewLine +
				"実行ファイルのあるフォルダに診断用ログが作成されていますので、併せてご覧ください。" );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>予期しない不具合発生メッセージボックスを表示します。</summary>
		/// 
		/// <param name="e">例外</param>
		public static void show( Exception e ) {
			show( "予期しない不具合が発生した為、ゲームを強制終了します。" + Environment.NewLine + Environment.NewLine + e.ToString() );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>メッセージボックスを表示します。</summary>
		/// 
		/// <param name="strText">表示したいメッセージ文字列</param>
		public static void show( String strText ) {
			CLogger.add( strText );
#if WINDOWS
			MessageBox.Show( strText, titleBar, MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand );
#else
			CGuideManager.reserveMessage( endMessageBox, strText );
#endif
		}

		//* -----------------------------------------------------------------------*
		/// <summary>メッセージボックス表示終了時の処理をします。</summary>
		/// 
		/// <param name="result">非同期操作のステータス</param>
		private static void endMessageBox( IAsyncResult result ) {
#if XBOX360
			Guide.EndShowMessageBox( result );
#endif
		}
	}
}
