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
using danmaq.nineball.entity.input.data;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.entity.input
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>各種入力制御・管理クラスのFaçadeなラッパー。</summary>
	/// <remarks>
	/// このクラスを使用することで、キーボード、ゲーム コントローラ用などの各種入力
	/// 制御・管理クラスを最大限に少ないコーディングで一つにまとめて使用できます。
	/// </remarks>
	public sealed class CInputManager : CInputCollection
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>入力クラス一覧。</summary>
		private static readonly List<CInputManager> _inputList = new List<CInputManager>(1);

		/// <summary>キーボード用ボタン割り当て一覧。</summary>
		private readonly List<Keys> _keyboardAssign = new List<Keys>();

		/// <summary>キーボード用方向ボタン割り当て一覧。</summary>
		private readonly Keys[] _keyboardDirectionAssign =
		{
			Keys.Up,
			Keys.Down,
			Keys.Left,
			Keys.Right,
		};

		/// <summary>XBOX360ゲーム コントローラ用ボタン割り当て一覧。</summary>
		private readonly List<Buttons> _xbox360Assign = new List<Buttons>();

		/// <summary>レガシ ゲーム コントローラ用ボタン割り当て一覧。</summary>
		private readonly List<short> _legacyAssign = new List<short>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>入力デバイス。</summary>
		private EInputDevice m_inputDevice = EInputDevice.None;

		/// <summary>キーボード用入力制御・管理クラス。</summary>
		private CInputKeyboard m_inputKeyboard = null;

		/// <summary>マウス用入力制御・管理クラス。</summary>
		private CInputMouse m_inputMouse = null;

		/// <summary>XBOX360ゲーム コントローラ用入力制御・管理クラス。</summary>
		private CInputCollection m_inputXbox360 = null;

		/// <summary>XBOX360チャットパッド用入力制御・管理クラス。</summary>
		private CInputXBOX360ChatPad m_inputXbox360Chatpad = null;

#if WINDOWS
		/// <summary>レガシ ゲーム コントローラ用入力制御・管理クラス。</summary>
		private CInputCollection m_inputLegacy = null;
