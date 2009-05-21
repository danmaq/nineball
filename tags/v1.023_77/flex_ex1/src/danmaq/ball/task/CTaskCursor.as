package danmaq.ball.task{

	import danmaq.ball.resource.*;
	import danmaq.nineball.core.*;
	
	import flash.display.*;
	import flash.geom.Point;

	/**
	 * キーカーソルを表示するタスクです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskCursor implements ITask{

		////////// CONSTANTS //////////

		/**	点灯時間が格納されます。 */
		private static const BLINK_INTERVAL:uint = 30;

		/**	カーソルのグラフィックが格納されます。 */
		private const shapeCursor:Shape = new Shape();

		////////// FIELDS //////////

		/**	簡易カウンタが格納されます。 */
		private var m_uCount:uint = 0;

		private var m_pos:Point = new Point();

		private var m_posPrev:Point = null;

		////////// PROPERTIES //////////

		/**
		 * レイヤ値を取得します。
		 * 
		 * @return レイヤ値
		 */
		public function get layer():uint{ return CONST.LAYER_CURSOR; }

		/**
		 * タスク管理クラスを設定します。
		 * このクラスでは特に必要ないので何も設定しません。
		 * 
		 * @param value タスク管理クラス
		 */
		public function set manager( value:CTaskManager ):void{}

		/**
		 * 一時停止に対応しているかどうかを取得します。
		 * 
		 * @return 無条件にfalse
		 */
		public function get isAvailablePause():Boolean{ return false; }

		/**
		 * 座標を取得します。
		 * 
		 * @return 座標
		 */
		public function get pos():Point{
			m_posPrev = m_pos.clone();
			return m_pos;
		}

		/**
		 * 座標を設定します。
		 * 
		 * @param value 座標
		 */
		public function set pos( value:Point ):void{
			m_posPrev = m_pos;
			m_pos = value == null ? new Point : value;
		}

		////////// METHODS //////////
		
		/**
		 * コンストラクタの後、タスクが管理クラスに登録された直後に、
		 * 1度だけ自動的に呼ばれます。
		 */
		public function initialize():void{
			shapeCursor.graphics.beginFill( 0xFFFFFF );
			shapeCursor.graphics.drawRect( 0, 0, 16, 16 );
			shapeCursor.graphics.endFill();
			shapeCursor.blendMode = BlendMode.INVERT;
			shapeCursor.cacheAsBitmap = true;
			CResource.screen.add( shapeCursor, layer );
		}
		
		/**
		 * 解放時に管理クラスから呼び出される処理です。
		 */
		public function dispose():void{ CResource.screen.remove( shapeCursor ); }
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return 無条件にtrue
		 */
		public function update():Boolean{
			if( m_posPrev != null && !m_pos.equals( m_posPrev ) ){
				shapeCursor.x = m_pos.x * 8;
				shapeCursor.y = m_pos.y * 16;
				m_posPrev = null;
			}
			if( m_uCount % BLINK_INTERVAL ){
				shapeCursor.visible = ( m_uCount % ( BLINK_INTERVAL * 2 ) < BLINK_INTERVAL );
			}
			m_uCount++;
			return true;
		}
	}
}
