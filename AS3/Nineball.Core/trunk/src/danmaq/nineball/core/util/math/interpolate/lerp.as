package danmaq.nineball.core.util.math.interpolate
{

	[Deprecated(replacement="danmaq.nineball.core.util.math.CInterpolate#lerp()")]
	
	/**
	 * 2つの数値間で線形補完の計算をします。
	 *
	 * @param expr1 対象値1。
	 * @param expr2 対象値2。
	 * @param amount 重み。
	 * @return amountの0～1に対応する、expr1～expr2の値。
	 */
	public function lerp(expr1:Number, expr2:Number, amount:Number):Number
	{
		return expr1 + (expr2 - expr1) * amount;
	}
}
