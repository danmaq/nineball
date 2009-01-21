package danmaq.nineball.task{

	import __AS3__.vec.Vector;
	
	import danmaq.nineball.core.*;
	import danmaq.nineball.misc.math.CMathMisc;
	import danmaq.nineball.struct.CAlign;
	
	import flash.errors.IllegalOperationError;
	import flash.geom.Point;

	/**
	 * フォント描画タスクです。
	 * タスクを殺すには管理クラスからeraseするか、
	 * またはタイマを0に設定します。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskFont implements ITask, IDisposed{

		////////// CONSTANTS //////////

		/**	基準となる座標(現時点では左上固定)が格納されます。 */
		public const pos:Point = new Point();

		/**	拡大率が格納されます。 */
		public const scale:Point = new Point( 1, 1 );
		
		/**	単文字フォントタスクが格納されます。 */
		private const taskByteList:Vector.<CTaskFontByte> = new Vector.<CTaskFontByte>();

		////////// FIELDS //////////
		
		/**
		 * 水平位置情報が格納されます。
		 * danmaq.nineball.struct.CAlignクラスの定数を使用してください。
		 */
		public var halign:int = 0;
		
		/**
		 * 垂直位置情報が格納されます。
		 * danmaq.nineball.struct.CAlignクラスの定数を使用してください。
		 */
		public var valign:int = 0;
		
		/**
		 * 生存タイマが格納されます。
		 * 負数にすることでタイマを切ることができます。
		 */
		public var timer:int = -1;

		/** テキスト入力時に自動レンダリングするかどうかが格納されます。 */
		public var autoRender:Boolean = false;

		/**	現在表示されているかどうかが格納されます。 */
		public var view:Boolean = false;

		/**
		 * カーニングの度合が格納されます。
		 * 0で-100%、1で0%です。
		 */
		public var kerning:Number = 1;

		/**	タスク管理クラスが格納されます。 */
		private var m_taskManager:CTaskManager = null;

		/**	レイヤ番号が格納されます。 */
		private var m_uLayer:uint;

		/**	画面番号が格納されます。 */
		private var m_uScreen:uint;

		/**	解放されたかどうかが格納されます。 */
		private var m_bDisposed:Boolean = false;
		
		/**	表示するテキストが格納されます。 */
		private var m_strText:String = "";

		/**	テキスト生成を予約するかどうかが格納されます。 */
		private var m_bReserveCreateText:Boolean = false;

		/**	色が格納されます。 */
		private var m_uColor:uint = 0xFFFFFF;

		/**	透明度が格納されます。 */
		private var m_fAlpha:Number = 1;


		////////// PROPERTIES //////////

		/**
		 * レイヤ値を取得します。
		 * 
		 * @return レイヤ値
		 */
		public function get layer():uint{ return m_uLayer; }
		
		/**
		 * タスク管理クラスを設定します。
		 * 
		 * @param value タスク管理クラス
		 */
		public function set manager( value:CTaskManager ):void{
			m_taskManager = value;
		}

		/**
		 * 解放したかどうかを取得します。
		 * 
		 * @return 解放している場合、true
		 */
		public function get disposed():Boolean{ return m_bDisposed; }

		/**
		 * 表示するテキストを取得します。
		 * 
		 * @return 表示するテキスト
		 */
		public function get text():String{ return m_strText; }

		/**
		 * 表示するテキストを設定します。
		 * 
		 * @param value 表示するテキスト
		 */
		public function set text( value:String ):void{
			if( value == null ){ throw new IllegalOperationError( "引数にnullを設定出来ません。" ); }
			if( m_strText != value ){
				m_strText = value;
				m_bReserveCreateText = ( m_taskManager == null );
				if( !m_bReserveCreateText ){ createChild(); }
			}
		}

		/**
		 * 文字列画像のサイズを取得します。
		 * 
		 * @return 文字列画像のサイズ
		 */
		public function get size():Point{
			var posResult:Point = new Point();
			for each( var task:CTaskFontByte in taskByteList ){
				posResult.x += task.size.x * scale.x * kerning;
				posResult.y = Math.max( posResult.y, task.size.y * scale.y );
			}
			return posResult;
		}

		/**
		 * 色を取得します。
		 *
		 * @return 色情報(赤8bit、緑8bit、青8bit)
		 */
		public function get color():uint{ return m_uColor; }

		/**
		 * 色を設定します。
		 * 範囲外の値を設定すると自動的に丸められます。
		 *
		 * @param value 色情報(赤8bit、緑8bit、青8bit)
		 */
		public function set color( value:uint ):void{ m_uColor = ( value & 0xFFFFFF ); }

		/**
		 * 透明度を取得します。
		 *
		 * @return 透明度(透明0～1不透明)
		 */
		public function get alpha():Number{ return m_fAlpha; }

		/**
		 * 透明度を設定します。
		 * 範囲外の値を設定すると自動的に丸められます。
		 *
		 * @param value 透明度(透明0～1不透明)
		 */
		public function set alpha( value:Number ):void{
			m_fAlpha = CMathMisc.clamp( value, 0, 1 );
		}

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param uScreen 画面番号
		 * @param uLayer レイヤ番号
		 */
		public function CTaskFont( uScreen:uint = 0, uLayer:uint = 0 ){
			m_uScreen = uScreen;
			m_uLayer = uLayer;
		}

		/**
		 * コンストラクタの後、タスクが管理クラスに登録された直後に、
		 * 1度だけ自動的に呼ばれます。
		 */
		public function initialize():void{}

		/**
		 * デストラクタ。
		 */
		public function dispose():void{
			deleteChild();
			m_bDisposed = true;
		}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return 生存タイマが0でない限りtrue
		 */
		public function update():Boolean{
			if( m_bReserveCreateText ){
				createChild();
				m_bReserveCreateText = false;
			}
			return ( timer < 0 || timer-- > 0 );
		}

		/**
		 * 指定したパラメータどおりにレンダリングします。
		 */
		public function render():void{
			var posSize:Point = size;
			var fX:Number;
			var fY:Number;
			switch( halign ){
				case CAlign.TOP_LEFT:			fX = pos.x;					break;
				case CAlign.BOTTOM_RIGHT:		fX = pos.x - posSize.x;		break;
				case CAlign.CENTER:	default:	fX = pos.x - posSize.x / 2;	break;
			}
			switch( valign ){
				case CAlign.TOP_LEFT:			fY = pos.y + posSize.y / 2;	break;
				case CAlign.BOTTOM_RIGHT:		fY = pos.y - posSize.y / 2;	break;
				case CAlign.CENTER:	default:	fY = pos.y;					break;
			}
			for each( var task:CTaskFontByte in taskByteList ){
				var fHWidth:Number = task.size.x * scale.x / 2;
				fX += fHWidth * kerning;
				task.pos.x = fX;
				task.pos.y = fY;
				task.scale.x = scale.x;
				task.scale.y = scale.y;
				task.color.color = color;
				task.render();
				task.alpha = alpha;
				task.view = view;
				fX += fHWidth * kerning;
			}
		}

		/**
		 * 子タスクを抹消します。
		 */
		private function deleteChild():void{
			while( taskByteList.length > 0 ){ m_taskManager.eraseTask( taskByteList.pop() ); }
		}

		/**
		 * 子タスクを生成します。
		 */
		private function createChild():void{
			deleteChild();
			try{
				var uLen:uint = m_strText.length;
				for( var i:uint = 0; i < uLen; i++ ){
					var task:CTaskFontByte =
						new CTaskFontByte( m_strText.charAt( i ), m_uScreen, layer );
					taskByteList.push( task );
					m_taskManager.add( task );
				}
				if( autoRender ){ render(); }
			}
			catch( e:Error ){
				deleteChild();
				m_strText = "";
				throw e;
			}
		}
	}
}
