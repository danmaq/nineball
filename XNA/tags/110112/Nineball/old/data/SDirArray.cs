////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.data;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.old.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>方向ボタンの入力状態を示す構造体。</summary>
	public struct SDirArray
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>上。</summary>
		public float up;

		/// <summary>下。</summary>
		public float down;

		/// <summary>左。</summary>
		public float left;

		/// <summary>右。</summary>
		public float right;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="up">上。</param>
		/// <param name="down">下。</param>
		/// <param name="left">左。</param>
		/// <param name="right">右。</param>
		public SDirArray(float up, float down, float left, float right)
		{
			this.up = up;
			this.down = down;
			this.left = left;
			this.right = right;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="vector">ベクトル。</param>
		public SDirArray(Vector2 vector)
		{
			
			up = MathHelper.Max(vector.Y, 0);
			down = -MathHelper.Min(vector.Y, 0);
			left = -MathHelper.Min(vector.X, 0);
			right = MathHelper.Max(vector.X, 0);
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンに対応する入力量を取得します。</summary>
		/// 
		/// <param name="dir">方向ボタン列挙体。</param>
		/// <returns>方向ボタンに対応する入力量。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 範囲外の値を引数に設定した場合。
		/// </exception>
		public float this[EDirection dir]
		{
			get{
				switch(dir)
				{
					case EDirection.up:
						return up;
					case EDirection.down:
						return down;
					case EDirection.left:
						return left;
					case EDirection.right:
						return right;
					default:
						throw new ArgumentOutOfRangeException("index");
				}
			}
			set
			{
				switch(dir)
				{
					case EDirection.up:
						up = value;
						break;
					case EDirection.down:
						down = value;
						break;
					case EDirection.left:
						left = value;
						break;
					case EDirection.right:
						right = value;
						break;
					default:
						throw new ArgumentOutOfRangeException("index");
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンに対応する入力量を取得します。</summary>
		/// 
		/// <param name="index">方向ボタン列挙体に対応する値。</param>
		/// <returns>方向ボタンに対応する入力量。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 範囲外の値を引数に設定した場合。
		/// </exception>
		public float this[int index]
		{
			get
			{
				return this[(EDirection)index];
			}
			set
			{
				this[(EDirection)index] = value;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ベクトルから構造体を初期化します。</summary>
		/// 
		/// <param name="vector">ベクトル。</param>
		/// <returns>方向ボタンの入力状態を示す構造体。</returns>
		public static implicit operator SDirArray(Vector2 vector)
		{
			return new SDirArray(vector);
		}
	}
}
