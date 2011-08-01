package danmaq.nineball.struct{

	/**
	 * 端寄せ情報定義リストです。
	 * danmaq.nineball.task.CTaskFontなどで使用します。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CAlign{
		
		////////// CONSTANTS //////////

		/**	上端寄せ、または左端寄せを指示するための定数が格納されます。 */
		public static const TOP_LEFT:int		= -1;
		
		/**	センタリングを指示するための定数が格納されます。 */
		public static const CENTER:int			= 0;
		
		/**	下端寄せ、または右端寄せを指示するための定数が格納されます。 */
		public static const BOTTOM_RIGHT:int	= 1;
		
		////////// METHODS //////////

		/**
		 * 指定した値がクラス定義値であるかどうかを取得します。
		 * 
		 * @param nExpr 検査対象値
		 * @return 値がクラス定義値である場合、true
		 */
		public static function isValid( nExpr:int ):Boolean{
			return (
				( nExpr == TOP_LEFT		) ||
				( nExpr == CENTER		) ||
				( nExpr == BOTTOM_RIGHT )
			);
		}
	}
}
