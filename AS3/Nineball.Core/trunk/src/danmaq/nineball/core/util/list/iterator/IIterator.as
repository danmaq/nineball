package danmaq.nineball.core.util.list.iterator
{

	/**
	 * 集合に対して反復処理をするメソッドを定義します。
	 * 
	 * @author Mc(danmaq)
	 */
	public interface IIterator
	{
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * 次の値を取得します。
		 * 
		 * @return 値。
		 * @throws IllegalOperationError 値が取得不可能である時にこのプロパティへアクセスした場合。
		 */
		function get next():*;
		
		/**
		 * 次の値が取得可能かどうかを取得します。
		 * 
		 * @return 次の値が取得可能である場合、true。
		 */
		function get hasNext():Boolean;
		
		//* instance methods ───────────────────────────*
		
		/**
		 * 反復子をリセットし、再度次の値を取得可能な状態にします。
		 * 
		 * @return リセットに成功した場合、true。
		 */
		function reset():Boolean;
		
	}
}
