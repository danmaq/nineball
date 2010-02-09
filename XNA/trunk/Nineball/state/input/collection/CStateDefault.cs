////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.entity.input;
using danmaq.nineball.entity.input.data;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.state.input.collection
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>マンマシンI/F入力制御・管理コレクションの既定の状態。</summary>
	public sealed class CStateDefault : CState<CInputCollection, CInputCollection.CPrivateMembers>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateDefault instance = new CStateDefault();

		/// <summary>方向ボタンフラグのバッファ。</summary>
		private readonly float[] dirFlagBuffer = new float[4];

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>ボタンフラグのバッファ。</summary>
		private bool[] m_buttonFlagBuffer = new bool[1];

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateDefault()
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(CInputCollection entity,
			CInputCollection.CPrivateMembers privateMembers, GameTime gameTime)
		{
			int nLength = initializeBuffer(privateMembers);
			privateMembers.axisFlag = EDirectionFlags.None;
			foreach(CInput input in privateMembers.childs)
			{
				if(!entity.releaseAwayController || input.connect)
				{
					input.update(gameTime);
					for(int i = nLength; --i >= 0; )
					{
						m_buttonFlagBuffer[i] =
							m_buttonFlagBuffer[i] || input.buttonStateList[i].press;
					}
					for(int i = 4; --i >= 0; )
					{
						dirFlagBuffer[i] =
							MathHelper.Max(dirFlagBuffer[i], input.dirInputState[i].analogValue);
					}
					Vector2 axis = entity.axisVector;
					float axisLength = axis.Length();
					axis += input.axisVector;
					axis.Normalize();
					axis *= MathHelper.Max(axisLength, input.axisVector.Length());
					privateMembers.axisVector = axis;
					privateMembers.axisFlag |= input.axisFlag;
				}
				else
				{
					input.Dispose();
				}
			}
			for(int i = nLength; --i >= 0; )
			{
				SInputState inputState = privateMembers.buttonStateList[i];
				inputState.refresh(m_buttonFlagBuffer[i]);
				privateMembers.buttonStateList[i] = inputState;
			}
			for(int i = 4; --i >= 0; )
			{
				entity.dirInputState[i].refresh(dirFlagBuffer[i]);
			}
			base.update(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(CInputCollection entity,
			CInputCollection.CPrivateMembers privateMembers, GameTime gameTime)
		{
			privateMembers.childs.ForEach(input => input.draw(gameTime));
			base.draw(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>バッファを初期化します。</summary>
		/// 
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <returns>初期化されているバッファのサイズ。</returns>
		private int initializeBuffer(CInputCollection.CPrivateMembers privateMembers)
		{
			int nLength = privateMembers.buttonStateList.Count;
			if(nLength > m_buttonFlagBuffer.Length)
			{
				m_buttonFlagBuffer = new bool[nLength];
			}
			Array.Clear(dirFlagBuffer, 0, 4);
			Array.Clear(m_buttonFlagBuffer, 0, nLength);
			return nLength;
		}
	}
}
