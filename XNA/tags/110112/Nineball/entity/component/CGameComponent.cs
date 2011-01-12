////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.data;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.component
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲームコンポーネントと状態を持つオブジェクトとのアダプタ クラス。</summary>
	public class CGameComponent :
		GameComponent, IGameComponentWithEntity
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>状態を持つオブジェクト。</summary>
		private readonly IEntity entity;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="game">ゲーム コンポーネントをアタッチするゲーム。</param>
		/// <param name="entity">状態を持つオブジェクト。</param>
		/// <param name="bDirectRegist">ゲーム コンポーネントを直接登録するかどうか。</param>
		/// <exception cref="System.ArgumentNullException">
		/// 状態を持つオブジェクトとして、nullが設定された場合。
		/// </exception>
		public CGameComponent(Game game, IEntity entity, bool bDirectRegist)
			: base(game)
		{
			if(entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			this.entity = entity;
			if(bDirectRegist)
			{
				game.Components.Add(this);
			}
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>状態が遷移された時に呼び出されるイベントを追加/削除します。</summary>
		/// 
		/// <value>状態が遷移された時に呼び出されるイベント。</value>
		public event EventHandler<CEventChangedState> changedState
		{
			add
			{
				entity.changedState += value;
			}
			remove
			{
				entity.changedState -= value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>最後に変化する前の状態を取得します。</summary>
		/// 
		/// <value>最後に変化する前の状態。初期値は<c>CState.empty</c>。</value>
		public IState previousState
		{
			get
			{
				return entity.previousState;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在の状態を取得します。</summary>
		/// 
		/// <value>現在の状態。初期値は<c>CState.empty</c>。</value>
		public IState currentState
		{
			get
			{
				return entity.currentState;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		public IState nextState
		{
			get
			{
				return entity.nextState;
			}
			set
			{
				entity.nextState = value;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理を実行します。</summary>
		public override void Initialize()
		{
			initialize();
			base.Initialize();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void Update(GameTime gameTime)
		{
			update(gameTime);
			base.Update(gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理を実行します。</summary>
		public void initialize()
		{
			entity.initialize();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public void update(GameTime gameTime)
		{
			entity.update(gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public void draw(GameTime gameTime)
		{
			entity.draw(gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		/// 
		/// <param name="disposing">
		/// マネージ リソースとアンマネージ リソースの両方を解放するかどうか。
		/// </param>
		protected override void Dispose(bool disposing)
		{
			entity.Dispose();
			base.Dispose(disposing);
		}
	}
}
