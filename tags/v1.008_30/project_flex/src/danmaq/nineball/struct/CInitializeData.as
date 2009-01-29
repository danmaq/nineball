package danmaq.nineball.struct{

	import danmaq.nineball.core.*;
	import danmaq.nineball.misc.CMisc;
	
	import flash.display.DisplayObjectContainer;
	import flash.events.Event;
	import flash.utils.Dictionary;
	
	import mx.utils.StringUtil;

	/**
	 * 初期化データを格納する構造体です。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CInitializeData{

		////////// FIELDS //////////

		/**
		 * 内部FPS可変更新レートが格納されます。この値を設定すると
		 * フレームレートが低下した際、一時的にFPSを嵩上げして理論値FPSに
		 * 近づけます。値を0にすると可変FPSは機能しません。低い値にするほど
		 * 可変FPSは敏感になりますが、反面負荷も高まります。
		 */
		public var fpsReflesh:uint = 0;
		
		/**	内部FPS理論値が格納されます。 */
		public var fps:uint = 60;
		
		/**	実測内部FPSの最低許容値が格納されます。 */
		public var fpsLowLimit:uint = 0;
		
		/**
		 * 実測内部FPSの最低許容値を下回る許容回数が格納されます。
		 * これを超えると描画FPSが1/nに低下します。
		 * 値を0にするとFPS強制低下は機能しません。
		 */
		public var fpsLowCount:uint = 0;
		
		/**	内部タスクが使用するレイヤ番号が格納されます。 */
		public var systemTaskLayer:uint = 0;
		
		/**	効果音再生フレームの解像度が格納されます。 */
		public var seResolution:uint = 1;
		
		/**	最初に実行されるシーンが格納されます。 */
		public var sceneFirst:Class = null;
		
		/**	ビットマップフォントの定義リストが格納されます。 */
		public var fontHash:Dictionary = new Dictionary();
		
		/**	画面コンテナクラスが格納されます。 */
		public var main:DisplayObjectContainer = null;
		
		/**	メインループクラスが格納されます。 */
		private var m_mainLoop:CMainLoop = null;

		/**	メインループが開始されたかどうかが格納されます。 */
		private var m_bStarted:Boolean = false;

		////////// PROPERTIES //////////
		
		/**
		 * メインループクラスを取得します。
		 * 
		 * @return メインループ
		 */
		public function get mainLoop():CMainLoop{ return m_mainLoop; }

		/**
		 * データの正当性を判断し、エラーメッセージを出力します。
		 * 
		 * @return エラーメッセージ。データがすべて正当な場合、空文字
		 */
		public function get error():String{
			var strErr:String = "";
			if( fps == 0 ){ strErr += "内部FPS理論値は1以上でなければなりません。\n"; }
			if( fps <= fpsLowLimit ){
				strErr += StringUtil.substitute(
					"FPS下限許容値は内部FPS理論値({0})未満でなければなりません。\n", fps );
			}
			if( seResolution == 0 ){ strErr += "効果音再生フレーム解像度は1以上でなければなりません。\n"; }
			if( main == null ){ strErr += "画面コンテナが定義されていません。\n"; }
			if( sceneFirst == null ){ strErr += "初回実行シーンが定義されていません。\n"; }
			return strErr;
		}

		////////// METHODS //////////
		
		/**
		 * DNLを起動します。
		 * 
		 * @return 起動出来たかどうか。
		 */
		public function run():Boolean{
			var strError:String = error;
			var bResult:Boolean = ( strError.length == 0 );
			if( bResult ){
				if( main.stage == null ){ main.addEventListener( Event.ADDED_TO_STAGE, __run ); }
				else{ __run( null ); }
			}
			else{
				trace( "一部データが不正のため、DNLを実行できませんでした。\n" );
				trace( strError );
			}
			return bResult;
		}
		
		/**
		 * このクラスの状態を文字列で取得します。
		 * 
		 * @return オブジェクトのストリング表現
		 */
		public function toString():String{
			return StringUtil.substitute(
				"内部FPS更新フレーム間隔   : {0}\n" +
				"内部FPS理論値             : {1}\n" +
				"実測内部FPS最低許容値     : {2}\n" +
				"FPS最低許容値未満許容回数 : {3}\n" +
				"内部タスク予約レイヤ番号  : {4}\n" +
				"効果音再生フレーム解像度  : {5}\n" +
				"初回実行シーン            : {6}\n" +
				"画面コンテナ              : {7}\n" +
				"メインループ              : {8}\n" +
				"フォント定義マップ        : {9}",
				fpsReflesh, fps, fpsLowLimit, fpsLowCount, systemTaskLayer, seResolution,
				CMisc.getClassName( sceneFirst ), main, mainLoop == null ? "未生成" : "生成",
				fontHash
			);
		}
		
		/**
		 * DNLを起動します。
		 * 
		 * @param e イベントデータ(nullになることもあります)
		 */
		private function __run( e:Event ):void{
			if( !m_bStarted ){
				m_bStarted = true;
				m_mainLoop = new CMainLoop( this );
			}
		}
	}
}
