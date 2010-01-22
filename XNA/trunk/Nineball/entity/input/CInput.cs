////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.ObjectModel;
using danmaq.nineball.data;
using System.Collections.Generic;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>マンマシンI/F入力制御・管理機能の基底クラス。</summary>
	public abstract class CInput : CEntity
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
		public abstract ushort playerNumber
		{
			get;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>接続されているかどうかを取得します。</summary>
		/// 
		/// <value>接続されている場合、<c>true</c>。</value>
		public abstract bool connect
		{
			get;
		}
	}
}
