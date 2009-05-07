package danmaq.ball.task{

	import danmaq.nineball.constant.CKeyboardEx;
	import danmaq.nineball.core.CMainLoop;
	import danmaq.nineball.struct.*;
	
	import flash.geom.Rectangle;
	import flash.ui.Keyboard;

	/**
	 * 自機玉タスクです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskBallPlayer extends CTaskBall{

		////////// CONSTANTS //////////
		
		/**	進行仮想ボタンが格納されます。 */
		private const vinputEnter:CVirtualInput = new CVirtualInput();

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 */
		public function CTaskBallPlayer(){
			super( 0x0000C0 );
			y = 120;
			vinputEnter.assignKeyCodeList.push( Keyboard.ENTER );
			vinputEnter.assignKeyCodeList.push( Keyboard.SPACE );
			vinputEnter.assignKeyCodeList.push( CKeyboardEx.Z );
			vinputEnter.assignMouseAreaList.push(
				new Rectangle( 0, 0, CScreen.size.x, CScreen.size.y ) );
			CMainLoop.instance.input.addVI( vinputEnter );
		}

		/**
		 * 解放時に管理クラスから呼び出される処理です。
		 */
		public override function dispose():void{
			CMainLoop.instance.input.resetVI();
			super.dispose();
		}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return ゴールにたどり着くまでの間、true
		 */
		public override function update():Boolean{
			if( vinputEnter.push ){ move(); }
			return super.update();
		}
	}
}
