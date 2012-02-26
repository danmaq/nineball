package danmaq.nineball.core.util.list.iterator
{

	/**
	 * 集合に対する反復処理をサポートする列挙子を公開します。
	 *
	 * <p>
	 * これと、<code>IIterator</code>インターフェイスを実装することにより、
	 * あらゆる集合体に対して、共通の列挙手段を提供することができます。
	 * </p>
	 *
	 * @author Mc(danmaq)
	 * @see danmaq.nineball.core.util.list.iterator.IIterator
	 */
	public interface IAggregate
	{

		//* instance properties ─────────────────────────-*

		/**
		 * 集合を反復処理する列挙子を取得します。
		 *
		 * @return 列挙子。
		 */
		function get iterator():IIterator;
	}
}
