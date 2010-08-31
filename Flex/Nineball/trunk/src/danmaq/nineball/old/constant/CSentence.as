////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.old.constant
{

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
	public final class CSentence
	{

		////////// CONSTANTS //////////
		
		/**	文章：一部データが不正のため、DNLを実行できませんでした。 */
		public static const DNL_FAILED:String =
			"一部データが不正のため、DNLを実行できませんでした。";

		/**	文章：内部FPS理論値は1以上でなければなりません。 */
		public static const FPS_TOOLOW:String = "内部FPS理論値は1以上でなければなりません。";
		
		/**	文章：FPS下限許容値は内部FPS理論値({0})未満でなければなりません。 */
		public static const FPS_LOLIMIT:String =
			"FPS下限許容値は内部FPS理論値({0})未満でなければなりません。";
		
		/**	文章：画面コンテナが定義されていません。 */
		public static const FPS_SCREEN:String = "画面コンテナが定義されていません。";
		
		/**	文章：初回実行シーンが定義されていません。 */
		public static const FPS_FIRST_SCENE:String = "初回実行シーンが定義されていません。";
		
		/**	文章：効果音再生フレーム解像度は1以上でなければなりません。 */
		public static const SE_RESOLUTION:String =
			"効果音再生フレーム解像度は1以上でなければなりません。";
		
		/**	文章：管理クラス登録後はレイヤ番号の変更は出来ません。 */
		public static const LAYER_CHANGE:String = "管理クラス登録後はレイヤ番号の変更は出来ません。";
		
		/**	文章：引数は1文字でなければなりません。 */
		public static const ARGS_NOT_CHAR:String = "引数は1文字でなければなりません。";
		
		/**	文章：画面オブジェクト管理クラス(danmaq.nineball.struct.CScreen)のルートクラスが画面オブジェクトとして登録されている必要があります。 */
		public static const ILLEGAL_SCREEN:String = "画面オブジェクト管理クラス(danmaq.nineball.struct.CScreen)のルートクラスが画面オブジェクトとして登録されている必要があります。";
		
		/**	文章：自分自身を次タスクとして連結することは出来ません。 */
		public static const TASK_LOOP:String = "自分自身を次タスクとして連結することは出来ません。";
		
		/**	文章：不正な内分カウンタ種類IDが指定されました。 */
		public static const ILLEGAL_INTERPOLATE:String = "不正な内分カウンタ種類IDが指定されました。";
	}
}
