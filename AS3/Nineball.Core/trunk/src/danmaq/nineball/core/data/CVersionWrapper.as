package danmaq.nineball.core.data
{

	/**
	 * バージョン情報を保持、比較するためのクラス。
	 * 
	 * <p>
	 * <code>CVersion</code>クラスをラッピングして、書き換え不能なクラスとして機能します。
	 * </p>
	 *
	 * @author Mc(danmaq)
	 */
	internal final class CVersionWrapper implements IVersion
	{
		
		//* fields ────────────────────────────────*
		
		/**	実体となるバージョン情報。 */
		private var _entity:IVersion;
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 
		 * @param entity 実体となるバージョン番号。
		 */
		public function CVersionWrapper(entity:IVersion)
		{
			_entity = entity;
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * メジャー バージョン番号を取得します。
		 */
		public function get major():int
		{
			return _entity.major;
		}
		
		/**
		 * マイナ バージョン番号を取得します。
		 */
		public function get minor():int
		{
			return _entity.minor;
		}
		
		/**
		 * リビジョン バージョン番号を取得します。
		 */
		public function get revision():int
		{
			return _entity.revision;
		}
		
		/**
		 * ビルド バージョン番号を取得します。
		 */
		public function get build():int
		{
			return _entity.build;
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * バージョンを比較します。
		 *
		 * @param target 比較対象のバージョン。
		 * @return <code>target</code>よりバージョンが高い場合、正数値。逆に低い場合負数値。
		 * 同一である場合、<code>0</code>。
		 */
		public function compare(target:IVersion):int
		{
			return _entity.compare(target);
		}
		
		/**
		 * バージョン情報の文字列表現を取得します。
		 * 
		 * @return バージョン情報の文字列表現。
		 */
		public function toString():String
		{
			return _entity.toString();
		}
	}
}
