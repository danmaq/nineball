////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.nineball.data.phase
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>フェーズ進行・カウンタ進行の管理をする構造体。</summary>
	/// <remarks>
	/// <c>count++</c>を毎フレーム呼ぶことでフレームカウンタとして使うと便利です。
	/// </remarks>
	public struct SPhase
		: IDisposable
	{

		// TODO : phaseのスタック積みできないかなぁ
		// TODO : リードオンリ版対応

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>初期化された構造体。</summary>
		public static readonly SPhase initialized;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>カウント変化時に進むフェーズ</summary>
		public int nextPhase;

		/// <summary>現在のフェーズ値</summary>
		private int m_nPhase;

		/// <summary>現在のカウント値</summary>
		private int m_nCount;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static SPhase()
		{
			SPhase phase = new SPhase();
			phase.Dispose();
			initialized = phase;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>現在のフェーズ値を設定/取得します。</summary>
		/// 
		/// <value>現在のフェーズ値。</value>
		public int phase
		{
			get
			{
				return m_nPhase;
			}
			set
			{
				prevPhase = m_nPhase;
				m_nPhase = value;
				phaseStartTime = m_nCount;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在のカウント値を設定/取得します。</summary>
		/// 
		/// <value>現在のカウント値。</value>
		public int count
		{
			get
			{
				return m_nCount;
			}
			set
			{
				m_nCount = value;
				if(reserveNextPhase)
				{
					phase = nextPhase;
					reserveNextPhase = false;
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在のフェーズが開始された時のカウント値を取得します。</summary>
		/// 
		/// <value>現在のフェーズが開始された時のカウント値。</value>
		public int phaseStartTime
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在のフェーズ内のカウント値を取得します。</summary>
		/// 
		/// <value>現在のフェーズ内のカウント値。</value>
		public int countPhase
		{
			get
			{
				return count - phaseStartTime;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>カウント変化時にフェーズを進めるかどうかを設定/取得します。</summary>
		/// 
		/// <value>カウント変化時にフェーズを進める場合、<c>true</c>。</value>
		public bool reserveNextPhase
		{
			get
			{
				return (nextPhase >= 0);
			}
			set
			{
				nextPhase = (value ? phase + 1 : -1);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>前回のフェーズ値を取得します。</summary>
		/// 
		/// <value>前回のフェーズ値。</value>
		public int prevPhase
		{
			get;
			private set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>現在のフェーズ値を取得します。</summary>
		/// 
		/// <param name="p">フェーズ・カウンタ管理クラス</param>
		/// <returns>現在のフェーズ値</returns>
		public static implicit operator int(SPhase p)
		{
			return p.phase;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// フェーズやカウントなど、内部データのリセットをします。
		/// </summary>
		public void Dispose()
		{
			m_nCount = 0;
			phase = 0;
			reserveNextPhase = false;
			nextPhase = -1;
		}
	}
}
