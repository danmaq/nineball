////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2009 danmaq all rights reserved.
//
//		──タスク本体の基底クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.Nineball.core.manager;
using danmaq.Nineball.core.raw;
using Microsoft.Xna.Framework;

namespace danmaq.Nineball.task {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>タスク本体のインターフェイスです。</para>
	/// <para>
	/// タスク管理クラスCTaskManagerに登録するタスクを作成するためには、
	/// このクラスを継承するか、ITaskを実装します。
	/// </para>
	/// </summary>
	public abstract class CTaskBase : ITask {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>フェーズ・カウンタ管理クラス オブジェクト。</summary>
		public readonly CPhaseManager phaseManager = new CPhaseManager();

		/// <summary>コルーチン管理クラス オブジェクト。</summary>
		protected readonly CCoRoutineManager coRoutineManager = new CCoRoutineManager();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>タスク管理クラス オブジェクト。</summary>
		private CTaskManager m_manager = null;

		/// <summary>所属レイヤ番号。</summary>
		private byte m_byLayer = 0;

		/// <summary>所属レイヤが変更可能かどうか。</summary>
		private bool m_bLockLayer = false;

		/// <summary>一時停止に対応しているかどうか。</summary>
		private bool m_bAvailablePause = true;

		/// <summary>前フレームからの経過時間。</summary>
		private GameTime m_gameTime = null;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>タスク管理クラス オブジェクト。</summary>
		public CTaskManager manager {
			protected get { return m_manager; }
			set { m_manager = value; }
		}

		/// <summary>所属レイヤ番号。</summary>
		/// <remarks>
		/// ロックされていない場合、代入することで変更出来ます。
		/// (ロックされている場合、無視されます)
		/// </remarks>
		public byte layer {
			get { return m_byLayer; }
			set {
				// ! TODO : 例外吐くようにしてもいいかも
				if( !lockLayer ) { m_byLayer = value; }
			}
		}

		/// <summary>所属レイヤが変更可能かどうか。</summary>
		/// <remarks>一度レイヤをロックしてしまうと、二度と解除できません。</remarks>
		public bool lockLayer {
			get { return m_bLockLayer; }
			set { m_bLockLayer = ( m_bLockLayer || value ); }
		}

		/// <summary>一時停止に対応しているかどうか。</summary>
		public bool isAvailablePause {
			get { return m_bAvailablePause; }
			protected set { m_bAvailablePause = value; }
		}

		/// <summary>前フレームからの経過時間。</summary>
		protected GameTime gameTime {
			get { return m_gameTime; }
			private set { m_gameTime = value; }
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>所属レイヤ番号を取得します。</summary>
		/// 
		/// <param name="t">タスク オブジェクト</param>
		/// <returns>レイヤ番号</returns>
		public static implicit operator int( CTaskBase t ) { return t.layer; }

		//* -----------------------------------------------------------------------*
		/// <summary>タスクが管理クラスに登録された直後に、1度だけ自動的に呼ばれます。</summary>
		public void initialize() { lockLayer = true; }

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理をします。</summary>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		/// <returns>次フレームも存続し続ける場合、<c>true</c></returns>
		public bool update( GameTime gameTime ) {
			this.gameTime = gameTime;
			bool bContinue = coRoutineManager.update();
			phaseManager.count++;
			return bContinue;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>継承先で1フレーム分の描画処理を記述します。</summary>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		/// <param name="sprite">スプライト描画管理クラス</param>
		public virtual void draw( GameTime gameTime, CSprite sprite ) { }

		//* -----------------------------------------------------------------------*
		/// <summary>継承先でタスク終了時の処理を記述します。</summary>
		public abstract void Dispose();
	}
}
