////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library / Copyright (c) 2008-2009 danmaq all rights reserved.
//		──解像度管理クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.Nineball.core.data {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>解像度管理クラス。</summary>
	public sealed class CResolution {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>対応している解像度一覧。</summary>
		private readonly List<EResolution> supports = new List<EResolution>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>現在の解像度の矩形情報。</summary>
		private Rectangle rect;

		/// <summary>現在の解像度。</summary>
		private EResolution m_now = EResolution.VGA;

		/// <summary>現在の解像度とVGAとの比率ギャップ。</summary>
		private Vector2 m_scaleGapFromVGA;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CResolution() {
			foreach( DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes ) {
				for( EResolution i = EResolution.VGA; i < EResolution.__reserved; i++ ) {
					Rectangle rect = toRect( i );
					if( mode.Width == rect.Width && mode.Height == rect.Height && !supports.Contains( i ) ) {
						supports.Add( i );
						break;
					}
				}
			}
			supports.Sort();
			now = supports[ 0 ];
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CResolution( EResolution r ) : this() { now = r; }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>現在の解像度とVGAとの比率ギャップ。</summary>
		public Vector2 scaleGapFromVGA {
			get { return m_scaleGapFromVGA; }
			private set { m_scaleGapFromVGA = value; }
		}

		/// <summary>アスペクト比。</summary>
		private float aspect {
			get { return ( float )( rect.Width ) / ( float )( rect.Height ); }
		}

		/// <summary>現在の解像度。</summary>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		private EResolution now {
			get { return m_now; }
			set {
				if( supports.Contains( value ) ) {
					m_now = value;
					rect = toRect( value );
					scaleGapFromVGA = getScaleGap( value, EResolution.VGA );
				}
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>指定の解像度でオブジェクトを作成します。</summary>
		/// 
		/// <param name="r">解像度列挙体</param>
		/// <returns>解像度 オブジェクト</returns>
		public static implicit operator CResolution( EResolution r ) {
			return new CResolution( r );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在の解像度を取得します。</summary>
		/// 
		/// <param name="r">解像度 オブジェクト</param>
		/// <returns>解像度列挙体</returns>
		public static implicit operator EResolution( CResolution r ) { return r.now; }

		//* -----------------------------------------------------------------------*
		/// <summary>現在の解像度を取得します。</summary>
		/// 
		/// <param name="r">解像度 オブジェクト</param>
		/// <returns>解像度</returns>
		public static implicit operator Rectangle( CResolution r ) { return r.rect; }

		//* -----------------------------------------------------------------------*
		/// <summary>アスペクト比を取得します。</summary>
		/// 
		/// <param name="r">解像度 オブジェクト</param>
		/// <returns>アスペクト比</returns>
		public static implicit operator float( CResolution r ) { return r.aspect; }

		//* -----------------------------------------------------------------------*
		/// <summary>解像度列挙体に対応する解像度を取得します。</summary>
		/// 
		/// <param name="resolution">解像度列挙体</param>
		/// <returns>解像度</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static Rectangle toRect( EResolution resolution ) {
			switch( resolution ) {
			case EResolution.VGA:
				return new Rectangle( 0, 0, 640, 480 );
			case EResolution.SVGA:
				return new Rectangle( 0, 0, 800, 600 );
			case EResolution.XGA:
				return new Rectangle( 0, 0, 1024, 768 );
			case EResolution.XGAplus:
				return new Rectangle( 0, 0, 1154, 864 );
			case EResolution.SXGA43:
				return new Rectangle( 0, 0, 1280, 960 );
			case EResolution.SXGA:
				return new Rectangle( 0, 0, 1280, 1024 );
			case EResolution.SXGAplus:
				return new Rectangle( 0, 0, 1400, 1050 );
			case EResolution.UXGA:
				return new Rectangle( 0, 0, 1600, 1200 );
			}
			throw new ArgumentOutOfRangeException();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>解像度間の誤差を取得します。</summary>
		/// 
		/// <param name="res1">解像度列挙体1</param>
		/// <param name="res2">解像度列挙体2</param>
		/// <returns>解像度</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static Vector2 getScaleGap( EResolution res1, EResolution res2 ) {
			Rectangle rectRes1 = toRect( res1 );
			Rectangle rectRes2 = toRect( res2 );
			return new Vector2(
				( float )( rectRes1.Width ) / ( float )( rectRes2.Width ),
				( float )( rectRes1.Height ) / ( float )( rectRes2.Height ) );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>解像度列挙体に対応する解説を取得します。</summary>
		/// 
		/// <param name="resolution">解像度列挙体</param>
		/// <returns>解説</returns>
		static public string ToString( EResolution resolution ) {
			string strRes = resolution.ToString().Replace( "plus", "+" ).Replace( "43", "" );
			Rectangle rect = CResolution.toRect( resolution );
			return strRes + string.Format( "({0}x{1})", rect.Width, rect.Height );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>VGA基準の値を引き延ばします。</summary>
		/// 
		/// <param name="srcRect">VGA基準の値</param>
		/// <returns>現在の解像度基準の値</returns>
		public Rectangle resizeFromVGA( Rectangle srcRect ) {
			if( now == EResolution.VGA ) { return srcRect; }
			else {
				Vector2 scaleGap = scaleGapFromVGA;
				return new Rectangle(
					( int )( scaleGap.X * srcRect.X ),
					( int )( scaleGap.Y * srcRect.Y ),
					( int )( scaleGap.X * srcRect.Width ),
					( int )( scaleGap.Y * srcRect.Height ) );
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>VGA基準の値を引き延ばします。</summary>
		/// 
		/// <param name="srcPoint">VGA基準の値</param>
		/// <returns>現在の解像度基準の値</returns>
		public Vector2 resizeFromVGA( Vector2 srcPoint ) {
			return ( now == EResolution.VGA ? srcPoint : srcPoint * scaleGapFromVGA );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>より上位のデバイスが対応している解像度を取得します。</summary>
		/// <remarks>
		/// もし与えられた解像度が最上位の場合、最下位の解像度を返します。
		/// </remarks>
		/// 
		/// <param name="resolusion">基準となる解像度列挙体</param>
		/// <returns>より上位の解像度列挙体</returns>
		public EResolution getNext( EResolution resolusion ) {
			for( EResolution __res = resolusion + 1; __res < EResolution.__reserved; __res++ ) {
				if( supports.Contains( __res ) ) { return __res; }
			}
			return EResolution.VGA;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 現在設定されている解像度列挙体に対応する解説を取得します。
		/// </summary>
		/// 
		/// <returns>解説</returns>
		public override string ToString() { return ToString( now ); }
	}
}
