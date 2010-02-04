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
using System.Collections.ObjectModel;
using System.Linq;
using danmaq.nineball.entity.input.data;
using danmaq.nineball.state;
using danmaq.nineball.state.input.collection;
using danmaq.nineball.state.input.xbox360;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ゲーム コントローラ入力制御・管理クラス。</summary>
	public sealed class CInputXBOX360 : CInput
	{
		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers : IDisposable
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>ボタンの入力状態一覧。</summary>
			public readonly List<SInputState> buttonStateList;

			/// <summary>フォース フィードバックのためのAI。</summary>
			public readonly CEntity aiForceFeedback;

			/// <summary>XBOX360ゲーム コントローラ入力制御・管理クラス。</summary>
			private readonly CInputXBOX360 input;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="input">XBOX360ゲーム コントローラ入力制御・管理クラス。</param>
			public CPrivateMembers(CInputXBOX360 input)
			{
				aiForceFeedback = new CEntity(input);
				this.input = input;
				buttonStateList = input._buttonStateList;
			}

			//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* properties ──────────────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>方向ボタンの状態をベクトルで設定します。</summary>
			/// 
			/// <value>方向ボタンの状態。</value>
			public Vector2 axisVector
			{
				set
				{
					input.axis = value;
				}
			}

			//* -----------------------------------------------------------------------*
			/// <summary>方向ボタンの状態をフラグで設定/取得します。</summary>
			/// <example>
			/// bool bDown = (obj.axisFlag &amp; EDirectionFlags.down) != 0;
			/// </example>
			/// 
			/// <value>方向ボタンの状態。</value>
			public EDirectionFlags axisFlag
			{
				get
				{
					return input.axisFlag;
				}
				set
				{
					input.axisFlag = value;
				}
			}

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>このオブジェクトの終了処理を行います。</summary>
			public void Dispose()
			{
				aiForceFeedback.Dispose();
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>プレイヤー番号一覧。</summary>
		public static readonly ReadOnlyCollection<PlayerIndex> allPlayerIndex = new List<PlayerIndex>
		{
			PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four
		}.AsReadOnly();

		/// <summary>クラス オブジェクト一覧。</summary>
		private static readonly ReadOnlyCollection<CInputXBOX360> instanceList;

		/// <summary>XBOX360 プレイヤー番号。</summary>
		public readonly PlayerIndex playerIndex;

		/// <summary>ボタン割り当て値の一覧。</summary>
		private readonly List<Buttons> m_assignList = new List<Buttons>();

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _privateMemebers;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>方向ボタンとして使用するボタン種類。</summary>
		private EAxisXBOX360 m_useForAxis = EAxisXBOX360.DPad;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static CInputXBOX360()
		{
			List<CInputXBOX360> _instanceList = new List<CInputXBOX360>(allPlayerIndex.Count);
			foreach(PlayerIndex playerIndex in allPlayerIndex)
			{
				_instanceList.Add(new CInputXBOX360(playerIndex));
			}
			instanceList = _instanceList.AsReadOnly();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="playerIndex">XBOX360 プレイヤー番号。</param>
		private CInputXBOX360(PlayerIndex playerIndex)
			: base(-1, CState.empty)
		{
			this.playerIndex = playerIndex;
			_privateMemebers = new CPrivateMembers(this);
			useForAxis = EAxisXBOX360.DPad;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>接続されているかどうかを取得します。</summary>
		/// 
		/// <value>接続されている場合、<c>true</c>。</value>
		public override bool connect
		{
			get
			{
				return playerNumber >= 0 && GamePad.GetState(playerIndex).IsConnected;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンの状態をベクトルで取得します。</summary>
		/// 
		/// <value>方向ボタンの状態。</value>
		public override Vector2 axisVector
		{
			get
			{
				return base.axisVector;
			}
			protected set
			{
				axis = value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// 状態として、nullを設定しようとした場合。
		/// </exception>
		public new IState<CInputXBOX360, CPrivateMembers> nextState
		{
			set
			{
				nextStateBase = value;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ボタン割り当て値の一覧を設定/取得します。</summary>
		/// 
		/// <value>ボタン割り当て値の一覧。</value>
		public IList<Buttons> assignList
		{
			get
			{
				return m_assignList.AsReadOnly();
			}
			set
			{
				m_assignList.Clear();
				m_assignList.AddRange(value);
				int buttonsNum = ButtonsNum;
				while(m_assignList.Count > buttonsNum)
				{
					m_assignList.RemoveAt(m_assignList.Count - 1);
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>方向ボタンとして使用するボタン種類を設定/取得します。</summary>
		/// 
		/// <value>方向ボタンとして使用するボタン種類。</value>
		public EAxisXBOX360 useForAxis
		{
			get
			{
				return m_useForAxis;
			}
			set
			{
				if(value != m_useForAxis)
				{
					m_useForAxis = value;
					switch(value)
					{
						case EAxisXBOX360.DPad:
							nextState = CStateDpad.instance;
							break;
						case EAxisXBOX360.LeftStick:
							nextState = CStateStick.left;
							break;
						case EAxisXBOX360.RightStick:
							nextState = CStateStick.right;
							break;
						default:
							nextState = CStateNoAxis.instance;
							break;
					}
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// オブジェクトと状態クラスのみがアクセス可能なフィールドを取得します。
		/// </summary>
		/// 
		/// <value>オブジェクトと状態クラスのみがアクセス可能なフィールド。</value>
		protected override object privateMembers
		{
			get
			{
				return _privateMemebers;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>クラス オブジェクトを取得します。</summary>
		/// 
		/// <param name="playerIndex">XBOX360 プレイヤー番号。</param>
		/// <param name="playerNumber">設定したいプレイヤー番号。</param>
		/// <returns>プレイヤー番号に対応したクラス オブジェクト。</returns>
		/// <exception cref="System.ArgumentException">
		/// 該当XBOX360プレイヤー番号のクラス オブジェクトが既に使用中である場合。
		/// </exception>
		/// <exception cref="System.InvalidOperationException">
		/// XBOX360プレイヤー番号に対応したクラス オブジェクトが見つからなかった場合。
		/// </exception>
		public static CInputXBOX360 getInstance(PlayerIndex playerIndex, short playerNumber)
		{
			CInputXBOX360 instance = instanceList.First(input => input.playerIndex == playerIndex);
			if(instance.connect)
			{	// クラス オブジェクトが既に使用中である場合
				throw new ArgumentException("playerIndex");
			}
			instance.playerNumber = playerNumber;
			return instance;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// XBOX360ゲーム コントローラを自動認識する入力制御・管理クラスを作成します。
		/// </summary>
		/// 
		/// <param name="playerNumber">設定したいプレイヤー番号。</param>
		/// <returns>XBOX360ゲーム コントローラ入力制御・管理クラスコレクション。</returns>
		public static CInputCollection createDetector(short playerNumber)
		{
			return new CInputCollection(playerNumber, CStateXBOX360Detect.instance);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理を実行します。</summary>
		public override void initialize()
		{
			_privateMemebers.aiForceFeedback.initialize();
			base.initialize();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(GameTime gameTime)
		{
			_privateMemebers.aiForceFeedback.update(gameTime);
			base.update(gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(GameTime gameTime)
		{
			_privateMemebers.aiForceFeedback.draw(gameTime);
			base.draw(gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			_privateMemebers.Dispose();
			base.Dispose();
		}
	}
}
