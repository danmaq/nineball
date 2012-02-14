package danmaq.nineball.core.util.math
{
	
	/**
	 * 絶対値を取得します。
	 *
	 * @param expr 対象値。
	 * @return 対象値に対する絶対値。
	 */
	public function abs(expr:Number):Number
	{
		return expr * (expr >= 0 ? 1 : -1);
	}
}
