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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using danmaq.nineball.entity;
using danmaq.nineball.entity.input;
using danmaq.nineball.entity.input.data;
using danmaq.nineball.Properties;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.state.input.detector
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// マンマシンI/F入力制御・管理クラスコレクション用レガシ ゲーム コントローラ自動認識状態。
	/// </summary>
	public sealed class CStateLegacyDetect : CState<CAI<CInputDetector>, List<SInputState>>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateLegacyDetect instance = new CStateLegacyDetect();

		/// <summary>
		/// レガシ ゲーム コントローラ入力制御・管理クラス オブジェクト一覧。
		/// </summary>
		private readonly ReadOnlyCollection<CInputLegacy> instanceList = CInputLegacy.instanceList;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>POVの入力を検出するかどうか。</summary>
		public bool detectPOV = true;

		/// <summary>スライダーの入力を検出するかどうか。</summary>
		public bool detectSlider = true;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateLegacyDetect()
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>状態が開始された時に呼び出されます。</para>
		/// <para>このメソッドは、遷移元の<c>teardown</c>よりも後に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されたオブジェクト。</param>
		/// <param name="buttonsState">ボタン押下情報一覧。</param>
		public override void setup(CAI<CInputDetector> entity, List<SInputState> buttonsState)
		{
			CInputCollection collection = entity.owner;
			setCapacity(collection);
			collection.releaseAwayController = true;
			base.setup(entity, buttonsState);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="buttonsState">ボタン押下情報一覧。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(
			CAI<CInputDetector> entity, List<SInputState> buttonsState, GameTime gameTime
		)
		{
			CInputCollection collection = entity.owner;
			if(collection.Count == 0)
			{
				CInputLegacy input = instanceList.FirstOrDefault(
					item => item.isPushAnyKey(detectPOV, detectSlider));
				if(input != null)
				{
					collection.Add(input);
				}
			}
			if(collection.Count != 0)
			{
				setCapacity(collection);
				entity.nextState = CStateWaitDetect.instance;
			}
			base.update(entity, buttonsState, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>子入力クラスとして受け入れる最大値を初期化します。</summary>
		/// 
		/// <param name="collection">この状態を終了したオブジェクト。</param>
		/// <exception cref="System.InvalidOperationException">
		/// 現在の保有個数が2個以上の場合。
		/// </exception>
		private void setCapacity(CInputCollection collection)
		{
			try
			{
				collection.capacity = 1;
			}
			catch(Exception e)
			{
				throw new InvalidOperationException(Resources.ERR_INPUT_DETECT_DUPLICATION, e);
			}
		}
	}
}

#endif
