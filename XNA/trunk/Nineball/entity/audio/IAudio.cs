////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Audio;

namespace danmaq.nineball.entity.audio
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XACT音声制御・管理クラスのインターフェイス。</summary>
	public interface IAudio
		: IEntity
	{

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>オーディオ エンジンを取得します。</summary>
		/// 
		/// <value>オーディオ エンジン。</value>
		AudioEngine engine
		{
			get;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>サウンド バンクを取得します。</summary>
		/// 
		/// <value>サウンド バンク。</value>
		SoundBank soundBank
		{
			get;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>波形バンクを取得します。</summary>
		/// 
		/// <value>波形バンク。</value>
		WaveBank waveBank
		{
			get;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>キュー一覧を取得します。</summary>
		/// 
		/// <value>キュー一覧。</value>
		ReadOnlyCollection<Cue> cues
		{
			get;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>音声再生の予約をします。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		void play(string name);

		//* -----------------------------------------------------------------------*
		/// <summary>音声を一時停止します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>一時停止した場合、<c>true</c>。</returns>
		bool pause(string name);

		//* -----------------------------------------------------------------------*
		/// <summary>音声を再開します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>再開した場合、<c>true</c>。</returns>
		bool resume(string name);

		//* -----------------------------------------------------------------------*
		/// <summary>音声を停止します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>再開した場合、<c>true</c>。</returns>
		bool stop(string name);

		//* -----------------------------------------------------------------------*
		/// <summary>音声を停止します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <param name="options">
		/// どのようにサウンドを停止するかを指定する列挙値。
		/// </param>
		/// <returns>再開した場合、<c>true</c>。</returns>
		bool stop(string name, AudioStopOptions options);

		//* -----------------------------------------------------------------------*
		/// <summary>音声を全て停止します。</summary>
		void stop();

		//* -----------------------------------------------------------------------*
		/// <summary>音声を全て停止します。</summary>
		/// 
		/// <param name="options">
		/// どのようにサウンドを停止するかを指定する列挙値。
		/// </param>
		void stop(AudioStopOptions options);

		//* -----------------------------------------------------------------------*
		/// <summary>音声が登録されている場合、そのキューを取得します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>キュー。</returns>
		Cue find(string name);

		//* -----------------------------------------------------------------------*
		/// <summary>現在登録されている音声に対し、任意の命令を発行します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <param name="action">命令。</param>
		/// <returns>命令を実行できた場合、<c>true</c>。</returns>
		bool command(string name, Action<Cue> action);
	}
}
