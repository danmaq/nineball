////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.util.resolution {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>解像度管理クラス。</summary>
	public class CResolution {

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>グラフィック アダプタのワイド画面サポート。</summary>
		private static CValue<bool> m_bWide = null;

		/// <summary>対応している解像度一覧。</summary>
		private static List<EResolution> m_supports = null;

		/// <summary>現在の解像度の矩形情報。</summary>
		protected Rectangle m_rect;

		/// <summary>現在の解像度。</summary>
		protected EResolution m_now = EResolution.VGA;

		/// <summary>縦画面にするかどうか。</summary>
		private bool m_vertical;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CResolution() : this( null ) { }

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="gdm"></param>
		public CResolution( GraphicsDeviceManager gdm ) {
			int nTarget = 0;
			Rectangle backbuffer = Rectangle.Empty;
			if( gdm != null ) {
				backbuffer.Width = gdm.PreferredBackBufferWidth;
				backbuffer.Height = gdm.PreferredBackBufferHeight;
				int nLength = supports.Count;
				for( int i = 0; i < nLength && nTarget == 0; i++ ) {
					if( backbuffer.Equals( supports[i].toRect() ) ) { nTarget = i; }
				}
			}
			now = supports[nTarget];
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="r">解像度定数</param>
		public CResolution( EResolution r ) : this() { now = r; }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>グラフィック アダプタがワイド画面をサポートしているかどうか。</summary>
		public static bool supportWideScreen {
			get {
				if( m_bWide == null ) { m_bWide = GraphicsAdapter.DefaultAdapter.IsWideScreen; }
				return m_bWide;
			}
		}

		/// <summary>対応している解像度一覧。</summary>
		protected static List<EResolution> supports {
			get {
				if( m_supports == null ) {
					m_supports = new List<EResolution>();
					foreach( DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes ) {
						for( EResolution i = EResolution.VGA; i < EResolution.__reserved; i++ ) {
							Rectangle rect = i.toRect();
							if(
								mode.Width == rect.Width && mode.Height == rect.Height &&
								!m_supports.Contains( i )
							) {
								m_supports.Add( i );
								break;
							}
						}
					}
					m_supports.Sort();
				}
				return m_supports;
			}
		}

		/// <summary>現在の解像度とVGAとの比率ギャップ。</summary>
		public virtual Vector2 scaleGapFromVGA { get; protected set; }

		/// <summary>アスペクト比。</summary>
		public virtual float aspect {
			get { return ( float )( m_rect.Width ) / ( float )( m_rect.Height ); }
		}

		/// <summary>現在の解像度。</summary>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public virtual EResolution now {
			get { return m_now; }
			protected set {
				if( supports.Contains( value ) ) {
					m_now = value;
					m_rect = value.toRect();
					if( vertical ) { m_rect = rotate( m_rect ); }
					scaleGapFromVGA = getScaleGap( value, EResolution.VGA );
				}
			}
		}

		/// <summary>縦画面にするかどうか。</summary>
		public virtual bool vertical {
			get { return m_vertical; }
			set {
				if( m_vertical != value ) {
					m_vertical = value;
					EResolution __now = now;
					now = __now;
				}
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>指定の解像度でオブジェクトを作成します。</summary>
		/// 
		/// <param name="r">解像度列挙体。</param>
		/// <returns>解像度 オブジェクト。</returns>
		public static implicit operator CResolution( EResolution r ) {
			return new CResolution( r );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在の解像度でオブジェクトを作成します。</summary>
		/// 
		/// <param name="r"></param>
		/// <returns>解像度 オブジェクト。</returns>
		public static implicit operator CResolution( GraphicsDeviceManager r ) {
			return new CResolution( r );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在の解像度を取得します。</summary>
		/// 
		/// <param name="r">解像度 オブジェクト。</param>
		/// <returns>解像度列挙体</returns>
		public static implicit operator EResolution( CResolution r ) { return r.now; }

		//* -----------------------------------------------------------------------*
		/// <summary>現在の解像度を取得します。</summary>
		/// 
		/// <param name="r">解像度 オブジェクト。</param>
		/// <returns>解像度</returns>
		public static implicit operator Rectangle( CResolution r ) { return r.m_rect; }

		//* -----------------------------------------------------------------------*
		/// <summary>アスペクト比を取得します。</summary>
		/// 
		/// <param name="r">解像度 オブジェクト。</param>
		/// <returns>アスペクト比</returns>
		public static implicit operator float( CResolution r ) { return r.aspect; }

		//* -----------------------------------------------------------------------*
		/// <summary>矩形を回転します。</summary>
		/// 
		/// <param name="expr">矩形。</param>
		/// <returns>回転した矩形。</returns>
		public static Rectangle rotate( Rectangle expr ) {
			expr.Width ^= expr.Height;
			expr.Height ^= expr.Width;
			expr.Width ^= expr.Height;
			expr.X ^= expr.Y;
			expr.Y ^= expr.X;
			expr.X ^= expr.Y;
			return expr;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>解像度間の誤差を取得します。</summary>
		/// 
		/// <param name="res1">解像度列挙体1。</param>
		/// <param name="res2">解像度列挙体2。</param>
		/// <returns>解像度。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static Vector2 getScaleGap( EResolution res1, EResolution res2 ) {
			Rectangle rectRes1 = res1.toRect();
			Rectangle rectRes2 = res2.toRect();
			return new Vector2(
				( float )( rectRes1.Width ) / ( float )( rectRes2.Width ),
				( float )( rectRes1.Height ) / ( float )( rectRes2.Height ) );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>解像度間の誤差を取得します。</summary>
		/// 
		/// <param name="res1">解像度列挙体1。</param>
		/// <param name="res2">解像度列挙体2。</param>
		/// <returns>解像度</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static Vector2 getScaleGap( Rectangle res1, Rectangle res2 ) {
			return new Vector2(
				( float )( res1.Width ) / ( float )( res2.Width ),
				( float )( res1.Height ) / ( float )( res2.Height ) );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>より上位のデバイスが対応している解像度を取得します。</summary>
		/// <remarks>
		/// もし与えられた解像度が最上位の場合、最下位の解像度を返します。
		/// </remarks>
		/// 
		/// <param name="resolution">基準となる解像度列挙体。</param>
		/// <returns>より上位の解像度列挙体。</returns>
		public static EResolution getNext( EResolution resolution ) {
			bool bWide = resolution.isWide();
			for( EResolution __res = resolution + 1; __res < EResolution.__reserved; __res++ ) {
				if( supports.Contains( __res ) && __res.isWide() == bWide ) { return __res; }
			}
			return getMinResolution( bWide );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>より上位のデバイスが対応している解像度を取得します。</summary>
		/// <remarks>
		/// もし与えられた解像度が最上位の場合、最下位の解像度を返します。
		/// </remarks>
		/// 
		/// <param name="resolution">基準となる解像度列挙体。</param>
		/// <returns>より上位の解像度列挙体。</returns>
		public static EResolution getPrev( EResolution resolution ) {
			bool bWide = resolution.isWide();
			for( EResolution __res = resolution - 1; __res > EResolution.VGA; __res-- ) {
				if( supports.Contains( __res ) && __res.isWide() == bWide ) { return __res; }
			}
			return getMinResolution( bWide );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>対応する最低解像度を取得します。</summary>
		/// 
		/// <returns>対応する最低解像度。</returns>
		public static EResolution getMinResolution() { return getMinResolution( supportWideScreen ); }

		//* -----------------------------------------------------------------------*
		/// <summary>対応する最低解像度を取得します。</summary>
		/// 
		/// <param name="bWide">ワイド画面を優先して検索するかどうか。</param>
		/// <returns>対応する最低解像度。</returns>
		public static EResolution getMinResolution( bool bWide ) {
			for(
				EResolution resolution = EResolution.VGA;
				resolution < EResolution.__reserved; resolution++
			) {
				if( supports.Contains( resolution ) && resolution.isWide() == bWide ) {
					return resolution;
				}
			}
			return bWide ? getMinResolution( false ) : EResolution.VGA;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>対応する最大解像度を取得します。</summary>
		/// 
		/// <returns>対応する最大解像度。</returns>
		public static EResolution getMaxResolution() { return getMaxResolution( supportWideScreen ); }

		//* -----------------------------------------------------------------------*
		/// <summary>対応する最大解像度を取得します。</summary>
		/// 
		/// <param name="bWide">ワイド画面を優先して検索するかどうか。</param>
		/// <returns>対応する最大解像度。</returns>
		public static EResolution getMaxResolution( bool bWide ) {
			EResolution resolution = getMinResolution( bWide );
			EResolution resNext = resolution;
			do {
				resolution = resNext;
				resNext = getNext( resolution );
			}
			while( resolution < resNext );
			return resolution;
		}
#if WINDOWS

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>解像度を自動で検索し、設定します。</para>
		/// <para>Windowsでは最小に、XBOX360では最大に設定します。</para>
		/// </summary>
		/// 
		/// <param name="gdm">グラフィック デバイスの構成・管理クラス。</param>
		/// <param name="bWide">ワイド画面を優先して検索するかどうか。</param>
		/// <param name="bFullScreen">全画面モードにするかどうか。</param>
		/// <returns>
		/// 自動設定の結果が現在の設定と異なり、かつ設定完了した場合、<c>true</c>。
		/// </returns>
		public static bool applyScreenChange( GraphicsDeviceManager gdm, bool bWide, bool bFullScreen ) {
			bool bResult = false;
			EResolution resolution = CResolution.getMinResolution( bWide );
			if( bFullScreen ) { resolution = CResolution.getPrev( CResolution.getMaxResolution( bWide ) ); }
			Rectangle screenRect = resolution.toRect();
			bResult = ( gdm.IsFullScreen != bFullScreen ) ||
				( gdm.PreferredBackBufferWidth != screenRect.Width ) ||
				( gdm.PreferredBackBufferHeight != screenRect.Height );
			if( bResult ) {
				gdm.IsFullScreen = bFullScreen;
				gdm.PreferredBackBufferWidth = screenRect.Width;
				gdm.PreferredBackBufferHeight = screenRect.Height;
				gdm.ApplyChanges();
			}
			return bResult;
		}
#endif

		//* -----------------------------------------------------------------------*
		/// <summary>VGA基準の値を引き延ばします。</summary>
		/// 
		/// <param name="srcRect">VGA基準の値。</param>
		/// <returns>現在の解像度基準の値。</returns>
		public virtual Rectangle resizeFromVGA( Rectangle srcRect ) {
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
		/// <param name="srcPoint">VGA基準の値。</param>
		/// <returns>現在の解像度基準の値。</returns>
		public virtual Vector2 resizeFromVGA( Vector2 srcPoint ) {
			return ( now == EResolution.VGA ? srcPoint : srcPoint * scaleGapFromVGA );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 現在設定されている解像度列挙体に対応する解説を取得します。
		/// </summary>
		/// 
		/// <returns>解説。</returns>
		public override string ToString() { return now.ToString(); }
	}
}
