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

namespace danmaq.nineball.util.math
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>三角関数系の演算関数集クラス。</summary>
	public static class CTriangle
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>周回軌道の角速度。</summary>
		public static readonly Func<float, float, float> _cycricOrbit = (fRadius, fSpeed) =>
			fSpeed / fRadius;

		/// <summary>セカント。</summary>
		public static readonly Func<double, double> _sec = (dRadian) => 1 / Math.Cos(dRadian);

		/// <summary>コセカント。</summary>
		public static readonly Func<double, double> _cosec = (dRadian) => 1 / Math.Sin(dRadian);

		/// <summary>コタンジェント。</summary>
		public static readonly Func<double, double> _cotan = (dRadian) => 1 / Math.Tan(dRadian);

		/// <summary>アークセカント。</summary>
		public static readonly Func<double, double> _asec = (dRadian) =>
			MathHelper.PiOver2 * Math.Atan(Math.Sqrt(dRadian * dRadian) - 1) +
			Math.Sign(dRadian) - 1;

		/// <summary>アークコセカント。</summary>
		public static readonly Func<double, double> _acosec = (dRadian) =>
			MathHelper.PiOver2 * Math.Atan(1 / Math.Sqrt(dRadian * dRadian) - 1) +
			Math.Sign(dRadian) - 1;

		/// <summary>アークコタンジェント。</summary>
		public static readonly Func<double, double> _acotan = (dRadian) =>
			MathHelper.PiOver2 - Math.Atan(dRadian);

		/// <summary>ハイパーボリック セカント。</summary>
		public static readonly Func<double, double> _secH = (dRadian) =>
			2 / (Math.Exp(dRadian) + Math.Exp(-dRadian));

		/// <summary>ハイパーボリック コセカント。</summary>
		public static readonly Func<double, double> _cosecH = (dRadian) =>
			2 / (Math.Exp(dRadian) - Math.Exp(-dRadian));

		/// <summary>ハイパーボリック コタンジェント。</summary>
		public static readonly Func<double, double> _cotanH = (dRadian) =>
			Math.Exp(-dRadian) / (Math.Exp(dRadian) - Math.Exp(-dRadian)) * 2 + 1;

		/// <summary>ハイパーボリック アークサイン。</summary>
		public static readonly Func<double, double> _asinH = (dRadian) =>
			Math.Log(dRadian + Math.Sqrt(Math.Pow(dRadian, 2) + 1));

		/// <summary>ハイパーボリック アークコサイン。</summary>
		public static readonly Func<double, double> _acosH = (dRadian) =>
			Math.Log(dRadian + Math.Sqrt(Math.Pow(dRadian, 2) - 1));

		/// <summary>ハイパーボリック アークタンジェント。</summary>
		public static readonly Func<double, double> _atanH = (dRadian) =>
			Math.Log((1 + dRadian) / (1 - dRadian)) / 2;

		/// <summary>ハイパーボリック アークセカント。</summary>
		public static readonly Func<double, double> _asecH = (dRadian) =>
			Math.Log((Math.Sqrt(-dRadian * dRadian + 1) + 1) / dRadian);

		/// <summary>ハイパーボリック アークコセカント。</summary>
		public static readonly Func<double, double> _acosecH = (dRadian) =>
			Math.Log(Math.Sign(dRadian) *
				(Math.Sqrt(Math.Pow(dRadian, 2) + 1) + 1) / dRadian);

		/// <summary>ハイパーボリック アークコタンジェント。</summary>
		public static readonly Func<double, double> _acotanH = (dRadian) =>
			Math.Log((dRadian + 1) / (dRadian - 1)) / 2;

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>周回軌道の角速度を計算します。</summary>
		/// 
		/// <param name="fRadius">径</param>
		/// <param name="fSpeed">速度</param>
		/// <returns>角速度</returns>
		public static float cycricOrbit(float fRadius, float fSpeed)
		{
			return _cycricOrbit(fRadius, fSpeed);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>セカントを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns><paramref name="dRadian"/>に対応するセカント値。</returns>
		public static double sec(double dRadian)
		{
			return _sec(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コセカントを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns><paramref name="dRadian"/>に対応するコセカント値。</returns>
		public static double cosec(double dRadian)
		{
			return _cosec(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コタンジェントを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns><paramref name="dRadian"/>に対応するコタンジェント値。</returns>
		public static double cotan(double dRadian)
		{
			return _cotan(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アークセカントを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns><paramref name="dRadian"/>に対応するアークセカント値。</returns>
		public static double asec(double dRadian)
		{
			return _asec(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アークコセカントを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns><paramref name="dRadian"/>に対応するアークコセカント値。</returns>
		public static double acosec(double dRadian)
		{
			return _acosec(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アークコタンジェントを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns><paramref name="dRadian"/>に対応するアークコタンジェント値。</returns>
		public static double acotan(double dRadian)
		{
			return _acotan(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ハイパーボリック セカントを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns><paramref name="dRadian"/>に対応するハイパーボリック セカント値。</returns>
		public static double secH(double dRadian)
		{
			return _secH(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ハイパーボリック コセカントを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns><paramref name="dRadian"/>に対応するハイパーボリック コセカント値。</returns>
		public static double cosecH(double dRadian)
		{
			return _cosecH(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ハイパーボリック コタンジェントを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns>
		/// <paramref name="dRadian"/>に対応するハイパーボリック コタンジェント値。
		/// </returns>
		public static double cotanH(double dRadian)
		{
			return _cotanH(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ハイパーボリック アークサインを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns>
		/// <paramref name="dRadian"/>に対応するハイパーボリック アークサイン値。
		/// </returns>
		public static double asinH(double dRadian)
		{
			return _asinH(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ハイパーボリック アークコサインを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns>
		/// <paramref name="dRadian"/>に対応するハイパーボリック アークコサイン値。
		/// </returns>
		public static double acosH(double dRadian)
		{
			return _acosH(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ハイパーボリック アークタンジェントを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns>
		/// <paramref name="dRadian"/>に対応するハイパーボリック アークタンジェント値。
		/// </returns>
		public static double atanH(double dRadian)
		{
			return _atanH(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ハイパーボリック アークセカントを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns>
		/// <paramref name="dRadian"/>に対応するハイパーボリック アークセカント値。
		/// </returns>
		public static double asecH(double dRadian)
		{
			return _asecH(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ハイパーボリック アークコセカントを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns>
		/// <paramref name="dRadian"/>に対応するハイパーボリック アークコセカント値。
		/// </returns>
		public static double acosecH(double dRadian)
		{
			return _acosecH(dRadian);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ハイパーボリック アークコタンジェントを計算します。</summary>
		/// 
		/// <param name="dRadian">ラジアン。</param>
		/// <returns>
		/// <paramref name="dRadian"/>に対応するハイパーボリック アークコタンジェント値。
		/// </returns>
		public static double acotanH(double dRadian)
		{
			return _acotanH(dRadian);
		}
	}
}
