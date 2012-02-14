package danmaq.nineball.core.util.math
{
	
	/**
	 * 値を範囲内に丸めます。
	 *
	 * @param expr 対象値。
	 * @param limit1 限界値。
	 * @param limit2 限界値。
	 * @return 丸められた値。
	 */
	public function clamp(expr:Number, limit1:Number, limit2:Number):Number
	{
		if (limit1 > limit2)
		{
			var tmp:Number = limit1;
			limit1 = limit2;
			limit2 = tmp;
		}
		return limit1 == limit2 ? limit1 : min(limit2, max(limit1, expr));
	}
}
