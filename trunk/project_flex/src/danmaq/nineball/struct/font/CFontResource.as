package danmaq.nineball.struct.font{

	import flash.display.*;
	import flash.errors.IllegalOperationError;
	import flash.geom.Point;
	import flash.utils.Dictionary;

	/**
	 * フォントリソースクラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CFontResource{

		////////// FIELDS //////////

		/**	画像一覧が格納されます。 */
		private var m_dicImage:Dictionary;

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param dicImage 画像一覧
		 * @param spaceSize スペースサイズ
		 */
		public function CFontResource( dicImage:Dictionary, spaceSize:Point = null ){
			if( dicImage[ " " ] == null ){
				if( spaceSize == null ){ spaceSize = new Point( 1, 1 ); }
				dicImage[ " " ] = new Bitmap( new BitmapData( spaceSize.x, spaceSize.y ) );
			}
			m_dicImage = dicImage;
		}
		
		/**
		 * 文字に対応する画像を取得します。
		 * 
		 * @param strByte 単文字
		 * @return 文字に対応する画像。存在しない場合、null
		 */
		public function getImage( strByte:String ):Bitmap{
			if( strByte == null || strByte.length == 0 || strByte.length >= 2 ){
				throw new IllegalOperationError( "引数は1文字でなければなりません。" );
			}
			return new Bitmap( ( m_dicImage[ strByte ] as Bitmap ).bitmapData.clone() );
		}
	}
}
