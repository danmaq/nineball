package danmaq.nineball.core.data
{

	/**
	 * バージョン情報を保持するためのインターフェイス。
	 *
	 * @author Mc(danmaq)
	 * @see danmaq.nineball.core.data.CVersion
	 */
	public interface IVersion
	{

		//* instance properties ─────────────────────────-*
		
		/**
		 * メジャー バージョン番号を取得します。
		 */
		function get major():int;
		
		/**
		 * マイナ バージョン番号を取得します。
		 */
		function get minor():int;
		
		/**
		 * リビジョン バージョン番号を取得します。
		 */
		function get revision():int;
		
		/**
		 * ビルド バージョン番号を取得します。
		 */
		function get build():int;

		//* instance methods ───────────────────────────*
		
		/**
		 * バージョンを比較します。
		 *
		 * @param target 比較対象のバージョン。
		 * @return <code>target</code>よりバージョンが高い場合、正数値。逆に低い場合負数値。
		 * 同一である場合、<code>0</code>。
		 */
		function compare(target:IVersion):int;

		/**
		 * バージョン情報の文字列表現を取得します。
		 * 
		 * @return バージョン情報の文字列表現。
		 */
		function toString():String;
	}
}
