////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.data;
using danmaq.nineball.entity;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.state.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>FPS計測・計算クラス。</summary>
	public sealed class CStateFPSCalculator : CState
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>FPS保持データ。</summary>
		public struct SFPSData
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>フェーズ・カウンタ管理クラス</summary>
			public CPhase m_phaseManager;

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

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateFPSCalculator instance =
			new CStateFPSCalculator();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>前回計測時の更新稼働時間(秒)</summary>
		private SFPSData m_dataUpdate = new SFPSData();

		/// <summary>前回計測時の描画稼働時間(秒)</summary>
		private SFPSData m_dataDraw = new SFPSData();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateFPSCalculator()
		{
			m_dataUpdate.m_phaseManager = new CPhase();
			m_dataDraw.m_phaseManager = new CPhase();
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>更新処理のFPSを取得します。</summary>
		/// 
		/// <value>更新処理のFPS。</value>
		public int fpsUpdate
		{
			get
			{
				return m_dataUpdate.m_fps;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画処理のFPSを取得します。</summary>
		/// 
		/// <value>描画処理のFPS。</value>
		public int fpsDraw
		{
			get
			{
				return m_dataDraw.m_fps;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(IEntity entity, object privateMembers, GameTime gameTime)
		{
			m_dataUpdate.update(gameTime);
			base.update(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(IEntity entity, object privateMembers, GameTime gameTime)
		{
			m_dataDraw.update(gameTime);
			base.draw(entity, privateMembers, gameTime);
		}
	}
}
