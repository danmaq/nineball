////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.util.math;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.data.animation
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>フェードのためのコンテンツ データ。</summary>
	[Serializable]
	public struct SFadeData
		: IAnimationData<SFadeData.SData>
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>フォグデータを構成する構造体。</summary>
		public struct SData
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>色。</summary>
			public Color color;
		}

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>線形補完パターン。</summary>
		public EInterpolate interpolate;

		/// <summary>開始値。</summary>
		public SData start;

		/// <summary>終了値。</summary>
		public SData end;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>現在シーンが有効な時間を取得/設定します。</summary>
		/// 
		/// <value>現在シーンが有効な時間。</value>
		public int interval
		{
			get;
			set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>次に移動するシーンを相対値で取得/設定します。</summary>
		/// 
		/// <value>次に移動するシーン(相対指定)。</value>
		public int next
		{
			get;
			set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>現在のフォグ情報を取得します。</summary>
		/// 
		/// <param name="now">現在の時間。</param>
		/// <returns>現在のフォグ情報。</returns>
		public SData getNow(int now)
		{
			SData data = new SData();
			float amount = interpolate.interpolate(0, 1, now, interval);
			data.color = Color.Lerp(start.color, end.color, amount);
			return data;
		}
	}
}
