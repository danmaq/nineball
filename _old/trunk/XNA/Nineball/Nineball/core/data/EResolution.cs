////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2009 danmaq all rights reserved.
//
//		──画面解像度列挙体
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

namespace danmaq.Nineball.core.data {

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

		/// <summary>予約(使用出来ません)</summary>
		__reserved
	}
}
