package danmaq.nineball.core.util.math
{
	
	/**
	 * 1となっているビット数を計算します。
	 * 
	 * @param expr 対象値。
	 * @return 1となっているビット数。
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
