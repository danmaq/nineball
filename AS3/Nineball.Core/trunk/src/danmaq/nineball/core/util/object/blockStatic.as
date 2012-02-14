package danmaq.nineball.core.util.object
{

	/**
	 * 静的クラスのインスタンス生成を抑制します。
	 *
	 * @throws ArgumentError 常に発生します。
	 */
	public function blockStatic():void
	{
		throw new ArgumentError(
			"このオブジェクトは静的クラスです。直接生成は許可されていません。");
	}
}
