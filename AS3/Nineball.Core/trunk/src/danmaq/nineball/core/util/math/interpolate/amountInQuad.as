package danmaq.nineball.core.util.math.interpolate
{

	/**
	 * 曲線形の重み計算をします。
	 *
	 * @param numerator 分子。
	 * @param denominator 分母。
	 * @return <code>numerator</code>～<code>denominator</code>に対応する、0.0～1.0の値。
	 */
	public function amountInQuad(numerator:Number, denominator:Number):Number
	{
		return validateParams(numerator, denominator) ? (numerator /= denominator) * numerator : 0;
	}
}
