////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.util.math;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.data.animation
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>カメラパスデータの構造体。</summary>
	[Serializable]
	public struct SCameraPathData
		: IAnimationData<SCameraPathData.SData>
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>カメラデータを構成する構造体。</summary>
		public struct SData
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>カメラ位置。</summary>
			public Vector3 from;

			/// <summary>カメラ注視先。</summary>
			public Vector3 to;

			/// <summary>カメラ上方向(ロール制御用)。</summary>
			public Vector3 up;

			/// <summary>視野の角度。</summary>
			public float fov;

			//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* properties ──────────────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>現在のビュー行列を取得します。</summary>
			/// 
			/// <value>現在のビュー行列。</value>
			public Matrix view
			{
				get
				{
					return Matrix.CreateLookAt(from, to, up);
				}
			}

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>現在の射影行列を取得します。</summary>
			/// 
			/// <param name="aspect">アスペクト比。</param>
			/// <param name="clipNear">手前のクリップ境界。</param>
			/// <param name="clipFar">奥のクリップ境界。</param>
			/// <returns>現在の射影行列。</returns>
			public Matrix getProjection(float aspect, float clipNear, float clipFar)
			{
				return Matrix.CreatePerspectiveFieldOfView(fov, aspect, clipNear, clipFar);
			}
		}

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>線形補完パターン。</summary>
		public EInterpolate interpolate;

		/// <summary>開始値。</summary>
		public SData start;

		/// <summary>終了値。</summary>
		public SData end;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>現在シーンが有効な時間を取得/設定します。</summary>
		/// 
		/// <value>現在シーンが有効な時間。</value>
		public int interval
		{
			get;
			set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>次に移動するシーンを相対値で取得/設定します。</summary>
		/// 
		/// <value>次に移動するシーン(相対指定)。</value>
		public int next
		{
			get;
			set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>現在のカメラ情報を取得します。</summary>
		/// 
		/// <param name="now">現在の時間。</param>
		/// <returns>現在のカメラ情報。</returns>
		public SData getNow(int now)
		{
			SData data = new SData();
			float amount = 0.5f * 
				(interpolate.interpolate(0, 1, now, interval) +
				CInterpolate.amountLinear(now, interval));
			data.up = Vector3.Lerp(start.up, end.up, amount);
			data.from = Vector3.Lerp(start.from, end.from, amount);
			data.to = Vector3.Lerp(start.to, end.to, amount);
			data.fov = MathHelper.Lerp(start.fov, end.fov, amount);
			return data;
		}
	}
}
