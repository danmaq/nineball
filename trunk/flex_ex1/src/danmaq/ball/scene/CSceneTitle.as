package danmaq.ball.scene{

	import danmaq.ball.CMiscEx;
	import danmaq.ball.resource.CONST;
	import danmaq.ball.task.CTaskCursor;
	import danmaq.nineball.core.*;
	import danmaq.nineball.struct.CScreen;
	
	import flash.events.FocusEvent;
	import flash.geom.Point;
	
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

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 */
		public function CSceneTitle(){
			super();
			print( CONST.TITLE, new Point( 21, 8 ), 0x00FFFF );
			print( CONST.COPY, new Point( 17, 10 ), 0x00FFFF );
			print( "難易度を選択してください。", new Point( 6, 14 ) );
			sceneTaskManager.add( taskCursor );
			taskCursor.pos = new Point( 6, 16 );
			var uY:uint = 16;
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
			super.dispose();
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
