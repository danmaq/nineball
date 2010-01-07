////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.entity.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.input {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360対応コントローラ専用の入力状態。</summary>
	public sealed class CStateXBOX360Controller: CState<CInput, List<SInputState>> {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateXBOX360Controller instance = new CStateXBOX360Controller();

		public PlayerIndex? primaryPlayer = null;

		/// <summary>ボタン割り当て値の一覧。</summary>
		private readonly List<Buttons> assignList = new List<Buttons>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateXBOX360Controller() { }

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 現在のボタン割り当て一覧を破棄して、新しい割り当てを設定します。
		/// </summary>
		/// 
		/// <param name="collection">ボタン割り当て一覧。</param>
		public void setAssignList( IEnumerable<Buttons> collection ) {
			assignList.Clear();
			assignList.AddRange( collection );
		}
	}
}
