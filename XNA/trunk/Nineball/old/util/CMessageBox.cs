////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using danmaq.nineball.Properties;
using danmaq.nineball.util;

#if WINDOWS
using System.Windows.Forms;
#else
using Microsoft.Xna.Framework.GamerServices;
using danmaq.nineball.state.manager;
#endif

namespace danmaq.nineball.old.util
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>メッセージボックス 補助クラス。</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete("このクラスは今後サポートされません。util.storage.CGuideWrapperを使用してください。")]
	public static class CMessageBox
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>タイトルバーに表示するアプリケーション タイトル。</summary>
		private static string m_strTitleBar = Resources.NAME;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// タイトルバーに表示するアプリケーション タイトルを設定/取得します。
		/// </summary>
		/// 
		/// <value>アプリケーション タイトル文字列。</value>
		public static string titleBar
		{
			get
			{
				return m_strTitleBar;
			}
			set
			{
				if(value != null)
				{
					m_strTitleBar = value;
				}
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>予期しない不具合発生メッセージボックスを表示します。</summary>
		/// 
		/// <param name="e">例外</param>
		public static void show(Exception e)
		{
			show(Resources.ERR_EXCEPTION + Environment.NewLine + Environment.NewLine +
				e.ToString());
		}

		//* -----------------------------------------------------------------------*
		/// <summary>メッセージボックスを表示します。</summary>
		/// 
		/// <param name="strText">表示したいメッセージ文字列</param>
		public static void show(string strText)
		{
			CLogger.add(strText);
#if WINDOWS
			MessageBox.Show(strText, titleBar, MessageBoxButtons.OK, MessageBoxIcon.Hand);
#else
			CStateGuideHelper.instance.reserveMessage( endMessageBox, strText );
#endif
		}

		//* -----------------------------------------------------------------------*
		/// <summary>メッセージボックス表示終了時の処理をします。</summary>
		/// 
		/// <param name="result">非同期操作のステータス</param>
		private static void endMessageBox(IAsyncResult result)
		{
#if XBOX360
			Guide.EndShowMessageBox( result );
#endif
		}
	}
}
