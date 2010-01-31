﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

namespace danmaq.nineball.old.core.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>フォース フィードバックのプリセット列挙体。</summary>
	public enum EForcePreset
	{

		/// <summary>フォース停止(レガシ デバイスのみ対応)</summary>
		None,

		/// <summary>一定量(テスト用)・中程度のフォース。</summary>
		Square,

		/// <summary>超短時間減衰・中程度のフォース。</summary>
		Short,

		/// <summary>中時間減衰・中程度のフォース。</summary>
		Mild,

		/// <summary>長時間減衰・非常に強いフォース。</summary>
		Hard
	}
}