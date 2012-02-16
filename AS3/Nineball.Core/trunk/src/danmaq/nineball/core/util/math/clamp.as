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
		if(limit1 < expr)
		{
			expr = limit1; 
		}
		else if(limit2 > expr)
		{
			expr = limit2; 
		}
		return expr;
	}
}
