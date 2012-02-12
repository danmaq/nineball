package danmaq.nineball.core.util.list.iterator
{

	/**
	 * 集合に対する反復処理をサポートする列挙子を公開します。
	 * 
	 * @author Mc(danmaq)
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
