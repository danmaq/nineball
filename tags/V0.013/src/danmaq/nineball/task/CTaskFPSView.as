package danmaq.nineball.task{

	import danmaq.nineball.core.*;

	/**
	 * FPS表示タスクです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskFPSView implements ITask, IDisposed{

		////////// CONSTANTS //////////

		/**	フェーズ管理クラスが格納されます。 */
		private const phaseManager:CPhaseManager = new CPhaseManager();

		////////// FIELDS //////////

		/**	FPS更新のフレーム時間間隔が格納されます。 */
		public var interval:uint = 60;

		/**	レイヤ番号が格納されます。 */
		private var m_uLayer:uint;

		/**	画面番号が格納されます。 */
		private var m_uScreen:uint;

		/**	フォントタスクが格納されます。 */
		private var m_taskFont:CTaskFont = null;

		/**	タスク管理クラスが格納されます。 */
		private var m_taskManager:CTaskManager = null;

		/**	接頭語が格納されます。 */
		private var m_strPrefix:String = "";

		/**	接尾語が格納されます。 */
		private var m_strSuffix:String = "";

		/**	フォントがタスク管理クラスに登録されたかどうかが格納されます。 */
		private var m_bReady:Boolean = false;
		
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
		public function get disposed():Boolean{
			return m_taskFont == null || m_taskFont.disposed;
		}

		/**
		 * フォントタスクを取得します。
		 * 
		 * @return フォントタスク
		 */
		public function get font():CTaskFont{ return m_taskFont; }

		/**
		 * 接頭語を取得します。
		 * 
		 * @return 接頭語
		 */
		public function get prefix():String{ return m_strPrefix; }

		/**
		 * 接頭語を設定します。
		 * 
		 * @param value 接頭語
		 */
		public function set prefix( value:String ):void{
			m_strPrefix = value == null ? "" : value;
		}

		/**
		 * 接尾語を取得します。
		 * 
		 * @return 接尾語
		 */
		public function get suffix():String{ return m_strSuffix; }

		/**
		 * 接尾語を設定します。
		 * 
		 * @param value 接尾語
		 */
		public function set suffix( value:String ):void{
			m_strSuffix = value == null ? "" : value;
		}

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param uScreen 画面番号
		 * @param uLayer レイヤ番号
		 */
		public function CTaskFPSView( uScreen:uint = 0, uLayer:uint = 0 ){
			m_uScreen = uScreen;
			m_uLayer = uLayer;
			m_taskFont = new CTaskFont( uScreen, uLayer );
			m_taskFont.autoRender = true;
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
			if( m_bReady ){ m_taskManager.eraseTask( m_taskFont ); }
		}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return 無条件でtrue
		 */
		public function update():Boolean{
			var uPCount:uint = phaseManager.phaseCount;
			if( uPCount == 0 ){ reflesh(); }
			phaseManager.isReserveNextPhase = ( uPCount >= interval );
			phaseManager.count++;
			return true;
		}
		
		private function reflesh():void{
			if( !m_bReady ){
				m_taskManager.add( m_taskFont );
				m_bReady = true;
				m_taskFont.view = true;
			}
			font.text = prefix + CMainLoop.instance.timer.realFPS + suffix;
		}
	}
}