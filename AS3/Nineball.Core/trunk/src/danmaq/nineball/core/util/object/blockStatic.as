package danmaq.nineball.core.util.object
{

	import flash.errors.IllegalOperationError;

	/**
	 * 静的クラスのインスタンス生成を抑制するための例外を発生します。
	 *
	 * <p>
	 * この関数を静的クラスのコンストラクタで呼び出してください。この関数を呼び出した、即ち
	 * 静的クラスのインスタンスが作成されようとした場合、下記のメッセージを含んだ例外が発生します。
	 * </p>
	 * <pre>このオブジェクトは静的クラスです。直接生成は許可されていません。</pre>
	 *
	 * @throws IllegalOperationError 常に発生します。
	 */
	public function blockStatic():void
	{
		throw new IllegalOperationError(
			"このオブジェクトは静的クラスです。直接生成は許可されていません。");
	}
}
