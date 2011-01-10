////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if false

using danmaq.ball.core;
using danmaq.ball.entity.font;
using danmaq.ball.state.scene;
using danmaq.nineball;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;

namespace danmaq.ball.state.font.cursor
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>カーソル制御状態クラス。</summary>
	public sealed class CStateCursor : CState<CCursor, object>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateCursor instance = new CStateCursor();

		/// <summary>入力管理クラス。</summary>
		private readonly CInput inputManager = CGame.instance.inputManager;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>ゲーム難易度。</summary>
		private short m_sLevel = 0;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateCursor()
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(CCursor entity, object privateMembers, GameTime gameTime)
		{
			bool bLeftInput = inputManager.dirInputState[(int)EDirection.left].pushLoop;
			bool bRightInput = inputManager.dirInputState[(int)EDirection.right].pushLoop;
			if(bLeftInput || bRightInput)
			{
				Vector2 locate = entity.locate;
				short sGap = (short)(bLeftInput ? -1 : 1);
				m_sLevel += sGap;
				locate.X += 8 * sGap;
				if(locate.X < 6)
				{
					locate.X = 70;
					m_sLevel = 8;
				}
				else if(locate.X > 70)
				{
					locate.X = 6;
					m_sLevel = 0;
				}
				entity.locate = locate;
			}
			if(inputManager.buttonStateList[0].push)
			{
				CStateGame.instance.level = (ushort)m_sLevel;
				CStarter.scene.nextState = CStateGame.instance;
			}
			base.update(entity, privateMembers, gameTime);
		}
	}
}

#endif