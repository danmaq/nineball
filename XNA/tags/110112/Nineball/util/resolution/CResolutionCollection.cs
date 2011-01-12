////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.util.resolution
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>対応解像度コレクション。</summary>
	public sealed class CResolutionCollection : IEnumerable<EResolution>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CResolutionCollection instance = new CResolutionCollection();

		/// <summary>対応している解像度一覧。</summary>
		private readonly List<EResolution> m_supports = new List<EResolution>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CResolutionCollection()
		{
			m_supports = new List<EResolution>();
			foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
			{
				for (EResolution i = EResolution.sQCIF; i < EResolution.__reserved; i++)
				{
					Point size = i.getXY();
					if (mode.Width == size.X && mode.Height == size.Y && !m_supports.Contains(i))
					{
						m_supports.Add(i);
						break;
					}
				}
			}
			m_supports.Sort();
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>対応している解像度を取得します。</summary>
		/// 
		/// <param name="index">解像度のインデックス番号。</param>
		/// <value>対応している解像度。</value>
		public EResolution this[int index]
		{
			get
			{
				return m_supports[index];
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>対応している解像度の総数を取得します。</summary>
		/// 
		/// <value>対応している解像度の総数。</value>
		public int Count
		{
			get
			{
				return m_supports.Count;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>対応する最低解像度を取得します。</summary>
		/// 
		/// <value>対応する最低解像度。</value>
		public EResolution min
		{
			get
			{
				return m_supports[0];
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>対応する最大解像度を取得します。</summary>
		/// 
		/// <value>対応する最大解像度。</value>
		public EResolution max
		{
			get
			{
				return m_supports[Count - 1];
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1段階小さい解像度を取得します。</summary>
		/// 
		/// <param name="resolution">解像度列挙体。</param>
		/// <returns>解像度列挙体。</returns>
		public EResolution prev(EResolution resolution)
		{
			int i = m_supports.BinarySearch(resolution);
			return this[i == 0 ? Count - 1 : --i];
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1段階大きい解像度を取得します。</summary>
		/// 
		/// <param name="resolution">解像度列挙体。</param>
		/// <returns>解像度列挙体。</returns>
		public EResolution next(EResolution resolution)
		{
			int i = m_supports.BinarySearch(resolution);
			return this[++i == Count ? 0 : i];
		}

		//* -----------------------------------------------------------------------*
		/// <summary>特定の値が格納されているかどうかを判断します。</summary>
		/// 
		/// <param name="item">検索するオブジェクト。</param>
		/// <returns>存在する場合、<c>true</c>。</returns>
		public bool Contains(EResolution item)
		{
			return m_supports.Contains(item);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 指定した型のコレクションに対する単純な反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		public IEnumerator<EResolution> GetEnumerator()
		{
			return m_supports.GetEnumerator();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 非ジェネリック コレクションに対する反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)m_supports).GetEnumerator();
		}
	}
}
