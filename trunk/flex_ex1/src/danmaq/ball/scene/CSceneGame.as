package danmaq.ball.scene{

	import danmaq.ball.resource.CONST;
	import danmaq.ball.resource.CResource;
	import danmaq.nineball.constant.CKeyboardEx;
	import danmaq.nineball.core.CMainLoop;
	import danmaq.nineball.struct.CVirtualInput;
	
	import flash.display.Shape;
	import flash.geom.Point;
	import flash.ui.Keyboard;
	
	import mx.utils.StringUtil;
	
	/**
	 * ゲーム画面シーンです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CSceneGame extends CSceneBase{
		
		////////// CONSTANTS //////////
		
		/**	背景色の矩形が格納されます。 */
		private const bgPattern:Shape = new Shape();
		
		/**	進行仮想ボタンが格納されます。 */
		private const vinputEnter:CVirtualInput = new CVirtualInput();

		////////// FIELDS //////////

		/**	難易度が格納されます。 */
		private var m_uLevel:uint;

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 * 
		 * @param uLevel 難易度
		 */
		public function CSceneGame( uLevel:uint ){
			super();
			m_uLevel = uLevel;
			initializeBackGround();
			initializeVirtualInput();
		}
		
		/**
		 * このシーンが終了する際、管理クラスより呼ばれ
		 * 事実上のデストラクタとして機能するメソッドです。
		 */
		public override function dispose():void{
			CResource.screen.remove( bgPattern );
			CMainLoop.instance.input.resetVI();
			super.dispose();
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

		/**
		 * 仮想ボタンを初期化します。
		 */
		private function initializeVirtualInput():void{
			vinputEnter.assignKeyCodeList.push( Keyboard.ENTER );
			vinputEnter.assignKeyCodeList.push( Keyboard.SPACE );
			vinputEnter.assignKeyCodeList.push( CKeyboardEx.Z );
			CMainLoop.instance.input.addVI( vinputEnter );
		}
	}
}
