////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.nineball.data.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>フォース フィードバックのプリセット列挙体。</summary>
	public enum EForcePreset
	{

		/// <summary>フォース停止。</summary>
		None,

		/// <summary>一定量(テスト用)・中程度のフォース。</summary>
		Square,

		/// <summary>超短時間減衰・中程度のフォース。</summary>
		Short,

		/// <summary>中時間減衰・中程度のフォース。</summary>
		Mild,

		/// <summary>長時間減衰・非常に強いフォース。</summary>
		Hard,

		/// <summary>予約。(使用しないでください)</summary>
		__reserved

	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>フォース フィードバックのプリセット列挙体の拡張機能。</summary>
	public static class EForcePresetExtension
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>フォース情報一覧。</summary>
		private static readonly SForceData[] dataList =
			new SForceData[(int)EForcePreset.__reserved];

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static EForcePresetExtension()
		{
			dataList[(int)EForcePreset.None] = new SForceData(0, 0, 0, 0);
			dataList[(int)EForcePreset.Square] = new SForceData(0.1f, 0.25f, 35, 35);
			dataList[(int)EForcePreset.Short] = new SForceData(
				0, new SGradation(0.2f, 0, 0, 1), 0, 3);
			dataList[(int)EForcePreset.Mild] = new SForceData(
				new SGradation(0.3f, 0, 0, 1), new SGradation(0.7f, 0, 0, 1), 60, 40);
			dataList[(int)EForcePreset.Hard] = new SForceData(
				new SGradation(2, 0, 0, 1), new SGradation(2, 0, 0, 1), 80, 120);
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>プリセットされたフォース情報を取得します。</summary>
		/// 
		/// <param name="index">フォース フィードバックのプリセット列挙体。</param>
		/// <returns>フォース情報。</returns>
		public static SForceData getPresetData(this EForcePreset index)
		{
			return dataList[(int)index];
		}
	}
}
