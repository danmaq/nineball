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
	/// <summary>Singletonのためのジェネリックス クラス。</summary>
	public static class CSingleton<_T> where _T : class, new() {

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>クラス インスタンス</summary>
		private static _T m_instance = null;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>クラス インスタンス</summary>
		public static _T instance {
			get {
				if(!isCreated) { m_instance = new _T(); }
				return m_instance;
			}
		}

		/// <summary>インスタンスが作成されたかどうか。</summary>
		public static bool isCreated {
			get { return (m_instance != null); }
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>インスタンスを強制的に破壊します。</summary>
		/// 
		/// <returns>破壊出来た場合、true</returns>
		public static bool crash() {
			bool bResult = isCreated;
			if(bResult) { m_instance = null; }
			GC.Collect();
			return bResult;
		}
	}
}
