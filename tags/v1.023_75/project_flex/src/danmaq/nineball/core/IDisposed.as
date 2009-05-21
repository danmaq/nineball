package danmaq.nineball.core{

	/**
	 * 解放したかどうかを取得できるクラスインターフェイスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public interface IDisposed{

		////////// PROPERTIES //////////

		/**
		 * 解放したかどうかを取得します。
		 * 
		 * @return 解放している場合、true
		 */
		function get disposed():Boolean;
	}
}
