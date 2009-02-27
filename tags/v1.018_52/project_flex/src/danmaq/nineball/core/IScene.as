package danmaq.nineball.core{

	/**
	 * シーンの基底となるインターフェイスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public interface IScene{

		////////// PROPERTIES //////////

		/**
		 * 次のシーンを取得します。
		 * 
		 * <p>
		 * このプロパティにnull以外を設定すると、
		 * 現在のシーンから次のシーンへGOSUBします。
		 * 設定直後にupdateメソッドでfalseを返すとGOTOします。
		 * </p>
		 * 
 		 * @return 次のシーン。無い場合、null
 		 * 
 		 * @see #update()
		 */
		function get nextScene():IScene;
		
		////////// METHODS //////////
		
		/**
		 * シーンの解放処理を記述してください。
		 * 
		 * <p>
		 * 解放時に管理クラスから呼び出され、
		 * 事実上のデストラクタとして機能します。
		 * </p>
		 */
		function dispose():void;

		/**
		 * シーンの動作処理を記述してください。
		 * 
		 * <p>
		 * |next ＼戻値|true|false |<br />
		 * |nextScene有|call| goto |<br />
		 * |nextScene無|next|return|<br />
		 * </p>
		 * 
		 * @return 次のフレームもこのシーンが存続する場合、true
		 */
		function update():Boolean;
	}
}
