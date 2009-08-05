////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2009 danmaq all rights reserved.
//
//		──FPS計測・計算タスク
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.Nineball.core.manager;

namespace danmaq.Nineball.task {

	class CTaskFPSCalculator : ITask {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>フェーズ・カウンタ管理クラス</summary>
		private readonly CPhaseManager PHASE_MANAGER = new CPhaseManager();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>前回計測時の稼働時間(秒)</summary>
		private int prevSeconds = 0;

		/// <summary>FPS実測値</summary>
		private int m_nFPS = 0;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		public CTaskManager manager {
			set { }
		}

		// ! TODO : 作りかけ！
		public byte layer {
			get { throw new System.NotImplementedException(); }
		}

		public bool isAvailablePause {
			get { throw new System.NotImplementedException(); }
		}

		public void initialize() {
			throw new System.NotImplementedException();
		}

		public bool update( Microsoft.Xna.Framework.GameTime gameTime ) {
			throw new System.NotImplementedException();
		}

		public void draw( Microsoft.Xna.Framework.GameTime gameTime, danmaq.Nineball.core.raw.CSprite sprite ) {
			throw new System.NotImplementedException();
		}

		public void Dispose() {
			throw new System.NotImplementedException();
		}
	}
}
