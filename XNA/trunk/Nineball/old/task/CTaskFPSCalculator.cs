////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.data.phase;
using danmaq.nineball.old.core.manager;
using danmaq.nineball.old.core.raw;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.old.task
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>FPS計測・計算クラス。</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete("このクラスは今後サポートされません。CStateFPSCalculatorを使用してください。")]
	public class CTaskFPSCalculator : ITask
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>FPS保持データ。</summary>
		public struct SFPSData
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>フェーズ・カウンタ管理クラス</summary>
			public SPhase m_phaseManager;

			/// <summary>前回計測時の稼働時間(秒)</summary>
			public int m_prevSeconds;

			/// <summary>FPS実測値</summary>
			public int m_fps;

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>FPS計測のためのデータを収集します。</summary>
			/// <remarks>
			/// 1秒ごとに収集データをもとに自動的にFPSが算出されます。
			/// このメソッドを毎フレーム呼び出してください。
			/// </remarks>
			/// 
			/// <param name="gameTime">前フレームからの経過時間</param>
			public void update(GameTime gameTime)
			{
				m_phaseManager.count++;
				int nNowSeconds = gameTime.TotalRealTime.Seconds;
				if(m_prevSeconds != nNowSeconds)
				{
					m_prevSeconds = nNowSeconds;
					m_fps = m_phaseManager.countPhase;
					m_phaseManager.phase++;
				}
			}
		}

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>前回計測時の稼働時間(秒)</summary>
		private SFPSData m_dataUpdate = new SFPSData();

		/// <summary>FPS実測値</summary>
		private SFPSData m_dataDraw = new SFPSData();

		/// <summary>所属レイヤ番号。</summary>
		private byte m_byLayer = 0;

		/// <summary>一時停止に対応しているかどうか。</summary>
		private bool m_bAvailablePause = false;

		/// <summary>所属レイヤが変更可能かどうか。</summary>
		private bool m_bLockLayer = false;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>タスク管理クラス オブジェクト。</summary>
		/// <remarks>このクラスでは不要なので何もしません。</remarks>
		public CTaskManager manager
		{
			set
			{
			}
		}

		/// <summary>所属レイヤ番号。</summary>
		/// <remarks>
		/// ロックされていない場合、代入することで変更出来ます。
		/// (ロックされている場合、無視されます)
		/// </remarks>
		public byte layer
		{
			get
			{
				return m_byLayer;
			}
			set
			{
				if(!isLockedLayer)
				{
					m_byLayer = value;
				}
			}
		}

		/// <summary>一時停止に対応しているかどうか。</summary>
		public bool isAvailablePause
		{
			get
			{
				return m_bAvailablePause;
			}
			set
			{
				m_bAvailablePause = value;
			}
		}

		/// <summary>所属レイヤ番号がロックされているかどうか。</summary>
		public bool isLockedLayer
		{
			get
			{
				return m_bLockLayer;
			}
			private set
			{
				m_bLockLayer = m_bLockLayer || value;
			}
		}

		/// <summary>更新処理のFPS。</summary>
		public int fpsUpdate
		{
			get
			{
				return m_dataUpdate.m_fps;
			}
		}

		/// <summary>描画処理のFPS。</summary>
		public int fpsDraw
		{
			get
			{
				return m_dataDraw.m_fps;
			}
		}

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CTaskFPSCalculator()
		{
			m_dataUpdate.m_phaseManager = SPhase.initialized;
			m_dataDraw.m_phaseManager = SPhase.initialized;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>タスクが管理クラスに登録された直後に、1度だけ自動的に呼ばれます。</summary>
		public void initialize()
		{
			isLockedLayer = true;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>タスク終了時の処理です。</summary>
		public void Dispose()
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理をします。</summary>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		/// <returns>無条件に<c>true</c></returns>
		public bool update(GameTime gameTime)
		{
			m_dataUpdate.update(gameTime);
			return true;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理をします。</summary>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		/// <param name="sprite">スプライト描画管理クラス</param>
		public void draw(GameTime gameTime, CSprite sprite)
		{
			m_dataDraw.update(gameTime);
		}
	}
}
