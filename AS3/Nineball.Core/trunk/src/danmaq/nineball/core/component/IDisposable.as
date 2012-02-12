package danmaq.nineball.core.component
{

	/**
	 * 明示的にオブジェクトを解放可能な状態にするメソッドを定義します。
	 * 
	 * @author Mc(danmaq)
	 */
	public interface IDisposable
	{

		//* instance methods ───────────────────────────*

		/**
		 * 明示的にオブジェクトを解放可能な状態にします。
		 */
		function dispose():void;
	}
}
