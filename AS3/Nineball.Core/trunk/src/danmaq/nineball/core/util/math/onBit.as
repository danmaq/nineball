package danmaq.nineball.core.util.math
{

	[Deprecated(replacement="danmaq.nineball.core.util.math.CMathHelper#onBit()")]
	
	/**
	 * 対象値を最大32個のビット配列とみなし、1となっているビット数を計算します。
	 *
	 * <p>
	 * 例えば10進法の数値204は、2進法表記の場合11001100となり、
	 * 1となっているビット数は4個のため、この関数の戻り値は4となります。
	 * </p>
	 *
	 * @param expr 対象値。
	 * @return 1となっているビット数(0～32)。
	 */
	public function onBit(expr:uint):uint
	{
		return CMathHelper.onBit(expr);
	}
}
