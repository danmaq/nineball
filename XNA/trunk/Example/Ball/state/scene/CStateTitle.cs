////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.ball.entity.font;
using danmaq.ball.Properties;
using danmaq.nineball.data;
using danmaq.nineball.entity;
using danmaq.nineball.entity.input.data;
using danmaq.nineball.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.scene
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>タイトル画面シーン。</summary>
	public sealed class CStateTitle : CSceneBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateTitle instance = new CStateTitle();

		/// <summary>カーソル。</summary>
		private readonly CCursor cursor = CCursor.instance;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateTitle() : base(Resources.SCENE_TITLE)
		{
			cursor.initialize();
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
		public override void setup(IEntity entity, object privateMembers)
		{
			base.setup(entity, privateMembers);
			CPrint[] printList = {
				new CPrint(Resources.TITLE, new Vector2(40, 7), EAlign.Center, Color.Aqua),
				new CPrint(Resources.CREDIT, new Vector2(40, 9), EAlign.Center, Color.Aqua),
				new CPrint(Resources.DESC_LEVEL, new Vector2(6, 14), EAlign.LeftTop, Color.White),
				new CPrint(string.Format("１{0}２{0}３{0}４{0}５{0}６{0}７{0}８{0}９", "      "),
					new Vector2(6, 16), EAlign.LeftTop, Color.White),
			};
			foreach(CPrint print in printList)
			{
				localGameComponentManager.addDrawableEntity(print);
			}
			cursor.locate = new Vector2(6, 16);
			localGameComponentManager.addDrawableEntity(cursor);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(IEntity entity, object privateMembers, GameTime gameTime)
		{
//			entity.nextState = CStateGame.instance;
			if(inputManager.axisFlag == EDirectionFlags.left)
			{
				CLogger.add("LEFT");
			}
			if(inputManager.dirInputState[(int)EDirection.up].push)
			{
				CLogger.add("UP");
			}
			base.update(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(IEntity entity, object privateMembers, GameTime gameTime)
		{
			base.draw(entity, privateMembers, gameTime);
		}
	}
}
