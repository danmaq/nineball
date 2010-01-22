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
using danmaq.nineball.state;
using danmaq.nineball.state.input;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.input
{

	// TODO : ボタンを「挿入」できないかなぁ

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>コントローラ 入力制御・管理クラス。</summary>
	public class CInputParent : CEntity
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>ボタンの入力状態一覧。</summary>
		protected readonly List<SInputState> _buttonStateList = new List<SInputState>(1);

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* events ────────────────────────────────*

		/// <summary>ボタンの数が変更されたときに発生するイベント。</summary>
		public event EventHandler<CEventMonoValue<ushort>> changedButtonsNum;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

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
		public ushort ButtonsNum
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

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CInputParent() : this(CStateDefault.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="firstState">初期状態。</param>
		public CInputParent(IState firstState)
		{
			nextStateBase = firstState;
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
		public new IState<CInputParent, List<SInputState>> nextState
		{
			set
			{
				nextStateBase = value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>プレイヤー番号を取得します。</summary>
		/// 
		/// <value>プレイヤー番号。</value>
		public ushort playerNumber
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>接続されているかどうかを取得します。</summary>
		/// 
		/// <value>接続されている場合、<c>true</c>。</value>
		public bool connect
		{
			get
			{
				throw new NotImplementedException();
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

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>移動ボタンのベクトルを計算します。</summary>
		/// 
		/// <returns>移動ベクトル。</returns>
		public static Vector2 createVector(
			float up, float down, float left, float right
		)
		{
			float[] srcList = { up, down, left, right };
			float fVelocity = 0;
			foreach(float fSrc in srcList)
			{
				fVelocity = MathHelper.Max(fVelocity, Math.Abs(fSrc));
			}
			Vector2 result = new Vector2(-left, -up) + new Vector2(right, down);
			result.Normalize();
			return result * fVelocity;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			changedButtonsNum = null;
			_buttonStateList.Clear();
			_buttonStateList.TrimExcess();
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
