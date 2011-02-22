////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework;

#if WINDOWS
using Microsoft.DirectX.DirectInput;
#endif

namespace danmaq.nineball.old.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ用の予約されたボタンID。</summary>
	[Obsolete]
	public enum EReservedButtonAxisID : short
	{

		/// <summary>POV(上)。</summary>
		povUp = -1,

		/// <summary>POV(下)。</summary>
		povDown = -2,

		/// <summary>POV(左)。</summary>
		povLeft = -3,

		/// <summary>POV(右)。</summary>
		povRight = -4,

		/// <summary>アナログ(上)。</summary>
		analogUp = -5,

		/// <summary>アナログ(下)。</summary>
		analogDown = -6,

		/// <summary>アナログ(左)。</summary>
		analogLeft = -7,

		/// <summary>アナログ(右)。</summary>
		analogRight = -8,

	}

#if WINDOWS

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ状態の拡張機能。</summary>
	[Obsolete]
	public static class JoystickStateExtension
	{

		//* -----------------------------------------------------------------------*
		/// <summary>POVの入力状態をベクトルで取得します。</summary>
		/// 
		/// <param name="state">入力状態の格納された構造体。</param>
		/// <returns>入力されたPOVのベクトル(-1.0～+1.0)。</returns>
		public static Vector2 getVectorPOV(this JoystickState state)
		{
			Vector2 result = Vector2.Zero;
			int pov = state.GetPointOfView()[0];
			if(pov != -1)
			{
				float fRadian = MathHelper.ToRadians(pov * 0.01f) - MathHelper.PiOver2;
				result = new Vector2((float)Math.Cos(fRadian), (float)Math.Sin(fRadian));
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>スライダーの入力状態を2次元ベクトルで取得します。</summary>
		/// 
		/// <param name="state">入力状態の格納された構造体。</param>
		/// <returns>入力されたスライダーの2次元ベクトル(-1.0～+1.0)。</returns>
		public static Vector2 getVector2Slider(this JoystickState state)
		{
			return new Vector2(state.X, state.Y);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>スライダーの入力状態を3次元ベクトルで取得します。</summary>
		/// 
		/// <param name="state">入力状態の格納された構造体。</param>
		/// <returns>入力されたスライダーの3次元ベクトル(-1.0～+1.0)。</returns>
		public static Vector3 getVector3Slider(this JoystickState state)
		{
			return new Vector3(state.X, state.Y, state.Z);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>入力状態をアナログ値で取得します。</summary>
		/// <remarks>
		/// デジタル値でのみ取得できるボタンを指定した場合、戻り値は0.0か1.0となります。
		/// </remarks>
		/// 
		/// <param name="state">入力状態の格納された構造体。</param>
		/// <param name="sButtonID">ボタンID。</param>
		/// <param name="nRange">アナログ値の取りうる最大値。</param>
		/// <returns>入力されたアナログ値(0.0～1.0)。</returns>
		public static float getInputState(this JoystickState state, short sButtonID, int nRange)
		{
			float fResult = 0f;
			switch(sButtonID)
			{
				case (short)EReservedButtonAxisID.povUp:
				case (short)EReservedButtonAxisID.povDown:
				case (short)EReservedButtonAxisID.povLeft:
				case (short)EReservedButtonAxisID.povRight:
					{
						Vector2 vector = state.getVectorPOV();
						if(vector != Vector2.Zero)
						{
							switch(sButtonID)
							{
								case (short)EReservedButtonAxisID.povUp:
									fResult = -MathHelper.Min(vector.Y, 0);
									break;
								case (short)EReservedButtonAxisID.povDown:
									fResult = MathHelper.Max(vector.Y, 0);
									break;
								case (short)EReservedButtonAxisID.povLeft:
									fResult = -MathHelper.Min(vector.X, 0);
									break;
								case (short)EReservedButtonAxisID.povRight:
									fResult = MathHelper.Max(vector.X, 0);
									break;
							}
						}
					}
					break;
				case (short)EReservedButtonAxisID.analogUp:
				case (short)EReservedButtonAxisID.analogDown:
				case (short)EReservedButtonAxisID.analogLeft:
				case (short)EReservedButtonAxisID.analogRight:
					{
						int[] sliders = state.GetSlider();
						switch(sButtonID)
						{
							case (short)EReservedButtonAxisID.analogUp:
								fResult = -MathHelper.Min(sliders[1], 0);
								break;
							case (short)EReservedButtonAxisID.analogDown:
								fResult = MathHelper.Max(sliders[1], 0);
								break;
							case (short)EReservedButtonAxisID.analogLeft:
								fResult = -MathHelper.Min(sliders[0], 0);
								break;
							case (short)EReservedButtonAxisID.analogRight:
								fResult = MathHelper.Max(sliders[0], 0);
								break;
						}
						fResult /= (float)nRange;
					}
					break;
				default:
					fResult = (state.GetButtons()[sButtonID] == 0) ? 0f : 1f;
					break;
			}
			return fResult;
		}

	}

#endif

}
