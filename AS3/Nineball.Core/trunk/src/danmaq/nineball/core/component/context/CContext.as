package danmaq.nineball.core.component.context
{

	import danmaq.nineball.core.component.state.CStateEmpty;
	import danmaq.nineball.core.component.state.IState;
	import danmaq.nineball.core.events.CDisposableEventDispatcher;
	
	import flash.events.Event;
	import flash.utils.getTimer;

	/**
	 * 状態が変化された際に発行されるイベントです。
	 * 
	 * @eventType flash.events.Event.CHANGE
	 */
	[Event(name="change", type="flash.events.Event")]
	/**
	 * 明示的に解放可能な状態にした、即ちdisposeメソッドを実行した際に発行されるイベントです。
	 * 
	 * @eventType flash.events.Event.UNLOAD
	 */
	[Event(name="unload", type="flash.events.Event")]
	/**
	 * 状態を持つ実体クラス。
	 * 
	 * @author Mc(danmaq)
	 */
	public class CContext extends CDisposableEventDispatcher implements IContext
	{
		
		//* fields ────────────────────────────────*
		
		/** 現在の状態。 */
		private var _previousState:IState = CStateEmpty.instance;
		
		/** 直前の状態。 */
		private var _currentState:IState = CStateEmpty.instance;
		
		/** 次の状態。 */
		private var _nextState:IState;

		/** この実体のアクセサ。 */
		private var _body:CContextBody;
		
		/** 最後に状態変化した時のフレーム カウンタ値。 */
		private var _stateChangedCounter:int;
		
		/** 最後に状態変化した時の時刻。 */
		private var _stateChangedTime:int = getTimer();
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 
		 * @param firstState 初回の状態。
		 */
		public function CContext(firstState:IState = null)
		{
			_body = createBody();
			nextState = firstState == null ? defaultState : firstState;
			commitState();
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * 直前の状態を取得します。
		 * 
		 * @default CStateEmpty.instance
		 * @return 直前の状態。
		 */
		public function get previousState():IState
		{
			return _previousState;
		}
		
		/**
		 * 現在の状態を取得します。
		 * 
		 * @default CStateEmpty.instance
		 * @return 現在の状態。
		 */
		public function get currentState():IState
		{
			return _currentState;
		}
		
		/**
		 * 次の状態の予約を取得します。
		 * 
		 * @default null
		 * @return 次の状態。未決定の場合、null。
		 */
		public function get nextState():IState
		{
			return _nextState;
		}
		
		/**
		 * 次の状態の予約を設定、または取り消しします。
		 * 
		 * @param value 次の状態。
		 */
		public function set nextState(value:IState):void
		{
			_nextState = value;
		}
		
		/**
		 * 既定の状態を取得します。
		 * 
		 * @return 既定の状態。
		 */
		public function get defaultState():IState
		{
			return CStateEmpty.instance;
		}
		
		/**
		 * 汎用フレーム カウンタを取得します。
		 * 
		 * @default 0
		 * @return フレーム カウンタ。
		 */
		public function get counter():int
		{
			return body.counter;
		}

		/**
		 * 最後に状態変化した時のフレーム カウンタ値を取得します。
		 * 
		 * @return フレーム カウンタ。
		 */
		public function get stateChangedCounter():int
		{
			return _stateChangedCounter;
		}
		
		/**
		 * 最後に状態変化した時の時刻を取得します。
		 * 
		 * @return 時刻(AVM2起動時からの経過ミリ秒)。
		 */
		public function get stateChangedTime():int
		{
			return _stateChangedTime;
		}
		
		/**
		 * この実体へのアクセサ オブジェクトを取得します。
		 * 
		 * @return この実体へのアクセサ オブジェクト。
		 */
		protected function get body():CContextBody
		{
			return _body;
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * @inheritDoc
		 */
		override public function dispose():void
		{
			body.dispose();
			var empty:IState = CStateEmpty.instance;
			nextState = empty;
			commitState();
			_previousState = empty;
			_currentState = empty;
			if(hasEventListener(Event.UNLOAD))
			{
				dispatchEvent(new Event(Event.UNLOAD));
			}
			super.dispose();
			nextState = defaultState;
			commitState();
		}
		
		/**
		 * 1フレーム分の更新処理を実行します。
		 */
		public function update():void
		{
			body.update();
			commitState();
		}
		
		/**
		 * 予約されている状態は、updateメソッドによって確定されますが、
		 * このメソッドを使用することで強制的に即時確定します。
		 */
		public function commitState():void
		{
			if(nextState != null)
			{
				currentState.teardown(body);
			}
			if(nextState != null)
			{
				_previousState = _currentState;
				_currentState = nextState;
				nextState = null;
				_stateChangedCounter = counter;
				_stateChangedTime = getTimer();
				_currentState.setup(body);
				if(hasEventListener(Event.CHANGE))
				{
					dispatchEvent(new Event(Event.CHANGE));
				}
			}
		}
		
		/**
		 * この実体へのアクセサを生成します。
		 * 
		 * @return アクセサ オブジェクト。
		 */
		protected function createBody():CContextBody
		{
			return new CContextBody(this);
		}
	}
}
