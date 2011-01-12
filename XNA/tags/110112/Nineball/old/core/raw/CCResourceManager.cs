////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.old.core.data;
using Microsoft.Xna.Framework.Content;

namespace danmaq.nineball.old.core.raw
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>リソース・キャッシュ</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete]
	public sealed class CResourceManager<_T> where _T : class
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>リソースのリスト。</summary>
		private readonly LinkedList<CResource<_T>> resources =
			new LinkedList<CResource<_T>>();

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>登録されているリソースを再読み込みします。</summary>
		/// 
		/// <param name="bForce">リソース本体が<c>null</c>でなくても強制的に再読み込みするかどうか</param>
		/// <param name="mgrContent">コンテンツマネージャ</param>
		public void reload(bool bForce, ContentManager mgrContent)
		{
			foreach(CResource<_T> resource in resources)
			{
				resource.load(bForce, mgrContent);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>リソースを読み出し・登録します。</summary>
		/// <remarks>
		/// <paramref name="mgrContent"/>に<c>null</c>を指定すると
		/// 登録のみで実際の読み出しは<c>reload</c>を呼び出したときに行われます。
		/// </remarks>
		/// 
		/// <param name="strAsset">アセット名文字列</param>
		/// <param name="mgrContent">コンテンツマネージャ</param>
		public CResource<_T> load(string strAsset, ContentManager mgrContent)
		{
			CResource<_T> resource = new CResource<_T>();
			resource.asset = strAsset;
			if(mgrContent != null)
			{
				resource.load(true, mgrContent);
			}
			resources.AddLast(resource);
			return resource;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>リソースを登録します。</summary>
		/// <remarks>
		/// ここではアセット名の登録のみされます。実際の読み込みは
		/// <c>reload</c>を呼び出したときに初めて行われます。
		/// </remarks>
		/// 
		/// <param name="strAsset">アセット名文字列</param>
		public CResource<_T> load(string strAsset)
		{
			return load(strAsset, null);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>リソースを検索します。</summary>
		/// 
		/// <param name="strAsset">アセット名文字列</param>
		/// <param name="bCreate">対象リソースまだ読まれていない場合、読み出すかどうか</param>
		/// <returns>アセット名に対応するリソース。存在しない場合、null</returns>
		public CResource<_T> search(string strAsset, bool bCreate)
		{
			CResource<_T> result = null;
			foreach(CResource<_T> resource in resources)
			{
				if(resource.asset.Equals(strAsset))
				{
					result = resource;
					break;
				}
			}
			if(result == null && bCreate)
			{
				result = load(strAsset);
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>リソースを検索します。</summary>
		/// <remarks>まだ読み込まれていない場合、自動的に読み込みます。</remarks>
		/// 
		/// <param name="strAsset">アセット名文字列</param>
		/// <returns>アセット名に対応するリソース。存在しない場合、null</returns>
		public CResource<_T> search(string strAsset)
		{
			return search(strAsset, true);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>テクスチャを閉じ、キャッシュからも削除します。</summary>
		/// 
		/// <param name="resource">リソースとアセットのペア オブジェクト</param>
		public void close(CResource<_T> resource)
		{
			resources.Remove(resource);
		}
	}
}
