package danmaq.nineball.core.util.math
{

	[Deprecated(replacement="danmaq.nineball.core.util.math.CMathHelper#contains()")]

	/**
	 * 値が範囲内に含まれているかどうかを取得します。
	 *
	 * <p>
	 * <code>limit1</code>に最小値、<code>limit2</code>に最大値を設定することを強く推奨します。
	 * 逆にしても内部で自動認識して置換しますが、その分の余計な負荷がかかってしまいます。
	 * </p>
	 *
	 * @param expr 対象値。
	 * @param limit1 限界値。
	 * @param limit2 限界値。
	 * @return 値が範囲内に含まれている場合、true。
	 */
	public function contains(expr:Number, limit1:Number, limit2:Number):Boolean
	{
		return CMathHelper.contains(expr, limit1, limit2);
	}
}
