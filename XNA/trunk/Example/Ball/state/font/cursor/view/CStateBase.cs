////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.entity;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;

namespace danmaq.ball.state.font.cursor.view
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>カーソル状態基底クラス。</summary>
	public abstract class CStateBase : CState<CEntity, Matrix>
	{

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="world">カーソルの3D位置を示すワールド行列。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(CEntity entity, Matrix world, GameTime gameTime)
		{
			base.update(entity, world, gameTime);
			if ((DateTime.Now - entity.lastStateChangeTime).Milliseconds > 500)
			{
				entity.nextState = onBlink();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>カーソルの明滅を切り替えます。</summary>
		/// 
		/// <returns>切り替え先の状態。</returns>
		protected abstract CStateBase onBlink();
	}
}
