////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>入力情報構造体。</summary>
	public struct SInputInfo
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>入力方向。</summary>
		public Vector3 velocity;

		/// <summary>前回入力方向との差。</summary>
		public Vector3 velocityGap;

		/// <summary>位置。</summary>
		public Vector3 position;

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

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>入力方向を更新します。</summary>
		/// 
		/// <param name="velocity">入力方向。</param>
		/// <returns>更新された状態。</returns>
		public SInputInfo updateVelocity(Vector3 velocity)
		{
			velocityGap = velocity - this.velocity;
			this.velocity = velocity;
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
