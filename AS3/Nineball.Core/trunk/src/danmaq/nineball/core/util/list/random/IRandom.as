package danmaq.nineball.core.util.list.random
{
	import danmaq.nineball.core.util.list.iterator.IAggregate;
	
	/**
	 * 疑似乱数に必要なメソッドを定義します。
	 * 
	 * @author Mc(danmaq)
	 */
	public interface IRandom extends IAggregate
	{
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * 擬似乱数系列の開始値を計算するために使用するシード値を取得します。
		 * 
		 * @return シード値。
		 */
		function get seed():uint;
		
		/**
		 * 乱数の使用カウンタを取得します。
		 * 
		 * @return 乱数の使用カウント値。
		 */
		function get counter():uint;
		
		/**
		 * 0からmaxプロパティまでの範囲内の擬似乱数を取得します。
		 * 
		 * @return 乱数値。
		 */
		function get next():uint;
		
		/**
		 * 最大値を取得します。
		 * 
		 * @return 最大値。
		 */
		function get max():uint;
		
		//* instance methods ───────────────────────────*
		
		/**
		 * 乱数を初期化します。
		 * 
		 * @param seed シード値。負数を指定した場合、システム依存値が設定されます。
		 */
		function reset(seed:int = int.MIN_VALUE):void;
	}
}
