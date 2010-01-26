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
using danmaq.nineball.data;
using danmaq.nineball.entity.input.data;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.input
{

	// TODO : ボタンを「挿入」できないかなぁ

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>マンマシンI/F入力制御・管理機能の基底クラス。</summary>
	public abstract class CInput : CEntity
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>ボタンの入力状態一覧。</summary>
		protected readonly List<SInputState> _buttonStateList = new List<SInputState>(1);

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>アナログ入力を認識する閾値。</summary>
		public float analogThreshold = 0.5f;

		/// <summary>方向ボタンの状態。</summary>
		protected Vector2 axis = Vector2.Zero;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* events ────────────────────────────────*

		/// <summary>ボタンの数が変更されたときに発生するイベント。</summary>
		public event EventHandler<CEventMonoValue<ushort>> changedButtonsNum;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="playerNumber">プレイヤー番号。</param>
		/// <param name="firstState">初期状態。</param>
		public CInput(short playerNumber, IState firstState)
			: base(firstState)
		{
			this.playerNumber = playerNumber;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// 状態として、nullを設定しようとした場合。
		/// </exception>
		public new IState<CInput, List<SInputState>> nextState
		{
			set
			{
				nextStateBase = value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ボタンの入力状態一覧を取得します。</summary>
		/// 
		/// <value>ボタンの入力状態一覧。</value>
		public ReadOnlyCollection<SInputState> buttonStateList
		{
			get
			{
				return _buttonStateList.AsReadOnly();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ボタンの数を設定/取得します。</summary>
		/// 
		/// <value>ボタンの数。</value>
		public virtual ushort ButtonsNum
		{
			get
			{
				return (ushort)_buttonStateList.Count;
			}
			set
			{
				bool bChanged = value != ButtonsNum;
				while(value < ButtonsNum)
				{
					_buttonStateList.RemoveAt(_buttonStateList.Count - 1);
				}
				while(value > ButtonsNum)
				{
					_buttonStateList.Add(new SInputState());
				}
				if(bChanged && changedButtonsNum != null)
				{
					changedButtonsNum(this, ButtonsNum);
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>プレイヤー番号を取得します。</summary>
		/// 
		/// <value>プレイヤー番号。</value>
		public virtual short playerNumber
		{
			get;
			protected set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンの状態をベクトルで取得します。</summary>
		/// 
		/// <value>方向ボタンの状態。</value>
		public virtual Vector2 axisVector
		{
			get
			{
				return axis;
			}
			protected set
			{
				axis = value;
				axisFlag = EDirectionFlags.None;
				if(value.Length() >= analogThreshold)
				{
					if(-MathHelper.Min(value.Y, 0) > analogThreshold)
					{
						axisFlag |= EDirectionFlags.up;
					}
					if(MathHelper.Max(value.Y, 0) > analogThreshold)
					{
						axisFlag |= EDirectionFlags.down;
					}
					if(-MathHelper.Min(value.X, 0) > analogThreshold)
					{
						axisFlag |= EDirectionFlags.left;
					}
					if(MathHelper.Max(value.X, 0) > analogThreshold)
					{
						axisFlag |= EDirectionFlags.right;
					}
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンの状態をフラグで取得します。</summary>
		/// <example>
		/// bool bDown = (obj.axisFlag &amp; EDirectionFlags.down) != 0;
		/// </example>
		/// 
		/// <value>方向ボタンの状態。</value>
		public virtual EDirectionFlags axisFlag
		{
			get;
			protected set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>接続されているかどうかを取得します。</summary>
		/// 
		/// <value>接続されている場合、<c>true</c>。</value>
		public abstract bool connect
		{
			get;
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
				return _buttonStateList;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// 状態として、nullを設定しようとした場合。
		/// </exception>
		protected IState nextStateBase
		{
			set
			{
				base.nextState = value;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			changedButtonsNum = null;
			_buttonStateList.Clear();
			_buttonStateList.TrimExcess();
			playerNumber = -1;
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ボタン数が変化したときに呼び出されるメソッドです。</summary>
		/// 
		/// <param name="sender">送信元のオブジェクト。</param>
		/// <param name="e">変化後のボタンの数。</param>
		public void onChangedButtonsNum(object sender, CEventMonoValue<ushort> e)
		{
			ButtonsNum = e;
		}
	}
}
