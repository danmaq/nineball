////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if XBOX360

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace danmaq.nineball.old.core.inner
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ガイド 補助クラス。</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete]
	public static class CGuideManager
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>ガイド表示情報の共通基底クラス。</summary>
		private abstract class CGuideInfoBase
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>XBOX360ガイド操作が終了すると呼び出されるメソッド。</summary>
			protected readonly AsyncCallback CALLBACK;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="callback">XBOX360ガイド表示用デリゲート</param>
			protected CGuideInfoBase(AsyncCallback callback)
			{
				CALLBACK = callback;
			}

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>XBOX360ガイドを開始します。</summary>
			public abstract bool start();
		}

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>メッセージボックス ガイド表示情報クラス。</summary>
		private sealed class CGuideInfoMessageBox : CGuideInfoBase
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>ボタン一覧。</summary>
			private readonly string[] BUTTONS = new string[] { "OK" };

			/// <summary>メッセージ テキスト文字列。</summary>
			private readonly string MESSAGE;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="callback">XBOX360ガイド表示用デリゲート</param>
			/// <param name="strMessage">メッセージ テキスト文字列</param>
			public CGuideInfoMessageBox(AsyncCallback callback, string strMessage)
				: base(callback)
			{
				MESSAGE = strMessage;
			}

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>XBOX360ガイドを開始します。</summary>
			public override bool start()
			{
				bool bResult = false;
				if(!Guide.IsVisible)
				{
					try
					{
						Guide.BeginShowMessageBox(PlayerIndex.One, "エラーが発生しました。",
							MESSAGE, BUTTONS, 0, MessageBoxIcon.Error, CALLBACK, null);
						bResult = true;
					}
					catch(Exception)
					{
					}
				}
				return bResult;
			}
		}

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>メッセージボックス ガイド表示情報クラス。</summary>
		private sealed class CGuideInfoSelectDevice : CGuideInfoBase
		{

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="callback">XBOX360ガイド表示用デリゲート</param>
			public CGuideInfoSelectDevice(AsyncCallback callback) : base(callback)
			{
			}

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>XBOX360ガイドを開始します。</summary>
			public override bool start()
			{
				bool bResult = false;
				if(!Guide.IsVisible)
				{
					try
					{
						Guide.BeginShowStorageDeviceSelector(CALLBACK, null);
						bResult = true;
					}
					catch(Exception)
					{
					}
				}
				return bResult;
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>XBOX360ガイド表示のキュー。</summary>
		private readonly static Queue<CGuideInfoBase> QUEUE_CALLBACK = new Queue<CGuideInfoBase>();

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>補助記憶装置選択ガイド表示を予約します。</summary>
		/// 
		/// <param name="callback">
		/// XBOX360ガイド操作が終了すると呼び出されるメソッド
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		/// 引数にnullが渡された場合。
		/// </exception>
		public static void reserveSelectDevice(AsyncCallback callback)
		{
			if(callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			else
			{
				QUEUE_CALLBACK.Enqueue(new CGuideInfoSelectDevice(callback));
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>メッセージボックス ガイド表示を予約します。</summary>
		/// 
		/// <param name="callback">
		/// XBOX360ガイド操作が終了すると呼び出されるメソッド
		/// </param>
		/// <param name="strText">メッセージ テキスト文字列</param>
		/// <exception cref="System.ArgumentNullException">
		/// 引数にnullが渡された場合。
		/// </exception>
		public static void reserveMessage(AsyncCallback callback, string strText)
		{
			if(callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			else if(strText == null)
			{
				throw new ArgumentNullException("strText");
			}
			else
			{
				QUEUE_CALLBACK.Enqueue(new CGuideInfoMessageBox(callback, strText));
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>予約されているガイドの表示を試みます。</summary>
		public static void update()
		{
			if(QUEUE_CALLBACK.Count > 0 && !Guide.IsVisible)
			{
				if(QUEUE_CALLBACK.Peek().start())
				{
					QUEUE_CALLBACK.Dequeue();
				}
			}
		}
	}
}

#endif
