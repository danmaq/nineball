////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.nineball.util.collection
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// 自動的にデフォルトのコンストラクタ、及び
	/// <c>Dispose()</c>メソッドを呼び出すFlyweightパターン。
	/// </summary>
	/// 
	/// <typeparam name="_T">Flyweight対応にする型。</typeparam>
	public class CFlyweightNewDisposable<_T> : CFlyweight<_T> where _T : class, IDisposable, new()
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>休眠時に<c>Dispose()</c>メソッドを呼び出すかどうか。</summary>
		public bool disposeOnSleep = false;

		/// <summary>登録可能な最大数。</summary>
		public int Capacity = int.MaxValue;

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>未使用のインスタンスを取得します。</para>
		/// <para>
		/// 存在しないか、全て使用中の場合は既定の
		/// コンストラクタを使用して新規インスタンスを作成します。
		/// </para>
		/// </summary>
		/// 
		///	<returns>
		///	未使用のインスタンス。登録可能な最大数を超過している場合、<c>null</c>。
		///	</returns>
		public override _T get()
		{
			_T result = base.get();
			if(result == default(_T) && Count < Capacity)
			{
				result = new _T();
				Add(result);
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>インスタンスを休眠させます。</summary>
		/// 
		///	<param name="instance">インスタンス。</param>
		///	<returns>インスタンスが休眠した場合、<c>true</c>。</returns>
		public override bool sleep(_T instance)
		{
			bool bResult = base.sleep(instance);
			if(bResult && disposeOnSleep)
			{
				instance.Dispose();
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>インスタンスを削除します。</summary>
		/// 
		///	<param name="instance">インスタンス。</param>
		///	<returns>インスタンスを削除出来た場合、<c>true</c>。</returns>
		public override bool Remove(_T instance)
		{
			bool bResult = base.Remove(instance);
			if(bResult && disposeOnSleep)
			{
				instance.Dispose();
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このクラスを解放します。</summary>
		public override void Dispose()
		{
			list.ForEach(info => info.m_instance.Dispose());
			base.Dispose();
		}
	}
}
