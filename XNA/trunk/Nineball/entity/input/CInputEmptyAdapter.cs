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
using System.Collections.ObjectModel;
using danmaq.nineball.data.input;
using danmaq.nineball.state;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>空の高位入力制御・管理クラス。</summary>
	public class CInputEmptyAdapter
		: CEntity, IInputAdapter
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers : IDisposable
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

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
		protected readonly CPrivateMembers _privateMembers;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>アナログ入力におけるハイパス値(0～1)。</summary>
		public float threshold = 0.25f;

		/// <summary>割り当て一覧。</summary>
		private ReadOnlyCollection<int> m_assignList = new List<int>().AsReadOnly();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CInputEmptyAdapter()
			: this(null)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="firstState">初期状態。</param> 
		public CInputEmptyAdapter(IState firstState)
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
		public virtual ReadOnlyCollection<int> assignList
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

		//* -----------------------------------------------------------------------*
		/// <summary>入力情報をリセットします。</summary>
		public void reset()
		{
			List<SInputInfo> btns = _privateMembers.buttonList;
			for (int i = btns.Count; --i >= 0; btns[i].Dispose())
				;
		}
	}
}
