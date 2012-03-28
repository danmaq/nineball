package danmaq.nineball.core.util.math.trignometric
{
	import danmaq.nineball.core.util.math.CTrignometric;

	[Deprecated(replacement="danmaq.nineball.core.util.math.CTrignometric#toRadians()")]

	/**
	 * 角度をラジアンに変換します。
	 *
	 * @param degrees 角度。
 	 * @return 角度に対応したラジアン値。
	 */
	public function toRadians(degrees:Number):Number
	{
		return CTrignometric.toRadians(degrees);
	}
}
