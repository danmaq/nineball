////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.old.core
{
	import danmaq.nineball.old.data.*;
	import danmaq.nineball.old.task.*;
	import danmaq.nineball.old.task.scene.CTaskSceneManager;
	
	import flash.display.*;
	import flash.events.TimerEvent;
	import flash.utils.Timer;

	/**
	 * メインループクラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CMainLoop
	{

		////////// CONSTANTS //////////

		/**	タスク管理クラスが格納されます。 */
		public const taskManager:CTaskManager = new CTaskManager();

		////////// FIELDS //////////

		/**	このクラスのインスタンスが格納されます。 */
		private static var m_instance:CMainLoop = null;

		/**	シーン管理クラスが格納されます。 */
		private var m_taskScene:CTaskSceneManager = new CTaskSceneManager();

		/**	排他制御付き効果音再生タスクが格納されます。 */
		private var m_taskSE:CTaskExclusiveSE = null;

		/**	フレームレート制御タスクが格納されます。 */
		private var m_taskFPS:CTaskFPSTimer = null;
		
		/**	仮想ボタン入力タスクが格納されます。 */
		private var m_taskInput:CTaskVirtualInput = null;

		/**	簡易カウンタが格納されます。 */
		private var m_uCount:uint = 0;
		
		/**	前フレームが動作中かどうかが格納されます。 */
		private var m_bDoPrevLoop:Boolean = false;

		/**	DNL起動時間が格納されます。 */
		private var m_fStartTime:Number;

		////////// PROPERTIES //////////
		
		/**
		 * このクラスのインスタンスを取得します。
		 * 
		 * @return このクラスのインスタンス
		 */
		public static function get instance():CMainLoop
		{
			return m_instance;
		}

		/**
		 * シーン管理タスクを取得します。
		 * 
		 * @return シーン管理タスク
		 */
		public function get sceneManager():CTaskSceneManager
		{
			return m_taskScene;
		}

		/**
		 * 排他制御付き効果音再生タスクを取得します。
		 * 
		 * @return 排他制御付き効果音再生タスク
		 */
		public function get se():CTaskExclusiveSE
		{
			return m_taskSE;
		}
		
		/**
		 * フレームレート制御タスクを取得します。
		 * 
		 * @return フレームレート制御タスク
		 */
		public function get timer():CTaskFPSTimer
		{
			return m_taskFPS;
		}

		/**
		 * フ仮想ボタン入力タスクを取得します。
		 * 
		 * @return 仮想ボタン入力タスク
		 */
		public function get input():CTaskVirtualInput
		{
			return m_taskInput;
		}

		/**
		 * danmaq Nineball Libraryの起動からの経過時間を取得します。
		 * 
		 * @return danmaq Nineball Libraryの起動からの経過時間
		 */
		public function get startTime():Number
		{
			return (new Date()).time - m_fStartTime;
		}

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param ini 初期化データ
		 */
		public function CMainLoop(ini:CInitializeData)
		{
			m_fStartTime = (new Date()).time;
			m_instance = this;
			m_taskFPS = new CTaskFPSTimer(ini.systemTaskLayer,
				ini.fpsReflesh, ini.fps, ini.fpsLowLimit, ini.fpsLowCount);
			m_taskSE = new CTaskExclusiveSE(ini.systemTaskLayer, ini.seResolution);
			m_taskInput = new CTaskVirtualInput(ini.systemTaskLayer);
			m_taskScene = new CTaskSceneManager(ini.systemTaskLayer);
			taskManager.add(timer);
			taskManager.add(se);
			taskManager.add(input);
			taskManager.add(sceneManager);
			sceneManager.add(new ini.sceneFirst());
			startLoop();
		}

		/**
		 * メインループを開始します。
		 */
		private function startLoop():void
		{
			var t:Timer = timer.timer;
			t.addEventListener(TimerEvent.TIMER, mainLoop);
			t.start();
		}

		/**
		 * 定期的に呼び出されるメインループ。
		 * 
		 * <p>
		 * 1秒ごとの呼び出し回数は負荷によって若干前後しますが、
		 * FPSとほぼ同等になります。
		 * </p>
		 */
		private function mainLoop(event:TimerEvent):void
		{
			if(!m_bDoPrevLoop)
			{
				m_bDoPrevLoop = true;
				taskManager.update();
				m_uCount++;
				if(m_uCount % timer.refleshInterval == 0)
				{
					startLoop();
				}
				m_bDoPrevLoop = false;
			}
			else
			{
				trace("skip");
			}
		}
	}
}
