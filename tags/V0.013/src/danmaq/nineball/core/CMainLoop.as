package danmaq.nineball.core{

	import __AS3__.vec.Vector;
	
	import danmaq.nineball.struct.*;
	import danmaq.nineball.task.*;
	
	import flash.display.*;
	import flash.events.TimerEvent;
	import flash.utils.Timer;

	/**
	 * メインループクラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CMainLoop{

		////////// CONSTANTS //////////

		/**	画面オブジェクト管理クラスが格納されます。 */
		public const screenList:Vector.<CScreen> = new Vector.<CScreen>();

		/**	シーン管理クラスが格納されます。 */
		private const sceneManager:CSceneManager = new CSceneManager();

		/**	タスク管理クラスが格納されます。 */
		private const taskManager:CTaskManager = new CTaskManager();

		////////// FIELDS //////////

		/**	このクラスのインスタンスが格納されます。 */
		private static var m_instance:CMainLoop = null;

		/**	排他制御付き効果音再生タスクが格納されます。 */
		private var m_taskSE:CTaskExclusiveSE = null;

		/**	フレームレート制御タスクが格納されます。 */
		private var m_taskFPS:CTaskFPSTimer = null;

		/**	簡易カウンタが格納されます。 */
		private var m_uCount:uint = 0;
		
		/**	前フレームが動作中かどうかが格納されます。 */
		private var m_bDoPrevLoop:Boolean = false;

		////////// PROPERTIES //////////
		
		/**
		 * このクラスのインスタンスを取得します。
		 * 
		 * @return このクラスのインスタンス
		 */
		public static function get instance():CMainLoop{ return m_instance; }

		/**
		 * 排他制御付き効果音再生タスクを取得します。
		 * 
		 * @return 排他制御付き効果音再生タスク
		 */
		public function get taskSE():CTaskExclusiveSE{ return m_taskSE; }
		
		/**
		 * フレームレート制御タスクを取得します。
		 * 
		 * @return フレームレート制御タスク
		 */
		public function get timer():CTaskFPSTimer{ return m_taskFPS; }

		/**
		 * 親画面オブジェクト管理クラスを取得します。
		 * 
		 * @return 親画面オブジェクト管理クラス
		 */
		public function get screenParent():CScreen{ return screenList[ 0 ].parent; }

		/**
		 * メイン描画領域(Stage)クラスを取得します。
		 * 
		 * @return メイン描画領域(Stage)クラス
		 */
		public function get stage():Stage{ return screenParent.screen.stage; }

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param ini 初期化データ
		 */
		public function CMainLoop( ini:CInitializeData ){
			m_instance = this;
			CTaskFontByte.fontHash = ini.fontHash;
			var screen:CScreen = new CScreen( 0 );
			screenList.push( screen );
			ini.main.addChild( screen.parent.screen );
			m_taskFPS = new CTaskFPSTimer( ini.main.stage, ini.systemTaskLayer,
				ini.fpsReflesh, ini.fps, ini.fpsLowLimit, ini.fpsLowCount );
			taskManager.add( timer );
			m_taskSE = new CTaskExclusiveSE( ini.systemTaskLayer, ini.seResolution );
			taskManager.add( taskSE );
			sceneManager.add( new ini.sceneFirst() );
			startLoop();
		}

		/**
		 * メインループを開始します。
		 */
		private function startLoop():void{
			var t:Timer = timer.timer;
			t.addEventListener( "timer", mainLoop );
			t.start();
		}

		/**
		 * メインループ。
		 * 毎秒およそ60回呼ばれます。
		 */
		private function mainLoop( event:TimerEvent ):void{
			if( !m_bDoPrevLoop ){
				m_bDoPrevLoop = true;
				taskManager.update();
				sceneManager.update();
				m_uCount++;
				if( m_uCount % timer.refleshInterval == 0 ){ startLoop(); }
				m_bDoPrevLoop = false;
			}
			else{ trace( "skip" ); }
		}
	}
}
