package danmaq.nineball.core.util.math.trignometric
{

	[Deprecated(replacement="danmaq.nineball.core.util.math.CMathHelper#toDegrees()")]
	
	/**
	 * ラジアンを角度に変換します。
	 *
	 * @param radians ラジアン値。
	 * @return ラジアン値に対応した角度。
	 */
	public function toDegrees(radians:Number):Number
	{
		return radians * 180 / Math.PI;
	}
}
