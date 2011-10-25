////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if WINDOWS

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using danmaq.nineball.entity.input.low;
using danmaq.nineball.util.math;
using Microsoft.DirectX.DirectInput;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.util.collection.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲームパッド専用低位入力制御・管理クラスのコレクション。</summary>
	public sealed class CLegacyInputCollection
		: IInputCollection
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CLegacyInputCollection instance = new CLegacyInputCollection();

		/// <summary>ゲームパッド専用低位入力制御・管理クラス一覧。</summary>
		public readonly ReadOnlyCollection<CLegacyInput> inputList;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>アナログ入力におけるハイパス値(0～1)。</summary>
		public float threshold = 0.25f;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CLegacyInputCollection()
		{
			IntPtr hWnd = Process.GetCurrentProcess().Handle;
			DeviceList srcList =
				Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
			List<CLegacyInput> dstList = new List<CLegacyInput>(srcList.Count);
			foreach (DeviceInstance item in srcList)
			{
				// TODO : なんかもうちょっとまともな区別方法ないの？
				if (!(Regex.IsMatch(item.ProductName, "Xbox ?360", RegexOptions.IgnoreCase)))
				{
					dstList.Add(new CLegacyInput(item.InstanceGuid, hWnd));
				}
			}
			inputList = dstList.AsReadOnly();
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ボタン入力を検出します。</summary>
		/// <remarks>
		/// 注意: このメソッドを呼び出すと、自動的に登録されているクラスに対して
		/// <c>update()</c>が実行されます。レガシ ゲームパッドが高位入力管理クラスにて
		/// アクティブの状態でこのメソッドを呼び出すと、高位入力側の判定が
		/// 1フレーム分欠落します。
		/// </remarks>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		/// <returns>
		/// ボタン入力が検出されたデバイスの管理クラス。検出しなかった場合、<c>null</c>。
		/// </returns>
		public CLegacyInput detectInput(GameTime gameTime)
		{
			CLegacyInput result = null;
			int threshold =
				(int)CInterpolate.lerpClampLinear(0, CLegacyInput.RANGE, this.threshold, 1);
			for (int i = inputList.Count; --i >= 0 && result == null; )
			{
				CLegacyInput input = inputList[i];
				input.update(gameTime);
				JoystickState state = input.nowInputState;
				if (state.Equals(input.prevInputState) && (
					Array.Exists<byte>(state.GetButtons(), b => b != 0) ||
					Math.Abs(state.X) >= threshold ||
					Math.Abs(state.Y) >= threshold ||
					Math.Abs(state.Z) >= threshold))
				{
					result = input;
				}
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ボタン入力を検出します。</summary>
		/// <remarks>
		/// 注意: このメソッドを呼び出すと、自動的に登録されているクラスに対して
		/// <c>update()</c>が実行されます。レガシ ゲームパッドが高位入力管理クラスにて
		/// アクティブの状態でこのメソッドを呼び出すと、高位入力側の判定が
		/// 1フレーム分欠落します。
		/// </remarks>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		/// <returns>
		/// ボタン入力が検出された場合、<c>true</c>。
		/// </returns>
		bool IInputCollection.detectInput(GameTime gameTime)
		{
			return detectInput(gameTime) != null;
		}
	}
}

#endif
