package danmaq.nineball.core.util.list.random
{
	import flash.display.BitmapData;
	import flash.geom.Point;

	/**
	 * (恐らく)システム既定のものを用いた疑似乱数ジェネレータ。
	 *
	 * @author Mc(danmaq)
	 */
	public final class CSystem extends CRandom
	{
		
		//* constants ──────────────────────────────-*
		
		/** 擬似乱数を計算に使用するビットマップ画像。 */
		private const bitmap:BitmapData = new BitmapData(16, 16);
		
		/** 座標インデックス情報。 */
		private const position:Point = new Point();
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 初期シード値に負数を指定した場合、システム依存値が設定されます。
		 *
		 * @param seed シード値。
		 */
		public function CSystem(seed:int=int.MIN_VALUE)
		{
			super(seed);
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * 最大値を取得します。
		 *
		 * @return 最大値。
		 */
		override public function get max():uint
		{
			return 0xFFFFFF;
		}
		
		/**
		 * 0からmaxプロパティまでの範囲内の擬似乱数を取得します。
		 *
		 * @return 乱数値。
		 */
		override public function get next():uint
		{
			addCounter();
			var result:int = bitmap.getPixel(position.x, position.y);
			if(++position.x >= bitmap.width)
			{
				position.x = 0;
				if(++position.y >= bitmap.height)
				{
					position.y = 0;
					bitmap.noise(result);
				}
			}
			return result & max;
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * @inheritDoc
		 */
		override public function reset(seed:int=int.MIN_VALUE):void
		{
			super.reset(seed);
			bitmap.noise(seed);
			position.x = 0;
			position.y = 0;
		}
	}
}
