////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.nineball.util
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>
	/// Singletonのためのジェネリックス クラスです。
	/// </para>
	/// <para>
	/// このクラスを使用すると、対象クラスのオブジェクト数が
	/// 常に1となることが保証されます。
	/// </para>
	/// <para>
	/// instanceプロパティ、またはgetInstance()メソッドを
	/// 最初に呼び出した瞬間にオブジェクトが生成されます。
	/// </para>
	/// </summary>
	/// <example>
	/// 下記のコードの場合、SingletonHogeをシングルトン クラスとして使用します。
	/// <code>
	/// public sealed class SingletonHoge	// シングルトンにしたいオブジェクト。
	/// {									// 継承できないようsealedする。
	///		private SingletonHoge() { }		// コンストラクタはprivateにする。
	/// }									// XBOX版ではprivateにしてはいけない。
	/// static class Program{
	///		static void Main( string[] args ){
	///			SingletonHoge hoge = CSingleton&lt;SingletonHoge&gt;.instance;
	///			// ↑ここで初めてSingletonHogeオブジェクトは作成された。
	///			
	///			SingletonHoge fuga = CSingleton&lt;SingletonHoge&gt;.instance;
	///			// ↑この時、hogeとfugaは同一の参照である。
	///			
	///			CSingleton&lt;SingletonHoge&gt;.crash();
	///			// ↑ここで、CSingletonはオブジェクトの管理を放棄し、
	///			// SingletonHogeオブジェクトはhoge(=fuga)だけとなった。
	///			
	///			SingletonHoge piyo = CSingleton&lt;SingletonHoge&gt;.instance;
	///			// ↑piyoとhoge・fugaは別個の参照である。
	///		}
	/// }
	/// </code>
	/// </example>
	/// 
	/// <typeparam name="_T">
	/// シングルトンにしたいクラスを指定します。
	/// XBOX360版では完全なシングルトンにすることはできません。
	/// (publicコンストラクタが必要)
	/// </typeparam>
	public static class CSingleton<_T> where _T : class
#if XBOX360
		, new()
#endif
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <returns>クラス オブジェクト。</returns>
		public static readonly _T instance;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static CSingleton()
		{
			// HACK : XBOX360版でもCreateInstanceできんかなぁ……orz
			// TODO : Activator.CreateInstance<T>はどうよ？
#if WINDOWS
			// private？でもそんなの関係ねぇ！
			instance = (_T)Activator.CreateInstance(typeof(_T), true);
#else
			instance = new _T();
#endif
		}
	}
}
