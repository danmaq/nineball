////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.entity
{
	import danmaq.nineball.state.IState;
	
	import flash.events.TimerEvent;
	import flash.utils.Timer;

	/**
	 * メインループなどに使用できる、周期的にUpdateが自動で呼び出されるクラスです。
	 * fpsTimerの設定を変更することにより、周期のカスタマイズもできます。
	 * 
	 * @author Mc(danmaq)
	 */
	public class CTimerLoop extends CEntity
	{
		////////// CONSTANTS //////////

		/**	フレームレート管理クラスが格納されます。 */
		public const fpsTimer:CFPSTimer = new CFPSTimer();

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param firstState 最初の状態。
		 */
		public function CTimerLoop(firstState:IState=null)
		{
			super(firstState);
		}
		
		/**
		 * ループを開始します。
		 */
		public function start():void
		{
			var t:Timer = fpsTimer.timer;
			t.addEventListener(TimerEvent.TIMER, onTimer);
			t.start();
		}
		
		/**
		 * タイマイベントが発生した時に呼び出されます。
		 * 
		 * @param e イベント情報。
		 */
		private function onTimer(e:TimerEvent):void
		{
			update();
			if(counter % fpsTimer.refleshInterval == 0)
			{
				start();
			}
		}
	}
}
