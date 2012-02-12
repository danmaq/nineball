package danmaq.nineball.core.util.list.random
{
	
	/**
	 * 線形合同法を用いた疑似乱数ジェネレータ。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CLCG extends CRandom
	{
		
		//* constants ──────────────────────────────-*
		
		/** 擬似乱数を計算するために使用する数値。 */
		private static const Y:uint = 1664525;
		
		/** 擬似乱数を計算するために使用する数値。 */
		private static const Z:uint = 1013904223;

		//* fields ────────────────────────────────*
		
		/** 擬似乱数を計算するために使用する数値。 */
		private var _x:Number;
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 初期シード値に負数を指定した場合、システム依存値が設定されます。
		 * 
		 * @param seed シード値。
		 */
		public function CLCG(seed:int=int.MIN_VALUE)
		{
			super(seed);
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * @inheritDoc
		 */
		override public function get max():uint
		{
			return 32767;
		}
		
		/**
		 * 0からmaxプロパティまでの範囲内の擬似乱数を取得します。
		 * 
		 * @return 乱数値。
		 */
		override public function get next():uint
		{
			addCounter();
			_x = (_x * Y + Z) % int.MAX_VALUE;
			return ((_x >> 16) % (max + 1));
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * @inheritDoc
		 */
		override public function reset(seed:int=int.MIN_VALUE):void
		{
			super.reset(seed);
			_x = this.seed;
		}
	}
}
