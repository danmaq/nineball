////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.ball.state.font.cursor;
using danmaq.nineball.entity;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;

namespace danmaq.ball.entity.font
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>カーソル オブジェクト。</summary>
	public sealed class CCursor : CEntity
	{

		// TODO : 表示だけでなく、中身も実装する。

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CCursor instance = new CCursor();

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
		private CCursor() : base(CStateVisible.instance)
		{
			locate = Vector2.Zero;
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
	}
}
