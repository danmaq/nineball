package danmaq.ball.scene{

	import danmaq.ball.resource.CResource;
	import danmaq.nineball.core.*;
	import danmaq.nineball.misc.CMisc;
	import danmaq.nineball.struct.CScreen;
	import danmaq.nineball.struct.font.CFontTransform;
	import danmaq.nineball.task.CTaskFPSView;
	import danmaq.nineball.task.CTaskFont;
	
	import flash.display.StageQuality;
	import flash.geom.Point;
	
	import mx.collections.ArrayCollection;
	import mx.controls.ComboBox;
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

		/**	各シーンごとのタスク管理クラスが格納されます。 */
		protected const sceneTaskManager:CTaskManager = new CTaskManager();

		/**	各シーンごとのフェーズ管理クラスが格納されます。 */
		protected const scenePhaseManager:CPhaseManager = new CPhaseManager();

		////////// FIELDS //////////

		/**	次に進むシーンが格納されます。 */
		protected var m_sceneNext:IScene = null;

		/**	初期化済みかどうかが格納されます。 */
		private static var m_bInitialized:Boolean = false;

		////////// PROPERTIES //////////

		/**
		 * 次のシーンを取得します。
		 * 
		 * @return 次のシーン オブジェクト。存在しない場合、null
		 */
		public function get nextScene():IScene{ return m_sceneNext; }

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
		 */
		protected function print( strText:String, posLocate:Point, uColor:uint = 0xFFFFFF ):void{
			var task:CTaskFont = new CTaskFont( CResource.font, CResource.screen );
			sceneTaskManager.add( task );
			task.text = strText;
			task.view = true;
			task.render( new CFontTransform( new Point( posLocate.x * 8, posLocate.y * 16 ), null,
				0, 1, uColor, false, 1, CFontTransform.TOP_LEFT, CFontTransform.TOP_LEFT ) );
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
		 * 初期化時に一度だけ実行されます。
		 */
		private function initialize():void{
			var taskFps:CTaskFPSView = new CTaskFPSView( CResource.font, CResource.screen );
			commonTaskManager.add( taskFps );
			taskFps.prefix = "FPS:";
			taskFps.transform = new CFontTransform(
				 new Point( 400, 0 ), new Point( 1, 1 ), 0, 1, 0xFFFFFF,
				false, 1, CFontTransform.TOP_LEFT, CFontTransform.TOP_LEFT );
			cbQuality.dataProvider = new ArrayCollection( [
				{ label: "Graphic Quality: Best",	data: StageQuality.BEST		},
				{ label: "Graphic Quality: High",	data: StageQuality.HIGH		},
				{ label: "Graphic Quality: Good",	data: StageQuality.MEDIUM	},
				{ label: "Graphic Quality: Lite",	data: StageQuality.LOW		}
			] );
			cbQuality.width = 184;
			cbQuality.x = CScreen.stage.width - cbQuality.width;
			cbQuality.addEventListener(FlexEvent.VALUE_COMMIT, onChangeQuality );
			cbQuality.selectedIndex = 0;
			CScreen.root.add( cbQuality );
		}
	}
}
