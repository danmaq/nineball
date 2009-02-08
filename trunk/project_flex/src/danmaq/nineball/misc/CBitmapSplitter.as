package danmaq.nineball.misc{

	import danmaq.nineball.resource.CSentence;
	
	import flash.display.*;
	import flash.errors.IllegalOperationError;
	import flash.geom.*;
	import flash.utils.Dictionary;
	
	import mx.utils.StringUtil;

	/**
	 * ビットマップ分割クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CBitmapSplitter{

		////////// FIELDS //////////

		/**	キーとなる文字列が格納されます。 */
		private var m_objKey:Object;
		
		/**	切り出し矩形座標が格納されます。 */
		private var m_rectClip:Rectangle;

		////////// PROPERTIES //////////

		/**
		 * キーとなるオブジェクトを取得します。
		 * 
		 * @return キーオブジェクト。存在しない場合、null
		 */
		public function get key():Object{ return m_objKey; }

		/**
		 * 切り出し矩形座標を取得します。
		 * 
		 * @return 切り出し矩形座標
		 */
		public function get rect():Rectangle{ return m_rectClip; }

		////////// METHODS //////////

		/**
		 * 切り出し情報に従って画像を切り出します。
		 * 
		 * <p>
		 * 戻り値の型はキーオブジェクトを設定している場合は
		 * Dictionary型、そうでない(nullである)場合Vector型となります。
		 * </p>
		 * 
		 * @param src ソースとなるビットマップ画像
		 * @param info 切り出し情報リスト
		 * @return ビットマップ画像のリスト
		 */
		public static function autoSplitter( src:BitmapData, info:Vector.<CBitmapSplitter> ):Object{
			var objResult:Object = null;
			if( info != null && info.length > 0 ){
				var bKey:Boolean = true;
				var item:CBitmapSplitter;
				for each( item in info ){
					bKey = ( item.key != null );
					if( !bKey ){ break; }
				}
				if( bKey ){
					var dic:Dictionary = new Dictionary();
					for each( item in info ){ dic[ item.key ] = clipBitmap( src, item.rect ); }
					objResult = dic;
				}
				else{
					var vec:Vector.<Bitmap> = new Vector.<Bitmap>();
					for each( item in info ){ vec.push( clipBitmap( src, item.rect ) ); }
					objResult = vec;
				}
			}
			return objResult;
		}

		/**
		 * ビットマップ画像をクリッピングします。
		 * 
		 * @param src ソースとなるビットマップ画像
		 * @param rect クリッピング範囲
		 * @return クリッピングされたビットマップ画像
		 */
		public static function clipBitmap( src:BitmapData, rect:Rectangle ):Bitmap{
			var __rect:Rectangle = new Rectangle( 0, 0, rect.width, rect.height );
			var dst:BitmapData = new BitmapData( rect.width, rect.height, true, 0 );
			var matrix:Matrix = new Matrix();
			matrix.translate( -rect.x, -rect.y );
			dst.draw( src, matrix, new ColorTransform(), null, __rect );
			return new Bitmap( dst );
		}

		/**
		 * コンストラクタ。
		 * 
		 * @param rect 切り出し矩形座標
		 * @param key キーとなるオブジェクト
		 */
		public function CBitmapSplitter( rect:Rectangle, key:Object = null ){
			if( rect == null ){
				throw new IllegalOperationError(
					StringUtil.substitute( CSentence.NOT_NULL, "rect" ) );
			}
			m_objKey = key;
			m_rectClip = rect;
		}
	}
}
