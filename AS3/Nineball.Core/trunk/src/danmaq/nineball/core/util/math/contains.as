package danmaq.nineball.core.util.math
{
	
	/**
	 * 値が範囲内に含まれているかどうかを取得します。
	 * 
	 * <p>
	 * <code>limit1</code>に最小値、<code>limit2</code>に最大値を設定することを推奨します。
	 * 逆にしても内部で自動認識して対応しますが、余計な負荷がかかります。
	 * </p>
	 *
	 * @param expr 対象値。
	 * @param limit1 限界値。
	 * @param limit2 限界値。
	 * @return 値が範囲内に含まれている場合、true。
	 */
	public function contains(expr:Number, limit1:Number, limit2:Number):Boolean
	{
		if (limit1 > limit2)
		{
			var tmp:Number = limit1;
			limit1 = limit2;
			limit2 = tmp;
		}
		return expr >= limit1 && expr <= limit2;
	}
}
