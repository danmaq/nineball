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
using danmaq.nineball.state.manager;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>マップ方式判定管理クラス。</summary>
	public class CMapJudgeManager<_T>
		: CEntity where _T : class, IEntity
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>近隣判定用列挙体。</summary>
		private enum ENearIndex : int
		{

			/// <summary>左上。</summary>
			leftUp,

			/// <summary>上。</summary>
			up,

			/// <summary>右上。</summary>
			rightUp,

			/// <summary>左。</summary>
			left,

			/// <summary>右。</summary>
			right,

			/// <summary>左下。</summary>
			leftDown,

			/// <summary>下。</summary>
			down,

			/// <summary>右下。</summary>
			rightDown,

			/// <summary>予約。(使用しないでください)</summary>
			__reserved,
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>マップの座標。</summary>
		public readonly Rectangle area;

		/// <summary>マップの精度。</summary>
		public readonly Point unit;

		/// <summary>マップの大きさ。</summary>
		public readonly Point size;

		/// <summary>判定用マップ。</summary>
		public readonly List<_T>[] map;

		/// <summary>近隣検索用マップ。</summary>
		private readonly int[] nearMap;

		/// <summary>近隣検索用バッファ。</summary>
		private readonly List<int> nearBuffer = new List<int>((int)ENearIndex.__reserved);

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="area">判定対象エリア。</param>
		/// <param name="unit">判定精度。</param>
		public CMapJudgeManager(Rectangle area, Point unit)
			: this(area, unit, CStateMapJudgeManager<_T>.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="area">判定対象エリア。</param>
		/// <param name="unit">判定精度。</param>
		/// <param name="firstState">初期状態。</param>
		public CMapJudgeManager(Rectangle area, Point unit, IState firstState)
			: base(firstState)
		{
			this.area = area;
			this.unit = unit;
			size = new Point(area.Width / unit.X + 1, area.Height / unit.Y + 1);
			nearMap = new int[]
			{
				-size.X - 1, -size.X, -size.X + 1,
				-1, 1,
				size.X - 1, size.X, size.X + 1,
			};
			int length = size.X * size.Y;
			map = new List<_T>[length];
			for (int i = length; --i >= 0; map[i] = new List<_T>())
				;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			clear();
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>マップの登録状況を消去します。</summary>
		public void clear()
		{
			for (int i = map.Length; --i >= 0; )
			{
				map[i].Clear();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定座標に該当するブロック番号を取得します。</summary>
		/// 
		/// <param name="x">X座標。</param>
		/// <param name="y">Y座標。</param>
		/// <returns>ブロック番号。判定範囲から外れている場合、負数。</returns>
		public int getIndexFromPos(int x, int y)
		{
			int result = -1;
			if (area.Contains(x, y))
			{
				result = size.X * ((y - area.Y) / unit.Y) + ((x - area.X) / unit.X);
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定ブロック番号に該当する座標を取得します。</summary>
		/// 
		/// <param name="index">ブロック番号。</param>
		/// <returns>ユニット座標。</returns>
		public Point getUnitPosFromIndex(int index)
		{
			Point result = Point.Zero;
			result.X = index % size.X;
			result.Y = index / size.X;
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定ブロック番号に該当する座標を取得します。</summary>
		/// 
		/// <param name="index">ブロック番号。</param>
		/// <returns>座標。</returns>
		public Point getRealPosFromIndex(int index)
		{
			Point result = getUnitPosFromIndex(index);
			result.X *= unit.X;
			result.Y *= unit.Y;
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定ブロックに存在する要素を取得します。</summary>
		/// 
		/// <param name="index">ブロック番号。</param>
		/// <param name="lr">左右のブロックも探索するかどうか。</param>
		/// <param name="ud">上下のブロックも探索するかどうか。</param>
		/// <returns>要素。存在しない場合、<c>null</c>。</returns>
		public virtual _T find(int index, bool lr, bool ud)
		{
			_T result = null;
			if (index >= 0 && index < map.Length)
			{
				result = find(index);
				if (result == null && (lr || ud))
				{
					// TODO : もうちょっと整理したいなぁ……。
					nearBuffer.Clear();
					bool left = lr && index % size.X > 0;
					bool right = lr && (index + 1) % size.X > 0;
					if(ud)
					{
						if (index > size.X)
						{
							nearBuffer.Add((int)ENearIndex.up);
							if (left)
							{
								nearBuffer.Add((int)ENearIndex.leftUp);
							}
							if (right)
							{
								nearBuffer.Add((int)ENearIndex.rightUp);
							}
						}
						if (index + size.X < map.Length)
						{
							nearBuffer.Add((int)ENearIndex.down);
							if (left)
							{
								nearBuffer.Add((int)ENearIndex.leftDown);
							}
							if (right)
							{
								nearBuffer.Add((int)ENearIndex.rightDown);
							}
						}
					}
					if (left)
					{
						nearBuffer.Add((int)ENearIndex.left);
					}
					if (right)
					{
						nearBuffer.Add((int)ENearIndex.right);
					}
					for (int i = nearBuffer.Count; --i >= 0 && result == null;
						find(index + nearMap[nearBuffer[i]]))
						;
				}
			}
			return result;
		}


		//* -----------------------------------------------------------------------*
		/// <summary>指定ブロックに存在する要素を取得します。</summary>
		/// 
		/// <param name="index">ブロック番号。</param>
		/// <returns>要素。存在しない場合、<c>null</c>。</returns>
		public virtual _T find(int index)
		{
			_T result = null;
			if (index >= 0 && index < map.Length)
			{
				result = map[index].Find(o => o.currentState != CState.empty);
			}
			return result;
		}
	}
}
