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
using danmaq.nineball.data.phase;
using danmaq.nineball.old.core.manager;
using danmaq.nineball.util;
using Microsoft.Xna.Framework.Audio;

namespace danmaq.nineball.old.core.raw
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>音響制御・管理クラス。</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	public sealed class CAudio : IDisposable
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>効果音キュー管理のためのクラス。</summary>
		private sealed class CSE
		{

			/// <summary>効果音ファイルのキュー。</summary>
			public Cue cue = null;

			/// <summary>効果音ファイルのキューが登録された時間。</summary>
			public int time = 0;

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			public CSE(Cue __cue, int __time)
			{
				cue = __cue;
				time = __time;
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>効果音を繰り返し再生する際の間隔。</summary>
		public readonly ushort LOOPSE_INTERVAL;

		/// <summary>
		/// インデックスをアセット名に変換するためのコールバック用デリゲート。
		/// </summary>
		private readonly Converter<ushort, string> INDEX2ASERT;

		/// <summary>効果音再生予約一覧。</summary>
		private readonly LinkedList<ushort> RESERVED_SE = new LinkedList<ushort>();

		/// <summary>効果音再生中一覧。</summary>
		private readonly Dictionary<ushort, CSE> CUE_SE = new Dictionary<ushort, CSE>();

		/// <summary>効果音の墓場(GC防止用)</summary>
		private readonly Queue<Cue> GRAVE_SE = new Queue<Cue>();

		/// <summary>マイクロスレッド管理 クラス。</summary>
		private readonly CCoRoutineManager MICROTHREAD_MANAGER = new CCoRoutineManager();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>効果音処理の優先度を上げるかどうか</summary>
		public bool isHighPriorityMode = true;

		/// <summary>オーディオ エンジン。</summary>
		private AudioEngine engine = null;

		/// <summary>キューのコレクションであるサウンド バンク。</summary>
		private SoundBank soundBank = null;

		/// <summary>効果音ファイルのコレクションであるWaveバンク。</summary>
		private WaveBank waveBankSE = null;

		/// <summary>BGMファイルのコレクションであるWaveバンク。</summary>
		private WaveBank waveBankBGM = null;

		/// <summary>BGMファイルのキュー。</summary>
		private Cue cueBGM = null;

		/// <summary>フェーズ・カウント管理クラス。</summary>
		private SPhase phaseManager = SPhase.initialized;

		/// <summary>BGMのインデックス。</summary>
		private ushort m_indexBGM = ushort.MaxValue;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="index2assert">インデックスをアセット名に変換するためのコールバック用デリゲート。</param>
		/// <param name="loopSEInterval">効果音を繰り返し再生する際の間隔。</param>
		/// <param name="fileXGS">XACTサウンドエンジン ファイル名。</param>
		/// <param name="fileXSB">XACT再生キュー ファイル名。</param>
		/// <param name="fileXWBSE">XACT波形バンク(効果音) ファイル名。</param>
		/// <param name="fileXWBBGM">XACT波形バンク(BGM) ファイル名。</param>
		public CAudio(
			Converter<ushort, string> index2assert, ushort loopSEInterval,
			string fileXGS, string fileXSB, string fileXWBSE, string fileXWBBGM
		)
		{
			CLogger.add("音響処理の初期化をしています...");
			LOOPSE_INTERVAL = loopSEInterval;
			try
			{
				INDEX2ASERT = index2assert;
				engine = new AudioEngine(fileXGS);
				waveBankSE = new WaveBank(engine, fileXWBSE);
				waveBankBGM = new WaveBank(engine, fileXWBBGM, 0, 32767);
				soundBank = new SoundBank(engine, fileXSB);
				MICROTHREAD_MANAGER.add(threadPlaySE());
				MICROTHREAD_MANAGER.add(threadGC());
			}
			catch(Exception e)
			{
				Dispose();
				CLogger.add(
					"音響処理の初期化に失敗しましたので、サウンドを切り離します。" + Environment.NewLine +
					e.ToString());
				CLogger.add(
					"Microsoft .NET Framework 1.1 がインストールされていない可能性があります。" + Environment.NewLine +
					"このゲームを実行するためにはMicrosoft .NET Framework 2.0 SP1または" + Environment.NewLine +
					"3.5 以降が必要ですが、音響処理の実行には 1.1 も別途必要となります。" + Environment.NewLine +
					"このランタイムは、下記のWebサイトで入手することが出来ます。" + Environment.NewLine + Environment.NewLine +
					"(日本語) http://www.microsoft.com/japan/msdn/netframework/" + Environment.NewLine + Environment.NewLine +
					"あるいは、最新のDirectXをインストールされていない可能性があります。" + Environment.NewLine +
					"このランタイムは、下記のWebサイトで入手することが出来ます。" + Environment.NewLine + Environment.NewLine +
					"(日本語) http://www.microsoft.com/japan/windows/DirectX/");
			}
			CLogger.add("音響処理の初期化完了。");
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>効果音処理の優先度を上げるかどうか</summary>
		/// <summary>BGMのインデックス。</summary>
		/// <remarks>これに定数を設定することでBGMを切り替えます。</remarks>
		public ushort bgm
		{
			get
			{
				return m_indexBGM;
			}
			set
			{
				if(!(soundBank == null || m_indexBGM == value))
				{
					if(cueBGM != null)
					{
						cueBGM.Dispose();
						cueBGM = null;
					}
					m_indexBGM = value;
					if(value != ushort.MaxValue)
					{
						cueBGM = soundBank.GetCue(INDEX2ASERT(value));
						cueBGM.Play();
					}
				}
			}
		}

		/// <summary>効果音のインデックス。</summary>
		/// <remarks>これに定数を設定することで効果音を再生します。</remarks>
		public ushort se
		{
			set
			{
				if(value != ushort.MaxValue && RESERVED_SE.Find(value) == null)
				{
					RESERVED_SE.AddLast(value);
				}
			}
		}

		/// <summary>BGMの音量。</summary>
		/// <remarks>0.0(-96dB)～2.0(+6dB)を設定します。</remarks>
		public float volumeBGM
		{
			set
			{
				if(engine != null)
				{
					engine.GetCategory("Music").SetVolume(value);
				}
			}
		}

		/// <summary>BGMの音量。</summary>
		/// <remarks>0.0(-96dB)～2.0(+6dB)を設定します。</remarks>
		public float volumeSE
		{
			set
			{
				if(engine != null)
				{
					engine.GetCategory("Default").SetVolume(value);
				}
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// アンマネージ リソースの解放およびリセットに関連付けられている
		/// アプリケーション定義のタスクを実行します。
		/// </summary>
		public void Dispose()
		{
			CLogger.add("音響リソースの解放をしています...");
			string strErr = string.Empty;
			try
			{
				if(cueBGM != null)
				{
					cueBGM.Dispose();
				}
			}
			catch(Exception e)
			{
				strErr += e.ToString() + "\r\n";
			}
			try
			{
				if(soundBank != null)
				{
					soundBank.Dispose();
				}
			}
			catch(Exception e)
			{
				strErr += e.ToString() + "\r\n";
			}
			try
			{
				if(waveBankBGM != null)
				{
					waveBankBGM.Dispose();
				}
			}
			catch(Exception e)
			{
				strErr += e.ToString() + "\r\n";
			}
			try
			{
				if(waveBankSE != null)
				{
					waveBankSE.Dispose();
				}
			}
			catch(Exception e)
			{
				strErr += e.ToString() + "\r\n";
			}
			try
			{
				if(engine != null)
				{
					engine.Dispose();
				}
			}
			catch(Exception e)
			{
				strErr += e.ToString() + "\r\n";
			}
			if(strErr.Length > 0)
			{
				CLogger.add(
					"音響処理の解放に失敗しました。" + Environment.NewLine +
					"完全に終了しないうちにウィンドウを非アクティブにするとこの現象が起きやすいです。" +
					Environment.NewLine + Environment.NewLine + strErr);
			}
			soundBank = null;
			cueBGM = null;
			waveBankBGM = null;
			waveBankSE = null;
			engine = null;
			CLogger.add("音響リソースの解放完了。");
		}

		//* -----------------------------------------------------------------------*
		/// <summary>オーディオ エンジンが要求する周期的処理を実行します。</summary>
		/// <remarks>予約されている効果音の一斉再生も同時に行います。</remarks>
		public void update()
		{
			if(engine != null)
			{
				engine.Update();
			}
			MICROTHREAD_MANAGER.update();
			phaseManager.count++;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>死んだ効果音を一斉破棄します。</summary>
		/// 
		/// <returns>破棄した件数</returns>
		public int GC()
		{
			int nResult = GRAVE_SE.Count;
			foreach(Cue cue in GRAVE_SE)
			{
				if(cue != null)
				{
					cue.Stop(AudioStopOptions.AsAuthored);
					cue.Dispose();
				}
			}
			GRAVE_SE.Clear();
			return nResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>効果音再生のスレッドです。</summary>
		/// 
		/// <returns>スレッドが実行される間、<c>true</c></returns>
		private IEnumerator<object> threadPlaySE()
		{
			while(!(engine == null || soundBank == null))
			{
				for(int i = 0; i < (isHighPriorityMode ? 2 : 5); i++)
				{
					yield return null;
				}
				int nCount = phaseManager.count;
				foreach(ushort index in RESERVED_SE)
				{
					CSE cueSE;
					bool bExist = CUE_SE.TryGetValue(index, out cueSE);
					if(bExist)
					{
						if(cueSE.time < nCount - 10)
						{
							if(cueSE.cue.IsPlaying)
							{
								cueSE.cue.Pause();
							}
							GRAVE_SE.Enqueue(cueSE.cue);
							cueSE.cue = soundBank.GetCue(INDEX2ASERT(index));
							cueSE.time = nCount;
							cueSE.cue.Play();
						}
					}
					else
					{
						cueSE = new CSE(soundBank.GetCue(INDEX2ASERT(index)), nCount);
						cueSE.cue.Play();
						CUE_SE.Add(index, cueSE);
					}
				}
				RESERVED_SE.Clear();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>死んだ効果音をゆっくり破棄するスレッドです。</summary>
		/// 
		/// <returns>スレッドが実行される間、<c>true</c></returns>
		private IEnumerator<object> threadGC()
		{
			while(true)
			{
				for(int i = 0; i < (isHighPriorityMode ? 6 : 20); i++)
				{
					yield return null;
				}
				if(GRAVE_SE.Count > 0)
				{
					Cue cue = GRAVE_SE.Dequeue();
					if(cue != null)
					{
						cue.Stop(AudioStopOptions.AsAuthored);
						cue.Dispose();
						cue = null;
					}
				}
			}
		}
	}
}
