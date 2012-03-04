package danmaq.nineball.core.util.object
{

	import flash.errors.IllegalOperationError;
	
	/**
	 * 未実装メソッドの呼び出しを抑制するための例外を発生します。
	 *
	 * <p>
	 * この関数を抽象メソッドおよびプロパティで呼び出してください。
	 * この関数を呼び出した場合、下記のメッセージを含んだ例外が発生します。
	 * </p>
	 * <pre>実装されていない抽象メソッド、または抽象プロパティが呼び出されました。</pre>
	 *
	 * @throws IllegalOperationError 常に発生します。
	 */
	public function blockNotImplements():void
	{
		throw new IllegalOperationError("実装されていない抽象メソッドが呼び出されました。");
	}
}
