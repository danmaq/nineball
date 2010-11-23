////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Audio;

namespace danmaq.nineball.entity.audio
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>空のXACT音声制御・管理クラス。</summary>
	public sealed class CEmptyAudio
		: CEntity, IAudio
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>オーディオ キューの一覧。</summary>
		private ReadOnlyCollection<Cue> m_cue = new ReadOnlyCollection<Cue>(new Cue[0]);

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
				return m_cue;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>音声再生の予約をします。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		public void play(string name)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声を一時停止します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>一時停止した場合、<c>true</c>。</returns>
		public bool pause(string name)
		{
			return false;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声を再開します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>再開した場合、<c>true</c>。</returns>
		public bool resume(string name)
		{
			return false;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声を停止します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>再開した場合、<c>true</c>。</returns>
		public bool stop(string name)
		{
			return false;
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
			return false;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声を全て停止します。</summary>
		public void stop()
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声を全て停止します。</summary>
		/// 
		/// <param name="options">
		/// どのようにサウンドを停止するかを指定する列挙値。
		/// </param>
		public void stop(AudioStopOptions options)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音声が登録されている場合、そのキューを取得します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>キュー。</returns>
		public Cue find(string name)
		{
			return null;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在登録されている音声に対し、任意の命令を発行します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <param name="action">命令。</param>
		/// <returns>命令を実行できた場合、<c>true</c>。</returns>
		public bool command(string name, Action<Cue> action)
		{
			return false;
		}
	}
}
