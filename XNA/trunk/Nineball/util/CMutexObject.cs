////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.Properties;

#if WINDOWS
using System.Threading;
#endif

namespace danmaq.nineball.util
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
	public sealed class CMutexObject : IDisposable
	{

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <exception cref="System.ApplicationException">多重起動した場合。</exception>
		public CMutexObject()
			: this(Resources.NAME)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="name">識別のためのユニークな名称。</param>
		/// <exception cref="System.ApplicationException">多重起動した場合。</exception>
		public CMutexObject(string name)
		{
#if WINDOWS
			Mutex _mutex = new Mutex(false, name);
			if (!_mutex.WaitOne(0, false))
			{
				throw new ApplicationException(Resources.IO_ERR_MUTEX);
			}
			mutex = _mutex;
#endif
		}

		//* -----------------------------------------------------------------------*
		/// <summary>デストラクタ。</summary>
		~CMutexObject()
		{
			Dispose();
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>ミューテックス オブジェクトを取得します。</summary>
		/// 
		/// <value>ミューテックス オブジェクト。</value>
		public object mutex
		{
			get;
			private set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public void Dispose()
		{
			if (mutex != null)
			{
#if WINDOWS
				((Mutex)mutex).Close();
#endif
				mutex = null;
			}
		}
	}
}
