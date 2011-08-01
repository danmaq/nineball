package danmaq.ball.scene{

	import danmaq.ball.CMiscEx;
	import danmaq.ball.resource.CONST;
	import danmaq.ball.task.CTaskCursor;
	import danmaq.nineball.constant.CKeyboardEx;
	import danmaq.nineball.core.*;
	import danmaq.nineball.struct.*;
	
	import flash.events.FocusEvent;
	import flash.geom.Point;
	import flash.system.IME;
	import flash.ui.Keyboard;
	
	import mx.controls.Button;
	import mx.events.FlexEvent;
	import mx.utils.StringUtil;

	/**
	 * タイトルシーンです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CSceneTitle extends CSceneBase{

		////////// CONSTANTS //////////

		/**	カーソルタスクが格納されます。 */
		private const taskCursor:CTaskCursor = new CTaskCursor();

		/**	ボタン一覧が格納されます。 */
		private const buttonList:Vector.<Button> = new Vector.<Button>();

		/**	決定仮想ボタンが格納されます。 */
		private const vinputEnter:CVirtualInput = new CVirtualInput();

		/**	左仮想ボタンが格納されます。 */
		private const vinputLeft:CVirtualInput = new CVirtualInput();

		/**	右仮想ボタンが格納されます。 */
		private const vinputRight:CVirtualInput = new CVirtualInput();
		
		/**	数字仮想ボタンのリストが格納されます。 */
		private const vinputNumList:Vector.<CVirtualInput> = new Vector.<CVirtualInput>();
		
		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 */
		public function CSceneTitle(){
			super();
			try{ IME.enabled = false; }
			catch( e:Error ){}
			print( CONST.TITLE, new Point( 21, 8 ), 0x00FFFF );
			print( CONST.COPY, new Point( 17, 10 ), 0x00FFFF );
			print( "難易度を選択してください。", new Point( 6, 14 ) );
			sceneTaskManager.add( taskCursor );
			taskCursor.pos = new Point( 6, 16 );
			var uY:uint = 16;
			initializeVirtualInput();
			for( var i:int = 0; i < 9; i++ ){
				var uX:uint = 6 + i * 8;
				print( CMiscEx.ConvNumH2Z( i + 1 ), new Point( uX, uY ) );
				var button:Button = new Button();
				button.styleName = "level";
				button.toolTip = StringUtil.substitute( "Level {0}", i + 1 );
				button.x = uX * 8;
				button.y = uY * 16;
				button.width = 16;
				button.height = 16;
				button.label = i.toString();
				button.alpha = 0;
				button.tabIndex = i + 1;
				CScreen.root.add( button );
				button.addEventListener( FlexEvent.BUTTON_DOWN, onClick );
				button.addEventListener( FocusEvent.FOCUS_IN, onFocus );
			}
		}
		
		/**
		 * このシーンが終了する際、管理クラスより呼ばれ
		 * 事実上のデストラクタとして機能するメソッドです。
		 */
		public override function dispose():void{
			for each( var button:Button in buttonList ){ CScreen.root.remove( button ); }
			CMainLoop.instance.input.resetVI();
			super.dispose();
		}
		
		/**
		 * シーンを1フレーム分動かします。
		 * 
		 * @return 次のシーンが設定されるまでの間、true
		 */
		public override function update():Boolean{
			if( vinputEnter.push ){ trace( "ENTER" ); }
			if( vinputLeft.push ){ taskCursor.pos.x -= 8; }
			if( vinputRight.push ){ taskCursor.pos.x += 8; }
			return super.update();
		}
		
		/**
		 * 仮想ボタンを割り当てます。
		 */
		private function initializeVirtualInput():void{
			vinputEnter.assignKeyCodeList.push( Keyboard.ENTER );
			vinputEnter.assignKeyCodeList.push( Keyboard.SPACE );
			vinputEnter.assignKeyCodeList.push( CKeyboardEx.Z );
			CMainLoop.instance.input.addVI( vinputEnter );
			vinputLeft.assignKeyCodeList.push( Keyboard.LEFT );
			vinputLeft.assignKeyCodeList.push( CKeyboardEx.A );
			CMainLoop.instance.input.addVI( vinputLeft );
			vinputRight.assignKeyCodeList.push( Keyboard.RIGHT );
			vinputRight.assignKeyCodeList.push( CKeyboardEx.D );
			CMainLoop.instance.input.addVI( vinputRight );
		}
		
		/**
		 * 難易度選択ボタンがクリックされた際に呼び出されるメソッドです。
		 * 
		 * @param e イベントパラメータ
		 */
		private function onClick( e:FlexEvent ):void{
			trace( "CLICK", ( e.currentTarget as Button ).label );
			
		}

		/**
		 * 難易度選択ボタンにフォーカスが当たった際に呼び出されるメソッドです。
		 * 
		 * @param e イベントパラメータ
		 */
		private function onFocus( e:FocusEvent ):void{
			trace( "FOCUS", ( e.currentTarget as Button ).label );
			taskCursor.pos.x = 6 + parseInt( ( e.currentTarget as Button ).label ) * 8;
		}
	}
}
