////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.data.phase;
using danmaq.nineball.old.core.manager;
using danmaq.nineball.util;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.old.scene
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>シーン本体の基底クラスです。</para>
	/// <para>
	/// シーン管理クラスCSceneManagerに登録するタスクを作成するためには、
	/// このクラスを継承するか、ISceneを実装します。
	/// </para>
	/// </summary>
	public abstract class CSceneBase : IScene
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>フェーズ・カウンタ管理クラス オブジェクト。</summary>
		public readonly CPhase phaseManager = new CPhase();

		/// <summary>タスク管理クラス オブジェクト。</summary>
		protected readonly CTaskManager taskManager = new CTaskManager();

		/// <summary>コルーチン管理クラス オブジェクト。</summary>
		protected readonly CCoRoutineManager coRoutineManager = new CCoRoutineManager();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>次に移行するシーン オブジェクト。</summary>
		/// <remarks>
		/// ここに次のシーン オブジェクトを代入することで
		/// 次のフレームからそのシーンが呼ばれるようになります。
		/// </remarks>
		private IScene m_nextScene = null;

		/// <summary>前フレームからの経過時間。</summary>
		private GameTime m_gameTime = null;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>次に移行するシーン オブジェクト</summary>
		/// <remarks>
		/// ここに次のシーン オブジェクトを代入することで
		/// 次のフレームからそのシーンが呼ばれるようになります。
		/// </remarks>
		public IScene nextScene
		{
			get
			{
				return m_nextScene;
			}
			set
			{
				m_nextScene = value;
			}
		}

		/// <summary>前フレームからの経過時間。</summary>
		protected GameTime gameTime
		{
			get
			{
				return m_gameTime;
			}
			private set
			{
				m_gameTime = value;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>継承先でシーン終了時の処理を記述します。</summary>
		public abstract void Dispose();

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理をします。</summary>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		/// <returns>
		/// 現在のフレームでこのシーンが終了しない場合、<c>true</c>
		/// </returns>
		public bool update(GameTime gameTime)
		{
			this.gameTime = gameTime;
			bool bContinue = coRoutineManager.update();
			taskManager.update(gameTime);
			phaseManager.count++;
			return bContinue;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>継承先で1フレーム分の描画処理を記述します。</summary>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		/// <param name="sprite">スプライト描画管理クラス</param>
		public abstract void draw(GameTime gameTime, CSprite sprite);
	}
}
