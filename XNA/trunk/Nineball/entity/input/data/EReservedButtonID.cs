////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if WINDOWS

using System;
using Microsoft.DirectX.DirectInput;
using Microsoft.Xna.Framework;
using danmaq.nineball.misc;

namespace danmaq.nineball.entity.input.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ用の予約されたボタンID。</summary>
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

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ状態の拡張機能。</summary>
	public static class JoystickStateExtension
	{

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
						int pov = state.GetPointOfView()[0];
						if(pov != -1)
						{
							float fRadian = MathHelper.ToRadians(pov * 0.01f) - MathHelper.PiOver2;
							new Vector2((float)Math.Cos(fRadian), (float)Math.Sin(fRadian));
						}
					}
					break;
				case (short)EReservedButtonAxisID.analogUp:
				case (short)EReservedButtonAxisID.analogDown:
				case (short)EReservedButtonAxisID.analogLeft:
				case (short)EReservedButtonAxisID.analogRight:
					break;
				default:
					fResult = (state.GetButtons()[sButtonID] == 0) ? 0f : 1f;
					break;
			}
			return fResult;
		}

	}
}

#endif
