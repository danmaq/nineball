package danmaq.nineball.resource{

	/**
	 * 文章定数定義クラスです。
	 * 
	 * <p>
	 * danmaq Nineball-Library内部で自動的に使用します。
	 * 通常ユーザが直接使用する必要はありません。
	 * </p>
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CSentence{

		////////// CONSTANTS //////////
		
		/**	文章：一部データが不正のため、DNLを実行できませんでした。 */
		public static const DNL_FAILED:String =
			"一部データが不正のため、DNLを実行できませんでした。";

		/**	文章：内部FPS理論値は1以上でなければなりません。 */
		public static const FPS_TOOLOW:String = "内部FPS理論値は1以上でなければなりません。";
		
		/**	文章：FPS下限許容値は内部FPS理論値({0})未満でなければなりません。 */
		public static const FPS_LOLIMIT:String =
			"FPS下限許容値は内部FPS理論値({0})未満でなければなりません。";
		
		/**	文章：FPS下限許容値は内部FPS理論値({0})未満でなければなりません。 */
		public static const FPS_SCREEN:String = "画面コンテナが定義されていません。";
		
		/**	文章：初回実行シーンが定義されていません。 */
		public static const FPS_FIRST_SCENE:String = "初回実行シーンが定義されていません。";
		
		/**	文章：効果音再生フレーム解像度は1以上でなければなりません。 */
		public static const SE_RESOLUTION:String =
			"効果音再生フレーム解像度は1以上でなければなりません。";
		
		/**	文章：引数{0}にnullを設定出来ません。 */
		public static const NOT_NULL:String = "引数{0}にnullを設定出来ません。";
	}
}
