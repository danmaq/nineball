////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.util.resolution {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>画面解像度列挙体。</summary>
	public enum EResolution {

		/// <summary>VGA(640x480)</summary>
		VGA,

		/// <summary>SVGA(800x600)</summary>
		SVGA,

		/// <summary>XGA(1024x768)</summary>
		XGA,

		/// <summary>XGA+(1152x864)</summary>
		XGAplus,

		/// <summary>SXGA(1280x960)</summary>
		SXGA43,

		/// <summary>SXGA(1280x1024)</summary>
		SXGA,

		/// <summary>SXGA+(1400x1050)</summary>
		SXGAplus,

		/// <summary>SXGA(1600x1200)</summary>
		UXGA,

		/// <summary>WVGA+(848x480)</summary>
		WVGAplus,

		/// <summary>HD 720p(1280x720)</summary>
		HD720p,

		/// <summary>WXGA(1280x768)</summary>
		WXGA,

		/// <summary>Full-wide XGA(1360x768)</summary>
		FullWideXGA,

		/// <summary>WXGA+(1440x900)</summary>
		WXGAplus,

		/// <summary>WSXGA+(1680x1050)</summary>
		WSXGAplus,

		/// <summary>Full HD(1920x1080)</summary>
		FullHD,

		/// <summary>予約(使用出来ません)</summary>
		__reserved
	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>画面解像度列挙体の拡張機能。</summary>
	public static class ERectangleExtention {

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>解像度列挙体に対応する解像度を取得します。</summary>
		/// 
		/// <param name="resolution">解像度列挙体</param>
		/// <returns>解像度</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static Rectangle toRect( this EResolution resolution ) {
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
				case EResolution.WVGAplus:
					return new Rectangle( 0, 0, 848, 480 );
				case EResolution.HD720p:
					return new Rectangle( 0, 0, 1280, 720 );
				case EResolution.WXGA:
					return new Rectangle( 0, 0, 1280, 768 );
				case EResolution.FullWideXGA:
					return new Rectangle( 0, 0, 1360, 768 );
				case EResolution.WXGAplus:
					return new Rectangle( 0, 0, 1440, 900 );
				case EResolution.WSXGAplus:
					return new Rectangle( 0, 0, 1680, 1050 );
				case EResolution.FullHD:
					return new Rectangle( 0, 0, 1920, 1080 );
			}
			throw new ArgumentOutOfRangeException();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定解像度のアスペクト比を取得します。</summary>
		/// 
		/// <param name="res">解像度列挙体</param>
		/// <returns>アスペクト比</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static float getAspect( this EResolution res ) {
			Rectangle rectRes = toRect( res );
			return ( float )( rectRes.Width ) / ( float )( rectRes.Height );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定解像度がワイド解像度であるかどうかを取得します。</summary>
		/// 
		/// <param name="res">解像度列挙体</param>
		/// <returns>ワイド解像度である場合、<c>true</c></returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static bool isWide( this EResolution res ) { return getAspect( res ) > 1.5f; }

		//* -----------------------------------------------------------------------*
		/// <summary>解像度列挙体に対応する解説を取得します。</summary>
		/// 
		/// <param name="resolution">解像度列挙体</param>
		/// <returns>解説</returns>
		public static string ToString( this EResolution resolution ) {
			string strRes = resolution.ToString().Replace( "plus", "+" ).Replace( "43", "" );
			Rectangle rect = resolution.toRect();
			return strRes + string.Format( "({0}x{1})", rect.Width, rect.Height );
		}
	}
}
