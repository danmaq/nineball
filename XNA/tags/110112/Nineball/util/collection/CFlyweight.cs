////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

#if WINDOWS
using System.Diagnostics;
#endif

namespace danmaq.nineball.util.collection
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>Flyweightパターンの簡単な実装。</summary>
	/// 
	/// <typeparam name="_T">Flyweight対応にする型。</typeparam>
	public class CFlyweight<_T> : IDisposable where _T : class
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>インスタンスと現在のアクティブ状態を持つ構造体。</summary>
#if WINDOWS
		[DebuggerDisplay("Active={m_bActive}, Instance={m_instance}")]
#endif
		protected struct SData
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>インスタンス。</summary>
			public _T m_instance;

			/// <summary>インスタンスが現在アクティブかどうか。</summary>
			public bool m_bActive;


			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="instance">インスタンス。</param>
			public SData(_T instance)
			{
				m_bActive = true;
				m_instance = instance;
			}

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>インスタンスを設定します。</summary>
			/// 
			///	<param name="instance">インスタンス。</param>
			public void set(_T instance)
			{
				m_bActive = true;
				m_instance = instance;
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>インスタンス一覧。</summary>
		protected readonly List<SData> list = new List<SData>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary><c>Dispose()</c>メソッドが呼び出された際に実行されるアクション。</summary>
		private Action<_T> m_onDisposeAction;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CFlyweight()
		{
			onDisposeAction = null;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>Dispose()</c>メソッドが呼び出された際に実行されるアクションを設定/取得します。
		/// </summary>
		/// 
		/// <value>デリゲート。nullの場合、空のラムダ式が設定されます。</value>
		public virtual Action<_T> onDisposeAction
		{
			get
			{
				return m_onDisposeAction;
			}
			set
			{
				m_onDisposeAction = value ?? (obj =>
				{
				});
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>実際に格納されている要素の数を取得します。</summary>
		/// 
		/// <value>要素の数。</value>
		public int Count
		{
			get
			{
				return list.Count;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>未使用のインスタンスを取得します。</summary>
		/// 
		///	<returns>
		///	未使用のインスタンス。存在しないか、全て使用中の場合、<c>null</c>。
		///	</returns>
		public virtual _T get()
		{
			_T result = default(_T);
			int nIndex = list.FindIndex(info => !info.m_bActive);
			if(nIndex >= 0)
			{
				SData data = list[nIndex];
				data.m_bActive = true;
				result = data.m_instance;
				list[nIndex] = data;
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>インスタンスを設定します。</summary>
		/// 
		///	<param name="instance">インスタンス。</param>
		///	<exception cref="System.ArgumentNullException">
		///	<c>null</c>を登録しようとした場合。
		///	</exception>
		///	<exception cref="System.ArgumentNullException">
		///	<paramref name="instance"/>が既に登録されている場合。
		///	</exception>
		public virtual void Add(_T instance)
		{
			if(instance == default(_T))
			{
				throw new ArgumentNullException("instance");
			}
			if(list.FindIndex(info => info.m_instance == instance) >= 0)
			{
				throw new ArgumentException("instance");
			}
			list.Add(new SData(instance));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>インスタンスを休眠させます。</summary>
		/// 
		///	<param name="instance">インスタンス。</param>
		///	<returns>インスタンスが休眠した場合、<c>true</c>。</returns>
		public virtual bool sleep(_T instance)
		{
			int nIndex = list.FindIndex(info => info.m_instance == instance);
			bool bResult = nIndex >= 0;
			if(bResult)
			{
				SData data = list[nIndex];
				data.m_bActive = false;
				list[nIndex] = data;
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>インスタンスを削除します。</summary>
		/// 
		///	<param name="instance">インスタンス。</param>
		///	<returns>インスタンスを削除出来た場合、<c>true</c>。</returns>
		public virtual bool Remove(_T instance)
		{
			return list.RemoveAll(info => info.m_instance == instance) > 0;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このクラスを解放します。</summary>
		public virtual void Dispose()
		{
			for (int i = list.Count; --i >= 0; )
			{
				onDisposeAction(list[i].m_instance);
			}
			list.Clear();
			list.TrimExcess();
		}
	}
}
