package danmaq.ball.resource{

	import danmaq.nineball.struct.CScreen;
	import danmaq.nineball.struct.font.CFontResource;

	/**
	 * リソース定義クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CResource{

		////////// CONSTANTS //////////
		
		/**	フォント画像が格納されます。 */
		[Embed(source="../data/font.png")]
			public static const FONT_IMAGE:Class;

		////////// FIELDS //////////

		/**	メイン画面の管理クラスが格納されます。 */
		public static var screen:CScreen = null;

		/**	フォントリソースクラスが格納されます。 */
		public static var font:CFontResource = null;
	}
}
