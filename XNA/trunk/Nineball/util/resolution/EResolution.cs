////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using Microsoft.Xna.Framework;

namespace danmaq.nineball.util.resolution
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>画面解像度列挙体。</summary>
	public enum EResolution
	{

		/// <summary>Sub-QCIF(128x96, AS1.333, 12KPixels)。</summary>
		sQCIF,

		/// <summary>QQVGA(160x120, AS1.333, 18.8KPixels)。</summary>
		QQVGA,

		/// <summary>QCIF(176x144, AS1.222, 24.8KPixels)。</summary>
		QCIF,

		/// <summary>QCGA(200x160, AS1.25, 31.3KPixels)。</summary>
		QCGA,

		/// <summary>HQVGA(240x160, AS1.5, 37.5KPixels)。</summary>
		HQVGA_3_2,

		/// <summary>HQVGA(240x176, AS1.364, 41.3KPixels)。</summary>
		HQVGA,

		/// <summary>HCGA(320x200, AS1.6, 62.5KPixels)。</summary>
		HCGA,

		/// <summary>QVGA(320x240, AS1.333, 75KPixels)。</summary>
		QVGA,

		/// <summary>SIF(352x240, AS1.467, 82.5KPixels)。</summary>
		SIF,

		/// <summary>QVGA+(354x240, AS1.438, 83KPixels)。</summary>
		QVGAplus,

		/// <summary>WQVGA(400x240, AS1.667, 93.8KPixels)。</summary>
		WQVGA,

		/// <summary>CIF(352x288, AS1.222, 99KPixels)。</summary>
		CIF,

		/// <summary>WQVGA+(427x240, AS1.78, 100.1KPixels)。</summary>
		WQVGAplus,

		/// <summary>Full-WQVGA+(432x240, AS1.8, 101.3KPixels)。</summary>
		FWQVGAplus,

		/// <summary>CGA(640x200, AS3.2, 125KPixels)。</summary>
		CGA,

		/// <summary>HVGA(480x320, AS1.5, 150KPixels)。</summary>
		HVGA,

		/// <summary>HVGA(480x360, AS1.333, 168.8KPixels)。</summary>
		HVGA_4_3,

		/// <summary>EGA(640x350, AS1.809, 218.8KPixels)。</summary>
		EGA,

		/// <summary>HVGAWide(640x360, AS1.667, 225KPixels)。</summary>
		HVGAWide,

		/// <summary>Hercules(720x348, AS2.069, 244.7KPixels)。</summary>
		HGC,

		/// <summary>MDA(720x350, AS2.057, 246.1KPixels)。</summary>
		MDA,

		/// <summary>DCGA(640x400, AS1.6, 250KPixels)。</summary>
		DCGA,

		/// <summary>VGA(640x480, AS1.333, 300KPixels)。</summary>
		VGA,

		/// <summary>VGA+(690x480, AS1.438, 323.4KPixels)。</summary>
		VGAplus,

		/// <summary>SD(720x480, AS1.5, 337.5KPixels)。DVD、D2画質。</summary>
		SD,

		/// <summary>WVGA(800x480, AS1.667, 375KPixels)。</summary>
		WVGA,

		/// <summary>NTSC(720x540, AS1.333, 379.7KPixels)。</summary>
		NTSC,

		/// <summary>4CIF(704x576, AS1.222, 396KPixels)。</summary>
		CIF_x4,

		/// <summary>WVGA+(848x480, AS1.767, 397.5KPixels)。</summary>
		WVGAplus,

		/// <summary>FWVGA(854x480, AS1.779, 400.3KPixels)。</summary>
		FWVGA,

		/// <summary>FWVGA(864x480, AS1.8, 405.0KPixels)。</summary>
		FWVGAplus,

		/// <summary>PAL(768x576, AS1.333, 432KPixels)。</summary>
		PAL,

		/// <summary>WVGA++(960x480, AS2, 450KPixels)。</summary>
		WVGAplusplus,

		/// <summary>SVGA(800x600, AS1.333, 468.8KPixels)。</summary>
		SVGA,

		/// <summary>UWVGA(1024x480, AS2.133, 480KPixels)。</summary>
		UWVGA,

		/// <summary>SVGA(832x624, AS1.333, 0.5MPixels)。</summary>
		SVGA_Mac,

		/// <summary>WSVGA(1024x576, AS1.778, 0.56MPixels)。</summary>
		WSVGA_16_9,

		/// <summary>WSVGA(1024x600, AS1.707, 0.59MPixels)。</summary>
		WSVGA,

		/// <summary>UWSVGA(1280x600, AS2.133, 0.73MPixels)。</summary>
		UWSVGA,

		/// <summary>XGA(1024x768, AS1.333, 0.75MPixels)。</summary>
		XGA,

		/// <summary>XGA(1024x800, AS1.280, 0.78MPixels)。</summary>
		XGA_2,

		/// <summary>H98 HiRes(1120x750, AS1.493, 0.8MPixels)。</summary>
		HiRes98,

		/// <summary>XGA(1024x852, AS1.202, 0.83MPixels)。</summary>
		XGA_3,

		/// <summary>HD 720(1280x720, AS1.778, 0.88MPixels)。D4画質。</summary>
		HD720,

		/// <summary>WXGA(1280x768, AS1.667, 0.94MPixels)。</summary>
		WXGA,

		/// <summary>XGA+(1152x864, AS1.333, 0.95MPixels)。</summary>
		XGAplus,

		/// <summary>XGA+(1152x870, AS1.324, 0.96MPixels)。</summary>
		XGAplusMac,

		/// <summary>WXGA(1280x800, AS1.6, 0.98MPixels)。</summary>
		WXGA_16_10,

		/// <summary>FWXGA(1360x768, AS1.771, 1MPixels)。</summary>
		FWXGA,

		/// <summary>WSXGA(1280x854, AS1.499, 1.04MPixels)。</summary>
		WSXGA,

		/// <summary>SXGA(1280x960, AS1.333, 1.17MPixels)。</summary>
		SXGA43,

		/// <summary>WXGA+(1440x900, AS1.6, 1.24MPixels)。</summary>
		WXGAplus,

		/// <summary>SXGA(1280x1024, AS1.25, 1.25MPixels)。</summary>
		SXGA,

		/// <summary>WXGA++(1600x900, AS1.778, 1.37MPixels)。</summary>
		WXGAplusplus,

		/// <summary>SXGA+(1400x1050, AS1.333, 1.4MPixels)。</summary>
		SXGAplus,

		/// <summary>HD(1440x1080, AS1.333, 1.48MPixels)。フルセグ画質。</summary>
		HD,

		/// <summary>16CIF(1408x1152, AS1.222, 1.55MPixels)。</summary>
		CIF_x16,

		/// <summary>WSXGA+(1680x1050, AS1.6, 1.68MPixels)。</summary>
		WSXGAplus,

		/// <summary>UXGA(1600x1200, AS1.333, 1.8MPixels)。</summary>
		UXGA,

		/// <summary>UXGA+(1600x1280, AS1.25, 1.95MPixels)。</summary>
		UXGAplus,

		/// <summary>Full HD(1920x1080, AS1.667, 1.98MPixels)。D5画質。</summary>
		FullHD,

		/// <summary>2K Cinema(2048x1080, AS1.896, 2.1MPixels)。</summary>
		Cinema2K,

		/// <summary>WUXGA(1920x1200, AS1.6, 2.2MPixels)。</summary>
		WUXGA,

		/// <summary>QWXGA(2048x1152, AS1.778, 2.3MPixels)。</summary>
		QWXGA,

		/// <summary>QXGA(2048x1576, AS1.299, 3.1MPixels)。</summary>
		QXGA,

		/// <summary>WQHD(2560x1440, AS1.778, 3.5MPixels)。</summary>
		WQHD,

		/// <summary>WQXGA(2560x1600, AS1.6, 3.9MPixels)。</summary>
        WQXGA,

		/// <summary>QSXGA(2560x2048, AS1.25, 5MPixels)。</summary>
		QSXGA,

		/// <summary>QSXGA+(2800x2100, AS1.333, 5.6MPixels)。</summary>
		QSXGAplus,

		/// <summary>WQSXGA(3200x2048, AS1.563, 6.3MPixels)。</summary>
		WQSXGA,

		/// <summary>QUXGA(3200x2400, AS1.333, 7.3MPixels)。</summary>
		QUXGA,

		/// <summary>4xFullHD(3840x2160, AS1.896, 8.4MPixels)。</summary>
		FullHD_x4,

		/// <summary>4K Cinema(4096x2160, AS1.896, 8.4MPixels)。</summary>
		Cinema4K,

		/// <summary>WQUXGA(3840x2400, AS1.6, 8.8MPixels)。</summary>
		WQUXGA,

		/// <summary>HXGA(4096x3072, AS1.333, 12MPixels)。</summary>
		HXGA,

		/// <summary>WHXGA(5120x3200, AS1.563, 15MPixels)。</summary>
		WHXGA,

		/// <summary>HSXGA(5120x4096, AS1.25, 20MPixels)。</summary>
		HSXGA,

		/// <summary>HSXGA(6400x4096, AS1.563, 25MPixels)。</summary>
		WHSXGA,

		/// <summary>HSXGA(6400x4800, AS1.333, 29.3MPixels)。</summary>
		HUXGA,

		/// <summary>Ultra HD(7680x4320, AS1.667, 31.6MPixels)。</summary>
		UltraHD,

		/// <summary>8K Cinema(8192x4320, AS1.896, 33.8MPixels)。</summary>
		Cinema8K,

		/// <summary>WHUXGA(7680x4800, AS1.6, 35.2MPixels)。</summary>
		WHUXGA,

		/// <summary>予約(使用出来ません)。</summary>
		__reserved
	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>画面解像度列挙体の拡張機能。</summary>
	public static class EResolutionExtention
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>解像度一覧。</summary>
		private static Point[] pointList = new Point[(int)EResolution.__reserved];

		/// <summary>解像度一覧。</summary>
		private static Rectangle[] rectList = new Rectangle[(int)EResolution.__reserved];

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static EResolutionExtention()
		{
			pointList[(int)EResolution.sQCIF] = new Point(128, 96);
			pointList[(int)EResolution.QQVGA] = new Point(160, 120);
			pointList[(int)EResolution.QCIF] = new Point(176, 144);
			pointList[(int)EResolution.QCGA] = new Point(200, 160);
			pointList[(int)EResolution.HQVGA_3_2] = new Point(240, 160);
			pointList[(int)EResolution.HQVGA] = new Point(240, 176);
			pointList[(int)EResolution.HCGA] = new Point(320, 200);
			pointList[(int)EResolution.QVGA] = new Point(320, 240);
			pointList[(int)EResolution.SIF] = new Point(352, 240);
			pointList[(int)EResolution.QVGAplus] = new Point(354, 240);
			pointList[(int)EResolution.WQVGA] = new Point(400, 240);
			pointList[(int)EResolution.CIF] = new Point(352, 288);
			pointList[(int)EResolution.WQVGAplus] = new Point(427, 240);
			pointList[(int)EResolution.FWQVGAplus] = new Point(432, 240);
			pointList[(int)EResolution.CGA] = new Point(640, 200);
			pointList[(int)EResolution.HVGA] = new Point(480, 320);
			pointList[(int)EResolution.HVGA_4_3] = new Point(480, 360);
			pointList[(int)EResolution.EGA] = new Point(640, 350);
			pointList[(int)EResolution.HVGAWide] = new Point(640, 360);
			pointList[(int)EResolution.HGC] = new Point(720, 348);
			pointList[(int)EResolution.MDA] = new Point(720, 350);
			pointList[(int)EResolution.DCGA] = new Point(640, 400);
			pointList[(int)EResolution.VGA] = new Point(640, 480);
			pointList[(int)EResolution.VGAplus] = new Point(690, 480);
			pointList[(int)EResolution.SD] = new Point(720, 480);
			pointList[(int)EResolution.WVGA] = new Point(800, 480);
			pointList[(int)EResolution.NTSC] = new Point(720, 540);
			pointList[(int)EResolution.CIF_x4] = new Point(704, 576);
			pointList[(int)EResolution.WVGAplus] = new Point(848, 480);
			pointList[(int)EResolution.FWVGA] = new Point(854, 480);
			pointList[(int)EResolution.FWVGAplus] = new Point(864, 480);
			pointList[(int)EResolution.PAL] = new Point(768, 576);
			pointList[(int)EResolution.WVGAplusplus] = new Point(960, 480);
			pointList[(int)EResolution.SVGA] = new Point(800, 600);
			pointList[(int)EResolution.UWVGA] = new Point(1024, 480);
			pointList[(int)EResolution.SVGA_Mac] = new Point(832, 624);
			pointList[(int)EResolution.WSVGA_16_9] = new Point(1024, 576);
			pointList[(int)EResolution.WSVGA] = new Point(1024, 600);
			pointList[(int)EResolution.UWSVGA] = new Point(1280, 600);
			pointList[(int)EResolution.XGA] = new Point(1024, 768);
			pointList[(int)EResolution.XGA_2] = new Point(1024, 800);
			pointList[(int)EResolution.HiRes98] = new Point(1120, 750);
			pointList[(int)EResolution.XGA_3] = new Point(1024, 852);
			pointList[(int)EResolution.HD720] = new Point(1280, 720);
			pointList[(int)EResolution.WXGA] = new Point(1280, 768);
			pointList[(int)EResolution.XGAplus] = new Point(1152, 864);
			pointList[(int)EResolution.XGAplusMac] = new Point(1152, 870);
			pointList[(int)EResolution.WXGA_16_10] = new Point(1280, 800);
			pointList[(int)EResolution.FWXGA] = new Point(1360, 768);
			pointList[(int)EResolution.WSXGA] = new Point(1280, 854);
			pointList[(int)EResolution.SXGA43] = new Point(1280, 960);
			pointList[(int)EResolution.WXGAplus] = new Point(1440, 900);
			pointList[(int)EResolution.SXGA] = new Point(1280, 1024);
			pointList[(int)EResolution.WXGAplusplus] = new Point(1600, 900);
			pointList[(int)EResolution.SXGAplus] = new Point(1400, 1050);
			pointList[(int)EResolution.HD] = new Point(1440, 1080);
			pointList[(int)EResolution.CIF_x16] = new Point(1408, 1152);
			pointList[(int)EResolution.WSXGAplus] = new Point(1680, 1050);
			pointList[(int)EResolution.UXGA] = new Point(1600, 1200);
			pointList[(int)EResolution.UXGAplus] = new Point(1600, 1280);
			pointList[(int)EResolution.FullHD] = new Point(1920, 1080);
			pointList[(int)EResolution.Cinema2K] = new Point(2048, 1080);
			pointList[(int)EResolution.WUXGA] = new Point(1920, 1200);
			pointList[(int)EResolution.QWXGA] = new Point(2048, 1152);
			pointList[(int)EResolution.QXGA] = new Point(2048, 1576);
			pointList[(int)EResolution.WQHD] = new Point(2560, 1440);
			pointList[(int)EResolution.WQXGA] = new Point(2560, 1600);
			pointList[(int)EResolution.QSXGA] = new Point(2560, 2048);
			pointList[(int)EResolution.QSXGAplus] = new Point(2800, 2100);
			pointList[(int)EResolution.WQSXGA] = new Point(3200, 2048);
			pointList[(int)EResolution.QUXGA] = new Point(3200, 2400);
			pointList[(int)EResolution.FullHD_x4] = new Point(3840, 2160);
			pointList[(int)EResolution.Cinema4K] = new Point(4096, 2160);
			pointList[(int)EResolution.WQUXGA] = new Point(3840, 2400);
			pointList[(int)EResolution.HXGA] = new Point(4096, 3072);
			pointList[(int)EResolution.WHXGA] = new Point(5120, 3200);
			pointList[(int)EResolution.HSXGA] = new Point(5120, 4096);
			pointList[(int)EResolution.WHSXGA] = new Point(6400, 4096);
			pointList[(int)EResolution.HUXGA] = new Point(6400, 4800);
			pointList[(int)EResolution.UltraHD] = new Point(7680, 4320);
			pointList[(int)EResolution.Cinema8K] = new Point(8192, 4320);
			pointList[(int)EResolution.WHUXGA] = new Point(7680, 4880);
			for (int i = pointList.Length; --i >= 0; )
			{
				Point pos = pointList[i];
				rectList[i] = new Rectangle(0, 0, pos.X, pos.Y);
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1段階小さい解像度を取得します。</summary>
		/// 
		/// <param name="resolution">解像度列挙体。</param>
		/// <returns>解像度。</returns>
		public static EResolution prev(this EResolution resolution)
		{
			int i = ((int)resolution) - 1;
			return i >= 0 ? (EResolution)i : EResolution.__reserved.prev();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1段階大きい解像度を取得します。</summary>
		/// 
		/// <param name="resolution">解像度列挙体。</param>
		/// <returns>解像度。</returns>
		public static EResolution next(this EResolution resolution)
		{
			int i = ((int)resolution) + 1;
			return i < (int)EResolution.__reserved ? (EResolution)i : (EResolution)0;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>解像度列挙体に対応する解像度を取得します。</summary>
		/// 
		/// <param name="resolution">解像度列挙体。</param>
		/// <returns>解像度。</returns>
		/// <exception cref="System.IndexOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static Point getXY(this EResolution resolution)
		{
			return pointList[(int)resolution];
		}

		//* -----------------------------------------------------------------------*
		/// <summary>解像度列挙体に対応する解像度を取得します。</summary>
		/// 
		/// <param name="resolution">解像度列挙体。</param>
		/// <returns>解像度。</returns>
		/// <exception cref="System.IndexOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static Rectangle toRect(this EResolution resolution)
		{
			return rectList[(int)resolution];
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定解像度のアスペクト比を取得します。</summary>
		/// 
		/// <param name="resolution">解像度列挙体。</param>
		/// <returns>アスペクト比。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static float getAspect(this EResolution resolution)
		{
			Point pos = resolution.getXY();
			return (float)(pos.X) / (float)(pos.Y);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定解像度がワイド解像度であるかどうかを取得します。</summary>
		/// 
		/// <param name="resolution">解像度列挙体。</param>
		/// <returns>ワイド解像度である場合、<c>true</c>。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static bool isWide(this EResolution resolution)
		{
			return getAspect(resolution) > 1.495f;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>解像度列挙体に対応する解説を取得します。</summary>
		/// 
		/// <param name="resolution">解像度列挙体。</param>
		/// <returns>解説。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public static string getDescription(this EResolution resolution)
		{
			string strRes = resolution.ToString().Replace("plus", "+");
			Point pos = resolution.getXY();
			return strRes + string.Format(
				"({0}x{1})", pos.X.ToString(), pos.Y.ToString());
		}
	}
}
