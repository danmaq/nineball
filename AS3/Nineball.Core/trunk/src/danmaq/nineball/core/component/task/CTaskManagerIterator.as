package danmaq.nineball.core.component.task
{
	import danmaq.nineball.core.util.list.iterator.IIterator;
	
	import flash.errors.IllegalOperationError;
	
	/**
	 * タスク管理コレクションの反復子クラス。
	 * 
	 * @author Mc(danmaq)
	 */
	internal final class CTaskManagerIterator implements IIterator
	{
		
		//* fields ────────────────────────────────*
		
		/** タスク管理クラスの中身。 */
		private var _proxy:CTaskManagerProxy;

		/** インデックス。 */
		private var _index:int;
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 
		 * @param proxy タスク管理クラスの中身。
		 */
		public function CTaskManagerIterator(proxy:CTaskManagerProxy)
		{
			_proxy = proxy;
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * 次の値を取得します。
		 * 
		 * @return 値。
		 * @throws IllegalOperationError 値が取得不可能である時にこのプロパティへアクセスした場合。
		 */
		public function get next():*
		{
			if(!hasNext)
			{
				throw new IllegalOperationError("反復子は既に終端まで到達しています。");
			}
			return _proxy.tasks[_index++];
		}
		
		/**
		 * 次の値が取得可能かどうかを取得します。
		 * 
		 * @return 次の値が取得可能である場合、true。
		 */
		public function get hasNext():Boolean
		{
			return _index < _proxy.tasks.length;
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * 反復子をリセットし、再度次の値を取得可能な状態にします。
		 * 
		 * @return リセットに成功した場合、true。
		 */
		public function reset():Boolean
		{
			_index = 0;
			return true;
		}
	}
}