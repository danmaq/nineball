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
using danmaq.nineball.util.math;
using Microsoft.Xna.Framework;
using danmaq.nineball.state.manager;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>カメラパス管理クラス。</summary>
	public class CCameraPath
		: CEntity
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>カメラパスデータの構造体。</summary>
		[Serializable]
		public struct SData
		{

			//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
			/// <summary>カメラデータを構成する構造体。</summary>
			public struct SCameraData
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

			/// <summary>現在シーンの時間。</summary>
			public int interval;

			/// <summary>次に移動するシーン(相対指定)。</summary>
			public int next;

			/// <summary>線形補完パターン。</summary>
			public EInterpolate interpolate;

			/// <summary>開始値。</summary>
			public SCameraData start;

			/// <summary>終了値。</summary>
			public SCameraData end;

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>現在のカメラ情報を取得します。</summary>
			/// 
			/// <param name="now">現在の時間。</param>
			/// <returns>現在のカメラ情報。</returns>
			public SCameraData getNow(int now)
			{
				SCameraData data = new SCameraData();
				float amount = interpolate.interpolate(0, 1, now, interval);
				data.up = Vector3.Lerp(start.up, end.up, amount);
				data.from = Vector3.Lerp(start.from, end.from, amount);
				data.to = Vector3.Lerp(start.to, end.to, amount);
				data.fov = interpolate.interpolate(start.fov, end.fov, now, interval);
				return data;
			}

		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>カメラパス定義一覧。</summary>
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
		public CCameraPath()
			: base(CStateCameraPath.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		public CCameraPath(IState firstState)
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
		public CCameraPath(IState firstState, object privateMembers)
			: base(firstState, privateMembers)
		{
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>現在のカメラパス定義を取得します。</summary>
		/// 
		/// <value>現在のカメラパス定義。</value>
		public SData nowScene
		{
			get
			{
				return data[index];
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在のカメラパス定義を取得します。</summary>
		/// 
		/// <value>現在のカメラパス定義。</value>
		public SData.SCameraData nowPath
		{
			get
			{
				return nowScene.getNow(counter);
			}
		}
	}
}
