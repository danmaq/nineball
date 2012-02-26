package danmaq.nineball.core.util.math
{

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
		expr = (expr & 0x55555555) + (expr >> 1 & 0x55555555);
		expr = (expr & 0x33333333) + (expr >> 2 & 0x33333333);
		expr = (expr & 0x0f0f0f0f) + (expr >> 4 & 0x0f0f0f0f);
		expr = (expr & 0x00ff00ff) + (expr >> 8 & 0x00ff00ff);
		return (expr & 0x0000ffff) + (expr >> 16 & 0x0000ffff);
	}
}
