////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM
//	サンプルゲーム用 コンテンツ プロセッサ
//		Copyright (c) 2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Reflection;
using danmaq.nineball.data.content;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>コンテンツ キャッシュの一覧。</summary>
	static class CONTENT
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>カーソル エフェクト。</summary>
		public static readonly CCache<Effect> fxCursor = new CCache<Effect>("cursor");

		/// <summary>カーソル エフェクト。</summary>
		public static readonly CCache<Texture2D> texBall = new CCache<Texture2D>("ball");

		/// <summary>danmaqロゴ。</summary>
		public static readonly CCache<Texture2D> texLogo = new CCache<Texture2D>("logo");

		/// <summary>よゆ風ビットマップ フォント。</summary>
		public static readonly CCache<SpriteFont> texFont98 = new CCache<SpriteFont>("font");

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>コンテンツ マネージャを設定します。</summary>
		/// 
		/// <param name="mgrContent">コンテンツ マネージャ。</param>
		public static void setContentManager(ContentManager mgrContent)
		{
			FieldInfo[] fields = typeof(CONTENT).GetFields();
			for (int i = fields.Length; --i >= 0; )
			{
				((ICache)fields[i].GetValue(null)).mgrContent =
					mgrContent;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>該当のコンテンツを検索します。</summary>
		/// 
		/// <param name="asset">検索対象となるアセット名。</param>
		/// <returns>コンテンツ キャッシュ。発見できない場合、<c>null</c>。</returns>
		public static ICache find(string asset)
		{
			ICache result = null;
			FieldInfo[] fields = typeof(CONTENT).GetFields();
			for (int i = fields.Length; --i >= 0 && result == null; )
			{
				ICache item = (ICache)fields[i].GetValue(null);
				if (item.asset == asset)
				{
					result = item;
				}
			}
			return result;
		}
	}
}
