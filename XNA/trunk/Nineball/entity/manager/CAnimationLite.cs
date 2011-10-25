////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.state.manager;
using danmaq.nineball.util.math;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>軽量なアニメーション管理クラス。</summary>
	public class CAnimationLite
		: CEntity
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>アニメーションに要する時間。</summary>
		public int interval;

		/// <summary>現在の値。</summary>
		public float now;

		/// <summary>状態開始時の値。</summary>
		public float start;

		/// <summary>目標値。</summary>
		public float target;

		/// <summary>アニメーションに使用する内分カウンタ。</summary>
		public EInterpolate interpolate;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CAnimationLite()
		{
			reset();
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			reset();
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アニメーションを開始します。</summary>
		/// 
		/// <param name="start">開始値。</param>
		/// <param name="target">目標値。</param>
		/// <param name="interval">アニメーションに要する時間。</param>
		/// <param name="interpolate">アニメーションに使用する内分カウンタ。</param>
		public void run(float start, float target, int interval, EInterpolate interpolate)
		{
			reset(start, target, interval, interpolate);
			nextState = CStateAnimationLite.instance;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>値を初期化します。</summary>
		private void reset()
		{
			reset(0f, 0f, 60, EInterpolate.clampLinear);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>値を初期化します。</summary>
		/// 
		/// <param name="start">開始値。</param>
		/// <param name="target">目標値。</param>
		/// <param name="interval">アニメーションに要する時間。</param>
		/// <param name="interpolate">アニメーションに使用する内分カウンタ。</param>
		public void reset(float start, float target, int interval, EInterpolate interpolate)
		{
			resetCounter();
			this.start = start;
			this.now = start;
			this.target = target;
			this.interval = interval;
			this.interpolate = interpolate;
		}
	}
}
