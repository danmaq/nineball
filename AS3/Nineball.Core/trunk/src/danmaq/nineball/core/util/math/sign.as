package danmaq.nineball.core.util.math
{

	[Deprecated(replacement="danmaq.nineball.core.util.math.CMathHelper#sign()")]

	/**
	 * 数値の符号を取得します。
	 *
	 * @param expr 対象の値。
	 * @return exprが正数である場合、1。負数である場合、-1。NaN含めいずれにも該当しない場合、0。
	 */
	public function sign(expr:Number):int
	{
		return CMathHelper.sign(expr);
	}
}
