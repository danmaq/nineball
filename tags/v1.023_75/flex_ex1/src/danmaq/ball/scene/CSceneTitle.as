package danmaq.ball.scene{

	import danmaq.ball.CMiscEx;
	import danmaq.ball.resource.CONST;
	import danmaq.ball.task.CTaskCursor;
	import danmaq.nineball.constant.CKeyboardEx;
	import danmaq.nineball.core.*;
	import danmaq.nineball.struct.*;
	
	import flash.events.FocusEvent;
	import flash.geom.Point;
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
		
		////////// FIELDS //////////

		/**	レベルが決定されたかどうかが格納されます。 */
		private var m_bCommitLevel:Boolean = false;

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 */
		public function CSceneTitle(){
			taskFpsView.transform.color = 0xFFFFFF;
			print( CONST.TEXT_TITLE, new Point( 21, 8 ), 0x00FFFF );
			print( CONST.TEXT_COPY, new Point( 17, 10 ), 0x00FFFF );
			if( m_nResult != 0 ){
				print( m_nResult == 1 ? CONST.TEXT_WON : CONST.TEXT_LOSE, new Point( 0, 3 ) );
			}
		}
		
		/**
		 * このシーンが終了する際、管理クラスより呼ばれ
		 * 事実上のデストラクタとして機能するメソッドです。
		 */
		public override function dispose():void{
			for each( var button:Button in buttonList ){
				button.removeEventListener( FlexEvent.BUTTON_DOWN, onClick );
				button.removeEventListener( FocusEvent.FOCUS_IN, onFocus );
				CScreen.root.remove( button );
			}
			super.dispose();
		}
		
		/**
		 * シーンを1フレーム分動かします。
		 * 
		 * @return 次のシーンが設定されるまでの間、true
		 */
		public override function update():Boolean{
			if( scenePhaseManager.phase == 1 ){
				if( vinputEnter.push ){ commitMenu( getMenuPosFromCursorX( taskCursor.pos.x ) ); }
				if( vinputLeft.push ){
					if( taskCursor.pos.x > 6 ){ taskCursor.pos.x -= 8; }
					else{ taskCursor.pos.x = 70; }
				}
				if( vinputRight.push ){
					if( taskCursor.pos.x < 70 ){ taskCursor.pos.x += 8; }
					else{ taskCursor.pos.x = 6; }
				}
			}
			else if( scenePhaseManager.phaseCount == 90 ){
				scenePhaseManager.isReserveNextPhase = true;
				initialize();
			}
			return super.update();
		}
		
		/**
		 * メニュー位置からカーソルX座標を取得します。
		 * 
		 * @param nMenuPos メニュー位置
		 * @return カーソルX座標
		 */
		private static function getCursorXFromMenuPos( nMenuPos:int ):int{
			return 6 + nMenuPos * 8;
		}

		/**
		 * カーソルX座標からメニュー位置を取得します。
		 * 
		 * @param nMenuPos カーソルX座標
		 * @return メニュー位置
		 */
		private static function getMenuPosFromCursorX( nCursorX:int ):int{
			return ( nCursorX - 6 ) / 8;
		}

		/**
		 * メニューのボタンオブジェクトからメニュー位置を取得します。
		 * 
		 * @param objButton ボタンオブジェクト
		 * @return メニュー位置
		 */
		private static function getMenuPosFromButton( objButton:Object ):int{
			return parseInt( ( objButton as Button ).label );
		}

		private function initialize():void{
			print( CONST.TEXT_SELECT_LEVEL, new Point( 6, 14 ) );
			sceneTaskManager.add( taskCursor );
			taskCursor.pos = new Point( getCursorXFromMenuPos( m_uLevel ), 16 );
			var uY:uint = 16;
			initializeVirtualInput();
			for( var i:int = 0; i < CONST.LEVEL_MAX; i++ ){
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
				buttonList.push( button );
			}
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
		 * メニュー選択(難易度)を確定します。
		 * 
		 * @param uMenuPos メニュー選択位置
		 */
		private function commitMenu( uMenuPos:uint ):void{
			if( !m_bCommitLevel ){
				m_bCommitLevel = true;
				CMainLoop.instance.input.resetVI();
				m_uLevel = uMenuPos;
				nextScene = new CSceneGame();
			}
		}
		
		/**
		 * 難易度選択ボタンがクリックされた際に呼び出されるメソッドです。
		 * 
		 * @param e イベントパラメータ
		 */
		private function onClick( e:FlexEvent ):void{
			var nButtonCursorX:int = getCursorXFromMenuPos( getMenuPosFromButton( e.currentTarget ) );
			if( taskCursor.pos.x == nButtonCursorX ){
				commitMenu( getMenuPosFromCursorX( taskCursor.pos.x ) );
			}
		}

		/**
		 * 難易度選択ボタンにフォーカスが当たった際に呼び出されるメソッドです。
		 * 
		 * @param e イベントパラメータ
		 */
		private function onFocus( e:FocusEvent ):void{
			taskCursor.pos.x = getCursorXFromMenuPos( getMenuPosFromButton( e.currentTarget ) );
		}
	}
}
