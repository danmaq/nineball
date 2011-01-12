////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.ball.state.cursor;
using danmaq.nineball.entity;
using danmaq.nineball.util;
using Microsoft.Xna.Framework;

namespace danmaq.ball.entity.font
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>カーソル オブジェクト。</summary>
	sealed class CCursor : CEntity
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CCursor instance = new CCursor();

		/// <summary>表示用のAI。</summary>
		public readonly CEntity aiView;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>カーソル位置。</summary>
		private Point m_locate = new Point(6, 16);

		/// <summary>ゲーム難易度。</summary>
		private short m_level = 0;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CCursor()
			: base(CStateCursor.instance)
		{
			locate = Point.Zero;
			aiView = new CEntity(null, this);
			level = 0;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>ワールド空間を取得します。</summary>
		/// 
		/// <value>ワールド空間。</value>
		public Matrix world
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ゲーム難易度を取得/設定します。</summary>
		/// 
		/// <value>ゲーム難易度。</value>
		public short level
		{
			get
			{
				return m_level;
			}
			set
			{
				short v = (short)CMisc.clampLoop(value, 0, 9);
				m_level = v;
				locate = new Point(6 + 8 * v, locate.Y);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>カーソル位置を設定/取得します。</summary>
		/// 
		/// <value>カーソル位置。</value>
		public Point locate
		{
			get
			{
				return m_locate;
			}
			private set
			{
				m_locate = value;
				Matrix w = Matrix.Identity;
				w.Translation = getCursorTranslation(value);
				world = w;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>カーソル座標から3D空間座標へ変換を行います。</summary>
		/// 
		/// <param name="locate">カーソル座標。</param>
		/// <returns>3D空間座標。</returns>
		public static Vector3 getCursorTranslation(Point locate)
		{
			return new Vector3((locate.X - 40f) * 8f, (locate.Y + 4.5f) * -16f, 0);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			aiView.Dispose();
			base.Dispose();
		}
	}
}
