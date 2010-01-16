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

#if XBOX360
using Microsoft.Xna.Framework.GamerServices;
#endif

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ボタンの入力状態を保持するクラス。</summary>
	[Serializable]
	public struct SInputState
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>連続入力となるまでのフレーム時間間隔。</summary>
		public static ushort loopStart = 30;

		/// <summary>押しっぱなしで連続入力となるフレーム時間間隔。</summary>
		public static ushort loopInterval = 5;

		/// <summary>アナログ入力を認識する閾値。</summary>
		public static float analogThreshold = 0.12f;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>現在ボタンが押されているかどうかを取得します。</summary>
		/// 
		/// <value>現在ボタンが押されている場合、<c>true</c>。</value>
		public bool press
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>最後にボタンの状態が更新されてからの時間を取得します。</summary>
		/// 
		/// <value>最後にボタンの状態が更新されてからの時間。</value>
		public int count
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アナログ入力値を取得します。</summary>
		/// 
		/// <value>アナログ入力値。</value>
		public float analogValue
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>閾値でハイパスフィルタしたアナログ入力値を取得します。</summary>
		/// 
		/// <value>閾値でハイパスフィルタしたアナログ入力値。</value>
		public float analogHPF
		{
			get
			{
				return analogValue < analogThreshold ? 0 : analogValue;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在のフレームでボタンが押されたかどうかを取得します。</summary>
		/// 
		/// <value>現在のフレームでボタンが押された場合、<c>true</c>。</value>
		public bool push
		{
			get
			{
				return (press && count == 0);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>現在のフレームでボタンが押されたかどうかを取得します。</para>
		/// <para>
		/// <c>loopInterval</c>のフレーム時間が経過するたびに毎回<c>true</c>となります。
		/// </para>
		/// </summary>
		/// 
		/// <value>現在のフレームでボタンが押された場合、<c>true</c>。</value>
		public bool pushLoop
		{
			get
			{
				return (press && countLoop == 0);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>最後にボタンの状態が更新されてからの時間を取得します。</para>
		/// <para>
		/// <c>loopInterval</c>のフレーム時間が経過するたびに毎回リセットされます。
		/// </para>
		/// </summary>
		/// 
		/// <value>最後にボタンの状態が更新されてからの時間。</value>
		public int countLoop
		{
			get
			{
				return ((count >= loopStart) ? (count % loopInterval) : count);
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つのボタンの押下情報をOR合成します。</para>
		/// <para>いずれかが押下されているかを判定するのに便利です。</para>
		/// </summary>
		/// 
		/// <param name="a">ボタン状態。</param>
		/// <param name="b">ボタン状態。</param>
		/// <returns>合成されたボタン状態。</returns>
		public static SInputState operator |(SInputState a, SInputState b)
		{
			SInputState result = new SInputState();
			result.press = a.press || b.press;
			result.count = Math.Min(a.count, b.count);
			result.analogValue = MathHelper.Max(a.analogValue, b.analogValue);
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>2つのボタンの押下情報をAND合成します。</para>
		/// <para>同時押しの判定などに便利です。</para>
		/// </summary>
		/// 
		/// <param name="a">ボタン状態。</param>
		/// <param name="b">ボタン状態。</param>
		/// <returns>合成されたボタン状態。</returns>
		public static SInputState operator &(SInputState a, SInputState b)
		{
			SInputState result = new SInputState();
			result.press = a.press && b.press;
			result.count = Math.Max(a.count, b.count);
			result.analogValue = MathHelper.Min(a.analogValue, b.analogValue);
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在ボタンが押されているかどうかを取得します。</summary>
		/// 
		/// <param name="b">ボタン状態</param>
		/// <returns>現在ボタンが押されている場合、<c>true</c></returns>
		public static implicit operator bool(SInputState b)
		{
			return b.press;
		}

#if XBOX360
		//* -----------------------------------------------------------------------*
		/// <summary>ゲーマー情報を参考にアナログ入力を認識する閾値を自動設定します。</summary>
		/// 
		/// <param name="cs">ゲーマー設定のアナログ入力感度。</param>
		public static void setAnalogThreshold( ControllerSensitivity cs ) {
			switch( cs ) {
				case ControllerSensitivity.High:
					analogThreshold = 0.06f;
					break;
				case ControllerSensitivity.Medium:
					analogThreshold = 0.12f;
					break;
				case ControllerSensitivity.Low:
					analogThreshold = 0.2f;
					break;
			}
		}
#endif

		//* -----------------------------------------------------------------------*
		/// <summary>ボタン状態の更新をします。</summary>
		/// 
		/// <param name="bState">最新のボタン状態</param>
		public void refresh(bool bState)
		{
			if(press == bState)
			{
				count++;
			}
			else
			{
				analogValue = bState ? 1f : 0f;
				press = bState;
				count = 0;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アナログボタン状態の更新をします。</summary>
		/// 
		/// <param name="fAnalogValue">最新のアナログボタン状態</param>
		public void refresh(float fAnalogValue)
		{
			if(
				(press && fAnalogValue >= analogThreshold) ||
				(!press && fAnalogValue < analogThreshold)
			)
			{
				count++;
			}
			else
			{
				press = !press;
				count = 0;
			}
			analogValue = fAnalogValue;
		}
	}
}
