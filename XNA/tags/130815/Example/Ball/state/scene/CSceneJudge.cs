////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.ball.core;
using danmaq.ball.data;
using danmaq.ball.misc;
using danmaq.nineball.entity;
using danmaq.nineball.entity.fonts;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.state.scene
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>判定画面シーン。</summary>
	sealed class CSceneJudge
		: CSceneBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>勝利時クラス オブジェクト。</summary>
		public static readonly IState<CEntity, CGame> won = new CSceneJudge("勝ち", Color.Aqua);

		/// <summary>敗北時クラス オブジェクト。</summary>
		public static readonly IState<CEntity, CGame> lose = new CSceneJudge("負け", Color.Red);

		/// <summary>カウントダウン表示用フォント。</summary>
		private readonly CFont description = new CFont(CONTENT.texFont98);

		/// <summary>背景色。</summary>
		private readonly Color bgColor;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="text">判定テキスト。</param>
		/// <param name="bgColor">背景色。</param>
		private CSceneJudge(string text, Color bgColor)
			: base(text)
		{
			description.pos = new Vector2(320, 200);
			description.isDrawShadow = false;
			description.sprite = CGame.sprite;
			description.scale = new Vector2(20);
			description.color = Color.Black;
			description.text = text;
			this.bgColor = bgColor;
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
			base.setup(entity, privateMembers);
			CGame.instance.bgColor = bgColor;
			description.gradationMode = false;
			taskManager.Add(description);
			CPresenceSender.sendAtJudge(this);
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
			if (entity.counter - entity.lastStateChangeCounter >= 30)
			{
				entity.nextState = CSceneMenu.instance;
			}
			base.update(entity, privateMembers, gameTime);
		}
	}
}
