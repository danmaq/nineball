﻿////////////////////////////////////////////////////////////////////////////////
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
using System.Collections.ObjectModel;
using danmaq.nineball.data;
using danmaq.nineball.entity.input.data;
using danmaq.nineball.Properties;
using danmaq.nineball.state;
using danmaq.nineball.state.input.collection;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>マンマシンI/F入力制御・管理クラスのコレクション。</summary>
	public class CInputCollection : CInput, ICollection<CInput>
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>ボタンの入力状態一覧。</summary>
			public readonly List<SInputState> buttonStateList;

			/// <summary>子入力クラス。</summary>
			public readonly List<CInput> childs;

			/// <summary>マンマシンI/F入力制御・管理クラスのコレクション。</summary>
			private readonly CInputCollection collection;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="collection">XBOX360ゲーム コントローラ入力制御・管理クラス。</param>
			public CPrivateMembers(CInputCollection collection)
			{
				this.collection = collection;
				buttonStateList = collection._buttonStateList;
				childs = collection.childs;
			}

			//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* properties ──────────────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>方向ボタンの状態をベクトルで設定します。</summary>
			/// 
			/// <value>方向ボタンの状態。</value>
			public Vector2 axisVector
			{
				set
				{
					collection.axis = value;
				}
			}

			//* -----------------------------------------------------------------------*
			/// <summary>方向ボタンの状態をフラグで設定/取得します。</summary>
			/// <example>
			/// bool bDown = (obj.axisFlag &amp; EDirectionFlags.down) != 0;
			/// </example>
			/// 
			/// <value>方向ボタンの状態。</value>
			public EDirectionFlags axisFlag
			{
				get
				{
					return collection.axisFlag;
				}
				set
				{
					collection.axisFlag = value;
				}
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>子入力クラス。</summary>
		private readonly List<CInput> childs = new List<CInput>(1);

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _privateMemebers;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* events ────────────────────────────────*

		/// <summary>子入力クラスの数が増減した時に呼び出されるイベント。</summary>
		public event EventHandler<CEventMonoValue<int>> changedChildCount;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>接続されていないコントローラを自動的に解放するかどうか。</summary>
		public bool releaseAwayController = false;

		/// <summary>子入力クラスとして受け入れる最大値。</summary>
		private ushort m_capacity = ushort.MaxValue;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。既定の状態で初期化します。</summary>
		/// 
		/// <param name="playerNumber">プレイヤー番号。</param>
		public CInputCollection(short playerNumber)
			: this(playerNumber, CStateDefault.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。指定の状態で初期化します。</summary>
		/// 
		/// <param name="playerNumber">プレイヤー番号。</param>
		/// <param name="firstState">初期状態。</param>
		public CInputCollection(short playerNumber, IState firstState)
			: base(playerNumber, firstState)
		{
			_privateMemebers = new CPrivateMembers(this);
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>接続されているかどうかを取得します。</summary>
		/// 
		/// <value>接続されている場合、<c>true</c>。</value>
		public override bool connect
		{
			get
			{
				return childs.Count > 0;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// 状態として、nullを設定しようとした場合。
		/// </exception>
		public new IState<CInputCollection, CPrivateMembers> nextState
		{
			set
			{
				nextStateBase = value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>子入力クラスとして受け入れる最大値を設定/取得します。</summary>
		/// 
		/// <value>子入力クラスとして受け入れる最大値。</value>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 現在の保有個数未満の値を設定した場合。
		/// </exception>
		public virtual ushort capacity
		{
			get
			{
				return m_capacity;
			}
			set
			{
				if(m_capacity != value)
				{
					childs.Capacity = value;
					m_capacity = value;
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>格納されている要素の数を取得します。</summary>
		/// 
		/// <value>格納されている要素の数。</value>
		public virtual int Count
		{
			get
			{
				return childs.Count;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>子リストを取得します。</summary>
		/// 
		/// <value>子リスト。</value>
		public virtual ReadOnlyCollection<CInput> childList
		{
			get
			{
				return childs.AsReadOnly();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>読み取り専用かどうかを示す値を設定/取得します。</summary>
		/// 
		/// <value>読み取り専用の場合、<c>true</c>。</value>
		public virtual bool IsReadOnly
		{
			get;
			set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// オブジェクトと状態クラスのみがアクセス可能なフィールドを取得します。
		/// </summary>
		/// 
		/// <value>オブジェクトと状態クラスのみがアクセス可能なフィールド。</value>
		protected override object privateMembers
		{
			get
			{
				return _privateMemebers;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理を実行します。</summary>
		public override void initialize()
		{
			childs.ForEach(input => input.initialize());
			base.initialize();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			Clear();
			changedChildCount = null;
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>要素を追加します。</summary>
		/// 
		/// <param name="item">要素。</param>
		/// <exception cref="System.ArgumentException">
		/// 既に登録されている子入力クラスを登録しようとした場合。
		/// </exception>
		/// <exception cref="System.ArgumentNullException">
		/// 要素が<c>null</c>である場合。
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 対象のプレイヤー番号が自身のものと相違する場合。
		/// </exception>
		/// <exception cref="System.InvalidOperationException">
		/// 許容値以上の数の子入力クラスを登録しようとした場合。
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		public virtual void Add(CInput item)
		{
			throwAtReadOnly();
			if(item == null)
			{
				throw new ArgumentNullException("item");
			}
			if(!(item.playerNumber == playerNumber))
			{
				throw new ArgumentOutOfRangeException("item");
			}
			if(Count == capacity)
			{
				throw new InvalidOperationException();
			}
			if(childs.Contains(item))
			{
				throw new ArgumentException("item");
			}
			changedButtonsNum += item.onChangedButtonsNum;
			item.ButtonsNum = ButtonsNum;
			childs.Add(item);
			if(changedChildCount != null)
			{
				changedChildCount(this, childs.Count);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理している子入力クラスを全て解放します。</summary>
		/// 
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		public virtual void Clear()
		{
			throwAtReadOnly();
			childs.ForEach(item => RemoveWithoutEvent(item));
			childs.Clear();
			childs.Capacity = 1;
			if(changedChildCount != null)
			{
				changedChildCount(this, 0);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>特定の値が格納されているかどうかを判断します。</summary>
		/// 
		/// <param name="item">検索するオブジェクト。</param>
		/// <returns>存在する場合、<c>true</c>。</returns>
		public virtual bool Contains(CInput item)
		{
			return childs.Contains(item);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>要素を配列にコピーします。</summary>
		/// 
		/// <param name="array">
		/// 要素がコピーされる1次元かつ0から始まるインデックス番号の配列。
		/// </param>
		/// <param name="arrayIndex">
		/// コピーの開始位置となる、配列の0から始まるインデックス番号。
		/// </param>
		public virtual void CopyTo(CInput[] array, int arrayIndex)
		{
			childs.CopyTo(array, arrayIndex);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理している子入力クラスを解放します。</summary>
		/// 
		/// <param name="item">子入力クラス。</param>
		/// <returns>解放できた場合、<c>true</c>。</returns>
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		public virtual bool Remove(CInput item)
		{
			bool bResult = RemoveWithoutEvent(item);
			if(changedChildCount != null)
			{
				changedChildCount(this, childs.Count);
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 指定した型のコレクションに対する単純な反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		public virtual IEnumerator<CInput> GetEnumerator()
		{
			return childs.GetEnumerator();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 非ジェネリック コレクションに対する反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)childs).GetEnumerator();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>読み取り専用状態かどうかを判断して、例外を発生します。</summary>
		/// 
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		protected virtual void throwAtReadOnly()
		{
			if(IsReadOnly)
			{
				throw new NotSupportedException(Resources.ERR_READONLY);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理している子入力クラスを解放します。</summary>
		/// 
		/// <param name="item">子入力クラス。</param>
		/// <returns>解放できた場合、<c>true</c>。</returns>
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		private bool RemoveWithoutEvent(CInput item)
		{
			throwAtReadOnly();
			bool bResult = childs.Remove(item);
			if(bResult)
			{
				changedButtonsNum -= item.onChangedButtonsNum;
				item.Dispose();
			}
			return bResult;
		}
	}
}
