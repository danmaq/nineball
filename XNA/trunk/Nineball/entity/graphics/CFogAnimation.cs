////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.state;
using danmaq.nineball.state.graphics;
using danmaq.nineball.util.math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.entity.graphics
{

	// TODO : アニメスプライトはまだしも、カメラパスとフォグアニメ、統合できるんじゃね？

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>フォグ アニメーション管理クラス。</summary>
	public class CFogAnimation
		: CEntity
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>フォグ アニメーションの変換済みコンテンツ データ。</summary>
		[Serializable]
		public struct SData
		{

			//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
			/// <summary>フォグデータを構成する構造体。</summary>
			public struct SFogData
			{

				//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
				//* fields ────────────────────────────────*

				/// <summary>色。</summary>
				public Color color;

				/// <summary>開始点。</summary>
				public float near;

				/// <summary>終了点。</summary>
				public float far;
			}

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>現在シーンの時間。</summary>
			public int interval;

			/// <summary>次に移動するシーン(相対指定)。</summary>
			public int next;

			/// <summary>線形補完パターン。</summary>
			public EInterpolate interpolate;

			/// <summary>開始値。</summary>
			public SFogData start;

			/// <summary>終了値。</summary>
			public SFogData end;

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>現在のフォグ情報を取得します。</summary>
			/// 
			/// <param name="now">現在の時間。</param>
			/// <returns>現在のフォグ情報。</returns>
			public SFogData getNow(int now)
			{
				SFogData data = new SFogData();
				float amount = interpolate.interpolate(0, 1, now, interval);
				data.color = Color.Lerp(start.color, end.color, amount);
				data.far = MathHelper.Lerp(start.far, end.far, amount);
				data.near = MathHelper.Lerp(start.near, end.near, amount);
				return data;
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>フォグ アニメーション定義一覧。</summary>
		public readonly List<SData> data = new List<SData>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>インデックス ポインタ。</summary>
		public int index = 0;
		
		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CFogAnimation()
			: base(CStateFogAnimation.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		public CFogAnimation(IState firstState)
			: base(firstState, null)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		/// <param name="privateMembers">
		///	オブジェクトと状態クラスのみがアクセス可能なフィールド。
		///	</param>
		public CFogAnimation(IState firstState, object privateMembers)
			: base(firstState, privateMembers)
		{
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>現在のフォグ アニメーション定義を取得します。</summary>
		/// 
		/// <value>現在のフォグ アニメーション定義。</value>
		public SData nowScene
		{
			get
			{
				return data[index];
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在のフォグ アニメーション定義を取得します。</summary>
		/// 
		/// <value>現在のフォグ アニメーション定義。</value>
		public SData.SFogData nowFog
		{
			get
			{
				return nowScene.getNow(counter);
			}
		}
	}
}
