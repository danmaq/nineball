package danmaq.nineball.core.util.math.interpolate
{
	import danmaq.nineball.core.util.math.CInterpolate;

	[Deprecated(replacement="danmaq.nineball.core.util.math.CInterpolate#amountLinear()")]
	
	/**
	 * 直線形の重み計算をします。
	 *
	 * @param numerator 分子。
	 * @param denominator 分母。
	 * @return <code>numerator</code>～<code>denominator</code>に対応する、0.0～1.0の値。
	 */
	public function amountLinear(numerator:Number, denominator:Number):Number
	{
		return CInterpolate.amountLinear(numerator, denominator);
	}
}
