package danmaq.nineball.core.util.list.random
{

	/**
	 * XOR Shift法を用いた疑似乱数ジェネレータ。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CXORShift extends CRandom
	{
		
		//* fields ────────────────────────────────*
		
		/** 擬似乱数を計算するために使用する数値。 */
		private var _x:uint;
		
		/** 擬似乱数を計算するために使用する数値。 */
		private var _y:uint;
		
		/** 擬似乱数を計算するために使用する数値。 */
		private var _z:uint;
		
		/** 擬似乱数を計算するために使用する数値。 */
		private var _w:uint;
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 初期シード値に負数を指定した場合、システム依存値が設定されます。
		 * 
		 * @param seed シード値。
		 */
		public function CXORShift(seed:int=int.MIN_VALUE)
		{
			super(seed);
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * @inheritDoc
		 */
		override public function get max():uint
		{
			return uint.MAX_VALUE;
		}
		
		/**
		 * 0からmaxプロパティまでの範囲内の擬似乱数を取得します。
		 * 
		 * @return 乱数値。
		 */
		override public function get next():uint
		{
			addCounter();
			var t:uint = (_x ^ (_x << 11));
			_x = _y;
			_y = _z;
			_z = _w;
			return (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * @inheritDoc
		 */
		override public function reset(seed:int=int.MIN_VALUE):void
		{
			super.reset(seed);
			seed = this.seed;
			_w = seed;
			_x = seed << 16 + seed >> 16;
			_y = seed + _x;
			_z = _x ^ _y;
		}
	}
}
