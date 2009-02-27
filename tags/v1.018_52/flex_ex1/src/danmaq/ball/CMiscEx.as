package danmaq.ball{

	/**
	 * 汎用関数群クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CMiscEx{

		/**
		 * 数字文字列を
		 */
		public static function ConvNumH2Z( uExpr:uint ):String{
			var strExpr:String = uExpr.toString();
			var strResult:String = "";
			var uLength:uint = strExpr.length;
			var uGap:uint;
			for( var i:uint = 0 ; i < strExpr.length ; i++ ){
				uGap = ( strExpr.charCodeAt( i ) >= 0x30 && strExpr.charCodeAt( i ) <= 0x39 ) ?
					0xFEE0 : 0;
				strResult += String.fromCharCode( strExpr.charCodeAt( i ) + uGap );
			}
			return strResult;
		}
	}
}
