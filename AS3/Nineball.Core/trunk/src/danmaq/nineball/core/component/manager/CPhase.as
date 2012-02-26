package danmaq.nineball.core.component.manager
{

	import danmaq.nineball.core.component.IDisposable;

	import flash.events.Event;

	/**
	 * フェーズ進行・カウンタ進行の管理をするクラス。
	 *
	 * <p>
	 * 使用するためには<code>count++</code>を毎フレーム呼び出してください。
	 * </p>
	 *
	 * @author Mc(danmaq)
	 */
	public final class CPhase implements IDisposable
	{

		//* fields ────────────────────────────────*

		/** 現在のフェーズ値。 */
		private var _phase:uint;

		/** 現在のカウント値。 */
		private var _counter:uint;

		/** 最後にフェーズ値が変化したときのカウント値。 */
		private var _phaseChangeCount:uint;

		/** 次に進行するフェーズ値の予約。 */
		private var _nextPhase:int;

		/** 前回のフェーズ値。 */
		private var _previousPhase:uint;

		//* constructor & destructor ───────────────────────*

		/**
		 * コンストラクタ。
		 *
		 * @param context 実体。
		 */
		public function CPhase()
		{
			dispose();
		}

		//* instance properties ─────────────────────────-*

		/**
		 * 現在のフェーズ値を取得します。
		 *
		 * @return 現在のフェーズ値。
		 * @default 0
		 */
		public function get phase():uint
		{
			return _phase;
		}

		/**
		 * 現在のフェーズ値を設定します。
		 *
		 * @param value 現在のフェーズ値。
		 */
		public function set phase(value:uint):void
		{
			if(_phase != value)
			{
				_previousPhase = _phase;
				_phase = value;
				_phaseChangeCount = counter;
			}
		}

		/**
		 * 次に進行するフェーズ値の予約を取得します。
		 * 負数の場合、予約されていないことを示します。
		 *
		 * @return 次に進行するフェーズ値。予約されていない場合、負数。
		 * @default -1
		 */
		public function get nextPhase():int
		{
			return _nextPhase;
		}

		/**
		 * 次に進行するフェーズ値を予約します。
		 * 予約を取り消したい場合、負数を設定します。
		 *
		 * @param value 予約したいフェーズ値。
		 */
		public function set nextPhase(value:int):void
		{
			if(phase != value)
			{
				_nextPhase = value;
			}
		}

		/**
		 * カウント変化時にフェーズを進めるかどうかを取得します。
		 *
		 * @return カウント変化時にフェーズを進める場合、<code>true</code>
		 * @default false
		 */
		public function get reserveNextPhase():Boolean
		{
			return (nextPhase >= 0);
		}

		/**
		 * カウント変化時にフェーズをインクリメントするかどうかを設定します。
		 *
		 * @param value カウント変化時にフェーズをインクリメントするかどうか。
		 */
		public function set reserveNextPhase(value:Boolean):void
		{
			nextPhase = (value ? phase + 1 : -1);
		}

		/**
		 * 前回のフェーズ値を取得します。
		 *
		 * @return 前回のフェーズ値。
		 * @default 0
		 */
		public function get previousPhase():uint
		{
			return _previousPhase;
		}

		/**
		 * 最後にフェーズ値が変化したときのカウント値を取得します。
		 *
		 * @return 最後にフェーズ値が変化したときのカウント値。
		 * @default 0
		 */
		public function get phaseChangeCount():uint
		{
			return _phaseChangeCount;
		}

		/**
		 * 最後にフェーズ値が変化してからのカウント値を取得します。
		 *
		 * @return 最後にフェーズ値が変化してからのカウント値。
		 */
		public function get phaseCounter():uint
		{
			return counter - phaseChangeCount;
		}

		/**
		 * 現在のカウント値を取得します。
		 *
		 * @return カウント値。
		 * @default 0
		 */
		public function get counter():uint
		{
			return _counter;
		}

		/**
		 * 現在のカウント値を設定します。
		 *
		 * <p>
		 * フェーズ進行の予約が入っている場合、
		 * ここで次のフェーズへと移行します。
		 * </p>
		 *
		 * @param value カウント値。
		 */
		public function set counter(value:uint):void
		{
			_counter = value;
			if(reserveNextPhase){
				phase = nextPhase;
				reserveNextPhase = false;
			}
		}

		//* instance methods ───────────────────────────*

		/**
		 * 初期状態に戻します。
		 */
		public function dispose():void
		{
			_counter = 0;
			_phase = 0;
			_previousPhase = 0;
			_phaseChangeCount = 0;
			_nextPhase = -1;
		}
	}
}
