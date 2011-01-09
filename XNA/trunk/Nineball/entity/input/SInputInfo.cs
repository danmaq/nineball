////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
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

		/// <summary>汎用カウンタ。</summary>
		public int counter;

		/// <summary>最後に押下した時間。</summary>
		public int lastPressTimeX;

		/// <summary>最後に押下した時間。</summary>
		public int lastPressTimeY;

		/// <summary>最後に押下した時間。</summary>
		public int lastPressTimeZ;

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
		public bool pressZ
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
				return
					lastPressTimeX == counter ||
					lastPressTimeY == counter ||
					lastPressTimeZ == counter;
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
			counter = 0;
			lastPressTimeX = 0;
			lastPressTimeY = 0;
			lastPressTimeZ = 0;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>キーリピート状態かどうかを取得します。</summary>
		/// 
		/// <param name="delay">リピート開始までの時間。</param>
		/// <param name="interval">リピート間隔。</param>
		public bool pushLoop(int delay, int interval)
		{
			int elapsed =
				counter - Math.Max(Math.Max(lastPressTimeX, lastPressTimeY), lastPressTimeZ);
			return velocity.Length() > 0 && (elapsed == 0 || (elapsed >= delay && elapsed % interval == 0));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>キーリピート状態かどうかを取得します。</summary>
		/// 
		/// <param name="delay">リピート開始までの時間。</param>
		/// <param name="interval">リピート間隔。</param>
		public bool pushLoopX(int delay, int interval)
		{
			int elapsed = counter - lastPressTimeX;
			return velocity.X > 0 && (elapsed == 0 || (elapsed >= delay && elapsed % interval == 0));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>キーリピート状態かどうかを取得します。</summary>
		/// 
		/// <param name="delay">リピート開始までの時間。</param>
		/// <param name="interval">リピート間隔。</param>
		public bool pushLoopY(int delay, int interval)
		{
			int elapsed = counter - lastPressTimeY;
			return velocity.Y > 0 && (elapsed == 0 || (elapsed >= delay && elapsed % interval == 0));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>キーリピート状態かどうかを取得します。</summary>
		/// 
		/// <param name="delay">リピート開始までの時間。</param>
		/// <param name="interval">リピート間隔。</param>
		public bool pushLoopZ(int delay, int interval)
		{
			int elapsed = counter - lastPressTimeZ;
			return pressZ && (elapsed == 0 || (elapsed >= delay && elapsed % interval == 0));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>入力方向を更新します。</summary>
		/// 
		/// <param name="velocity">入力方向。</param>
		/// <returns>更新された状態。</returns>
		public SInputInfo updateVelocity(Vector3 velocity)
		{
			counter++;
			velocityGap = velocity - this.velocity;
			if (Math.Abs(velocity.X) > 0 && this.velocity.X == 0)
			{
				lastPressTimeX = counter;
			}
			if (Math.Abs(velocity.Y) > 0 && this.velocity.Y == 0)
			{
				lastPressTimeX = counter;
			}
			if (Math.Abs(velocity.Z) > 0 && this.velocity.Z == 0)
			{
				lastPressTimeZ = counter;
			}
			this.velocity = velocity;
			return this;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>入力方向を更新します。</summary>
		/// 
		/// <param name="velocity">入力方向。</param>
		/// <param name="threshold">閾値。</param>
		/// <returns>更新された状態。</returns>
		public SInputInfo updateVelocityWithAxisHPF(Vector3 velocity, float threshold)
		{
			Vector2 v2 = new Vector2(velocity.X, velocity.Y);
			if (v2.Length() < threshold)
			{
				velocity.X = 0;
				velocity.Y = 0;
			}
			else
			{
				float axisThreshold = threshold * 0.3333f;
				if (Math.Abs(velocity.X) < axisThreshold)
				{
					velocity.X = 0;
				}
				if (Math.Abs(velocity.Y) < axisThreshold)
				{
					velocity.Y = 0;
				}
			}
			if (velocity.Z < threshold)
			{
				velocity.Z = 0;
			}
			return updateVelocity(velocity);
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
