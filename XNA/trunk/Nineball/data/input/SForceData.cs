////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.nineball.data.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>フォース フィードバックのための情報。</summary>
	public struct SForceData
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>大モーター用の力の強さ。</summary>
		public SGradation strengthL;

		/// <summary>小モーター用の力の強さ。</summary>
		public SGradation strengthS;

		/// <summary>大モーター用の持続するフレーム時間。</summary>
		private int m_durationL;

		/// <summary>小モーター用の持続するフレーム時間。</summary>
		private int m_durationS;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="strengthL">大モーター用の力の強さ。</param>
		/// <param name="strengthS">小モーター用の力の強さ。</param>
		/// <param name="durationL">大モーター用の持続するフレーム時間。</param>
		/// <param name="durationS">小モーター用の持続するフレーム時間。</param>
		public SForceData(SGradation strengthL, SGradation strengthS, int durationL, int durationS)
			: this()
		{
			this.strengthL = strengthL;
			this.strengthS = strengthS;
			this.durationL = durationL;
			this.durationS = durationS;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>持続する総フレーム時間を取得/設定します。</summary>
		/// 
		/// <value>持続する総フレーム時間。</value>
		public int duration
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>フォースが常にゼロになるかどうかを取得します。</summary>
		/// 
		/// <value>常にゼロである場合、<c>true</c>。</value>
		public bool zero
		{
			get
			{
				return strengthL.zero && strengthS.zero;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>大モーター用の持続するフレーム時間を取得/設定します。</summary>
		/// 
		/// <value>大モーター用の持続するフレーム時間。</value>
		public int durationL
		{
			get
			{
				return m_durationL;
			}
			set
			{
				m_durationL = value;
				setDuration();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>小モーター用の持続するフレーム時間を取得/設定します。</summary>
		/// 
		/// <value>小モーター用の持続するフレーム時間。</value>
		public int durationS
		{
			get
			{
				return m_durationS;
			}
			set
			{
				m_durationS = value;
				setDuration();
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>持続する総フレーム時間を計算します。</summary>
		private void setDuration()
		{
			duration = Math.Max(durationL, durationS);
		}
	}
}
