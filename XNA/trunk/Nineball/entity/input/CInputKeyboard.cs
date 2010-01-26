////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.entity.input.data;
using danmaq.nineball.state;
using danmaq.nineball.state.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>キーボード入力制御・管理クラス。</summary>
	public class CInputKeyboard : CInput
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>ボタンの入力状態一覧。</summary>
			public readonly List<SInputState> buttonStateList;

			/// <summary>キーボード入力制御・管理クラス。</summary>
			private readonly CInputKeyboard keyboard;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="keyboard">キーボード入力制御・管理クラス。</param>
			public CPrivateMembers(CInputKeyboard keyboard)
			{
				this.keyboard = keyboard;
				buttonStateList = keyboard._buttonStateList;
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
					keyboard.axis = value;
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
					return keyboard.axisFlag;
				}
				set
				{
					keyboard.axisFlag = value;
				}
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>方向ボタン割り当て値の一覧。</summary>
		public readonly Keys[] directionAssignList =
		{
			Keys.None,
			Keys.None,
			Keys.None,
			Keys.None
		};

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _privateMemebers;

		/// <summary>ボタン割り当て値の一覧。</summary>
		private readonly List<Keys> m_assignList = new List<Keys>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="playerNumber">プレイヤー番号。</param>
		public CInputKeyboard(short playerNumber)
			: this(playerNumber, CStateKeyboard.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="playerNumber">プレイヤー番号。</param>
		/// <param name="firstState">初期状態</param>
		public CInputKeyboard(short playerNumber, IState firstState)
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
				return true;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンの状態をベクトルで取得します。</summary>
		/// 
		/// <value>方向ボタンの状態。</value>
		public override Vector2 axisVector
		{
			get
			{
				return base.axisVector;
			}
			protected set
			{
				axis = value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// 状態として、nullを設定しようとした場合。
		/// </exception>
		public new IState<CInputKeyboard, CPrivateMembers> nextState
		{
			set
			{
				nextStateBase = value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ボタン割り当て値の一覧を設定/取得します。</summary>
		/// 
		/// <value>ボタン割り当て値の一覧。</value>
		public IList<Keys> assignList
		{
			get
			{
				return m_assignList;
			}
			set
			{
				m_assignList.Clear();
				m_assignList.AddRange(value);
				int buttonsNum = ButtonsNum;
				while(m_assignList.Count > buttonsNum)
				{
					m_assignList.RemoveAt(m_assignList.Count - 1);
				}
			}
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
	}
}
