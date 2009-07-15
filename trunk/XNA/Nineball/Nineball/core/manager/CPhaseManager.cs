////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library / Copyright (c) 2008 danmaq all rights reserved.
//		──フェーズ進行管理を司るクラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

namespace danmaq.Nineball {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>フェーズ進行・カウンタ進行の管理をするクラス。</summary>
	/// <remarks>
	/// <c>count++</c>を毎フレーム呼ぶことでフレームカウンタとして使うと便利です。
	/// </remarks>
	public sealed class CPhaseManager {

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>現在のフェーズ値</summary>
		private int m_nPhase;

		/// <summary>現在のカウント値</summary>
		private int m_nCount;

		/// <summary>現在のフェーズが開始された時のカウント値</summary>
		private int m_nPhaseStartTime;

		/// <summary>カウント変化時に進むフェーズ</summary>
		private int m_nNextPhase;

		/// <summary>前回のフェーズ値</summary>
		private int m_nPrevPhase;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// <remarks>内部データのリセットを行います。</remarks>
		public CPhaseManager() { reset(); }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>現在のフェーズ値</summary>
		public int phase {
			get { return m_nPhase; }
			set {
				prevPhase = m_nPhase;
				m_nPhase = value;
				phaseStartTime = m_nCount;
			}
		}

		/// <summary>現在のカウント値</summary>
		/// <remarks>代入すると値の如何に問わずインクリメントします。</remarks>
		public int count {
			get { return m_nCount; }
			set {
				m_nCount++; // !!! XXX : m_nCount += valueできないか検証する
				if( reserveNextPhase ) {
					phase = nextPhase;
					reserveNextPhase = false;
				}
			}
		}

		/// <summary>現在のフェーズが開始された時のカウント値</summary>
		public int phaseStartTime {
			get { return m_nPhaseStartTime; }
			private set { m_nPhaseStartTime = value; }
		}

		/// <summary>現在のフェーズ内のカウント値</summary>
		public int countPhase {
			get { return count - phaseStartTime; }
		}

		/// <summary>カウント変化時にフェーズを進めるかどうか</summary>
		public bool reserveNextPhase {
			get { return ( nextPhase >= 0 ); }
			set { nextPhase = ( value ? phase + 1 : -1 ); }
		}

		/// <summary>カウント変化時に進むフェーズ</summary>
		public int nextPhase {
			get { return m_nNextPhase; }
			set { m_nNextPhase = value; }
		}

		/// <summary>前回のフェーズ値</summary>
		public int prevPhase {
			get { return m_nPrevPhase; }
			private set { m_nPrevPhase = value; }
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>現在のフェーズ値を取得します。</summary>
		/// 
		/// <param name="p">フェーズ・カウンタ管理クラス</param>
		/// <returns>現在のフェーズ値</returns>
		public static implicit operator int( CPhaseManager p ) { return p.phase; }

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// フェーズやカウントなど、内部データのリセットをします。
		/// </summary>
		public void reset() {
			m_nCount = 0;
			phase = 0;
			reserveNextPhase = false;
			nextPhase = -1;
		}
	}
}
