////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using Microsoft.Xna.Framework.GamerServices;

namespace danmaq.nineball.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>プレゼンス定義構造体。</summary>
	public struct SPresence
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>プレゼンスを無効にする場合。</summary>
		public static readonly SPresence none = GamerPresenceMode.None;

		/// <summary>体験版である場合。</summary>
		public static readonly SPresence trialMode = GamerPresenceMode.FreePlay;

		/// <summary>プレゼンス モード。</summary>
		public readonly GamerPresenceMode mode;

		/// <summary>カスタム状態値。</summary>
		public readonly int? value;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="mode">プレゼンス モード。</param>
		/// <param name="value">カスタム状態値。</param>
		public SPresence(GamerPresenceMode mode, int? value)
		{
			this.mode = mode;
			this.value = value;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="mode">プレゼンス モード。</param>
		public SPresence(GamerPresenceMode mode) : this(mode, null)
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>値から直接オブジェクトを作成します。</summary>
		/// 
		/// <param name="r">値</param>
		/// <returns>値オブジェクト</returns>
		public static implicit operator SPresence(GamerPresenceMode r)
		{
			return new SPresence(r);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在の設定データを全て文字列化します。</summary>
		/// 
		/// <returns>文字列化・整形された設定データ</returns>
		public override string ToString()
		{
			string strResult = string.Format("PRESENCE : {0}", mode.ToString());
			if (value.HasValue)
			{
				strResult += string.Format(" ({0})", value.Value.ToString());
			}
			return strResult;
		}
	}
}
