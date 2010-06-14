////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.state;

namespace danmaq.nineball.entity
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>AIオブジェクト。</summary>
	/// 
	/// <typeparam name="_O">親オブジェクト。</typeparam>
	public sealed class CAI<_O> : CEntity where _O : IEntity
	{

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="entity">親オブジェクト。</param>
		public CAI(_O entity)
			: base(entity, CState.empty)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="entity">親オブジェクト。</param>
		/// <param name="state">初期状態。</param>
		public CAI(_O entity, IState state)
			: base(entity, state)
		{
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトを所有する親オブジェクトを取得します。</summary>
		/// 
		/// <value>親オブジェクト。</value>
		public new _O owner
		{
			get
			{
				return (_O)base.owner;
			}
		}
	}
}
