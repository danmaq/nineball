package danmaq.nineball.core.util.math.trignometric
{

	[Deprecated(replacement="danmaq.nineball.core.util.math.CMathHelper#toRadians()")]

	/**
	 * 角度をラジアンに変換します。
	 *
	 * @param degrees 角度。
 	 * @return 角度に対応したラジアン値。
	 */
	public function toRadians(degrees:Number):Number
	{
		return degrees / 180 * Math.PI;
	}
}
