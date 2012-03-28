package danmaq.nineball.core.util.math.trignometric
{
	import danmaq.nineball.core.util.math.CTrignometric;

	[Deprecated(replacement="danmaq.nineball.core.util.math.CTrignometric#toDegrees()")]
	
	/**
	 * ラジアンを角度に変換します。
	 *
	 * @param radians ラジアン値。
	 * @return ラジアン値に対応した角度。
	 */
	public function toDegrees(radians:Number):Number
	{
		return CTrignometric.toDegrees(radians);
	}
}
