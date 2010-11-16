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
using System.Collections.ObjectModel;
using danmaq.nineball.entity.manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace danmaq.nineball.entity.audio
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XACT音声制御・管理クラス。</summary>
	public sealed class CAudio
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
		private readonly List<Cue> cueList = new List<Cue>();

		/// <summary>オーディオ キュー予約の一覧。</summary>
		private readonly List<string> reservedList = new List<string>();

		/// <summary>オーディオ エンジン更新のためのデリゲート。</summary>
		private readonly Action engineUpdate;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>ループ再生に必要な時間経過(ms単位)。</summary>
		public int loopInterval = 0;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="audio">XACT音響制御の基底クラス。</param>
		/// <param name="xwb">XACT波形バンク(BGM) ファイル名。</param>
		public CAudio(CAudio audio, string xwb)
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
		public CAudio(string xgs, string xsb, string xwb)
		{
			engine = new AudioEngine(xgs);
			waveBank = new WaveBank(engine, xwb);
			soundBank = new SoundBank(engine, xsb);
			engineUpdate = () => engine.Update();
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>キュー一覧を取得します。</summary>
		/// 
		/// <value>キュー一覧。</value>
		public ReadOnlyCollection<Cue> cues
		{
			get
			{
				return cueList.AsReadOnly();
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public void update(GameTime gameTime)
		{
			// TODO : なんかガーベージ乱発の予感がする。パフォーマンスモニタで要確認
			for (int i = cueList.Count; --i >= 0; )
			{
				Cue cue = cueList[i];
				if (cue.IsStopped)
				{
					cue.Dispose();
					cueList.RemoveAt(i);
				}
			}
			for (int i = reservedList.Count; --i >= 0; )
			{
				string name = reservedList[i];
				Cue cue = find(name);
				if (cue == null || cue.GetVariable("AttackTime") >= loopInterval)
				{
					Cue newCue = soundBank.GetCue(name);
					newCue.Play();
					cueList.Add(newCue);
					reservedList.RemoveAt(i);
				}
			}
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
			reservedList.Clear();
			for (int i = cueList.Count; --i >= 0; )
			{
				cueList[i].Dispose();
			}
			cueList.Clear();
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
		/// <summary>音声再生の予約をします。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		public void play(string name)
		{
			if (reservedList.Find(s => s == name) != null)
			{
				reservedList.Add(name);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声を一時停止します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>一時停止した場合、<c>true</c>。</returns>
		public bool pause(string name)
		{
			return command(name, c => c.Pause());
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声を再開します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>再開した場合、<c>true</c>。</returns>
		public bool resume(string name)
		{
			return command(name, c => c.Resume());
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声を再開します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>再開した場合、<c>true</c>。</returns>
		public bool stop(string name)
		{
			return command(name, c => c.Stop(AudioStopOptions.AsAuthored));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声が登録されている場合、そのキューを取得します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>キュー。</returns>
		public Cue find(string name)
		{
			return cueList.Find(c => c.Name == name);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在登録されている音声に対し、任意の命令を発行します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <param name="action">命令。</param>
		/// <returns>命令を実行できた場合、<c>true</c>。</returns>
		public bool command(string name, Action<Cue> action)
		{
			Cue cue = find(name);
			bool result = cue != null;
			if (result)
			{
				action(cue);
			}
			return result;
		}
	}
}
