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
using Microsoft.Xna.Framework;

namespace danmaq.nineball.util.resolution
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>スクイーズ座標変換する解像度管理クラス。</summary>
	public sealed class CResolutionSqueeze
		: CResolutionBase
	{

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CResolutionSqueeze()
			: this(EResolution.VGA, EResolution.VGA)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="source">変換元の解像度。</param>
		/// <param name="destination">変換先の解像度。</param>
		public CResolutionSqueeze(EResolution source, EResolution destination)
			: this(source.toRect(), destination.toRect())
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="source">変換元の解像度。</param>
		/// <param name="destination">変換先の解像度。</param>
		public CResolutionSqueeze(Rectangle source, Rectangle destination)
			: base(source, destination)
		{
			calcurate();
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>座標を変換します。</summary>
		/// 
		/// <param name="src">変換元の座標。</param>
		/// <returns>変換先の座標。</returns>
		public override Vector2 convertPosition(Vector2 src)
		{
			return activeConvertVector(src) * scale;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在設定されている座標で計算します。</summary>
		protected override void calcurate()
		{
			base.calcurate();
			bool side = ((int)top & 1) == 1;
			scale = new Vector2(
				(float)destination.Width / (float)(side ? source.Height : source.Width),
				(float)destination.Height / (float)(side ? source.Width : source.Height));
		}
	}
}
