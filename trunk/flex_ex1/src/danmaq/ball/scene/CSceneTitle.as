package danmaq.ball.scene{

	import danmaq.ball.resource.CONST;
	import danmaq.nineball.core.*;
	
	import flash.geom.Point;

	/**
	 * タイトルシーンです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CSceneTitle extends CSceneBase{

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 */
		public function CSceneTitle(){
			super();
			print( CONST.TITLE, new Point( 21, 8 ), 0x00FFFF );
			print( CONST.COPY, new Point( 17, 10 ), 0x00FFFF );
			print( "難易度を選択してください。", new Point( 5, 14 ) );
			var astrLevel:Vector.<String> = Vector.<String>( [
				"１", "２", "３", "４", "５", "６", "７", "８", "９" ] );
			var uLength:uint = astrLevel.length;
			for( var i:uint = 0; i < uLength; i++ ){
				print( astrLevel[ i ], new Point( 5 + i * 8, 16 ) );
			}
		}
	}
}
