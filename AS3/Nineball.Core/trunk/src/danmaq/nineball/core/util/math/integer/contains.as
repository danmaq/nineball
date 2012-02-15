package danmaq.nineball.core.util.math.integer
{
	
	/**
	 * 値が範囲内に含まれているかどうかを取得します。
	 *
	 * @param expr 対象値。
	 * @param limit1 限界値。
	 * @param limit2 限界値。
	 * @return 値が範囲内に含まれている場合、true。
	 */
	public function contains(expr:int, limit1:int, limit2:int):Boolean
	{
		if (limit1 > limit2)
		{
			limit1 ^= limit2;
			limit2 ^= limit1;
			limit1 ^= limit2;
		}
		return expr >= limit1 && expr <= limit2;
	}
}
