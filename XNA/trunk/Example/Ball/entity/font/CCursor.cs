////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.ball.state.font.cursor;
using danmaq.ball.state.font.cursor.view;
using danmaq.nineball.entity;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;

namespace danmaq.ball.entity.font
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>カーソル オブジェクト。</summary>
	public sealed class CCursor : CEntity
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CCursor instance = new CCursor();

		/// <summary>表示用のAI。</summary>
		private readonly CAI<CCursor> aiView;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>カーソル位置。</summary>
		private Vector2 m_locate;

		/// <summary>ワールド空間。</summary>
		private Matrix m_world = Matrix.Identity;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CCursor()
			: base(CStateCursor.instance)
		{
			locate = Vector2.Zero;
			aiView = new CAI<CCursor>(this, CStateVisible.instance);
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// 状態として、nullを設定しようとした場合。
		/// </exception>
		public new IState<CCursor, Matrix> nextState
		{
			set
			{
				base.nextState = value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>カーソル位置を設定/取得します。</summary>
		/// 
		/// <value>カーソル位置。</value>
		public Vector2 locate
		{
			get
			{
				return m_locate;
			}
			set
			{
				m_locate = value;
				m_world = Matrix.Identity;
				m_world.Translation = getCursorTranslation(value);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// オブジェクトと状態クラスのみがアクセス可能なフィールドを取得します。
		/// </summary>
		/// 
		/// <value>オブジェクトと状態クラスのみがアクセス可能なフィールド。</value>
		protected override object privateMembers
		{
			get
			{
				return m_world;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>カーソル座標から3D空間座標へ変換を行います。</summary>
		/// 
		/// <param name="locate">カーソル座標。</param>
		/// <returns>3D空間座標。</returns>
		public static Vector3 getCursorTranslation(Vector2 locate)
		{
			return new Vector3(((int)locate.X - 40f) * 8f, (-(int)locate.Y + 11.5f) * 16f, 0);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理を実行します。</summary>
		public override void initialize()
		{
			aiView.initialize();
			base.initialize();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			aiView.Dispose();
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(GameTime gameTime)
		{
			aiView.update(gameTime);
			base.update(gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(GameTime gameTime)
		{
			aiView.draw(gameTime);
			base.draw(gameTime);
		}
	}
}
