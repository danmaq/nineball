﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework.Content;

namespace danmaq.nineball.data.content
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>コンテンツ キャッシュのインターフェイス。</summary>
	/// <remarks>
	/// コンテンツ キャッシュにコンテンツ マネージャを設定するためだけに使用します。
	/// </remarks>
	public interface ICache : IDisposable
	{

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンテンツ マネージャを設定/取得します。</summary>
		/// 
		/// <value>コンテンツ マネージャ オブジェクト。</value>
		ContentManager mgrContent
		{
			get;
			set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>コンテンツを読み込みます。</summary>
		void preload();
	}
}