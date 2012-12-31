////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.data.input;
using danmaq.nineball.state;
using danmaq.nineball.state.input.low;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.input.low
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XNA汎用低位入力制御・管理クラス。</summary>
	/// 
	/// <typeparam name="_T">入力状態の型。</typeparam>
	public class CXNAInput<_T>
		: CEntity, ILowerInput<_T>
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers : IDisposable
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>フォース フィードバック用AI。</summary>
			public CEntity aiForce;

			/// <summary>最新の入力状態。</summary>
			public _T nowState;

			/// <summary>前回の入力状態。</summary>
			public _T prevState;

			/// <summary>割り当てられたプレイヤー番号。</summary>
			public PlayerIndex playerIndex;

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>このオブジェクトの終了処理を行います。</summary>
			public void Dispose()
			{
				aiForce.Dispose();
				nowState = default(_T);
				prevState = default(_T);
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _privateMembers;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>フォース情報。</summary>
		private SForceData m_force;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CXNAInput()
			: this(null)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="firstState">初期状態。</param> 
		public CXNAInput(IState firstState)
			: base(firstState, new CPrivateMembers())
		{
			_privateMembers = (CPrivateMembers)privateMembers;
			_privateMembers.aiForce = new CEntity(null, this);
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>最新の入力状態を取得します。</summary>
		/// 
		/// <value>最新の入力状態。</value>
		public _T nowInputState
		{
			get
			{
				return _privateMembers.nowState;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>前回の入力状態を取得します。</summary>
		/// 
		/// <value>前回の入力状態。</value>
		public _T prevInputState
		{
			get
			{
				return _privateMembers.prevState;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>割り当てられたプレイヤー番号を取得します。</summary>
		/// 
		/// <value>割り当てられたプレイヤー番号。</value>
		public PlayerIndex playerIndex
		{
			get
			{
				return _privateMembers.playerIndex;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>フォース情報を取得/設定します。</summary>
		/// 
		/// <value>フォース情報。</value>
		public SForceData force
		{
			get
			{
				return m_force;
			}
			set
			{
				m_force = value;
				_privateMembers.aiForce.nextState = CStateGamePadForceFeedback<_T>.instance;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			_privateMembers.Dispose();
			base.Dispose();
		}
	}
}
