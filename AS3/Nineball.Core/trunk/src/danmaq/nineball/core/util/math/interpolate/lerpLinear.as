package danmaq.nineball.core.util.math.interpolate
{
	
	/**
	 * 線形補完の直線変換をします。
	 * 
	 * @param expr1 対象値1。
	 * @param expr2 対象値2。
	 * @param numerator 重みを決定する分子。
	 * @param denominator 重みを決定する分母。
	 * @return numeratorの0～denominatorに対応する、expr1～expr2の値。
	 */
	public function lerpLinear(
		expr1:Number, expr2:Number, numerator:Number, denominator:Number):Number
	{
		return lerp(expr1, expr2, amountLinear(numerator, denominator));
	}
}
