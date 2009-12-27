////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.nineball.data {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>単一オブジェクトを送信するためのイベントデータ。</summary>
	/// 
	/// <typeparam name="_T">送信したい型。</typeparam>
	public sealed class CEventMonoValue<_T> : EventArgs {

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>送信したいオブジェクト。</summary>
		public _T value;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="value">送信したいオブジェクト。</param>
		public CEventMonoValue( _T value ) { this.value = value; }

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>送信したいオブジェクトから直接イベントデータを作成します。</summary>
		/// 
		/// <param name="r">送信したいオブジェクト。</param>
		/// <returns>イベントデータ。</returns>
		public static implicit operator CEventMonoValue<_T>( _T r ) {
			return new CEventMonoValue<_T>( r );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>送信されたオブジェクトを取得します。</summary>
		/// 
		/// <param name="v">イベントデータ オブジェクト。</param>
		/// <returns>送信したいオブジェクト単一オブジェクト。</returns>
		public static implicit operator _T( CEventMonoValue<_T> v ) { return v.value; }
	}
}
