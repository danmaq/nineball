////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace danmaq.nineball.entity.audio
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XACT背景音響制御・管理クラス。</summary>
	public sealed class CBGM
		: CAudioBase
	{

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="audio">XACT音響制御の基底クラス。</param>
		/// <param name="xwb">XACT波形バンク(BGM) ファイル名。</param>
		public CBGM(CAudioBase audio, string xwb)
			: base(audio, xwb)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="xgs">XACTサウンドエンジン ファイル名。</param>
		/// <param name="xsb">XACT再生キュー ファイル名。</param>
		/// <param name="xwb">XACT波形バンク(BGM) ファイル名。</param>
		public CBGM(string xgs, string xsb, string xwb)
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
			for (int i = cueList.Count; --i >= 0; )
			{
				Cue cue = cueList[i];
				if (cue.IsStopped)
				{
					cue.Dispose();
					cueList.RemoveAt(i);
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>BGMを再生します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <param name="allowMultiple">多重再生を許可するかどうか。</param>
		/// <returns>再生した場合、<c>true</c>。</returns>
		public bool play(string name, bool allowMultiple)
		{
			bool result = !command(name, c => {});
			if (result)
			{
				Cue cue = soundBank.GetCue(name);
				cue.Play();
				cueList.Add(cue);
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>BGMを一時停止します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>一時停止した場合、<c>true</c>。</returns>
		public bool pause(string name)
		{
			return command(name, c => c.Pause());
		}

		//* -----------------------------------------------------------------------*
		/// <summary>BGMを再開します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>再開した場合、<c>true</c>。</returns>
		public bool resume(string name)
		{
			return command(name, c => c.Resume());
		}

		//* -----------------------------------------------------------------------*
		/// <summary>BGMを再開します。</summary>
		/// 
		/// <param name="name">フレンドリ名。</param>
		/// <returns>再開した場合、<c>true</c>。</returns>
		public bool stop(string name)
		{
			return command(name, c => c.Stop(AudioStopOptions.AsAuthored));
		}
	}
}
