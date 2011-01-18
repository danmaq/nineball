////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.data.animation;
using danmaq.nineball.state;
using danmaq.nineball.state.manager;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>アニメーション管理クラス。</summary>
	/// 
	/// <typeparam name="_T">一定時間分のアニメーション データ。</typeparam>
	/// <typeparam name="_D">
	/// アニメーション データから1フレームだけを切り出したデータ。
	/// </typeparam>
	public class CAnimation<_T, _D> : CEntity
		where _T : IAnimationData<_D>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>既定の状態。</summary>
		public readonly IState defaultState;

		/// <summary>アニメーション定義一覧。</summary>
		public readonly List<_T> data = new List<_T>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>インデックス ポインタ。</summary>
		public int index = 0;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CAnimation()
			: this(CStateAnimation<_T, _D>.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		public CAnimation(IState firstState)
			: this(firstState, null)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		/// <param name="privateMembers">
		///	オブジェクトと状態クラスのみがアクセス可能なフィールド。
		///	</param>
		public CAnimation(IState firstState, object privateMembers)
			: base(firstState, privateMembers)
		{
			defaultState = CStateAnimation<_T, _D>.instance;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>現在のデータを取得します。</summary>
		/// 
		/// <value>現在のデータ。</value>
		public _T nowScene
		{
			get
			{
				return data[index];
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在のデータを取得します。</summary>
		/// 
		/// <value>現在のデータ。</value>
		public _D nowData
		{
			get
			{
				return nowScene.getNow(counter);
			}
		}
	}
}
