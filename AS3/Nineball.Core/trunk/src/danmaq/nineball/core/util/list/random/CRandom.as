package danmaq.nineball.core.util.list.random
{
	import danmaq.nineball.core.util.list.iterator.IIterator;
	import danmaq.nineball.core.util.object.blockAbstract;
	
	import flash.utils.getTimer;
	
	/**
	 * 疑似乱数ジェネレータの基底となる抽象クラス。
	 * 
	 * <p>
	 * これはIRandomインターフェイスのうち、各種疑似乱数ジェネレータに共通する最低限の
	 * 機能を実装した抽象クラスです。このクラスを単体で使用することはできません。
	 * 各種ロジックを実装した疑似乱数ジェネレータの詳細については、継承先クラスを参照してください。
	 * </p>
	 *
	 * @author Mc(danmaq)
	 * @see danmaq.nineball.core.util.list.random.CRandomUtil
	 * @see danmaq.nineball.core.util.list.random.CStaticRandom
	 */
	public class CRandom implements IRandom
	{

		//* fields ────────────────────────────────*
		
		/** シード値。 */
		private var _seed:uint;

		/** 反復子。 */
		private var _iterator:IIterator;
		
		/** 乱数の使用カウンタ。 */
		private var _counter:uint;
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 *
		 * <p>
		 * これは抽象クラスです。このクラスを直接生成することはできません。
		 * </p>
		 * 
		 * @param seed シード値。負数を指定した場合、システム依存値が設定されます。
		 */
		public function CRandom(seed:int=int.MIN_VALUE)
		{
			blockAbstract(this, CRandom);
			_iterator = new CRandomIterator(this);
			reset(seed);
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * 擬似乱数系列の開始値を計算するために使用するシード値を取得します。
		 *
		 * @return シード値。
		 * @see #reset
		 */
		public function get seed():uint
		{
			return _seed;
		}
		
		/**
		 * 乱数の使用カウンタを取得します。
		 *
		 * @return 乱数の使用カウント値。
		 */
		public function get counter():uint
		{
			return _counter;
		}
		
		/**
		 * 最大値を取得します。
		 *
		 * このメソッドは抽象メソッドです。継承先で上書きしないと例外が発生します。
		 *
		 * @return 最大値。
		 */
		public function get max():uint
		{
			throw new Error("実装されていない抽象メソッドが呼び出されました。");
		}
		
		/**
		 * 0からmaxプロパティまでの範囲内の擬似乱数を取得します。
		 *
		 * このメソッドは抽象メソッドです。継承先で上書きしないと例外が発生します。
		 *
		 * @return 乱数値。
		 */
		public function get next():uint
		{
			throw new Error("実装されていない抽象メソッドが呼び出されました。");
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
			return _iterator;
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * 乱数を初期化します。
		 *
		 * @param seed シード値。負数を指定した場合、システム依存値が設定されます。
		 */
		public function reset(seed:int=int.MIN_VALUE):void
		{
			_seed = seed >= 0 ? seed : getTimer();
			_counter = 0;
		}
		
		/**
		 * 乱数カウンタをインクリメントします。
		 */
		protected function addCounter():void
		{
			_counter++;
		}
	}
}
