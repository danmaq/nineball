////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2012 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

namespace Danmaq.Nineball.Core.State
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>状態を構成するための基底クラス。</summary>
	public abstract class CStateBase<T>
		: IState
		where T : class, IContextEncapsulation
	{

		//* instance properties ─────────────────────────-*

		/// <summary>この状態が型不一致である場合、自動的に遷移する型を取得します。</summary>
		/// <remarks>もし異なる型へ遷移したい場合、このプロパティをオーバーライドします。</remarks>
		protected virtual IState NextStateOnTTypeMismatch
		{
			get
			{
				return NullState.Instance;
			}
		}

		//* instance methods ───────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>この状態が開始された際に呼び出されます。</summary>
		/// 
		/// <param name="data">
		/// コンテクストと状態間で共有するカプセル化されたデータ。
		/// </param>
		public abstract void Setup(T data);

		//* -----------------------------------------------------------------------*
		/// <summary>状態を実行する際に呼び出されます。</summary>
		/// 
		/// <param name="data">
		/// コンテクストと状態間で共有するカプセル化されたデータ。
		/// </param>
		public abstract void Execute(T data);

		//* -----------------------------------------------------------------------*
		/// <summary>状態を終了する際に呼び出されます。</summary>
		/// 
		/// <param name="data">
		/// コンテクストと状態間で共有するカプセル化されたデータ。
		/// </param>
		public abstract void Teardown(T data);

		//* -----------------------------------------------------------------------*
		/// <summary>この状態が開始された際に呼び出されます。</summary>
		/// 
		/// <param name="data">
		/// コンテクストと状態間で共有するカプセル化されたデータ。
		/// </param>
		void IState.Setup(IContextEncapsulation data)
		{
			T real = data as T;
			if (real == null)
			{
				data.Context.NextState = NullState.Instance;
			}
			else
			{
				Setup(real);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>状態を実行する際に呼び出されます。</summary>
		/// 
		/// <param name="data">
		/// コンテクストと状態間で共有するカプセル化されたデータ。
		/// </param>
		void IState.Execute(IContextEncapsulation data)
		{
			Execute((T)data);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>状態を終了する際に呼び出されます。</summary>
		/// 
		/// <param name="data">
		/// コンテクストと状態間で共有するカプセル化されたデータ。
		/// </param>
		void IState.TearDown(IContextEncapsulation data)
		{
			Teardown((T)data);
		}
	}
}
