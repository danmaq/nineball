package danmaq.ball.scene{

	import danmaq.ball.resource.*;
	import danmaq.ball.task.*;
	import danmaq.nineball.task.CTaskFont;
	
	import flash.display.Shape;
	import flash.geom.Point;
	
	import mx.utils.StringUtil;
	
	/**
	 * ゲーム画面シーンです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CSceneGame extends CSceneCommon{
		
		////////// CONSTANTS //////////
		
		/**	カウントダウン表示のカラーテーブルが格納されます。 */
		private static const COUNTDOWN_COLOR_TABLE:Vector.<uint> =
			Vector.<uint>([ 0xA0A0A0, 0x808080, 0, 0x800000 ]);
		
		/**	背景色の矩形が格納されます。 */
		private const bgPattern:Shape = new Shape();
		
		////////// FIELDS //////////

		/**	カウントダウンフォントタスクが格納されます。 */
		private var m_taskCountDown:CTaskFont = null;

		/**	自機玉タスクが格納されます。 */
		private var m_taskPlayer:CTaskBallPlayer = null;

		/**	敵機玉タスクが格納されます。 */
		private var m_taskEnemy:CTaskBallEnemy = null;

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 * 
		 * @param uLevel 難易度
		 */
		public function CSceneGame(){
			initializeBackGround();
			taskScore.reset();
		}
		
		/**
		 * このシーンが終了する際、管理クラスより呼ばれ
		 * 事実上のデストラクタとして機能するメソッドです。
		 */
		public override function dispose():void{
			CResource.screen.remove(bgPattern);
			super.dispose();
		}
		
		/**
		 * シーンを1フレーム分動かします。
		 * 
		 * @return 次のシーンが設定されるまでの間、true
		 */
		public override function update():Boolean{
			var uPhase:uint = scenePhaseManager.phase;
			var uPCount:uint = scenePhaseManager.phaseCount;
			if(uPCount == 0){
				if(m_taskCountDown != null){ sceneTaskManager.eraseTask(m_taskCountDown); }
				switch(uPhase){
				case 1:
				case 2:
				case 3:
					m_taskCountDown = print(StringUtil.substitute("{0}", 4 - uPhase),
						new Point(39, 12), COUNTDOWN_COLOR_TABLE[ uPhase - 1 ]);
					break;
				case 4:
					m_taskPlayer = new CTaskBallPlayer();
					m_taskEnemy = new CTaskBallEnemy(m_uLevel, m_taskPlayer);
					sceneTaskManager.add(m_taskPlayer);
					sceneTaskManager.add(m_taskEnemy);
					m_taskCountDown = print(CONST.TEXT_GO,
						new Point(37, 12), COUNTDOWN_COLOR_TABLE[ uPhase - 1 ]);
					break;
				}
			}
			if(uPhase < 6){ scenePhaseManager.isReserveNextPhase = uPCount >= 60; }
			if(uPhase >= 4){
				var nXGap:int = m_taskPlayer.x - m_taskEnemy.x;
				if(uPCount % 5 == 0){ taskScore.add((nXGap + 640) / 100); }
				if(m_taskPlayer.disposed || m_taskEnemy.disposed){
					if(!m_taskEnemy.disposed){ taskScore.add(6400 + nXGap * 10); }
					m_nResult = m_taskEnemy.disposed ? -1 : 1;
					if(!m_taskPlayer.disposed){ sceneTaskManager.eraseTask(m_taskPlayer); }
					nextScene = new CSceneTitle();
				}
			}
			return super.update();
		}

		/**
		 * 背景を初期化します。
		 */
		private function initializeBackGround():void{
			taskFpsView.transform.color = 0;
			bgPattern.graphics.beginFill(0xFFFFFF);
			bgPattern.graphics.drawRect(0, 0, 640, 400);
			bgPattern.graphics.endFill();
			bgPattern.cacheAsBitmap = true;
			CResource.screen.add(bgPattern, int.MAX_VALUE);
			print(CONST.TEXT_DESC, new Point(27, 3), 0xA00000);
			print(CONST.TEXT_TITLE, new Point(21, 23), 0x800000);
			print(StringUtil.substitute(CONST.TEXT_LEVEL, m_uLevel + 1), new Point(0, 24), 0x80);
		}
	}
}
