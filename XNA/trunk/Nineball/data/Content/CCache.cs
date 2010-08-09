////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.Properties;
using Microsoft.Xna.Framework.Content;

namespace danmaq.nineball.data.content
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>コンテンツ キャッシュ。</summary>
	/// <remarks>
	/// ContentManager.Load&lt;&gt;()がどうもキャッシュして
	/// くれてなさそうな感じがするので、作ってみました。
	/// </remarks>
	/// 
	/// <typeparam name="_T">コンテンツの型情報。</typeparam>
	public class CCache<_T> : CValue<_T>, ICache where _T : class
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>
		/// 読み出し対象の型が<c>System.IDisposable</c>対応しているかどうか。
		/// </summary>
		private readonly bool disposable;

		/// <summary>読み出し対象のアセット文字列。</summary>
		private readonly string asset;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="asset">アセット名。</param>
		public CCache(string asset)
		{
			this.asset = asset;
			disposable = typeof(_T).GetInterface("System.IDisposable") != null;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>既に読み込み済みかどうかを取得します。</summary>
		/// 
		/// <value>既に読み込み済みである場合、<c>true</c>。</value>
		public bool isLoaded
		{
			get
			{
				return (value != null);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンテンツ マネージャを設定/取得します。</summary>
		/// 
		/// <value>コンテンツ マネージャ オブジェクト。</value>
		public ContentManager mgrContent
		{
			get;
			set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>値から直接オブジェクトを作成します。</summary>
		/// 
		/// <param name="s">値。</param>
		/// <returns>値オブジェクト。</returns>
		public static implicit operator CCache<_T>(string s)
		{
			return new CCache<_T>(s);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンテンツ本体を取得します。</summary>
		/// 
		/// <returns>コンテンツ本体オブジェクト。</returns>
		public _T get()
		{
			return this;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンテンツを読み込みます。</summary>
		public void preload()
		{
			if(!isLoaded)
			{
				if(mgrContent == null)
				{
					throw new InvalidOperationException(Resources.ERR_NULL_MGR_CONTENT);
				}
				value = mgrContent.Load<_T>(asset);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンテンツを解放します。</summary>
		public void Dispose()
		{
			if(isLoaded)
			{
				if(disposable)
				{
					((IDisposable)value).Dispose();
				}
				value = null;
			}
		}
	}
}
