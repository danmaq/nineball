package danmaq.nineball.core.component.context
{
	import danmaq.nineball.core.component.task.ITask;
	
	/**
	 * 実体と状態との橋渡しをするクラス。
	 * 
	 * @author Mc(danmaq)
	 */
	public class CContextProxy implements ITask
	{

		//* fields ────────────────────────────────*

		/** 汎用フレーム カウンタ。 */
		public var counter:int;
		
		/** 実体。 */
		private var _context:IContext;

		//* constructor & destructor ───────────────────────*

		/**
		 * コンストラクタ。
		 *
		 * @param context 実体。
		 */
		public function CContextProxy(context:IContext)
		{
			_context = context;
		}

		//* instance properties ─────────────────────────-*

		/**
		 * 実体を取得します。
		 * 
		 * @return 実体。
		 */
		public function get context():IContext
		{
			return _context;
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * 1フレーム分の更新処理を実行します。
		 */
		public function update():void
		{
			context.currentState.update(this);
			counter++;
		}
		
		/**
		 * 明示的にオブジェクトを解放可能な状態にします。
		 */
		public function dispose():void
		{
			counter = 0;
		}
	}
}
