////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.entity.manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace danmaq.nineball.entity.audio
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XACT効果音制御・管理クラス。</summary>
	public sealed class CSE
		: CAudioBase
	{

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="audio">XACT音響制御の基底クラス。</param>
		/// <param name="xwb">XACT波形バンク(BGM) ファイル名。</param>
		public CSE(CAudioBase audio, string xwb)
			: base(audio, xwb)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="xgs">XACTサウンドエンジン ファイル名。</param>
		/// <param name="xsb">XACT再生キュー ファイル名。</param>
		/// <param name="xwb">XACT波形バンク(BGM) ファイル名。</param>
		public CSE(string xgs, string xsb, string xwb)
			: base(xgs, xsb, xwb)
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(GameTime gameTime)
		{
			base.update(gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>効果音を再生します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		public void play(string name)
		{
			Cue cue = soundBank.GetCue(name);
			cue.Play();
//			cueList.Add(cue);
		}
	}
}
