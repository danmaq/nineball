////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2012 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Danmaq.Nineball.Core.State
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>Stateパターンにおける基本的なコンテクスト。</summary>
	public class Context
		: IContext
	{

		//* constants ──────────────────────────────-*

		/// <summary>状態の排他制御のためのオブジェクト。</summary>
		private readonly object syncState = new object();

		//* fields ────────────────────────────────*

		/// <summary>現在の状態。</summary>
		private IState currentState = StateNull.Instance;

		/// <summary>現在の状態の以前に設定されていた状態。</summary>
		private IState previousState = StateNull.Instance;

		/// <summary>次に遷移すべき状態。</summary>
		private IState nextState;

		/// <summary>状態間で共有するカプセル化されたデータ。</summary>
		private IContextEncapsulation encapsulationData;

		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="firstState">初回の状態。</param>
		public Context(IState firstState)
		{
			nextState = firstState;
		}

		//* instance properties ─────────────────────────-*

		/// <summary>現在の状態を取得します。</summary>
		public IState CurrentState
		{
			get
			{
				IState result;
				lock (syncState)
				{
					result = currentState;
				}
				return result;
			}
			private set
			{
				lock (syncState)
				{
					currentState = value;
				}
			}
		}

		/// <summary>現在の状態の以前に設定されていた状態を取得します。</summary>
		public IState PreviousState
		{
			get
			{
				IState result;
				lock (syncState)
				{
					result = previousState;
				}
				return result;
			}
			private set
			{
				lock (syncState)
				{
					previousState = value;
				}
			}
		}

		/// <summary>次に遷移すべき状態を取得、及び予約します。</summary>
		public virtual IState NextState
		{
			get
			{
				IState result;
				lock (syncState)
				{
					result = nextState;
				}
				return result;
			}
			set
			{
				lock (syncState)
				{
					nextState = value;
				}
			}
		}

		/// <summary>
		/// 指定したオブジェクトの状態がヌルであるかどうかを取得します。
		/// </summary>
		public bool EmptyState
		{
			get
			{
				return IsEmptyState(this);
			}
		}

		/// <summary>状態間で共有するカプセル化されたデータを取得します。</summary>
		protected IContextEncapsulation EncapsulationData
		{
			get
			{
				if (encapsulationData == null)
				{
					encapsulationData = CreateEncapsulationData();
				}
				return encapsulationData;
			}
		}

		//* class methods ────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 指定したオブジェクトの状態がヌルであるかどうかを取得します。
		/// </summary>
		/// 
		/// <param name="context">判定対象となるコンテクスト</param>
		/// <returns>状態が完全に空の場合、<c>true</c>。</returns>
		/// <exception cref="System.ArgumentNullException">
		/// 引数が<c>null</c>である場合。
		/// </exception>
		public static bool IsEmptyState(IContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			return context.CurrentState == StateNull.Instance && context.NextState == null;
		}

		//* instance methods ───────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>状態を実行します。</summary>
		public virtual void Execute()
		{
			CommitNextState(false);
			CurrentState.Execute(EncapsulationData);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>状態をリセットします。</summary>
		public virtual void Reset()
		{
			EncapsulationData.Reset();
			NextState = StateNull.Instance;
			CommitNextState(true);
			PreviousState = StateNull.Instance;
			NextState = null;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>予約されている状態を即座に確定します。</summary>
		/// 
		/// <param name="force">
		/// 現在の状態が状態遷移を拒否した場合、強制的に状態遷移を行うかどうか。
		/// </param>
		protected virtual void CommitNextState(bool force)
		{
			IState next = NextState;
			if (next != null)
			{
				CurrentState.TearDown(EncapsulationData);
			}
			if (!force)
			{
				next = NextState;
			}
			if (next != null)
			{
				PreviousState = CurrentState;
				CurrentState = next;
				NextState = null;
				CurrentState.Setup(EncapsulationData);
				if (NextState != null)
				{
					CommitNextState(force);
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>状態間で共有するカプセル化されたデータを生成します。</summary>
		/// <remarks>
		/// このメソッドはデータにアクセスしようとした際に、一度だけ呼び出されます。
		/// </remarks>
		/// 
		/// <returns>状態間で共有するカプセル化されたデータ。</returns>
		protected virtual IContextEncapsulation CreateEncapsulationData()
		{
			return new ContextEncapsulation(this);
		}
	}
}
