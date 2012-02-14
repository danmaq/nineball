package danmaq.nineball.core.util.math.integer
{
	
	/**
	 * 最小値を取得します。
	 *
	 * @param expr1 対象値。
	 * @param expr2 対象値。
	 * @return 最小値。
	 */
	public function min(expr1:int, expr2:int):int
	{
		return expr1 > expr2 ? expr2 : expr1;
	}
}
