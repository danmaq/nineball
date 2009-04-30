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
	public final class CSceneGame extends CSceneBase{
		
		////////// CONSTANTS //////////
		
		/**	カウントダウン表示のカラーテーブルが格納されます。 */
		private static const COUNTDOWN_COLOR_TABLE:Vector.<uint> =
			Vector.<uint>( [ 0xA0A0A0, 0x808080, 0, 0x800000 ] );
		
		/**	背景色の矩形が格納されます。 */
		private const bgPattern:Shape = new Shape();
		
		/**	自機玉タスクが格納されます。 */
		private const taskBall:CTaskBallPlayer = new CTaskBallPlayer();

		////////// FIELDS //////////

		/**	カウントダウンフォントタスクが格納されます。 */
		private var m_taskCountDown:CTaskFont = null;

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 * 
		 * @param uLevel 難易度
		 */
		public function CSceneGame(){
			super();
			initializeBackGround();
		}
		
		/**
		 * このシーンが終了する際、管理クラスより呼ばれ
		 * 事実上のデストラクタとして機能するメソッドです。
		 */
		public override function dispose():void{
			CResource.screen.remove( bgPattern );
			super.dispose();
		}
		
		/**
		 * シーンを1フレーム分動かします。
		 * 
		 * @return 次のシーンが設定されるまでの間、true
		 */
		public override function update():Boolean{
			var uPhase:uint = scenePhaseManager.phase;
			var uPCound:uint = scenePhaseManager.phaseCount;
			if( uPCound == 0 ){
				if( m_taskCountDown != null ){ sceneTaskManager.eraseTask( m_taskCountDown ); }
				switch( uPhase ){
				case 1:
				case 2:
				case 3:
					m_taskCountDown = print( StringUtil.substitute( "{0}", 4 - uPhase ),
						new Point( 39, 12 ), COUNTDOWN_COLOR_TABLE[ uPhase - 1 ] );
					break;
				case 4:
					sceneTaskManager.add( taskBall );
					m_taskCountDown = print( CONST.TEXT_GO,
						new Point( 37, 12 ), COUNTDOWN_COLOR_TABLE[ uPhase - 1 ] );
					break;
				}
			}
			if( uPhase < 6 ){ scenePhaseManager.isReserveNextPhase = uPCound >= 60; }
			return super.update();
		}

		/**
		 * 背景を初期化します。
		 */
		private function initializeBackGround():void{
			bgPattern.graphics.beginFill( 0xFFFFFF );
			bgPattern.graphics.drawRect( 0, 0, 640, 400 );
			bgPattern.graphics.endFill();
			bgPattern.cacheAsBitmap = true;
			CResource.screen.add( bgPattern, int.MAX_VALUE );
			print( CONST.TEXT_TITLE, new Point( 21, 23 ), 0x800000 );
			print( StringUtil.substitute( CONST.TEXT_LEVEL, m_uLevel + 1 ), new Point( 0, 24 ), 0x80 );
		}
	}
}
