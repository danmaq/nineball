package danmaq.nineball.core.component
{

	/**
	 * 明示的にオブジェクトを解放可能な状態にするメソッドを定義します。
	 * 
	 * <p>
	 * このインターフェイスを実装することにより、明示的にオブジェクトを
	 * 解放可能な状態にするための共通のメソッドを提供します。
	 * </p>
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
