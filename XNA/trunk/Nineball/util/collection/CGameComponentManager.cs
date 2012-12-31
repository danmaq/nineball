////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.entity;
using danmaq.nineball.entity.component;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.util.collection
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲーム コンポーネント管理クラス。</summary>
	public sealed class CGameComponentManager :
		CDisposablePartialCollection<IGameComponent, GameComponent>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>オブジェクトにアタッチされたゲーム。</summary>
		private readonly Game game;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="game">ゲーム クラス。</param>
		public CGameComponentManager(Game game) :
			base(game.Components)
		{
			this.game = game;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// ゲーム コンポーネントをアクティブにするかどうかを一括設定します。
		/// </summary>
		/// 
		/// <value>ゲーム コンポーネントをアクティブにするかどうか。</value>
		public bool Enabled
		{
			set
			{
				ForEach(item => item.Enabled = value);
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>オブジェクトをゲーム コンポーネントとして一覧に登録します。</summary>
		/// <remarks>
		/// オブジェクトは、自動的にゲーム コンポーネントでラッピングされます。
		/// </remarks>
		/// 
		/// <param name="entity">状態を持つオブジェクト。</param>
		/// <returns>ゲーム コンポーネントでラッピングされたオブジェクト。</returns>
		public CGameComponent addEntity(IEntity entity)
		{
			CGameComponent result = new CGameComponent(game, entity, false);
			Add(result);
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// オブジェクトを描画に対応したゲーム コンポーネントとして一覧に登録します。
		/// </summary>
		/// <remarks>
		/// オブジェクトは、自動的に描画に対応したゲーム コンポーネントでラッピングされます。
		/// </remarks>
		/// 
		/// <param name="entity">状態を持つオブジェクト。</param>
		/// <returns>ゲーム コンポーネントでラッピングされたオブジェクト。</returns>
		public CDrawableGameComponent addDrawableEntity(IEntity entity)
		{
			CDrawableGameComponent result =
				new CDrawableGameComponent(game, entity, false);
			Add(result);
			return result;
		}
	}
}
