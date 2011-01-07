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
	public class CCache<_T>
		: CValue<_T>, ICache
		where _T : class
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>
		/// 読み出し対象の型が<c>System.IDisposable</c>対応しているかどうか。
		/// </summary>
		private readonly bool disposable;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="asset">アセット名。</param>
		public CCache(string asset)
		{
			this.asset = asset;
			Type target = typeof(IDisposable);
			Type[] types = typeof(_T).GetInterfaces();
			for (int i = types.Length; --i >= 0 && !disposable; )
			{
				disposable = types[i] == target;
			}
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

		//* -----------------------------------------------------------------------*
		/// <summary>読み出し対象のアセット文字列を取得します。</summary>
		/// 
		/// <value>読み出し対象のアセット文字列。</value>
		public string asset
		{
			get;
			private set;
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
		/// <summary>値を取得します。</summary>
		/// 
		/// <param name="r">値オブジェクト。</param>
		/// <returns>値。</returns>
		public static implicit operator _T(CCache<_T> r)
		{
			return r.get();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンテンツ本体を取得します。</summary>
		/// 
		/// <returns>コンテンツ本体オブジェクト。</returns>
		public _T get()
		{
			if(!isLoaded)
			{
				preload();
			}
			return value;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンテンツを読み込みます。</summary>
		public void preload()
		{
			preload(false);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンテンツを読み込みます。</summary>
		/// 
		/// <param name="force">強制的にリロードするかどうか。</param>
		public void preload(bool force)
		{
			if (!isLoaded || force)
			{
				if (mgrContent == null)
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
