////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.entity;
using danmaq.nineball.entity.input;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.state.input.detector
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// マンマシンI/F入力制御・管理クラスコレクションの自動認識待機状態。
	/// </summary>
	public sealed class CStateWait : CState<CAI<CInputDetector>, CInputCollection.CPrivateMembers>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateWait instance = new CStateWait();

		/// <summary>要求する自動認識状態の型。</summary>
		private readonly Type detectType =
			typeof(CState<CAI<CInputDetector>, CInputCollection.CPrivateMembers>);

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateWait()
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>状態が開始された時に呼び出されます。</para>
		/// <para>このメソッドは、遷移元の<c>teardown</c>よりも後に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されたオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <exception cref="System.InvalidOperationException">
		/// 自動認識発動時に戻るべき状態が見つからない場合。
		/// </exception>
		public override void setup(
			CAI<CInputDetector> entity, CInputCollection.CPrivateMembers privateMembers)
		{
			Type type = entity.previousState.GetType();
			if(!(type == detectType || type.IsSubclassOf(detectType)))
			{
				throw new InvalidOperationException(
					"戻るべき自動認識状態を見つけることができませんでした。");
			}
			base.setup(entity, privateMembers);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(CAI<CInputDetector> entity,
			CInputCollection.CPrivateMembers privateMembers, GameTime gameTime)
		{
			CInputCollection collection = entity.owner;
			if(collection.Count == 0)
			{
				entity.nextState = (CState<CAI<CInputDetector>, CInputCollection.CPrivateMembers>)
					entity.previousState;
			}
			base.update(entity, privateMembers, gameTime);
		}
	}
}
