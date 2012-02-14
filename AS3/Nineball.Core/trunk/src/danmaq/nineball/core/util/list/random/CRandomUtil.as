package danmaq.nineball.core.util.list.random
{
	import danmaq.nineball.core.util.list.iterator.IIterator;
	import danmaq.nineball.core.util.object.blockStatic;

	/**
	 * 疑似乱数ジェネレータの補助関数群クラス。
	 *
	 * @author Mc(danmaq)
	 */
	public final class CRandomUtil implements IRandom
	{

		//* fields ────────────────────────────────*
		
		/** 擬似乱数ジェネレータ。 */
		private var _random:IRandom;
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 *
		 * @param random 疑似乱数ジェネレータ。
		 */
		public function CRandomUtil(random:IRandom = null)
		{
			if (random == null)
			{
				random = new CSFMT();
			}
			_random = random;
		}

		//* instance properties ─────────────────────────-*
		
		/**
		 * 擬似乱数系列の開始値を計算するために使用するシード値を取得します。
		 *
		 * @return シード値。
		 */
		public function get seed():uint
		{
			return random.seed;
		}

		/**
		 * 乱数の使用カウンタを取得します。
		 *
		 * @return 乱数の使用カウント値。
		 */
		public function get counter():uint
		{
			return random.counter;
		}
		
		/**
		 * 0からmaxプロパティまでの範囲内の擬似乱数を取得します。
		 *
		 * @return 乱数値。
		 */
		public function get next():uint
		{
			return random.next;
		}
		
		/**
		 * 最大値を取得します。
		 *
		 * @return 最大値。
		 */
		public function get max():uint
		{
			return random.max;
		}
		
		/**
		 * 集合を反復処理する列挙子を取得します。
		 *
		 * このクラスの反復子は常に同一のオブジェクトを返します。
		 *
		 * @return 列挙子。
		 */
		public function get iterator():IIterator
		{
			return random.iterator;
		}

		/**
		 * 擬似乱数ジェネレータを取得します。
		 *
		 * @return 擬似乱数ジェネレータ。
		 */
		public function get random():IRandom
		{
			return _random;
		}
		
		/**
		 * 0から1までの範囲内の擬似乱数を取得します。
		 *
		 * @return 乱数値。
		 */
		public function get nextNumber():Number
		{
			return next / max;
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * 乱数を初期化します。
		 *
		 * @param seed シード値。負数を指定した場合、システム依存値が設定されます。
		 */
		public function reset(seed:int = int.MIN_VALUE):void
		{
			random.reset(seed);
		}
		
		/**
		 * 乱数による誤差を計算します。
		 *
		 * @param blur 誤差のブレ幅。
		 * @return -blur～+blurまでの値。
		 */
		public function nextBlur(blur:Number):Number
		{
			return nextNumber * blur * 2 - blur;
		}
		
		/**
		 * 乱数を計算します。
		 *
		 * @param limit1 限界値。
		 * @param limit2 限界値。
		 * @return 値の範囲内の乱数。
		 */
		public function nextLimit(limit1:Number, limit2:Number):Number
		{
			if (limit1 > limit2)
			{
				var tmp:Number = limit1;
				limit1 = limit2;
				limit2 = tmp;
			}
			return next % (limit2 - limit1) + limit1;
		}

		/**
		 * 乱数を計算します。
		 *
		 * @param limit1 限界値。
		 * @param limit2 限界値。
		 * @return 値の範囲内の乱数。
		 */
		public function nextLimitInt(limit1:int, limit2:int):int
		{
			if (limit1 > limit2)
			{
				var tmp:int = limit1;
				limit1 = limit2;
				limit2 = tmp;
			}
			return next % (limit2 - limit1) + limit1;
		}
	}
}
