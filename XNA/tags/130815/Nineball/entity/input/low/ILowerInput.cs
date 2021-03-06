﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.data.input;
using danmaq.nineball.entity.manager;

namespace danmaq.nineball.entity.input.low
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>低位入力制御・管理クラスのインターフェイス。</summary>
	/// 
	/// <typeparam name="_T">入力状態の型。</typeparam>
	public interface ILowerInput<_T>
		: ITask
	{

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>最新の入力状態を取得します。</summary>
		/// 
		/// <value>最新の入力状態。</value>
		_T nowInputState
		{
			get;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>前回の入力状態を取得します。</summary>
		/// 
		/// <value>前回の入力状態。</value>
		_T prevInputState
		{
			get;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>フォース情報を取得/設定します。</summary>
		/// 
		/// <value>フォース情報。</value>
		SForceData force
		{
			get;
			set;
		}
	}
}
