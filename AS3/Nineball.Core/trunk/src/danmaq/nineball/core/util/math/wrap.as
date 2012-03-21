package danmaq.nineball.core.util.math
{

	[Deprecated(replacement="danmaq.nineball.core.util.math.CMathHelper#wrap()")]
	
	/**
	 * 値を指定された範囲内に制限し、超過した場合は範囲内でループします。
	 *
	 * <p>
	 * 注意：最小値と最大値を逆さに設定しても内部で自動的に認識・交換しますが、
	 * 無駄なオーバーヘッドが増えるだけなので極力避けてください。
	 * </p>
	 *
	 * @param expr 対象の値。
	 * @param min 制限値1(最小)。
	 * @param max 制限値1(最大)。
	 * @param allowMin 制限値1(最小)と同値の状態を許容するかどうか。
	 * @param allowMax 制限値2(最大)と同値の状態を許容するかどうか。
	 * @return 制限された値。
	 */
	public function wrap(expr:Number, min:Number, max:Number,
						 allowMin:Boolean = true, allowMax:Boolean = true):Number
	{
		return CMathHelper.wrap(expr, min, max, allowMin, allowMax);
	}
}
