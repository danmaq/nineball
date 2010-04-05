////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.data.phase;
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
		private struct SFPSData
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>初期化済み構造体。</summary>
			public static readonly SFPSData initializedData;

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>前回計測時の稼働時間(秒)。</summary>
			private int m_prevSeconds;

			/// <summary>フェーズ・カウンタ管理クラス。</summary>
			private SPhase m_mgrPhase;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>静的なコンストラクタ。</summary>
			static SFPSData()
			{
				initializedData = new SFPSData();
				initializedData.m_mgrPhase = SPhase.initialized;
			}

			//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* properties ──────────────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>実測FPSを取得します。</summary>
			/// 
			/// <value>FPS実測値。</value>
			public int fps
			{
				get;
				private set;
			}

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
				m_mgrPhase.count++;
				int nNowSeconds = gameTime.TotalRealTime.Seconds;
				if(m_prevSeconds != nNowSeconds)
				{
					m_prevSeconds = nNowSeconds;
					fps = m_mgrPhase.countPhase;
					m_mgrPhase.phase++;
				}
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateFPSCalculator instance =
			new CStateFPSCalculator();

		/// <summary>前回計測時の更新稼働時間(秒)</summary>
		private readonly SFPSData dataUpdate = SFPSData.initializedData;

		/// <summary>前回計測時の描画稼働時間(秒)</summary>
		private readonly SFPSData dataDraw = SFPSData.initializedData;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateFPSCalculator()
		{
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
				return dataUpdate.fps;
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
				return dataDraw.fps;
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
			dataUpdate.update(gameTime);
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
			dataDraw.update(gameTime);
			base.draw(entity, privateMembers, gameTime);
		}
	}
}
