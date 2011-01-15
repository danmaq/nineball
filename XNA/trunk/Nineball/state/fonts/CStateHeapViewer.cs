////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.entity.fonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.state.fonts
{
	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ヒープ メモリ使用量表示用の状態です。</summary>
	public sealed class CStateHeapViewer
		: CState<CFont, object>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateHeapViewer instance = new CStateHeapViewer();

		/// <summary>起動時ヒープ メモリ。</summary>
		public readonly long firstHeap;

		/// <summary>接続先。</summary>
		private readonly IState adaptee = CStateDefault.instance;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>テキスト。</summary>
		public string text = "MEM[USE:{0} KB / DELTA:{1}(+{2}) KB]";

		/// <summary>デルタが連続して正数を示した回数。</summary>
		private int plusCount;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateHeapViewer()
		{
			GC.Collect();
			firstHeap = GC.GetTotalMemory(false);
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>ヒープ メモリの使用量を取得します。</summary>
		/// 
		/// <value>ヒープ メモリの使用量。</value>
		public long heap
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ヒープ メモリの秒ごとのデルタを取得します。</summary>
		/// 
		/// <value>ヒープ メモリの秒ごとのデルタ。</value>
		public long delta
		{
			get;
			private set;
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
			entity.color = Color.YellowGreen;
			plusCount = 0;
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
			if (entity.counter % 30 == 0)
			{
				long use = GC.GetTotalMemory(false);
				long newDelta = use - heap;
				long newHeap = use;
				if (delta != newDelta || heap != newHeap)
				{
					delta = newDelta;
					heap = newHeap;
					Color color = Color.White;
					if (delta > 0)
					{
						if (++plusCount > 3)
						{
							color = Color.Orange;
						}
					}
					else
					{
						plusCount = 0;
						if (delta == 0)
						{
							color = Color.YellowGreen;
						}
						else if (delta < 0)
						{
							color = Color.Red;
						}
					}
					entity.color = color;
					entity.text = string.Format(text,
						(heap / 1024).ToString(),
						Math.Ceiling(delta / 1024f).ToString(),
						((heap - firstHeap) / 1024).ToString());
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
