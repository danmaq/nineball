////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.data;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>状態を持つオブジェクトの基底クラス。</para>
	/// <para>
	/// これを継承するか、または<c>IEntity</c>を実装することで、
	/// 状態を持つオブジェクトを作ることができます。
	/// </para>
	/// <para>また、このクラスに直接状態を持たせて使用することもできます。</para>
	/// </summary>
	public class CEntity
		: IEntity
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* events ────────────────────────────────*

		/// <summary>状態が遷移された時に呼び出されるイベント。</summary>
		public event EventHandler<CEventChangedState> changedState;

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>状態の排他制御。</summary>
		private readonly object syncState = new object();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>汎用カウンタの1フレーム辺りの進行量。</summary>
		public int counterStep = 1;

		/// <summary>状態遷移を遅らせるフレーム時間数。</summary>
		public int delayChangeState = 0;

		/// <summary>同じ状態同志で遷移することを認めるかどうか。</summary>
		public bool allowSameState = false;

		/// <summary>現在の状態。</summary>
		private IState m_stateCurrent = CState.empty;

		/// <summary>型名のキャッシュ。</summary>
		private string m_strTypeName = null;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CEntity()
			: this(null)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		public CEntity(IState firstState)
			: this(firstState, null)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		/// <param name="privateMembers">
		///	オブジェクトと状態クラスのみがアクセス可能なフィールド。
		///	</param>
		public CEntity(IState firstState, object privateMembers)
		{
			previousState = CState.empty;
			nextState = firstState;
			this.privateMembers = privateMembers;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>最後に変化する前の状態を取得します。</summary>
		/// 
		/// <value>最後に変化する前の状態。初期値は<c>CState.empty</c>。</value>
		public IState previousState
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在の状態を取得します。</summary>
		/// 
		/// <value>現在の状態。初期値は<c>CState.empty</c>。</value>
		public IState currentState
		{
			get
			{
				IState result;
				lock (syncState)
				{
					result = m_stateCurrent;
				}
				return result;
			}
			private set
			{
				lock (syncState)
				{
					m_stateCurrent = value;
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>汎用フレーム カウンタを取得します。</summary>
		/// 
		/// <value>汎用フレーム カウンタ。</value>
		public int counter
		{
			get;
			protected set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>最後に状態が変化したフレーム時間を取得します。</summary>
		/// 
		/// <value>最後に状態が変化したフレーム時間。</value>
		public int lastStateChangeCounter
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を予約します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		public IState nextState
		{
			get;
			set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 現在の状態が完全に空(<c>CState.empty</c>)かどうかを取得します。
		/// </summary>
		/// 
		/// <value>現在の状態が空である場合、<c>true</c>。</value>
		public bool emptyState
		{
			get
			{
				return currentState == CState.empty && nextState == null;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>オブジェクトが解放可能な状態であるかどうかを取得します。</summary>
		/// <remarks>
		/// このプロパティは、<c>Dispose</c>メソッドが実行されると
		/// 自動的に<c>true</c>に書き換えられます。
		/// </remarks>
		/// 
		/// <value>オブジェクトが解放可能な状態である場合、<c>true</c>。</value>
		public bool disposed
		{
			get;
			protected set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// オブジェクトと状態クラスのみがアクセス可能なフィールドを取得します。
		/// </summary>
		/// 
		/// <value>オブジェクトと状態クラスのみがアクセス可能なフィールド。</value>
		protected virtual object privateMembers
		{
			get;
			private set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの状態を含めた文字列情報を取得します。</summary>
		/// 
		/// <value>このオブジェクトを示す文字列情報。</value>
		public override string ToString()
		{
			if(m_strTypeName == null)
			{
				m_strTypeName = GetType().ToString();
			}
			return string.Format("{0} STATE[CUR:{1}, PREV:{2}] {3} MEMBERS:{4}",
				m_strTypeName, currentState, previousState,
				Environment.NewLine, privateMembers);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>カウンタをリセットします。</summary>
		public virtual void resetCounter()
		{
			counterStep = 1;
			counter = 0;
			lastStateChangeCounter = 0;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトに空の状態を設定します。</summary>
		public virtual void setEmptyState()
		{
			nextState = CState.empty;
		}
	
		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public virtual void Dispose()
		{
			nextState = CState.empty;
			commitNextState(allowSameState);
			resetCounter();
			currentState = CState.empty;
			previousState = CState.empty;
			nextState = null;
			changedState = null;
			delayChangeState = 0;
			allowSameState = false;
			disposed = true;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public virtual void update(GameTime gameTime)
		{
			if (delayChangeState-- <= 0 && nextState != null)
			{
				commitNextState(allowSameState);
			}
			currentState.update(this, privateMembers, gameTime);
			counter += counterStep;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public virtual void draw(GameTime gameTime)
		{
			currentState.draw(this, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>予約していた次の状態を強制的に確定します。</summary>
		/// 
		/// <param name="allowSameState">
		/// 同じ状態同志で遷移することを認めるかどうか。
		/// </param>
		public virtual void commitNextState(bool allowSameState)
		{
			if (nextState == currentState && !allowSameState)
			{
				nextState = null;
			}
			if (nextState != null)
			{
				IState _nextState = nextState;
				nextState = null;
				currentState.teardown(this, privateMembers, _nextState);
				IState oldPrevious = previousState;
				IState oldCurrent = currentState;
				previousState = currentState;
				currentState = _nextState;
				lastStateChangeCounter = counter;
				currentState.setup(this, privateMembers);
				if (changedState != null)
				{
					changedState(this, new CEventChangedState(oldPrevious, oldCurrent, _nextState));
				}
			}
		}
	}
}
