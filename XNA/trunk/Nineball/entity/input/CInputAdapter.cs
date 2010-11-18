////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using danmaq.nineball.entity.input.low;
using danmaq.nineball.state;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>高位入力制御・管理クラス。</summary>
	/// 
	/// <typeparam name="_T">低位入力制御・管理クラスの型。</typeparam>
	/// <typeparam name="_S">入力状態の型。</typeparam>
	public sealed class CInputAdapter<_T, _S>
		: CEntity, IInputAdapter where _T : ILowerInput<_S>
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers : IDisposable
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>入力情報一覧。</summary>
			public List<SInputInfo> buttonList = new List<SInputInfo>(1);

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>このオブジェクトの終了処理を行います。</summary>
			public void Dispose()
			{
				buttonList.Clear();
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _privateMembers;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>低位入力制御・管理クラス。</summary>
		public _T lowerInput;

		private ReadOnlyCollection<int> m_assignList = Array.AsReadOnly<int>(new int[0]);

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CInputAdapter()
			: this(null)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="firstState">初期状態。</param> 
		public CInputAdapter(IState firstState)
			: base(firstState, new CPrivateMembers())
		{
			_privateMembers = (CPrivateMembers)privateMembers;
			buttonList = _privateMembers.buttonList.AsReadOnly();
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>入力情報一覧を取得します。</summary>
		/// 
		/// <value>入力情報一覧。</value>
		public IList<SInputInfo> buttonList
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>割り当て一覧を取得します。</summary>
		/// 
		/// <value>割り当て一覧。</value>
		public ReadOnlyCollection<int> assignList
		{
			get
			{
				return m_assignList;
			}
			set
			{
				m_assignList = value;
				int length = assignList.Count;
				if (buttonList.Count != length)	// 数が食い違う場合は初期化
				{
					List<SInputInfo> btns = _privateMembers.buttonList;
					btns.Clear();
					for (int i = length; --i >= 0; )
					{
						btns.Add(new SInputInfo());
					}
				}
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			_privateMembers.Dispose();
			buttonList = new SInputInfo[0];
			base.Dispose();
		}
	}
}
