package danmaq.ball.struct{

	import danmaq.ball.resource.CResource;
	import danmaq.ball.scene.CSceneTitle;
	import danmaq.nineball.misc.CBitmapSplitter;
	import danmaq.nineball.struct.CInitializeData;
	import danmaq.nineball.struct.font.CFontResource;
	
	import flash.geom.*;
	import flash.utils.Dictionary;

	/**
	 * 初期データ設定用クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CInitializer{

		////////// PROPERTIES //////////

		/**
		 * DNL初期設定データを取得します。
		 * 
		 * @return DNL初期設定データ
		 */
		public static function get dnl():CInitializeData{
			var ini:CInitializeData = new CInitializeData();
			ini.fps = 60;
			ini.fpsReflesh = 30;
			ini.fpsLowLimit = 50;
			ini.fpsLowCount = 5;
			ini.seResolution = 3;
			ini.sceneFirst = CSceneTitle;
			return ini;
		}
		
		/**
		 * フォントリソースを初期化します。
		 */
		public static function initializeBitmapFontList():void{
			var strHalf:String = "()-/.0123456789:ACEFHIMOPRSacdeghilmnqrstv";
			var strFull:String = "０１２３４５６７８９。！ー●いくけ" + 
				"さしだちてをゲスペム易競玉勝青赤選走打択点度得難負連";
			var info:Vector.<CBitmapSplitter> = new Vector.<CBitmapSplitter>();
			var uLength:uint = strHalf.length;
			var i:uint;
			for( i = 0; i < uLength; i++ ){
				info.push( new CBitmapSplitter(
					new Rectangle( ( i * 8 ) % 128, int( i / 16 ) * 16, 8, 16 ),
					strHalf.charAt( i ) ) );
			}
			uLength = strFull.length;
			for( i = 0; i < uLength; i++ ){
				var uGap:uint = i + 21;
				info.push( new CBitmapSplitter(
					new Rectangle( ( uGap * 16 ) % 128, int( uGap / 8 ) * 16, 16, 16 ),
					strFull.charAt( i ) ) );
			}
			CResource.font = new CFontResource( CBitmapSplitter.autoSplitter(
				( new CResource.FONT_IMAGE() ).bitmapData, info ) as Dictionary, new Point( 8, 16 ) );
		}
	}
}
