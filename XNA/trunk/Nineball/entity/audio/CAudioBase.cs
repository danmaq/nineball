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
	/// <summary>XACT音響制御の基底クラス。</summary>
	public abstract class CAudioBase
		: ITask
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>オーディオ エンジン。</summary>
		public readonly AudioEngine engine;

		/// <summary>サウンド バンク。</summary>
		public readonly SoundBank soundBank;

		/// <summary>波形バンク。</summary>
		public readonly WaveBank waveBank;

		/// <summary>オーディオ キューの一覧。</summary>
		protected readonly List<Cue> cueList = new List<Cue>();

		/// <summary>オーディオ エンジン更新のためのデリゲート。</summary>
		private readonly Action engineUpdate;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="audio">XACT音響制御の基底クラス。</param>
		/// <param name="xwb">XACT波形バンク(BGM) ファイル名。</param>
		public CAudioBase(CAudioBase audio, string xwb)
		{
			engine = audio.engine;
			waveBank = new WaveBank(engine, xwb);
			soundBank = audio.soundBank;
			engineUpdate = () =>
			{
			};
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="xgs">XACTサウンドエンジン ファイル名。</param>
		/// <param name="xsb">XACT再生キュー ファイル名。</param>
		/// <param name="xwb">XACT波形バンク(BGM) ファイル名。</param>
		public CAudioBase(string xgs, string xsb, string xwb)
		{
			engine = new AudioEngine(xgs);
			waveBank = new WaveBank(engine, xwb);
			soundBank = new SoundBank(engine, xsb);
			engineUpdate = () => engine.Update();
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public virtual void update(GameTime gameTime)
		{
			engineUpdate();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public void draw(GameTime gameTime)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public void Dispose()
		{
			if (!soundBank.IsDisposed)
			{
				soundBank.Dispose();
			}
			if (!waveBank.IsDisposed)
			{
				waveBank.Dispose();
			}
			if (!engine.IsDisposed)
			{
				engine.Dispose();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在登録されているBGMに対し、任意の命令を発行します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <param name="action">命令。</param>
		/// <returns>命令を実行できた場合、<c>true</c>。</returns>
		public bool command(string name, Action<Cue> action)
		{
			Cue cue = cueList.Find(c => c.Name == name);
			bool result = cue != null;
			if (result)
			{
				action(cue);
			}
			return result;
		}
	}
}
