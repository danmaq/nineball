////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2012 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

#if WINDOWS
using System.Threading;
#endif

namespace Danmaq.Nineball.Core.Utils
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ミューテックスオブジェクトのラッパー クラス。</summary>
	/// <remarks>
	/// <para>
	/// Windows版でこのクラス インスタンスを複数作成した場合、<c>ApplicationException</c>
	/// 例外が発生します。また、Mutexを使用していますので、複数アプリ間においても有効です。
	/// </para>
	/// <para>
	/// 注意：XBOX360版では<c>IDisposable</c>が適用されたただの<c>Object</c>クラスと
	/// 事実上同等のものとなります。よってXBOX360版では上記の効果は全く発揮しません。
	/// </para>
	/// </remarks>
	public sealed class MutexWrapper
		: IDisposable
	{

		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <exception cref="System.Exception">多重起動した場合。</exception>
		public MutexWrapper()
			: this(Text.NAME)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="name">識別のためのユニークな名称。</param>
		/// <exception cref="System.Exception">多重起動した場合。</exception>
		public MutexWrapper(string name)
		{
#if WINDOWS
			Mutex = new Mutex(false, name);
			Mutex _mutex = (Mutex)Mutex;
			if (!_mutex.WaitOne(0, false))
			{
				_mutex.Dispose();
				throw new InvalidOperationException(Text.ERR_IO_MUTEX);
			}
#endif
		}

		//* -----------------------------------------------------------------------*
		/// <summary>デストラクタ。</summary>
		~MutexWrapper()
		{
			Dispose();
		}

		//* instance properties ─────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ミューテックス オブジェクトを取得します。</summary>
		public object Mutex
		{
			get;
			private set;
		}

		//* instance methods ───────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public void Dispose()
		{
			if (Mutex != null)
			{
#if WINDOWS
				((Mutex)Mutex).Close();
#endif
				Mutex = null;
				GC.SuppressFinalize(this);
			}
		}
	}
}
