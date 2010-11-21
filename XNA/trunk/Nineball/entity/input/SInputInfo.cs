////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>入力情報構造体。</summary>
	public struct SInputInfo
		: IDisposable
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>入力方向。</summary>
		public Vector3 velocity;

		/// <summary>前回入力方向との差。</summary>
		public Vector3 velocityGap;

		/// <summary>位置。</summary>
		public Vector3 position;

		/// <summary>押下カウンタ。</summary>
		public int pressCounter;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>前回入力方向を算出し、取得します。</summary>
		/// 
		/// <value>前回入力方向。</value>
		public Vector3 prevVelocity
		{
			get
			{
				return velocity + velocityGap;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在押されているかどうかを取得します。</summary>
		/// 
		/// <value>現在押されている場合、<c>true</c>。</value>
		public bool press
		{
			get
			{
				return velocity.Z > 0;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在押されたかどうかを取得します。</summary>
		/// 
		/// <value>現在押された場合、<c>true</c>。</value>
		public bool push
		{
			get
			{
				return velocity.Z > 0 && prevVelocity.Z == 0;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>入力情報をリセットします。</summary>
		public void Dispose()
		{
			velocity = Vector3.Zero;
			velocityGap = Vector3.Zero;
			position = Vector3.Zero;
			pressCounter = 0;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>キーリピート状態かどうかを取得します。</summary>
		/// 
		/// <param name="delay">リピート開始までの時間。</param>
		/// <param name="interval">リピート間隔。</param>
		public bool pushLoop(int delay, int interval)
		{
			return press && pressCounter >= delay && pressCounter % interval == 0;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>入力方向を更新します。</summary>
		/// 
		/// <param name="velocity">入力方向。</param>
		/// <returns>更新された状態。</returns>
		public SInputInfo updateVelocity(Vector3 velocity)
		{
			velocityGap = velocity - this.velocity;
			this.velocity = velocity;
			if (press)
			{
				pressCounter++;
			}
			else
			{
				pressCounter = 0;
			}
			return this;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>位置を更新します。</summary>
		/// 
		/// <param name="position">位置。</param>
		/// <returns>更新された状態。</returns>
		public SInputInfo updatePosition(Vector3 position)
		{
			updateVelocity(this.position - position);
			this.position = position;
			return this;
		}
	}
}
