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
	/// <summary>コンテクストと状態間で共有するカプセル化されたデータ。</summary>
	public class ContextEncapsulation
		: IContextEncapsulation
	{

		//* constants ──────────────────────────────-*

		/// <summary>データの排他制御のためのオブジェクト。</summary>
		private readonly object syncRoot = new object();

		//* fields ────────────────────────────────*

		/// <summary>コンテクスト。</summary>
		private IContext context;

		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="context">コンテクスト。</param>
		public ContextEncapsulation(IContext context)
		{
			lock (SyncRoot)
			{
				this.context = context;
			}
		}

		//* instance properties ─────────────────────────-*

		/// <summary>コンテクストを取得します。</summary>
		public IContext Context
		{
			get
			{
				return context;
			}
		}

		/// <summary>データの排他制御のためのオブジェクトを取得します。</summary>
		protected object SyncRoot
		{
			get
			{
				return syncRoot;
			}
		}

		//* instance methods ───────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>状態をリセットします。</summary>
		public virtual void Reset()
		{
		}
	}
}
