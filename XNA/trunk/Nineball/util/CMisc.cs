////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.Properties;
using danmaq.nineball.util.math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.util
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>雑多な関数集クラス。</summary>
	/// <remarks>
	/// 注意：ここに置かれている関数は、近いうちに他の名前空間に移動する場合があります。
	/// </remarks>
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
		/// <summary>
		/// 指定された述語によって定義された条件と一致する要素を検索し、
		/// 最もインデックス番号の大きい要素の 0 から始まるインデックスを返します。
		/// </summary>
		/// <remarks>
		/// 検索は最後の要素から開始して順方向に進み、最後の要素で終了します。
		/// このメソッドは O(n) 操作です。
		/// </remarks>
		/// 
		/// <typeparam name="_T">配列要素の型。</typeparam>
		/// <param name="array">検索対象となる1次元配列。</param>
		/// <param name="match">検索する要素の条件。</param>
		/// <returns>
		/// 定義された条件と一致する要素が存在した場合、最もインデックス番号の大きい
		/// 要素の 0 から始まるインデックス。それ以外の場合は –1。
		/// </returns>
		public static int FindLastIndex<_T>(_T[] array, Predicate<_T> match)
		{
			int result = -1;
			for (int i = array.Length; --i >= 0 && result < 0; )
			{
				if(match(array[i]))
				{
					result = i;
				}
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>使用されているリソースを解放します。</summary>
		/// 
		/// <typeparam name="_T">解放対象の型。</typeparam>
		/// <param name="obj">解放対象のオブジェクト。</param>
		/// <returns>解放された場合、<c>true</c>。</returns>
		public static bool safeDispose<_T>(ref _T obj) where _T : class, IDisposable
		{
			bool result = obj != null;
			if (result)
			{
				try
				{
					obj.Dispose();
				}
				catch (Exception e)
				{
					CLogger.add(
						string.Format(Resources.GENERAL_ERR_RELEASE, obj.GetType().FullName));
					CLogger.add(e);
				}
				obj = null;
			}
			return result;
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
		/// <summary>テクスチャをコピーします。</summary>
		/// <remarks>あまり最適化していないため、重いです。</remarks>
		/// 
		/// <typeparam name="_T">画素の型。</typeparam>
		/// <param name="src">コピー元。</param>
		/// <param name="dst">コピー先。</param>
		/// <param name="srcPoint">コピー元の左上座標。</param>
		/// <exception cref="System.ArgumentNullException">
		/// テクスチャが<c>null</c>である場合。
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// テクスチャのピクセル フォーマットが一致しない場合。
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="srcPoint"/>が<paramref name="src"/>の範囲外にある場合。
		/// および、<paramref name="src"/>または<paramref name="srcPoint"/>による
		/// 切り出し範囲がが<paramref name="dst"/>より小さい場合。
		/// </exception>
		public static void copyTexture<_T>(Texture2D src, Texture2D dst, Point srcPoint)
			where _T : struct
		{
			if (src == null)
			{
				throw new ArgumentNullException("src");
			}
			if (dst == null)
			{
				throw new ArgumentNullException("dst");
			}
			if (src.Format != dst.Format)
			{
				throw new ArgumentException(Resources.TEX_ERR_FORMAT);
			}
			Rectangle srcMaxRect = new Rectangle(0, 0, src.Width, src.Height);
			Rectangle dstMaxRect = new Rectangle(0, 0, dst.Width, dst.Height);
			Rectangle srcRect =
				new Rectangle(srcPoint.X, srcPoint.Y, dstMaxRect.Width, dstMaxRect.Height);
			if (!srcMaxRect.Contains(srcRect))
			{
				throw new ArgumentOutOfRangeException("srcPoint");
			}
			_T[] srcData = new _T[srcMaxRect.Width * srcMaxRect.Height];
			_T[] dstData = new _T[dstMaxRect.Width * dstMaxRect.Height];
			src.GetData(srcData);
			for (int y = dstMaxRect.Height; --y >= 0; )
			{
				int sy = y + srcPoint.Y;
				for (int x = dstMaxRect.Width; --x >= 0; )
				{
					int sx = x + srcPoint.X;
					dstData[y * dstMaxRect.Width + x] = srcData[sy * srcMaxRect.Width + sx];
				}
			}
			dst.SetData(dstData);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ベクトルを単位ベクトルに変換します。</summary>
		/// 
		/// <param name="expr">ベクトル。</param>
		/// <param name="alternative">
		/// <paramref name="expr"/>がゼロだった際の代替ベクトル。
		/// </param>
		/// <returns>単位ベクトル。</returns>
		[Obsolete("この関数は移動しました。今後はdanmaq.nineball.util.math.CVectorUtilクラスの同名関数を使用してください。")]
		public static Vector2 normalize(Vector2 expr, Vector2 alternative)
		{
			return CVectorUtil.normalize(expr, alternative);
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
		[Obsolete("この関数は移動しました。今後はdanmaq.nineball.util.math.CVectorUtilクラスの同名関数を使用してください。")]
		public static Vector2 normalize(Vector2 expr, Vector2 alternative, float unit)
		{
			return CVectorUtil.normalize(expr, alternative, unit);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>速度・角度からベクトルを取得します。</summary>
		/// 
		/// <param name="speed">速度。</param>
		/// <param name="angle">角度(ラジアン)。</param>
		/// <returns>ベクトル。</returns>
		[Obsolete("この関数は移動しました。今後はdanmaq.nineball.util.math.CVectorUtilクラスの同名関数を使用してください。")]
		public static Vector2 createVector2(float speed, float angle)
		{
			return CVectorUtil.createVector2(speed, angle);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ベクトルを回転した結果を取得します。</summary>
		/// <remarks>この計算によって、元のベクトルが変化することはありません。</remarks>
		/// 
		/// <param name="source">元のベクトル。</param>
		/// <param name="angle">角度(ラジアン)。</param>
		/// <returns>回転されたベクトル。</returns>
		[Obsolete("この関数は移動しました。今後はdanmaq.nineball.util.math.CVectorUtilクラスの同名関数を使用してください。")]
		public static Vector2 rotate(this Vector2 source, float angle)
		{
			return CVectorUtil.rotate(source, angle);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ベクトルを回転した結果を取得します。</summary>
		/// <remarks>この計算によって、元のベクトルが変化することはありません。</remarks>
		/// 
		/// <param name="source">元のベクトル。</param>
		/// <param name="axis">回転軸。</param>
		/// <param name="angle">角度(ラジアン)。</param>
		/// <returns>回転されたベクトル。</returns>
		[Obsolete("この関数は移動しました。今後はdanmaq.nineball.util.math.CVectorUtilクラスの同名関数を使用してください。")]
		public static Vector3 rotate(this Vector3 source, Vector3 axis, float angle)
		{
			return CVectorUtil.rotate(source, axis, angle);
		}
	}
}
