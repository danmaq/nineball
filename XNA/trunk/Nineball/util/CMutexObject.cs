////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

#if WINDOWS
using System.Threading;
using danmaq.nineball.Properties;
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

#if WINDOWS
		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <exception cref="System.ApplicationException">多重起動した場合。</exception>
		public CMutexObject()
		{
			Mutex _mutex = new Mutex(false, Resources.NAME);
			if (!_mutex.WaitOne(0, false))
			{
				throw new ApplicationException(Resources.ERR_MUTEX);
			}
			mutex = _mutex;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>デストラクタ。</summary>
		~CMutexObject()
		{
			Dispose();
		}
#endif

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
