////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;

namespace danmaq.nineball.util.collection {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>部分責任コレクション。</para>
	/// <para>
	/// このクラスを通じてコレクションに格納された要素は、
	/// このクラスを破棄することで道連れにされます。
	/// </para>
	/// </summary>
	/// 
	/// <typeparam name="_T">コレクション内の要素の型。</typeparam>
	public class CPartialCollection<_T> : CPartialCollection<_T, _T> {

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="collection">部分的に責任を持つ対象のリスト。</param>
		public CPartialCollection( ICollection<_T> collection ) : base( collection ) { }

	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>部分責任コレクション。</para>
	/// <para>
	/// このクラスを通じてコレクションに格納された要素は、
	/// このクラスを破棄することで道連れにされます。
	/// </para>
	/// </summary>
	/// 
	/// <typeparam name="_T">対象コレクション内の要素の基本型。</typeparam>
	/// <typeparam name="_P">
	/// 部分責任コレクション内の要素の型。
	/// <typeparamref name="_T"/>と同一型またはサブクラスを指定します。
	/// </typeparam>
	public class CPartialCollection<_T, _P> : ICollection<_P>, IDisposable where _P : _T {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>責任を持つオブジェクト一覧。</summary>
		protected readonly List<_P> partial = new List<_P>();

		/// <summary>部分的に責任を持つ対象のリスト。</summary>
		protected readonly ICollection<_T> collection;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="collection">部分的に責任を持つ対象のリスト。</param>
		public CPartialCollection( ICollection<_T> collection ) {
			this.collection = collection;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>デストラクタ。</summary>
		~CPartialCollection() { Dispose(); }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>格納されている要素の数を取得します。</summary>
		/// 
		/// <value>格納されている要素の数。</value>
		public virtual int Count {
			get { return partial.Count; }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>読み取り専用かどうかを示す値を設定/取得します。</summary>
		/// 
		/// <value>読み取り専用の場合、<c>true</c>。</value>
		public virtual bool IsReadOnly { get; set; }

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>管理している要素を全て解放します。</summary>
		public virtual void Dispose() { Clear(); }

		//* -----------------------------------------------------------------------*
		/// <summary>要素を追加します。</summary>
		/// 
		/// <param name="item">要素。</param>
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		public virtual void Add( _P item ) {
			throwAtReadOnly();
			collection.Add( item );
			partial.Add( item );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理している要素を全て解放します。</summary>
		/// 
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		public virtual void Clear() {
			throwAtReadOnly();
			foreach( _P item in partial ) { collection.Remove( item ); }
			castoff();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理している要素を解放します。</summary>
		/// 
		/// <param name="item">要素。</param>
		/// <returns>解放できた場合、<c>true</c>。</returns>
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		public virtual bool Remove( _P item ) {
			throwAtReadOnly();
			return castoff( item ) || partial.Remove( item );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>特定の値が格納されているかどうかを判断します。</summary>
		/// 
		/// <param name="item">検索するオブジェクト。</param>
		/// <returns>存在する場合、<c>true</c>。</returns>
		public virtual bool Contains( _P item ) { return partial.Contains( item ); }

		//* -----------------------------------------------------------------------*
		/// <summary>要素を配列にコピーします。</summary>
		/// 
		/// <param name="array">
		/// 要素がコピーされる1次元かつ0から始まるインデックス番号の配列。
		/// </param>
		/// <param name="arrayIndex">
		/// コピーの開始位置となる、配列の0から始まるインデックス番号。
		/// </param>
		public virtual void CopyTo( _P[] array, int arrayIndex ) {
			partial.CopyTo( array, arrayIndex );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 指定した型のコレクションに対する単純な反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		public virtual IEnumerator<_P> GetEnumerator() { return partial.GetEnumerator(); }

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 非ジェネリック コレクションに対する反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		IEnumerator IEnumerable.GetEnumerator() {
			return ( ( IEnumerable )partial ).GetEnumerator();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理している要素を全て管理放棄します。</summary>
		/// 
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		public virtual void castoff() {
			throwAtReadOnly();
			partial.Clear();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理している要素を管理放棄します。</summary>
		/// 
		/// <param name="item">要素。</param>
		/// <returns>管理放棄できた場合、<c>true</c>。</returns>
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		public virtual bool castoff( _P item ) {
			throwAtReadOnly();
			return partial.Remove( item );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>読み取り専用状態かどうかを判断して、例外を発生します。</summary>
		/// 
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		protected void throwAtReadOnly() {
			if( IsReadOnly ) { throw new NotSupportedException(); }
		}
	}
}
