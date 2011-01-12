////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.util;
using Microsoft.Xna.Framework.Content;

namespace danmaq.nineball.old.core.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>リソースとアセットのペア。</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete]
	public sealed class CResource<_T> where _T : class
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>リソース本体</summary>
		private _T m_resource;

		/// <summary>アセット名文字列</summary>
		private string m_strAsset;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>リソース本体</summary>
		public _T resource
		{
			get
			{
				return m_resource;
			}
			set
			{
				m_resource = value;
			}
		}

		/// <summary>アセット名文字列</summary>
		public string asset
		{
			get
			{
				return m_strAsset;
			}
			set
			{
				m_strAsset = value;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// アセット名に対応したリソースをコンテンツマネージャ経由で読み出します。
		/// </summary>
		/// 
		/// <param name="bForce">リソース本体が<c>null</c>でなくても強制的に再読み込みするかどうか</param>
		/// <param name="mgrContent">コンテンツマネージャ</param>
		public bool load(bool bForce, ContentManager mgrContent)
		{
			bool bNull = (resource == null);
			bool bResult = (asset != null && (bForce || bNull));
			if(bResult)
			{
				resource = mgrContent.Load<_T>(asset);
				CLogger.add("コンテンツ " + asset + " を読込しました。");
			}
			return bResult;
		}
	}
}
