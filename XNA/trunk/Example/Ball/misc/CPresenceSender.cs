////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.ball.entity.font;
using danmaq.ball.state.scene;
using danmaq.nineball.data;
using danmaq.nineball.entity.manager;
using danmaq.nineball.state;
using Microsoft.Xna.Framework.GamerServices;

namespace danmaq.ball.misc
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>プレゼンス情報送信クラス。</summary>
	public static class CPresenceSender
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>低難易度。</summary>
		private static readonly SPresence DifficultyEasy = GamerPresenceMode.DifficultyEasy;

		/// <summary>中難易度。</summary>
		private static readonly SPresence DifficultyMedium = GamerPresenceMode.DifficultyMedium;

		/// <summary>高難易度。</summary>
		private static readonly SPresence DifficultyHard = GamerPresenceMode.DifficultyHard;

		/// <summary>狂難易度。</summary>
		private static readonly SPresence DifficultyExtreme = GamerPresenceMode.DifficultyExtreme;

		/// <summary>難易度選択中。</summary>
		private static readonly SPresence AtMenu = GamerPresenceMode.AtMenu;

		/// <summary>戦闘中。</summary>
		private static readonly SPresence InCombat = GamerPresenceMode.InCombat;

		/// <summary>負け。</summary>
		private static readonly SPresence Losing = GamerPresenceMode.Losing;

		/// <summary>勝利。</summary>
		private static readonly SPresence Winning = GamerPresenceMode.Winning;

		/// <summary>クレジットの表示中。</summary>
		private static readonly SPresence WatchingCredits = GamerPresenceMode.WatchingCredits;

		/// <summary>設定されるプレゼンス一覧。</summary>
		private static readonly IList<SPresence> presenceList =
			CPresenceManager.instance.presenceList;

		/// <summary>レベル別難易度一覧。</summary>
		private static readonly SPresence[] levelList;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static CPresenceSender()
		{
			levelList = new SPresence[]
			{
				DifficultyEasy, DifficultyEasy, DifficultyEasy, 
				DifficultyMedium, DifficultyMedium, DifficultyHard,
				DifficultyHard, DifficultyExtreme, DifficultyExtreme,
			};
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>クレジット表示中の情報を送信します。</summary>
		public static void sendAtCredit()
		{
			presenceList.Clear();
			presenceList.Add(WatchingCredits);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>難易度選択中の情報を送信します。</summary>
		public static void sendAtMenu()
		{
			presenceList.Clear();
			presenceList.Add(AtMenu);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>敵の玉と競走中の情報を送信します。</summary>
		public static void sendAtGame()
		{
			presenceList.Clear();
			presenceList.Add(InCombat);
			presenceList.Add(levelList[CCursor.instance.level]);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>結果発表中の情報を送信します。</summary>
		/// 
		/// <param name="scene">現在の状態。</param>
		public static void sendAtJudge(IState scene)
		{
			presenceList.Clear();
			presenceList.Add(scene == CSceneJudge.won ? Winning : Losing);
		}
	}
}
