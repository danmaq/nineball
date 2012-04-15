////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using Microsoft.Xna.Framework;

namespace danmaq.nineball.util.math
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ベクトル関連の算術関数集クラス。</summary>
	public static class CVectorUtil
	{

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ベクトルのクロス積を計算します。</summary>
		/// 
		/// <param name="expr1">ベクトル。</param>
		/// <param name="expr2">ベクトル。</param>
		/// <returns>2つのベクトルのクロス積。</returns>
		public static float cross(Vector2 expr1, Vector2 expr2)
		{
			return expr1.X * expr2.Y - expr1.Y * expr2.X;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ベクトルを単位ベクトルに変換します。</summary>
		/// 
		/// <param name="expr">ベクトル。</param>
		/// <param name="alternative">
		/// <paramref name="expr"/>がゼロだった際の代替ベクトル。
		/// </param>
		/// <returns>単位ベクトル。</returns>
		public static Vector2 normalize(Vector2 expr, Vector2 alternative)
		{
			return normalize(expr, alternative, 1);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ベクトルを指定した長さに変換します。</summary>
		/// 
		/// <param name="expr">ベクトル。</param>
		/// <param name="alternative">
		/// <paramref name="expr"/>がゼロだった際の代替ベクトル。
		/// </param>
		/// <param name="unit">長さ。</param>
		/// <returns>ベクトル。</returns>
		public static Vector2 normalize(Vector2 expr, Vector2 alternative, float unit)
		{
			if (expr == Vector2.Zero)
			{
				expr = alternative;
			}
			expr.Normalize();
			return expr * unit;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>速度・角度からベクトルを取得します。</summary>
		/// 
		/// <param name="speed">速度。</param>
		/// <param name="angle">角度(ラジアン)。</param>
		/// <returns>ベクトル。</returns>
		public static Vector2 createVector2(float speed, float angle)
		{
			return new Vector2(speed, 0).rotate(angle);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ベクトルを回転した結果を取得します。</summary>
		/// <remarks>この計算によって、元のベクトルが変化することはありません。</remarks>
		/// 
		/// <param name="source">元のベクトル。</param>
		/// <param name="angle">角度(ラジアン)。</param>
		/// <returns>回転されたベクトル。</returns>
		public static Vector2 rotate(this Vector2 source, float angle)
		{
			return Vector2.Transform(source, Quaternion.CreateFromAxisAngle(Vector3.UnitZ, angle));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ベクトルを回転した結果を取得します。</summary>
		/// <remarks>この計算によって、元のベクトルが変化することはありません。</remarks>
		/// 
		/// <param name="source">元のベクトル。</param>
		/// <param name="axis">回転軸。</param>
		/// <param name="angle">角度(ラジアン)。</param>
		/// <returns>回転されたベクトル。</returns>
		public static Vector3 rotate(this Vector3 source, Vector3 axis, float angle)
		{
			return Vector3.Transform(source, Quaternion.CreateFromAxisAngle(axis, angle));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2Dベクトルの型変換を行います。</summary>
		/// 
		/// <param name="source">元のベクトル。</param>
		/// <returns>変換されたベクトル。</returns>
		public static Point toPoint(this Vector2 source)
		{
			return new Point((int)source.X, (int)source.Y);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>2Dベクトルの型変換を行います。</summary>
		/// 
		/// <param name="source">元のベクトル。</param>
		/// <returns>変換されたベクトル。</returns>
		public static Vector2 toVector2(this Point source)
		{
			return new Vector2(source.X, source.Y);
		}
	}
}
