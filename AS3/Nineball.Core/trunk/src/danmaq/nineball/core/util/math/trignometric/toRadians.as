package danmaq.nineball.core.util.math.trignometric
{

	/**
	 * 角度をラジアンに変換します。
	 *
	 * @param fDegree 角度
 	 * @return 角度に対応したラジアン値
	 */
	public function toRadians(degrees:Number):Number
	{
		return degrees / 180 * Math.PI;
	}
}
