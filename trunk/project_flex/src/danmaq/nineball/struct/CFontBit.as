package danmaq.nineball.struct{

	import danmaq.nineball.core.*;
	import danmaq.nineball.misc.CMisc;
	import danmaq.nineball.misc.math.CMathMisc;
	
	import flash.display.*;
	import flash.errors.IllegalOperationError;
	import flash.geom.*;
	import flash.utils.Dictionary;

	/**
	 * 単文字フォントクラスです。
	 * CTaskFontの内部で使用されるクラスです。
	 * 通常、直接使用することはありません。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CFontBit implements IDisposed{

		////////// CONSTANTS //////////

		/**	座標が格納されます。 */
		public const pos:Point = new Point();
		
		/**	拡大率が格納されます。 */
		public const scale:Point = new Point( 1, 1 );
		
		/**	色変換構造体が格納されます。 */
		public const color:ColorTransform = new ColorTransform();
		
		////////// FIELDS //////////
		
		/**	ビットマップフォントの定義リストが格納されます。 */
		public static var fontHash:Dictionary = new Dictionary();

		/**	回転角度が格納されます。 */
		public var rotate:Number = 0;
		
		/**	初期サイズが格納されます。 */
		private var m_size:Point = new Point();
		
		/**	レイヤ番号が格納されます。 */
		private var m_uLayer:uint;

		/**	画面番号が格納されます。 */
		private var m_uScreen:uint;

		/**	解放されたかどうかが格納されます。 */
		private var m_bDisposed:Boolean = false;
		
		/**	表示するテキストが格納されます。 */
		private var m_strText:String = "";

		/**	現在表示されているかどうかが格納されます。 */
		private var m_bView:Boolean = false;

		/**	画像が格納されます。 */
		private var m_image:DisplayObject = null;

		////////// PROPERTIES //////////

		/**
		 * 解放したかどうかを取得します。
		 * 
		 * @return 解放している場合、true
		 */
		public function get disposed():Boolean{ return m_bDisposed; }

		/**
		 * 現在表示されているかどうかを取得します。
		 * 
		 * @return 現在表示されているいる場合、true
		 */
		public function get view():Boolean{ return m_bView; }

		/**
		 * 現在表示されているかどうかを設定します。
		 * 
		 * @param value 現在表示されているかどうか
		 */
		public function set view( value:Boolean ):void{
			if( m_image == null ){
				throw new IllegalOperationError( "表示すべき画像が準備されていません。" );
			}
			if( m_bView != value ){
				m_bView = value;
				if( value ){
					CMainLoop.instance.screenList[ m_uScreen ].add( m_image, m_uLayer );
				}
				else{ CMainLoop.instance.screenList[ m_uScreen ].remove( m_image ); }
			}
		}

		/**
		 * 表示するテキストを取得します。
		 * 
		 * @return 表示するテキスト
		 */
		public function get text():String{ return m_strText; }

		/**
		 * 表示するテキストを設定します。
		 * 2文字以上設定すると、例外を出力します。
		 * 
		 * @param value 表示するテキスト
		 */
		private function set byte( value:String ):void{
			if( m_strText != value ){
				if( value == " " ){
					var shape:Shape = new Shape();
					shape.graphics.drawRect( 0, 0, fontHash[ " " ], 1 );
					m_image = shape;
				}
				else{
					var imgByte:Class = getImage( value );
					if( imgByte == null ){
						throw new IllegalOperationError(
							"指定の文字は割り当てられていません。:" + value );
					}
					m_image = new imgByte();
					try{ ( m_image as Bitmap ).smoothing = true; }
					catch( e:Error ){}
				}
				m_size = new Point( m_image.width, m_image.height );
				m_strText = value;
			}
		}

		/**
		 * 拡縮しない状態での画像サイズを取得します。
		 * 
		 * @return 画像サイズ
		 */
		public function get size():Point{ return m_size.clone(); }

		/**
		 * 透過度を取得します。
		 * 
		 * @return 透過度(0～1)
		 */
		public function get alpha():Number{ return m_image.alpha; }

		/**
		 * 透過度を設定します。
		 * 
		 * @param value 透過度(0～1)
		 */
		public function set alpha( value:Number ):void{
			m_image.alpha = CMathMisc.clamp( value, 0, 1 );
		}

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param strByte 単文字
		 * @param uScreen 画面番号
		 * @param uLayer レイヤ番号
		 * @param fSpacing スペース幅
		 */
		public function CFontBit(
			strByte:String, uScreen:uint = 0, uLayer:uint = 0, fSpacing:Number = 4
		){
			initializeHash( fSpacing );
			m_uScreen = uScreen;
			m_uLayer = uLayer;
			byte = strByte;
		}

		/**
		 * デストラクタ。
		 */
		public function dispose():void{
			view = false;
			m_bDisposed = true;
		}

		/**
		 * レンダリングします。
		 */
		public function render():void{
			m_image.transform.colorTransform = color;
			CMisc.setMatrix( m_image, scale, rotate, pos );
		}
		
		/**
		 * 文字に対応する画像クラスを取得します。
		 * 
		 * @param strByte 単文字
		 * @return 文字に対応する画像クラス、存在しない場合、null
		 */
		private static function getImage( strByte:String ):Class{
			if( strByte == null || strByte.length == 0 || strByte.length >= 2 ){
				throw new IllegalOperationError( "引数は1文字でなければなりません。" );
			}
			return fontHash[ strByte ];
		}
		
		/**
		 * フォント定義リストにスペース幅が入っていない場合、補完します。
		 * 
		 * @param fSpacing スペース幅
		 */
		private function initializeHash( fSpacing:Number ):void{
			if( fontHash[ " " ] == null ){ fontHash[ " " ] = fSpacing; }
		} 
	}
}
