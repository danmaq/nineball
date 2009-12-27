using System;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.misc {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>三角関数系の演算関数集クラス。</summary>
	public static class CMath {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>周回軌道の角速度。</summary>
		public static readonly Func<float, float, float> cycricOrbit = ( fRadius, fSpeed ) =>
			fSpeed / fRadius;

		/// <summary>セカント。</summary>
		public static readonly Func<double, double> sec = ( dRadian ) => 1 / Math.Cos( dRadian );

		/// <summary>コセカント。</summary>
		public static readonly Func<double, double> cosec = ( dRadian ) => 1 / Math.Sin( dRadian );

		/// <summary>コタンジェント。</summary>
		public static readonly Func<double, double> cotan = ( dRadian ) => 1 / Math.Tan( dRadian );

		/// <summary>アークセカント。</summary>
		public static readonly Func<double, double> asec = ( dRadian ) =>
			MathHelper.PiOver2 * Math.Atan( Math.Sqrt( dRadian * dRadian ) - 1 ) +
			Math.Sign( dRadian ) - 1;

		/// <summary>アークコセカント。</summary>
		public static readonly Func<double, double> acosec = ( dRadian ) =>
			MathHelper.PiOver2 * Math.Atan( 1 / Math.Sqrt( dRadian * dRadian ) - 1 ) +
			Math.Sign( dRadian ) - 1;

		/// <summary>アークコタンジェント。</summary>
		public static readonly Func<double, double> acotan = ( dRadian ) =>
			MathHelper.PiOver2 - Math.Atan( dRadian );

		/// <summary>ハイパーボリック セカント。</summary>
		public static readonly Func<double, double> secH = ( dRadian ) =>
			2 / ( Math.Exp( dRadian ) + Math.Exp( -dRadian ) );

		/// <summary>ハイパーボリック コセカント。</summary>
		public static readonly Func<double, double> cosecH = ( dRadian ) =>
			2 / ( Math.Exp( dRadian ) - Math.Exp( -dRadian ) );

		/// <summary>ハイパーボリック コタンジェント。</summary>
		public static readonly Func<double, double> cotanH = ( dRadian ) =>
			Math.Exp( -dRadian ) / ( Math.Exp( dRadian ) - Math.Exp( -dRadian ) ) * 2 + 1;

		/// <summary>ハイパーボリック アークサイン。</summary>
		public static readonly Func<double, double> asinH = ( dRadian ) =>
			Math.Log( dRadian + Math.Sqrt( Math.Pow( dRadian, 2 ) + 1 ) );

		/// <summary>ハイパーボリック アークコサイン。</summary>
		public static readonly Func<double, double> acosH = ( dRadian ) =>
			Math.Log( dRadian + Math.Sqrt( Math.Pow( dRadian, 2 ) - 1 ) );

		/// <summary>ハイパーボリック アークタンジェント。</summary>
		public static readonly Func<double, double> atanH = ( dRadian ) =>
			Math.Log( ( 1 + dRadian ) / ( 1 - dRadian ) ) / 2;

		/// <summary>ハイパーボリック アークセカント。</summary>
		public static readonly Func<double, double> asecH = ( dRadian ) =>
			Math.Log( ( Math.Sqrt( -dRadian * dRadian + 1 ) + 1 ) / dRadian );

		/// <summary>ハイパーボリック アークコセカント。</summary>
		public static readonly Func<double, double> acosecH = ( dRadian ) =>
			Math.Log( Math.Sign( dRadian ) *
				( Math.Sqrt( Math.Pow( dRadian, 2 ) + 1 ) + 1 ) / dRadian );

		/// <summary>ハイパーボリック アークコタンジェント。</summary>
		public static readonly Func<double, double> acotanH = ( dRadian ) =>
			Math.Log( ( dRadian + 1 ) / ( dRadian - 1 ) ) / 2;
	}
}
