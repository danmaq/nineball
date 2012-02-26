package danmaq.nineball.core.data
{

	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.display.PixelSnapping;
	import flash.geom.Matrix;
	import flash.geom.Rectangle;
	import flash.utils.Dictionary;
	
	/**
	 * ビットマップを分割するためのクラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CBitmapSplitter
	{
		
		//* fields ────────────────────────────────*
		
		/** 切り出し矩形座標。 */
		private var _rect:Rectangle;
		
		/** キーとなるオブジェクト。 */
		private var _key:Object;

		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 *
		 * @param rect 切り出し矩形座標。
		 * @param key キーとなるオブジェクト。
		 * @throws ArgumentError <code>rect</code>に<code>null</code>を設定しようとした場合。
		 */
		public function CBitmapSplitter(rect:Rectangle, key:Object = null)
		{
			if(rect == null)
			{
				throw new ArgumentError("rectはnullに設定できません。");
			}
			_rect = rect;
			_key = key;
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * 切り出し矩形座標を取得します。
		 */
		public function get rect():Rectangle
		{
			return _rect;
		}
		
		/**
		 * キーとなるオブジェクトを取得します。
		 */
		public function get key():Object
		{
			return _key;
		}

		//* class methods ────────────────────────────-*
		
		/**
		 * 切り出し情報に従って画像を切り出します。
		 * 
		 * <p>
		 * 戻り値の型はキーオブジェクトを設定している場合は<code>Dictionary</code>型、
		 * そうでない(<code>null</code>である)場合<code>Vector.&lt;Bitmap&gt;</code>型となります。
		 * </p>
		 * 
		 * @param src ソースとなるビットマップ画像。
		 * @param info 切り出し情報リスト。
		 * @param smoothing ビットマップを拡大縮小するときにスムージングするかどうか。
		 * @return ビットマップのリスト。
		 */
		public static function autoSplitter(
			src:BitmapData, info:Vector.<CBitmapSplitter>, smoothing:Boolean = false):Object
		{
			var result:Object = null;
			if(info != null && info.length > 0)
			{
				var bKey:Boolean = true;
				var item:CBitmapSplitter;
				for each(item in info)
				{
					bKey = (item.key != null);
					if(!bKey)
					{
						break;
					}
				}
				if(bKey)
				{
					var dic:Dictionary = new Dictionary();
					for each(item in info)
					{
						dic[item.key] = new Bitmap(
							clipBitmap(src, item.rect, smoothing), PixelSnapping.AUTO, smoothing);
					}
					result = dic;
				}
				else
				{
					var vec:Vector.<Bitmap> = new Vector.<Bitmap>();
					for each(item in info)
					{
						vec.push(new Bitmap(
							clipBitmap(src, item.rect, smoothing), PixelSnapping.AUTO, smoothing));
					}
					result = vec;
				}
			}
			return result;
		}
		
		/**
		 * ビットマップ画像をクリッピングします。
		 * 
		 * @param src ソースとなるビットマップ画像。
		 * @param rect クリッピング範囲。
		 * @param smoothing ビットマップを拡大縮小するときにスムージングするかどうか。
		 * @return クリッピングされたビットマップ画像。
		 */
		public static function clipBitmap(
			src:BitmapData, rect:Rectangle, smoothing:Boolean = false):BitmapData
		{
			var dst:BitmapData = new BitmapData(rect.width, rect.height, true, 0);
			var matrix:Matrix = new Matrix();
			matrix.translate(-rect.x, -rect.y);
			dst.draw(src, matrix, null, null,
				new Rectangle(0, 0, rect.width, rect.height), smoothing);
			return dst;
		}
	}
}
