////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.util.math;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>音量管理クラス。</summary>
	/// <remarks>
	/// 音量を設定すると自動的に0～2の範囲内に調整され、
	/// またSingle型とほぼ同じ感覚に扱えます。
	/// </remarks>
	[Serializable]
	public struct SVolume
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>ミュート状態を示す定数。</summary>
		public static readonly SVolume Zero = new SVolume();

		/// <summary>既定状態(1dB)を示す定数。</summary>
		public static readonly SVolume One = new SVolume(1);

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>音量。</summary>
		private float m_fVolume;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>初期音量が設定できます。</para>
		/// </summary>
		/// 
		/// <param name="fVolume">初期音量</param>
		public SVolume(float fVolume)
		{
			m_fVolume = 0f;
			volume = fVolume;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>音量。</summary>
		/// <remarks>設定すると自動的に0～2の範囲内に調整されます。</remarks>
		public float volume
		{
			get
			{
				return m_fVolume;
			}
			set
			{
				m_fVolume = CMisc.clampLoop(value, 0.0f, 2.0f);
				if(Math.Abs(m_fVolume - 1) < 0.001f)
				{
					m_fVolume = 1.0f;
				}
			}
		}

		/// <summary>音量(dB単位)。</summary>
		/// <remarks>減速線形補完を使用した非常に雑な変換です。</remarks>
		public float dB
		{
			get
			{
				return (volume > 1 ?
					CInterpolate._clampSmooth(0, 6, volume - 1, 1) :
					CInterpolate._clampSlowdown(-96, 0, volume, 1));
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>音量値を取得します。</summary>
		/// 
		/// <param name="v">音量クラス オブジェクト</param>
		/// <returns>音量値</returns>
		public static implicit operator float(SVolume v)
		{
			return v.volume;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音量値から音量クラスを生成します。</summary>
		/// 
		/// <param name="f">音量値</param>
		/// <returns>音量クラス オブジェクト</returns>
		public static implicit operator SVolume(float f)
		{
			return new SVolume(f);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音量クラス オブジェクトを0.1加算します。</summary>
		/// 
		/// <param name="v">音量クラス オブジェクト</param>
		/// <returns>音量クラス オブジェクト</returns>
		public static SVolume operator ++(SVolume v)
		{
			v.volume += 0.1f;
			return v;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音量クラス オブジェクトを0.1減算します。</summary>
		/// 
		/// <param name="v">音量クラス オブジェクト</param>
		/// <returns>音量クラス オブジェクト</returns>
		public static SVolume operator --(SVolume v)
		{
			v.volume -= 0.1f;
			return v;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音量クラス オブジェクトを指定値加算します。</summary>
		/// 
		/// <param name="v">音量クラス オブジェクト</param>
		/// <param name="f">加算値</param>
		/// <returns>音量クラス オブジェクト</returns>
		public static SVolume operator +(SVolume v, float f)
		{
			return new SVolume((float)v + f);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音量クラス オブジェクトを指定値減算します。</summary>
		/// 
		/// <param name="v">音量クラス オブジェクト</param>
		/// <param name="f">減算値</param>
		/// <returns>音量クラス オブジェクト</returns>
		public static SVolume operator -(SVolume v, float f)
		{
			return new SVolume((float)v - f);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音量値を文字列化します。</summary>
		/// 
		/// <param name="bSlider">スライダーを挿入するかどうか</param>
		/// <returns>文字列化した音量値</returns>
		public string ToString(bool bSlider)
		{
			string strResult = string.Empty;
			string strDB = String.Format("{0:+0.0;-0.0;0}dB", dB.ToString());
			if(bSlider)
			{
				char[] szVolume = new string('・', 10).ToCharArray();
				szVolume[(int)MathHelper.Min(CInterpolate._clampSmooth(0, 10, volume, 2), 9)] = '◆';
				strResult += new string(szVolume) + Environment.NewLine;
				strDB = string.Format("({0})", strDB);
			}
			return strResult + strDB;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音量値を文字列化します。</summary>
		/// <remarks><c>ToString(true)と同等です。</c></remarks>
		/// 
		/// <returns>文字列化した音量値</returns>
		public override string ToString()
		{
			return ToString(true);
		}
	}
}
