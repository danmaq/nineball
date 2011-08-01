////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2009 danmaq all rights reserved.
//
//		──リソースとアセットのペア。
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using Microsoft.Xna.Framework.Content;
using danmaq.Nineball.core.raw;

namespace danmaq.Nineball.core.data {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>リソースとアセットのペア。</summary>
	public sealed class CResource<_T> where _T : class {

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>リソース本体</summary>
		private _T m_resource;

		/// <summary>アセット名文字列</summary>
		private string m_strAsset;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>リソース本体</summary>
		public _T resource {
			get { return m_resource; }
			set { m_resource = value; }
		}

		/// <summary>アセット名文字列</summary>
		public string asset {
			get { return m_strAsset; }
			set { m_strAsset = value; }
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
		public bool load( bool bForce, ContentManager mgrContent ) {
			bool bNull = ( resource == null );
			bool bResult = ( asset != null && ( bForce || bNull ) );
			if( bResult ) {
				resource = mgrContent.Load<_T>( asset );
				CLogger.add( "コンテンツ " + asset + " を読込しました。" );
			}
			return bResult;
		}
	}
}
