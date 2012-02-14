package danmaq.nineball.core.util.math.integer
{
	
	/**
	 * 絶対値を取得します。
	 *
	 * @param expr 対象値。
	 * @return 対象値に対する絶対値。
	 */
	public function abs(expr:int):int
	{
		return expr * (expr >= 0 ? 1 : -1);
	}
}