#endif

		/// <summary>XBOX360ゲーム コントローラ用の方向ボタン割り当て定数。</summary>
		private EAxisXBOX360 m_xbox360AxisAssign = EAxisXBOX360.None;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="playerNumber">プレイヤー番号。</param>
		private CInputManager(short playerNumber)
			: base(playerNumber)
		{
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>入力クラス一覧を取得します。</summary>
		/// 
		/// <value>入力クラス一覧。</value>
		public static ReadOnlyCollection<CInputManager> inputList
		{
			get
			{
				return _inputList.AsReadOnly();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>入力デバイス一覧を設定/取得します。</summary>
		/// 
		/// <value>入力デバイス一覧。</value>
		public EInputDevice inputDevice
		{
			get
			{
				return m_inputDevice;
			}
			set
			{
				if(m_inputDevice != value)
				{
					applyInputDevice<CInputKeyboard>(
						EInputDevice.Keyboard, ref m_inputKeyboard, createKeyboardInstance);
					applyInputDevice<CInputMouse>(
						EInputDevice.Mouse, ref m_inputMouse, createMouseInstance);
					applyInputDevice<CInputCollection>(
						EInputDevice.XBOX360, ref m_inputXbox360, createXBOX360Instance);
					applyInputDevice<CInputXBOX360ChatPad>(
						EInputDevice.XBOX360ChatPad, ref m_inputXbox360Chatpad,
						createXBOX360ChatPadInstance);
#if WINDOWS
					applyInputDevice<CInputCollection>(
						EInputDevice.LegacyPad, ref m_inputLegacy, createLegacyInstance);
#endif
					m_inputDevice = value;
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>キーボード用のボタン割り当て一覧を設定/取得します。</summary>
		/// 
		/// <value>キーボード用のボタン割り当て一覧。</value>
		public IList<Keys> keyboardAssign
		{
			get
			{
				return _keyboardAssign.AsReadOnly();
			}
			set
			{
				_keyboardAssign.Clear();
				_keyboardAssign.AddRange(value);
				if(m_inputKeyboard != null)
				{
					m_inputKeyboard.assignList = _keyboardAssign;
				}
				if(m_inputXbox360Chatpad != null)
				{
					m_inputXbox360Chatpad.assignList = _keyboardAssign;
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>キーボード用の方向ボタン割り当て一覧を設定/取得します。</summary>
		/// 
		/// <value>キーボード用の方向ボタン割り当て一覧。</value>
		public IList<Keys> keyboardDirectionAssign
		{
			get
			{
				return Array.AsReadOnly<Keys>(_keyboardDirectionAssign);
			}
			set
			{
				if(value.Count < 4)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				for(int i = 3; i >= 0; i--)
				{
					_keyboardDirectionAssign[i] = value[i];
				}
				if(m_inputKeyboard != null)
				{
					Array.Copy(_keyboardDirectionAssign, m_inputKeyboard.directionAssignList, 4);
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// XBOX360ゲーム コントローラ用のボタン割り当て一覧を設定/取得します。
		/// </summary>
		/// 
		/// <value>XBOX360ゲーム コントローラ用のボタン割り当て一覧。</value>
		public IList<Buttons> xbox360Assign
		{
			get
			{
				return _xbox360Assign.AsReadOnly();
			}
			set
			{
				_xbox360Assign.Clear();
				_xbox360Assign.AddRange(value);
				CInputXBOX360 input = m_inputXbox360.getInstance<CInputXBOX360>();
				if(input != null)
				{
					input.assignList = _xbox360Assign;
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// XBOX360ゲーム コントローラ用の方向ボタン割り当て定数を設定/取得します。
		/// </summary>
		/// 
		/// <value>XBOX360ゲーム コントローラ用の方向ボタン割り当て定数。</value>
		public EAxisXBOX360 xbox360AxisAssign
		{
			get
			{
				return m_xbox360AxisAssign;
			}
			set
			{
				if(m_xbox360AxisAssign != value)
				{
					m_xbox360AxisAssign = value;
					CInputXBOX360 input = m_inputXbox360.getInstance<CInputXBOX360>();
					if(input != null)
					{
						input.useForAxis = value;
					}
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// レガシ ゲーム コントローラ用のボタン割り当て一覧を設定/取得します。
		/// </summary>
		/// 
		/// <value>レガシ ゲーム コントローラ用のボタン割り当て一覧。</value>
		public IList<short> legacyAssign
		{
			get
			{
				return _legacyAssign.AsReadOnly();
			}
			set
			{
				_legacyAssign.Clear();
				_legacyAssign.AddRange(value);
#if WINDOWS
				CInputLegacy input = m_inputLegacy.getInstance<CInputLegacy>();
				if(input != null)
				{
					input.assignList = _legacyAssign;
				}
#endif
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>入力クラスを作成します。</summary>
		public static CInputManager create()
		{
			CInputManager input = new CInputManager((short)_inputList.Count);
			_inputList.Add(input);
			return input;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>入力クラスを解放します。</summary>
		public static void reset()
		{
			_inputList.ForEach(input => input.Dispose());
			_inputList.Clear();
			_inputList.Capacity = 1;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理している子入力クラスを解放します。</summary>
		/// 
		/// <param name="item">子入力クラス。</param>
		/// <returns>解放できた場合、<c>true</c>。</returns>
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		public override bool Remove(CInput item)
		{
			bool bResult = base.Remove(item);
			castOffAtEquals<CInputKeyboard>(EInputDevice.Keyboard, ref m_inputKeyboard, item);
			castOffAtEquals<CInputMouse>(EInputDevice.Mouse, ref m_inputMouse, item);
			castOffAtEquals<CInputCollection>(EInputDevice.XBOX360, ref m_inputXbox360, item);
			castOffAtEquals<CInputCollection>(EInputDevice.XBOX360, ref m_inputXbox360, item);
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理している子入力クラスを全て解放します。</summary>
		/// 
		/// <exception cref="System.NotSupportedException">
		/// 読み取り専用状態でこのメソッドを実行した場合。
		/// </exception>
		public override void Clear()
		{
			m_inputDevice = EInputDevice.None;
			m_inputKeyboard = null;
			m_inputMouse = null;
			m_inputXbox360 = null;
			m_inputXbox360Chatpad = null;
			m_inputLegacy = null;
			base.Clear();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 管理している入力クラスが対象の値と同じ場合、管理放棄します。
		/// </summary>
		/// 
		/// <typeparam name="_T">適用する入力制御・管理クラスの型。</typeparam>
		/// <param name="device">
		/// <typeparamref name="_T"/>に対応する入力デバイス。
		/// </param>
		/// <param name="input">
		/// 管理しているマンマシンI/F入力制御・管理クラスの格納先。
		/// </param>
		/// <param name="expr">比較対象のマンマシンI/F入力制御・管理クラス。</param>
		private void castOffAtEquals<_T>(
			EInputDevice device, ref _T input, CInput expr) where _T : CInput
		{
			if(input != null && expr != null && expr == input)
			{
				m_inputDevice &= ~device;
				input = null;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <c>inputDevice</c>プロパティで設定された、入力デバイスの設定を適用します。
		/// </summary>
		/// 
		/// <typeparam name="_T">適用する入力制御・管理クラスの型。</typeparam>
		/// <param name="device">
		/// <typeparamref name="_T"/>に対応する入力デバイス。
		/// </param>
		/// <param name="input">
		/// 作成または解放されたマンマシンI/F入力制御・管理クラスの格納先。
		/// </param>
		/// <param name="instanceCreator">
		/// <typeparamref name="_T"/>に対応するマンマシン
		/// I/F入力制御・管理クラスを生成するためのデリゲート。
		/// </param>
		private void applyInputDevice<_T>(
			EInputDevice device, ref _T input, Func<_T> instanceCreator) where _T : CInput
		{
			device = (m_inputDevice & device);
			if(device != EInputDevice.None && input == null)
			{
				input = instanceCreator();
				Add(input);
			}
			else if(device == EInputDevice.None && input != null)
			{
				Remove(input);
				input = null;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>キーボード入力制御・管理クラスを生成します。</summary>
		/// 
		/// <returns>キーボード入力制御・管理クラス。</returns>
		private CInputKeyboard createKeyboardInstance()
		{
			CInputKeyboard input = new CInputKeyboard(playerNumber);
			input.assignList = keyboardAssign;
			Array.Copy(_keyboardDirectionAssign, m_inputKeyboard.directionAssignList, 4);
			return input;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>マウス入力制御・管理クラスを生成します。</summary>
		/// <remarks>現在未実装のため、NotSupportedExceptionが発生します。</remarks>
		/// 
		/// <returns>マウス入力制御・管理クラス。</returns>
		/// <exception cref="System.NotSupportedException">
		/// 現在未実装のため、このメソッドを呼び出すと必ず発生します。
		/// </exception>
		private CInputMouse createMouseInstance()
		{
			throw new NotSupportedException(
				"現在のバージョンのNineballライブラリは、マウスの入力管理をまだサポートしていません。");
			//return CInputMouse.instance;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// XBOX360ゲーム コントローラの自動認識・入力制御・管理クラスを生成します。
		/// </summary>
		/// 
		/// <returns>
		/// XBOX360ゲーム コントローラの自動認識・入力制御・管理クラス。
		/// </returns>
		private CInputCollection createXBOX360Instance()
		{
			CInputCollection collection = CInputXBOX360.createDetector(playerNumber);
			collection.changedChildCount += (sender, count) =>
			{
				if(count > 0)
				{
					CInputXBOX360 input = ((CInputCollection)sender).getInstance<CInputXBOX360>();
					input.assignList = xbox360Assign;
					input.useForAxis = xbox360AxisAssign;
				}
			};
			return collection;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>XBOX360チャットパッド入力制御・管理クラスを生成します。</summary>
		/// 
		/// <returns>XBOX360チャットパッド入力制御・管理クラス。</returns>
		/// <exception cref="System.InvalidOperationException">
		/// XBOX360ゲーム コントローラが認識されていない状態でこのメソッドを呼び出した場合。
		/// </exception>
		private CInputXBOX360ChatPad createXBOX360ChatPadInstance()
		{
			if(m_inputXbox360 == null || m_inputXbox360.Count == 0)
			{
				throw new InvalidOperationException(
					"XBOX360チャットパッドを使用するためには、まずXBOX360ゲーム コントローラが認識されている必要があります。");
			}
			CInputXBOX360ChatPad input = CInputXBOX360ChatPad.getInstance(
				(CInputXBOX360)m_inputXbox360.childList[0], playerNumber);
			input.assignList = keyboardAssign;
			return input;
		}

#if WINDOWS

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// レガシ ゲーム コントローラの自動認識・入力制御・管理クラスを生成します。
		/// </summary>
		/// 
		/// <returns>
		/// レガシ ゲーム コントローラの自動認識・入力制御・管理クラス。
		/// </returns>
		private CInputCollection createLegacyInstance()
		{
			CInputCollection collection = CInputLegacy.createDetector(playerNumber);
			collection.changedChildCount += (sender, count) =>
			{
				if(count > 0)
				{
					CInputLegacy input = ((CInputCollection)sender).getInstance<CInputLegacy>();
					input.assignList = legacyAssign;
//					input.useForAxis = xbox360AxisAssign;
				}
			};
			return collection;
		}
#endif
	}
}
