////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.entity.fonts;
using danmaq.nineball.state.manager;
using danmaq.nineball.util.math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.state.fonts
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>FPS表示用の状態です。</summary>
	public sealed class CStateFPSViewer
		: CState<CFont, object>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateFPSViewer instance = new CStateFPSViewer();

		/// <summary>接続先。</summary>
		private readonly IState adaptee = CStateDefault.instance;

		/// <summary>FPS計測・計算クラス。</summary>
		private readonly CStateFPSCalculator calcurator = CStateFPSCalculator.instance;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>テキスト。</summary>
		public string text = "FPS[UPDATE:{0} / DRAW:{1}]";

		/// <summary>FPSが真っ赤になる誤差値。</summary>
		public int redzone = 20;

		/// <summary>前回計測時の更新FPS。</summary>
		private int prevFPSUpdate;

		/// <summary>前回計測時の描画FPS。</summary>
		private int prevFPSDraw;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateFPSViewer()
		{
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
		public override void setup(CFont entity, object privateMembers)
		{
			adaptee.setup(entity, privateMembers);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(CFont entity, object privateMembers, GameTime gameTime)
		{
			if (entity.counter % 60 == 0)
			{
				int fpsUpdate = calcurator.fpsUpdate;
				int fpsDraw = calcurator.fpsDraw;
				if (prevFPSUpdate != fpsUpdate || prevFPSDraw != fpsDraw)
				{
					prevFPSUpdate = fpsUpdate;
					prevFPSDraw = fpsDraw;
					entity.color = Color.Lerp(Color.White, Color.Red,
						CInterpolate.amountOutQuadLoop(
							Math.Min(Math.Abs(60 - fpsUpdate), redzone), redzone));
					entity.text = string.Format(text, fpsUpdate.ToString(), fpsDraw.ToString());
				}
			}
			adaptee.update(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(CFont entity, object privateMembers, GameTime gameTime)
		{
			adaptee.draw(entity, privateMembers, gameTime);
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
		public override void teardown(CFont entity, object privateMembers, IState nextState)
		{
			adaptee.teardown(entity, privateMembers, nextState);
		}
	}
}
