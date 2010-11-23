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
		: CEntity, IAudio
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

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>オーディオ エンジン更新のためのデリゲート。</summary>
			public Action engineUpdate = () =>
			{
			};

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

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _privateMembers;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>ループ再生に必要な時間経過(ms単位)。</summary>
		public int loopInterval = 0;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>
		/// ストリーミング波形バンクを使用するためには、<paramref name="offset"/>と
		/// <paramref name="packetSize"/>を設定する必要があります。
		/// </para>
		/// </summary>
		/// <remarks>
		/// このコンストラクタで初期化されたインスタンスは、
		/// オーディオ エンジンの更新を実行しません。
		/// </remarks>
		/// 
		/// <param name="audio">XACT音響制御の基底クラス。</param>
		/// <param name="xwb">XACT波形バンク ファイル名。</param>
		/// <param name="offset">
		/// Wave バンク データ ファイル内のオフセット。
		/// このオフセットは DVD セクター単位で揃える必要があります。
		/// </param>
		/// <param name="packetSize">
		/// 各ストリームで使用されるストリームのパケット サイズ (セクター単位)。
		/// 最小値は 2 です。
		/// </param>
		public CAudio(IAudio audio, string xwb, int? offset, short? packetSize)
			: this()
		{
			engine = audio.engine;
			if (offset.HasValue && packetSize.HasValue)
			{
				waveBank = new WaveBank(engine, xwb, offset.Value, packetSize.Value);
			}
			else
			{
				waveBank = new WaveBank(engine, xwb);
			}
			soundBank = audio.soundBank;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>
		/// ストリーミング波形バンクを使用するためには、<paramref name="offset"/>と
		/// <paramref name="packetSize"/>を設定する必要があります。
		/// </para>
		/// </summary>
		/// 
		/// <param name="xgs">XACTサウンドエンジン ファイル名。</param>
		/// <param name="xsb">XACT再生キュー ファイル名。</param>
		/// <param name="xwb">XACT波形バンク ファイル名。</param>
		/// <param name="offset">
		/// Wave バンク データ ファイル内のオフセット。
		/// このオフセットは DVD セクター単位で揃える必要があります。
		/// </param>
		/// <param name="packetSize">
		/// 各ストリームで使用されるストリームのパケット サイズ (セクター単位)。
		/// 最小値は 2 です。
		/// </param>
		public CAudio(string xgs, string xsb, string xwb, int? offset, short? packetSize)
			: this()
		{
			engine = new AudioEngine(xgs);
			if (offset.HasValue && packetSize.HasValue)
			{
				waveBank = new WaveBank(engine, xwb, offset.Value, packetSize.Value);
			}
			else
			{
				waveBank = new WaveBank(engine, xwb);
			}
			soundBank = new SoundBank(engine, xsb);
			_privateMembers.engineUpdate = engineUpdate;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CAudio()
			: base(CStateAudio.instance, new CPrivateMembers())
		{
			_privateMembers = (CPrivateMembers)privateMembers;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>オーディオ エンジンを取得します。</summary>
		/// 
		/// <value>オーディオ エンジン。</value>
		public AudioEngine engine
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>サウンド バンクを取得します。</summary>
		/// 
		/// <value>サウンド バンク。</value>
		public SoundBank soundBank
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>波形バンクを取得します。</summary>
		/// 
		/// <value>波形バンク。</value>
		public WaveBank waveBank
		{
			get;
			private set;
		}

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
			if (_privateMembers.reservedList.Find(s => s == name) == null)
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
		/// <summary>音声を停止します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>再開した場合、<c>true</c>。</returns>
		public bool stop(string name)
		{
			return stop(name, AudioStopOptions.AsAuthored);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声を停止します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <param name="options">
		/// どのようにサウンドを停止するかを指定する列挙値。
		/// </param>
		/// <returns>再開した場合、<c>true</c>。</returns>
		public bool stop(string name, AudioStopOptions options)
		{
			return command(name, c => c.Stop(options));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声を全て停止します。</summary>
		public void stop()
		{
			stop(AudioStopOptions.AsAuthored);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声を全て停止します。</summary>
		/// 
		/// <param name="options">
		/// どのようにサウンドを停止するかを指定する列挙値。
		/// </param>
		public void stop(AudioStopOptions options)
		{
			List<Cue> cueList = _privateMembers.cueList;
			for (int i = cueList.Count; --i >= 0; )
			{
				cueList[i].Stop(options);
			}
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

		//* -----------------------------------------------------------------------*
		/// <summary>オーディオ エンジンを更新します。</summary>
		private void engineUpdate()
		{
			engine.Update();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>何もしない関数です。</summary>
		private void noop()
		{
		}
	}
}
