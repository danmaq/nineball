////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.data;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.util.resolution
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>解像度管理の基底クラス。</summary>
	public class CResolutionBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>座標変換プリセットの一覧。</summary>
		private readonly Converter<Vector2, Vector2>[] convertVectorList;

		/// <summary>座標変換プリセットの一覧。</summary>
		private readonly Converter<Point, Point>[] convertPointList;

		/// <summary>矩形変換プリセットの一覧。</summary>
		private readonly Converter<Rectangle, Rectangle>[] convertRectangleList;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>現在有効な座標変換プリセット。</summary>
		protected Converter<Vector2, Vector2> activeConvertVector;

		/// <summary>現在有効な座標変換プリセット。</summary>
		protected Converter<Point, Point> activeConvertPoint;

		/// <summary>現在有効な矩形変換プリセット。</summary>
		protected Converter<Rectangle, Rectangle> activeConvertRectangle;

		/// <summary>変換元の解像度。</summary>
		private Rectangle m_src;

		/// <summary>変換先の解像度。</summary>
		private Rectangle m_dst;

		/// <summary>どの方向が画面上部になるか。</summary>
		private EDirection m_top;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="source">変換元の解像度。</param>
		/// <param name="destination">変換先の解像度。</param>
		public CResolutionBase(Rectangle source, Rectangle destination)
		{
			// ----- for Vector2 ----- //
			convertVectorList = new Converter<Vector2, Vector2>[(int)EDirection.__reserved];
			convertVectorList[(int)EDirection.up] = s => s;
			convertVectorList[(int)EDirection.down] =
				s => new Vector2(source.Width - s.X, source.Height - s.Y);
			convertVectorList[(int)EDirection.left] = s => new Vector2(s.Y, source.Width - s.X);
			convertVectorList[(int)EDirection.right] =
				s => new Vector2(source.Height - s.Y, s.X);
			// ----- for Point ----- //
			convertPointList = new Converter<Point, Point>[(int)EDirection.__reserved];
			convertPointList[(int)EDirection.up] = s => s;
			convertPointList[(int)EDirection.down] =
				s => new Point(source.Width - s.X, source.Height - s.Y);
			convertPointList[(int)EDirection.left] = s => new Point(s.Y, source.Width - s.X);
			convertPointList[(int)EDirection.right] = s => new Point(source.Height - s.Y, s.X);
			// ----- for Rectangle ----- //
			convertRectangleList = new Converter<Rectangle, Rectangle>[(int)EDirection.__reserved];
			convertRectangleList[(int)EDirection.up] = s => s;
			convertRectangleList[(int)EDirection.down] = s => new Rectangle(
				source.Width - (s.X + s.Width), source.Height - (s.Y + s.Height),
				s.Width, s.Height);
			convertRectangleList[(int)EDirection.left] = s => new Rectangle(
				s.Y, source.Width - (s.X + s.Width), s.Height, s.Width);
			convertRectangleList[(int)EDirection.right] = s => new Rectangle(
				source.Height - (s.Y + s.Width), s.X, s.Height, s.Width);

			// -----  ----- //
			m_src = source;
			m_dst = destination;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>変換元の解像度を取得/設定します。</summary>
		/// 
		/// <value>変換元の解像度。</value>
		public Rectangle source
		{
			get
			{
				return m_src;
			}
			set
			{
				m_src = value;
				calcurate();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>変換先の解像度を取得/設定します。</summary>
		/// 
		/// <value>変換先の解像度。</value>
		public Rectangle destination
		{
			get
			{
				return m_dst;
			}
			set
			{
				m_dst = value;
				calcurate();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>どの方向が画面上部になるかを取得/設定します。</summary>
		/// 
		/// <value>方向。</value>
		public EDirection top
		{
			get
			{
				return m_top;
			}
			set
			{
				m_top = value;
				calcurate();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>座標誤差を取得します。</summary>
		/// 
		/// <value>座標誤差。</value>
		public Vector2 gap
		{
			get;
			protected set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>拡大率を取得します。</summary>
		/// 
		/// <value>拡大率。</value>
		public Vector2 scale
		{
			get;
			protected set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>回転量を取得します。</summary>
		/// 
		/// <value>回転量。</value>
		public float rotate
		{
			get;
			protected set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>座標を変換します。</summary>
		/// 
		/// <param name="src">変換元の座標。</param>
		/// <returns>変換先の座標。</returns>
		public virtual Vector2 convertPosition(Vector2 src)
		{
			return activeConvertVector(src) * scale + gap;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>座標を変換します。</summary>
		/// 
		/// <param name="src">変換元の座標。</param>
		/// <returns>変換先の座標。</returns>
		public virtual Point convertPosition(Point src)
		{
			Point result = activeConvertPoint(src);
			result.X = (int)(result.X * scale.X + gap.X);
			result.Y = (int)(result.Y * scale.Y + gap.Y);
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>矩形を変換します。</summary>
		/// 
		/// <param name="src">変換元の矩形。</param>
		/// <returns>変換先の矩形。</returns>
		public virtual Rectangle convertRectangle(Rectangle src)
		{
			Rectangle result = activeConvertRectangle(src);
			result.X = (int)(result.X * scale.X + gap.X);
			result.Y = (int)(result.Y * scale.Y + gap.Y);
			result.Width = (int)(result.Width * scale.X);
			result.Height = (int)(result.Height * scale.X);
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在設定されている座標で計算します。</summary>
		protected virtual void calcurate()
		{
			int _top = (int)top;
			bool side = (_top & 1) == 1;
			scale = Vector2.One;
			rotate = MathHelper.PiOver2 * _top;
			activeConvertVector = convertVectorList[_top];
			activeConvertPoint = convertPointList[_top];
			activeConvertRectangle = convertRectangleList[_top];
		}
	}
}
