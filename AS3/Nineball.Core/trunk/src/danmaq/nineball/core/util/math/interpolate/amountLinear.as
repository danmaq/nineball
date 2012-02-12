package danmaq.nineball.core.util.math.interpolate
{
	
	/**
	 * 直線形の重み計算をします。
	 * 
	 * @param numerator 分子。
	 * @param denominator 分母。
	 * @return numerator～denominatorに対応する、0～1の値。
	 */
	public function amountLinear(numerator:Number, denominator:Number):Number
	{
		return numerator / denominator;
	}
}
