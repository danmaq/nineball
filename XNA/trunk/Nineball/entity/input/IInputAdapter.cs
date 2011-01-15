////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Collections.ObjectModel;
using danmaq.nineball.data.input;
using danmaq.nineball.entity.manager;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>高位入力制御・管理クラスのインターフェイス。</summary>
	public interface IInputAdapter
		: ITask
	{

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>入力情報一覧を取得します。</summary>
		/// 
		/// <value>入力情報一覧。</value>
		IList<SInputInfo> buttonList
		{
			get;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>割り当て一覧を取得します。</summary>
		/// 
		/// <value>割り当て一覧。</value>
		ReadOnlyCollection<int> assignList
		{
			get;
			set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>入力情報をリセットします。</summary>
		void reset();
	}
}
