package danmaq.nineball.core.component.manager
{

	import danmaq.nineball.core.component.context.CSpriteWithContext;
	import danmaq.nineball.core.component.state.IState;
	
	import flash.events.Event;
	import flash.utils.getTimer;
	
	/**
	 * FPSが変化された際に発行されるイベントです。
	 *
	 * @eventType flash.events.Event.CHANGE
	 */
	[Event(name="change", type="flash.events.Event")]

	/**
	 * FPS計算クラス。
	 *
	 * <p>
	 * 既定では空の状態が設定してありますが、これを任意の状態に差し替えることにより、
	 * 計測したFPSを表示したり、FPS値によって任意の動作を実行したりすることができます。
	 * </p>
	 * <p>
	 * 注意：このクラスのFPS計測には、<code>CContextBody.counter</code>プロパティを使用しています。
	 * この値を変更すると、正しい計測が行えなくなります。
	 * </p>
	 *
	 * @author Mc(danmaq)
	 * @see danmaq.nineball.core.component.context.CContext
	 * @see danmaq.nineball.core.component.context.CContextBody#counter
	 */
	public class CFPSCalculator extends CSpriteWithContext
	{

		//* fields ────────────────────────────────*
		
		/** FPS実測値。 */
		private var _fps:int;
		
		/** 次にFPS値を確定する時間。 */
		private var _nextCommitTime:int = getTimer() + 1000;
		
		/** 最後にFPS値を確定した時のカウント値。 */
		private var _commitCounter:int;
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 *
		 * @param firstState 初回の状態。
		 */
		public function CFPSCalculator(firstState:IState=null)
		{
			super(firstState);
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * FPS実測値を取得します。
		 */
		public function get fps():int
		{
			return _fps;
		}

		//* instance methods ───────────────────────────*
		
		/**
		 * @inheritDoc 
		 */
		override public function dispose():void
		{
			_fps = 0;
			_commitCounter = 0;
			_nextCommitTime = getTimer() + 1000;
			super.dispose();
		}

		/**
		 * @inheritDoc 
		 */
		override public function update():void
		{
			var currentTime:int = getTimer();
			if(currentTime >= _nextCommitTime)
			{
				_nextCommitTime += 1000;
				var counter:int = context.counter;
				_fps = counter - _commitCounter;
				_commitCounter = counter;
				dispatchEvent(new Event(Event.CHANGE));
			}
			super.update();
		}
	}
}
