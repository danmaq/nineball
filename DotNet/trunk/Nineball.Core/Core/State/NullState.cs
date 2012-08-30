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
	/// <summary><c>null</c>を示す状態。</summary>
	public sealed class NullState
		: IState
	{

		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly IState Instance = new NullState();

		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private NullState()
		{
		}

		//* instance methods ───────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>この状態が開始された際に呼び出されます。</summary>
		/// 
		/// <param name="data">
		/// コンテクストと状態間で共有するカプセル化されたデータ。
		/// </param>
		public void Setup(IContextEncapsulation data)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>状態を実行する際に呼び出されます。</summary>
		/// 
		/// <param name="data">
		/// コンテクストと状態間で共有するカプセル化されたデータ。
		/// </param>
		public void Execute(IContextEncapsulation data)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>状態を終了する際に呼び出されます。</summary>
		/// 
		/// <param name="data">
		/// コンテクストと状態間で共有するカプセル化されたデータ。
		/// </param>
		public void TearDown(IContextEncapsulation data)
		{
		}
	}
}
