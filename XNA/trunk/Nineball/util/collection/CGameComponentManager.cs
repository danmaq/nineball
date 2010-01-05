////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace danmaq.nineball.util.collection {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲーム コンポーネント管理クラス。</summary>
	public sealed class CGameComponentManager :
		CDisposablePartialCollection<IGameComponent, GameComponent>
	{

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="gameComponentCollection">
		/// ゲームが所有しているゲーム コンポーネントのコレクション。
		/// </param>
		public CGameComponentManager( GameComponentCollection gameComponentCollection ) :
			base( gameComponentCollection ) { }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// ゲーム コンポーネントをアクティブにするかどうかを一括設定します。
		/// </summary>
		/// 
		/// <value>ゲーム コンポーネントをアクティブにするかどうか。</value>
		public bool Enabled {
			set { partial.ForEach( item => item.Enabled = value ); }
		}
	}
}
