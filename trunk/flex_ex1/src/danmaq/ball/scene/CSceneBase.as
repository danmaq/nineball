package danmaq.ball.scene{

	import danmaq.ball.resource.*;
	import danmaq.nineball.core.*;
	import danmaq.nineball.misc.CMisc;
	import danmaq.nineball.struct.CScreen;
	import danmaq.nineball.struct.font.CFontTransform;
	import danmaq.nineball.task.*;
	
	import flash.display.*;
	import flash.events.*;
	import flash.geom.*;
	
	import mx.collections.ArrayCollection;
	import mx.controls.*;
	import mx.events.FlexEvent;

	/**
	 * ゲーム中全てのシーンの基底となるクラスです。
	 * 共通するコントロールなどが格納されます。
	 * 
	 * @author Mc(danmaq)
	 */
	public class CSceneBase implements IScene{

		////////// CONSTANTS //////////

		/**	全シーン共通のタスク管理クラスが格納されます。 */
		private static const commonTaskManager:CTaskManager = new CTaskManager();

		/**	画質調整用コンボボックスが格納されます。 */
		private static const cbQuality:ComboBox = new ComboBox();

		/**	フルスクリーン切り替えボタンが格納されます。 */
		private static const btnChangeScreenMode:Button = new Button();

		/**	各シーンごとのタスク管理クラスが格納されます。 */
		protected const sceneTaskManager:CTaskManager = new CTaskManager();

		/**	各シーンごとのフェーズ管理クラスが格納されます。 */
		protected const scenePhaseManager:CPhaseManager = new CPhaseManager();

		////////// FIELDS //////////

		/**	難易度が格納されます。 */
		protected static var m_uLevel:uint = 0;
		
		/**	次に進むシーンが格納されます。 */
		protected var m_sceneNext:IScene = null;

		/**	FPS描画タスクが格納されます。 */
		private static var m_taskFps:CTaskFPSView = null;

		/**	初期化済みかどうかが格納されます。 */
		private static var m_bInitialized:Boolean = false;

		////////// PROPERTIES //////////

		/**
		 * 次のシーンを取得します。
		 * 
		 * @return 次のシーン オブジェクト。存在しない場合、null
		 */
		public function get nextScene():IScene{ return m_sceneNext; }

		/**
		 * FPS描画タスクを取得します。
		 * 
		 * @return FPS描画タスク
		 */
		protected static function get taskFpsView():CTaskFPSView{ return m_taskFps; }

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 */
		public function CSceneBase(){
			if( !m_bInitialized ){
				initialize();
				m_bInitialized = true;
			}
		}
		
		/**
		 * このシーンが終了する際、管理クラスより呼ばれ
		 * 事実上のデストラクタとして機能するメソッドです。
		 */
		public function dispose():void{
			sceneTaskManager.dispose();
			if( !CMisc.isRelate( CSceneBase, this ) ){
				CScreen.root.remove( cbQuality );
				commonTaskManager.dispose();
				m_bInitialized = false;
			}
		}
		
		/**
		 * シーンを1フレーム分動かします。
		 * 
		 * @return 次のシーンが設定されるまでの間、true
		 */
		public function update():Boolean{
			commonTaskManager.update();
			sceneTaskManager.update();
			scenePhaseManager.count++;
			return m_sceneNext == null;
		}

		/**
		 * 文字を描画します。
		 * 
		 * @param strText 文字列
		 * @param posLocate 文字単位座標
		 * @param uColor カラーコード
		 * @return 文字列タスク
		 */
		protected function print(
			strText:String, posLocate:Point, uColor:uint = 0xFFFFFF
		):CTaskFont{
			var task:CTaskFont =
				new CTaskFont( CResource.font, CResource.screen, CONST.LAYER_TEXT );
			sceneTaskManager.add( task );
			task.text = strText;
			task.view = true;
			task.render( new CFontTransform( new Point( posLocate.x * 8, posLocate.y * 16 ), null,
				0, 1, uColor, false, 1, CFontTransform.TOP_LEFT, CFontTransform.TOP_LEFT ) );
			return task;
		}

		/**
		 * 初期化時に一度だけ実行されます。
		 */
		private static function initialize():void{
			CScreen.stage.scaleMode = StageScaleMode.SHOW_ALL;
			m_taskFps = new CTaskFPSView( CResource.font, CResource.screen );
			commonTaskManager.add( taskFpsView );
			taskFpsView.prefix = "FPS:";
			taskFpsView.transform = new CFontTransform(
				 new Point( 336, 0 ), new Point( 1, 1 ), 0, 1, 0xFFFFFF,
				false, 1, CFontTransform.TOP_LEFT, CFontTransform.TOP_LEFT );
			initializeQualityComboBox();
			initializeChangeScreenButton();
		}
		
		/**
		 * 画質設定コンボボックスの初期化をします。
		 */
		private static function initializeQualityComboBox():void{
			cbQuality.dataProvider = new ArrayCollection( [
				{ label: "Quality: Best",	data: StageQuality.BEST		},
				{ label: "Quality: High",	data: StageQuality.HIGH		},
				{ label: "Quality: Good",	data: StageQuality.MEDIUM	},
				{ label: "Quality: Lite",	data: StageQuality.LOW		}
			] );
			cbQuality.width = 144;
			cbQuality.x = CScreen.stage.width - cbQuality.width;
			cbQuality.addEventListener( FlexEvent.VALUE_COMMIT, onChangeQuality );
			cbQuality.selectedIndex = 0;
			cbQuality.toolTip = "Graphic quality level"
			CScreen.root.add( cbQuality );
		}
		
		/**
		 * フルスクリーン切り替えボタンの初期化をします。
		 */
		private static function initializeChangeScreenButton():void{
			btnChangeScreenMode.width = 96;
			btnChangeScreenMode.x = cbQuality.x - btnChangeScreenMode.width;
			btnChangeScreenMode.label = "FullScreen";
			btnChangeScreenMode.toggle = true;
			btnChangeScreenMode.addEventListener( Event.CHANGE, onToggleChangeScreen );
			CScreen.stage.fullScreenSourceRect =
				new Rectangle( 0, 0, CScreen.size.x, CScreen.size.y );
			CScreen.stage.addEventListener( FullScreenEvent.FULL_SCREEN, onFullScreen );
			CScreen.root.add( btnChangeScreenMode );
		}

		/**
		 * 画質調整コンボボックスの値が変更された時に
		 * 自動的にコールバックされるメソッドです。
		 * 
		 * @param e イベントパラメータ
		 */
		private static function onChangeQuality( e:FlexEvent ):void{
			CScreen.stage.quality = cbQuality.value as String;
		}

		/**
		 * フルスクリーン切り替えボタンが押された時に
		 * 自動的にコールバックされるメソッドです。
		 * 
		 * @param e イベントパラメータ
		 */
		private static function onToggleChangeScreen( e:Event ):void{
			CScreen.stage.displayState = btnChangeScreenMode.selected ?
				StageDisplayState.FULL_SCREEN : StageDisplayState.NORMAL;
		}

		/**
		 * 画面がフルスクリーンモードになった時に
		 * 自動的にコールバックされるメソッドです。
		 * 
		 * @param e イベントパラメータ
		 */
		private static function onFullScreen( e:FullScreenEvent ):void{
			btnChangeScreenMode.selected = e.fullScreen;
		}
	}
}
