////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.data;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity
{
	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>状態を持つオブジェクトの基底クラス。</para>
	/// <para>
	/// これを継承するか、または<c>IEntity</c>を実装することで、
	/// 状態を持つオブジェクトを作ることができます。
	/// </para>
	/// <para>また、このクラスに直接状態を持たせて使用することもできます。</para>
	/// </summary>
	public class CEntity : IEntity
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* events ────────────────────────────────*

		/// <summary>状態が遷移された時に呼び出されるイベント。</summary>
		public event EventHandler<CEventChangedState> changedState;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>型名のキャッシュ。</summary>
		private string m_strTypeName = null;

		/// <summary>現在の状態。</summary>
		private IState m_nowState = CState.empty;

		/// <summary>次の状態。</summary>
		private IState m_nextState = null;

		/// <summary><c>Update</c>メソッド実行後の動作。</summary>
		private Action m_actionAfterUpdate;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CEntity()
		{
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		public IState previousState
		{
			get { throw new NotImplementedException(); }
		}

		public IState currentState
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>汎用フレーム カウンタを取得します。</summary>
		/// 
		/// <value>汎用フレーム カウンタ。</value>
		public int count
		{
			get;
			protected set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// オブジェクトと状態クラスのみがアクセス可能なフィールドを取得します。
		/// </summary>
		/// 
		/// <value>オブジェクトと状態クラスのみがアクセス可能なフィールド。</value>
		protected virtual object privateMembers
		{
			get
			{
				return null;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの状態を含めた文字列情報を取得します。</summary>
		/// 
		/// <value>このオブジェクトを示す文字列情報。</value>
		public override string ToString()
		{
			if (m_strTypeName == null)
			{
				m_strTypeName = GetType().ToString();
			}
			return string.Format("{0} STATE[CUR:{1}, PREV:{2}] {3} MEMBERS:{4}",
				m_strTypeName, currentState, previousState,
				Environment.NewLine, privateMembers);
		}

		public void update(GameTime gameTime)
		{
			throw new NotImplementedException();
		}

		public void draw(GameTime gameTime)
		{
			currentState.draw(this, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public void Dispose()
		{
			changedState = null;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトに空の状態を設定します。</summary>
		public virtual void setEmptyState()
		{
//			nextState = CState.empty;
		}
	}
}
