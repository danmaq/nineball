package danmaq.nineball.core.component.context
{
	import danmaq.nineball.core.component.task.ITask;
	
	/**
	 * 実体に持つデータのうち、カプセル化により隠蔽したい情報を持つクラス。
	 * 
	 * ここにあるデータは、実体からはprotectedに、状態からはpublicに参照可能です。
	 * また、実体を継承してアクセサを設置することで読み込みのみをpublicにすることも可能です。
	 * 
	 * @author Mc(danmaq)
	 */
	public class CContextBody implements ITask
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
		public function CContextBody(context:IContext)
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
