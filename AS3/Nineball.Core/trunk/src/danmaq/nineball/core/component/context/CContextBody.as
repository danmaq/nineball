package danmaq.nineball.core.component.context
{
	import danmaq.nineball.core.component.task.ITask;
	
	/**
	 * <code>CContext</code>実体に持つデータのうち、カプセル化により隠蔽したい情報を持つクラス。
	 * 
	 * <p>
	 * ここにあるデータは、実体からは<code>protected</code>に、状態からは
	 * <code>public</code>に参照可能です。また、実体を継承してアクセサを
	 * 設置することで読み込みのみを<code>public</code>にすることもできます。
	 * </p>
	 * 
	 * @author Mc(danmaq)
	 * @see danmaq.nineball.core.component.context.CContext
	 */
	public class CContextBody implements ITask
	{

		//* fields ────────────────────────────────*

		/** 汎用フレーム カウンタ。 */
		public var counter:int;
		
		/** 実体。 */
		private var _context:IContext;
		
		/** 親とするオブジェクト。 */
		private var _owner:Object;

		//* constructor & destructor ───────────────────────*

		/**
		 * コンストラクタ。
		 *
		 * @param context 実体。
		 * @param owner 親とするオブジェクト。
		 */
		public function CContextBody(context:IContext, owner:Object)
		{
			_context = context;
			_owner = owner;
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
		
		/**
		 * 親オブジェクトを取得します。
		 * 
		 * @return 親オブジェクト。
		 */
		public function get owner():Object
		{
			return _owner;
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
