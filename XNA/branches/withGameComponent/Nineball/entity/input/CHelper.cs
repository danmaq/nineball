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
using danmaq.nineball.entity.input.data;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>マンマシンI/F入力制御・管理機能の補助関数群。</summary>
	public static class CHelper
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>方向ボタンに対応する、ベクトル一覧。</summary>
		private static readonly Vector2[] axisVectorList =
		{
			new Vector2(0, -1),
			new Vector2(0, 1),
			new Vector2(-1, 0),
			new Vector2(0, 1),
		};

		/// <summary>方向ボタンに対応する、フラグ一覧。</summary>
		private static readonly EDirectionFlags[] axisFlagsList =
		{
			EDirectionFlags.up,
			EDirectionFlags.down,
			EDirectionFlags.left,
			EDirectionFlags.right,
		};

		/// <summary>入力クラスの型に対応する、入力デバイスの列挙体一覧。</summary>
		private static readonly Dictionary<Type, EInputDevice> devices =
			new Dictionary<Type,EInputDevice>
		{
			{typeof(CInputKeyboard), EInputDevice.Keyboard},
			{typeof(CInputMouse), EInputDevice.Mouse},
			{typeof(CInputXBOX360), EInputDevice.XBOX360},
			{typeof(CInputXBOX360ChatPad), EInputDevice.XBOX360ChatPad},
			{typeof(CInputLegacy), EInputDevice.LegacyPad},
		};

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンのベクトルを計算します。</summary>
		/// 
		/// <param name="up">上方向アナログ入力値(0.0f～1.0f)。</param>
		/// <param name="down">下方向アナログ入力値(0.0f～1.0f)。</param>
		/// <param name="left">左方向アナログ入力値(0.0f～1.0f)。</param>
		/// <param name="right">右方向アナログ入力値(0.0f～1.0f)。</param>
		/// <returns>移動ベクトル。</returns>
		public static Vector2 createVector(float up, float down, float left, float right)
		{
			List<float> srcList = new List<float> { up, down, left, right };
			float fVelocity = 0;
			srcList.ForEach(expr => fVelocity = MathHelper.Max(fVelocity, Math.Abs(expr)));
			Vector2 result = new Vector2(-left, -up) + new Vector2(right, down);
			if(result != Vector2.Zero)
			{
				result.Normalize();
			}
			return result * fVelocity;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンのベクトルを計算します。</summary>
		/// 
		/// <param name="axis">方向ボタンの入力状態。</param>
		/// <param name="vector">方向ボタンのベクトル。</param>
		/// <param name="flags">方向ボタンの合成されたフラグ。</param>
		public static void createVector(
			IList<SInputState> axis, out Vector2 vector, out EDirectionFlags flags)
		{
			if(axis.Count < 4)
			{
				throw new ArgumentOutOfRangeException("axis");
			}
			vector = Vector2.Zero;
			flags = EDirectionFlags.None;
			for(int i = 4; --i >= 0; )
			{
				if(axis[i])
				{
					flags |= axisFlagsList[i];
					vector += axisVectorList[i];
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンのベクトルを計算します。</summary>
		/// 
		/// <param name="axis">方向ボタンの入力状態。</param>
		/// <param name="vector">方向ボタンのベクトル。</param>
		/// <param name="flags">方向ボタンの合成されたフラグ。</param>
		public static void createVector(
			IList<bool> axis, out Vector2 vector, out EDirectionFlags flags)
		{
			if(axis.Count < 4)
			{
				throw new ArgumentOutOfRangeException("axis");
			}
			vector = Vector2.Zero;
			flags = EDirectionFlags.None;
			for(int i = 4; --i >= 0; )
			{
				if(axis[i])
				{
					flags |= axisFlagsList[i];
					vector += axisVectorList[i];
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンのベクトルを計算します。</summary>
		/// 
		/// <param name="axis">方向ボタンの入力状態。</param>
		/// <param name="flags">方向ボタンの合成されたフラグ。</param>
		public static void createVector(IList<SInputState> axis, out EDirectionFlags flags)
		{
			if(axis.Count < 4)
			{
				throw new ArgumentOutOfRangeException("axis");
			}
			flags = EDirectionFlags.None;
			for(int i = 4; --i >= 0; )
			{
				if(axis[i])
				{
					flags |= axisFlagsList[i];
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンのベクトルを計算します。</summary>
		/// 
		/// <param name="axis">方向ボタンの入力状態。</param>
		/// <param name="flags">方向ボタンの合成されたフラグ。</param>
		public static void createVector(IList<bool> axis, out EDirectionFlags flags)
		{
			if(axis.Count < 4)
			{
				throw new ArgumentOutOfRangeException("axis");
			}
			flags = EDirectionFlags.None;
			for(int i = 4; --i >= 0; )
			{
				if(axis[i])
				{
					flags |= axisFlagsList[i];
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>入力クラスの型から入力デバイス固有の定数を取得します。</summary>
		/// 
		/// <param name="type">入力クラスの型。</param>
		/// <returns>入力デバイス固有の定数。</returns>
		public static EInputDevice getDeviceEnum(Type type)
		{
			EInputDevice result;
			if(!devices.TryGetValue(type, out result))
			{
				result = EInputDevice.None;
			}
			return result;
		}
	}
}
