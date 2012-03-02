package danmaq.nineball.flex.components
{

	import danmaq.nineball.core.component.context.CContext;
	import danmaq.nineball.core.component.state.IState;
	import danmaq.nineball.core.component.task.ITask;
	
	import flash.events.Event;
	
	import mx.core.UIComponent;
	
	/**
	 * 状態による制御AIを持った表示オブジェクト。
	 *
	 * <p>
	 * AIである<code>context</code>オブジェクトには、毎フレーム開始時に<code>update()</code>
	 * メソッドが呼び出されます。このオブジェクトに任意の状態を設定して制御します。
	 * 状態からは<code>context.owner</code>で表示オブジェクトを参照することができます。
	 * </p>
	 *
	 * @author Mc(danmaq)
	 * @see danmaq.nineball.core.component.context.CContext
	 */
	public class CUIComponentWithContext extends UIComponent implements ITask
	{
		
		//* fields ────────────────────────────────*
		
		/** 状態による制御AI。 */
		private var _context:CContext;
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 *
		 * @param firstState 初回の状態。
		 */
		public function CUIComponentWithContext(firstState:IState = null)
		{
			_context = new CContext(firstState, this);
			addEventListener(Event.ENTER_FRAME, updateFromEvent, false, 0, true);
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * 状態による制御AIを取得します。
		 *
		 * @return 状態による制御AI。
		 */
		public function get context():CContext
		{
			return _context;
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * 強制的に1フレーム分の更新処理を実行します。
		 */
		public function update():void
		{
			context.update();
		}
		
		/**
		 * 明示的にオブジェクトを解放可能な状態にします。
		 */
		public function dispose():void
		{
			context.dispose();
		}
		
		/**
		 * <code>update()</code>メソッドのラッパーです。
		 *
		 * @param evt イベント情報。(無視されます)
		 * @see #update()
		 */
		private function updateFromEvent(evt:Event = null):void
		{
			update();
		}
	}
}
