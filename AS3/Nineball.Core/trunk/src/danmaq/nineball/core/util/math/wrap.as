package danmaq.nineball.core.util.math
{

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
		var result:Number = min;
		if (min != max)
		{
			if (min > max)
			{
				var tmp:Number = max;
				max = min;
				min = tmp;
			}
			var cmpMore:Function;
			var cmpLess:Function;
			cmpMore = allowMax ? more : eqMore;
			cmpLess = allowMin ? less : eqLess;
			var moreMax:Boolean = false;
			var lessMin:Boolean = false;
			do
			{
				// TODO : ちょっと作りがダサイ。それに値によっては無限ループになり得る。
				moreMax = cmpMore(expr, max);
				lessMin = cmpLess(expr, min);
				if (moreMax)
				{
					expr = min + expr - max - 1;
				}
				if (lessMin)
				{
					expr = max - Math.abs(expr - min) + 1;
				}
			}
			while (moreMax || lessMin);
			result = clamp(expr, min, max);
		}
		return result;
	}
}

/**
 * 値が閾値未満かどうかを取得します。
 *
 * @param expr 対象の値。
 * @param threshold 閾値。
 * @return 値が閾値未満である場合、true。
 */
function less(expr:Number, threshold:Number):Boolean
{
	return expr < threshold;
}

/**
 * 値が閾値以下かどうかを取得します。
 *
 * @param expr 対象の値。
 * @param threshold 閾値。
 * @return 値が閾値以下である場合、true。
 */
function eqLess(expr:Number, threshold:Number):Boolean
{
	return expr <= threshold;
}

/**
 * 値が閾値を超過しているかどうかを取得します。
 *
 * @param expr 対象の値。
 * @param threshold 閾値。
 * @return 値が閾値を超過している場合、true。
 */
function more(expr:Number, threshold:Number):Boolean
{
	return expr > threshold;
}

/**
 * 値が閾値以上かどうかを取得します。
 *
 * @param expr 対象の値。
 * @param threshold 閾値。
 * @return 値が閾値以下である場合、true。
 */
function eqMore(expr:Number, threshold:Number):Boolean
{
	return expr >= threshold;
}
