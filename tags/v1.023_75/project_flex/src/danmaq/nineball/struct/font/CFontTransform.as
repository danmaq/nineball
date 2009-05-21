package danmaq.nineball.struct.font{

	import flash.geom.Point;
	
	import mx.utils.StringUtil;

	/**
	 * フォントの描画調整情報を保持する構造体です。
	 * 
	 * @see danmaq.nineball.task.CTaskFont
	 * @author Mc(danmaq) 
	 */
	public final class CFontTransform extends CFontTransformBit{

		////////// CONSTANTS //////////

		/**	上端寄せ、または左端寄せを指示するための定数が格納されます。 */
		public static const TOP_LEFT:String		= "TOP_LEFT";
		
		/**	センタリングを指示するための定数が格納されます。 */
		public static const CENTER:String		= "CENTER";
		
		/**	下端寄せ、または右端寄せを指示するための定数が格納されます。 */
		public static const BOTTOM_RIGHT:String	= "BOTTOM_RIGHT";

		////////// FIELDS //////////
		
		/**	水平位置揃え情報が格納されます。 */
		public var halign:String;
		
		/**	垂直位置揃え情報が格納されます。 */
		public var valign:String;
		
		/**
		 * カーニングの度合が格納されます。
		 * 0で-100%、1で0%です。
		 */
		public var kerning:Number;

		////////// PROPERTIES //////////

		/**
		 * オブジェクトのコピーを取得します。
		 *
		 * @return オブジェクトのコピー
		 */
		public override function get clone():CFontTransformBit{
			return new CFontTransform(
				pos, scale, rotate, alpha, color, smoothing, kerning, halign, valign );
		}

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 *
		 * @param pos 座標
		 * @param scale 拡大率
		 * @param rotate 回転角度
		 * @param alpha 透過度
		 * @param color 乗算するカラーコード
		 * @param smoothing アンチエイリアスを施すかどうか
		 * @param kerning カーニング度合
		 * @param halign 水平位置揃え情報
		 * @param valign 垂直位置揃え情報
		 */
		public function CFontTransform(
			pos:Point = null, scale:Point = null, rotate:Number = 0,
			alpha:Number = 1, color:uint = 0xFFFFFF, smoothing:Boolean = false,
			kerning:Number = 1, halign:String = CENTER, valign:String = CENTER
		){
			super( pos, scale, rotate, alpha, color, smoothing );
			this.kerning = kerning;
			this.halign = halign;
			this.valign = valign;
		}
		
		/**
		 * オブジェクトのストリング表現を取得します。
		 * このクラスでは、設定された値の一覧を文字列形式で習得します。
		 *
		 * @param 設定された値の一覧文字列
		 */
		public override function toString():String{
			return StringUtil.substitute( "{0},KERNING:{1},ALIGN[H:{2},V:{3}]",
				super.toString(), kerning, halign, valign );
		}
	}
}
