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
using Microsoft.Xna.Framework;

namespace danmaq.nineball.util.math
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>雑多な演算関数集クラス。</summary>
	public static class CMisc
	{

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>値を指定された範囲内に制限します。</summary>
		/// <remarks>
		/// 最小値と最大値を逆さに設定しても内部で自動的に認識・交換しますが、
		/// 無駄なオーバーヘッドが増えるだけなので極力避けてください。
		/// </remarks>
		/// 
		/// <param name="fExpr">対象の値</param>
		/// <param name="fMin">制限値(最小)</param>
		/// <param name="fMax">制限値(最大)</param>
		/// <returns><paramref name="fMin"/>～<paramref name="fMax"/>に制限された値</returns>
		public static float clampLoop(float fExpr, float fMin, float fMax)
		{
			return clampLoop(fExpr, fMin, fMax, false, true);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>値を指定された範囲内に制限します。</summary>
		/// <remarks>
		/// 最小値と最大値を逆さに設定しても内部で自動的に認識・交換しますが、
		/// 無駄なオーバーヘッドが増えるだけなので極力避けてください。
		/// </remarks>
		/// 
		/// <param name="fExpr">対象の値</param>
		/// <param name="fMin">制限値(最小)</param>
		/// <param name="fMax">制限値(最大)</param>
		/// <param name="bClampMinEqual"><paramref name="fExpr"/>が<paramref name="fMin"/>と等しい場合、ループするかどうか</param>
		/// <param name="bClampMaxEqual"><paramref name="fExpr"/>が<paramref name="fMax"/>と等しい場合、ループするかどうか</param>
		/// <returns><paramref name="fMin"/>～<paramref name="fMax"/>に制限された値</returns>
		public static float clampLoop(
			float fExpr, float fMin, float fMax, bool bClampMinEqual, bool bClampMaxEqual
		)
		{
			if(fMin == fMax)
			{
				return fMin;
			}
			else if(fMin > fMax)
			{
				float fBuffer = fMax;
				fMax = fMin;
				fMin = fBuffer;
			}
			while(
				(bClampMaxEqual ? fExpr >= fMax : fExpr > fMax) ||
				(bClampMinEqual ? fExpr <= fMin : fExpr < fMin)
			)
			{
				if(bClampMaxEqual ? fExpr >= fMax : fExpr > fMax)
				{
					fExpr = fMin + fExpr - fMax;
				}
				if(bClampMinEqual ? fExpr <= fMin : fExpr < fMin)
				{
					fExpr = fMax - Math.Abs(fExpr - fMin);
				}
			}
			return MathHelper.Clamp(fExpr, fMin, fMax);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>矩形を中心を原点として拡大します。</summary>
		/// 
		/// <param name="rectExpr">矩形</param>
		/// <param name="fScale">拡大率</param>
		/// <returns>拡大した矩形</returns>
		public static Rectangle Inflate(this Rectangle rectExpr, float fScale)
		{
			float fScaleHalf = fScale * 0.5f;
			Rectangle result = rectExpr;
			result.Inflate(
				(int)(result.Width * fScaleHalf),
				(int)(result.Height * fScaleHalf));
			return result;
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
	}
}
