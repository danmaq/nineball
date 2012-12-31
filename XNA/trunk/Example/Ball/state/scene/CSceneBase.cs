////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.ball.core;
using danmaq.nineball.data;
using danmaq.nineball.entity;
using danmaq.nineball.entity.fonts;
using danmaq.nineball.entity.manager;
using danmaq.nineball.state;
using danmaq.nineball.state.manager.task;
using danmaq.nineball.util;
using danmaq.nineball.util.storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.scene
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>シーン基底クラス。</summary>
	abstract class CSceneBase
		: CState<CEntity, CGame>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>シーンの名前。</summary>
		public readonly string sceneName;

		/// <summary>タスク管理 クラス。</summary>
		protected readonly CTaskManager taskManager = new CTaskManager();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="sceneName">シーン名称。</param>
		protected CSceneBase(string sceneName)
		{
			this.sceneName = sceneName;
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
		public override void setup(CEntity entity, CGame privateMembers)
		{
#if TRACE
			CLogger.add(sceneName + "シーンを開始します。");
#endif
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(CEntity entity, CGame privateMembers, GameTime gameTime)
		{
			if (!CGuideWrapper.instance.IsVisible)
			{
				taskManager.update(gameTime);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(CEntity entity, CGame privateMembers, GameTime gameTime)
		{
			taskManager.draw(gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>オブジェクトが別の状態へ移行する時に呼び出されます。</para>
		/// <para>このメソッドは、遷移先の<c>setup</c>よりも先に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を終了したオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="nextState">オブジェクトが次に適用する状態。</param>
		public override void teardown(CEntity entity, CGame privateMembers, IState nextState)
		{
			taskManager.Dispose();
			taskManager.nextState = CStateDefault.instance;
			GC.Collect();
#if TRACE
			CLogger.add(sceneName + "シーンを終了しました。");
#endif
		}

		//* -----------------------------------------------------------------------*
		/// <summary>文字を表示します。</summary>
		/// 
		/// <param name="locate">基準カーソル位置。</param>
		/// <param name="hAlign">水平位置揃え情報。</param>
		/// <param name="color">文字色。</param>
		/// <param name="text">テキスト。</param>
		/// <returns>フォント オブジェクト。</returns>
		protected CFont print(Point locate, EAlign hAlign, Color color, string text)
		{
			CFont result = danmaq.ball.misc.CMisc.create98Font(locate, hAlign, color, text);
			taskManager.Add(result);
			return result;
		}
	}
}
