package danmaq.nineball.core.util.math.interpolate
{
	
	/**
	 * 引数の検証を行います。
	 *
	 * @param numerator 分子。
	 * @param denominator 分母。
	 * @return 引数が正当な値である場合、<code>true</code>。
	 */
	internal function validateParams(numerator:Number, denominator:Number):Boolean
	{
		return !(denominator == 0 || isNaN(numerator) || isNaN(denominator));
	}
}