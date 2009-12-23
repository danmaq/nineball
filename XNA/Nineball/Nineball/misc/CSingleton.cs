////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2009 danmaq all rights reserved.
//
//		──Singletonのためのジェネリックス クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.Nineball.misc{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>
	/// Singletonのためのジェネリックス クラスです。
	/// </para>
	/// <para>
	/// このクラスを使用すると、対象クラスのオブジェクト数が
	/// 最小で0から、最大で1になることが保証されます。
	/// </para>
	/// <para>
	/// instanceプロパティ、またはgetInstance()メソッドを
	/// 最初に呼び出した瞬間にオブジェクトが生成されます。
	/// </para>
	/// </summary>
	/// <example>
	/// 下記のコードの場合、SingletonHogeをシングルトン クラスとして使用します。
	/// <code>
	/// public class SingletonHoge{ // シングルトンにしたいオブジェクト。
	///		private SingletonHoge() { }	// コンストラクタはprivateにする。
	/// }
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

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>クラス オブジェクト</summary>
		private static volatile _T m_instance = null;

		/// <summary>Thread-safe対策のためのオブジェクト。</summary>
		private static readonly object syncRoot = new object();

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>クラス<typeparamref name="_T"/>のオブジェクトを取得します。</summary>
		/// <remarks><c>getInstance()</c>のラッパーです。</remarks>
		public static _T instance {
			get { return getInstance(); }
		}

		/// <summary>インスタンスが作成されたかどうか。</summary>
		public static bool isCreated {
			get { return ( m_instance != null ); }
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>クラス<typeparamref name="_T"/>のオブジェクトを取得します。</summary>
		/// <remarks>諸事情でプロパティが使用できない場合などに使用します。</remarks>
		/// 
		/// <returns>オブジェクト</returns>
		public static _T getInstance() {
			// 要らぬことでlockさせないよう2回チェックする
			if( !isCreated ) {
				lock( syncRoot ) {
					if( !isCreated ) {
						// HACK : XBOX360版でもCreateInstanceできんかなぁ……orz
#if WINDOWS
						// private？でもそんなの関係ねぇ！
						Type t = typeof( _T );
						var obj = Activator.CreateInstance( t, true );
						m_instance = obj as _T;
#else
						m_instance = new _T();
#endif
					}
				}
			}
			return m_instance;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>オブジェクトを強制的に破壊します。</summary>
		/// 
		/// <returns>破壊出来た場合、<c>true</c></returns>
		public static bool crash() {
			bool bResult = false;
			// 要らぬことでlockさせないよう2回チェックする
			if( isCreated ) {
				lock( syncRoot ) {
					bResult = isCreated;
					if( bResult ) {
						m_instance = null;
						GC.Collect();
					}
				}
			}
			return bResult;
		}
	}
}
