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
using danmaq.nineball.state.audio;
using Microsoft.Xna.Framework.Audio;

namespace danmaq.nineball.entity.audio
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XACT音声制御・管理クラス。</summary>
	public sealed class CAudio
		: CEntity
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers : IDisposable
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>オーディオ キューの一覧。</summary>
			public readonly List<Cue> cueList = new List<Cue>();

			/// <summary>オーディオ キュー予約の一覧。</summary>
			public readonly List<string> reservedList = new List<string>();

			/// <summary>オーディオ エンジン更新のためのデリゲート。</summary>
			public readonly Action engineUpdate;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="engineUpdate">
			/// オーディオ エンジン更新のためのデリゲート。
			/// </param>
			public CPrivateMembers(Action engineUpdate)
			{
				this.engineUpdate = engineUpdate;
			}

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

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
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>オーディオ エンジン。</summary>
		public readonly AudioEngine engine;

		/// <summary>サウンド バンク。</summary>
		public readonly SoundBank soundBank;

		/// <summary>波形バンク。</summary>
		public readonly WaveBank waveBank;

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _privateMembers;

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
		/// <param name="xwb">XACT波形バンク ファイル名。</param>
		public CAudio(CAudio audio, string xwb)
			: this(xwb, () => engine.Update())
		{
			engine = audio.engine;
			soundBank = audio.soundBank;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="xgs">XACTサウンドエンジン ファイル名。</param>
		/// <param name="xsb">XACT再生キュー ファイル名。</param>
		/// <param name="xwb">XACT波形バンク ファイル名。</param>
		public CAudio(string xgs, string xsb, string xwb)
			: this(xwb, () => engine.Update())
		{
			engine = new AudioEngine(xgs);
			soundBank = new SoundBank(engine, xsb);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="xwb">XACT波形バンク ファイル名。</param>
		/// <param name="engineUpdate">オーディオ エンジン更新のためのデリゲート。</param>
		private CAudio(string xwb, Action engineUpdate)
			: base(CStateAudio.instance, new CPrivateMembers(engineUpdate))
		{
			_privateMembers = (CPrivateMembers)privateMembers;
			waveBank = new WaveBank(engine, xwb);
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
				return _privateMembers.cueList.AsReadOnly();
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			_privateMembers.Dispose();
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
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声再生の予約をします。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		public void play(string name)
		{
			if (_privateMembers.reservedList.Find(s => s == name) != null)
			{
				_privateMembers.reservedList.Add(name);
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
			return _privateMembers.cueList.Find(c => c.Name == name);
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
