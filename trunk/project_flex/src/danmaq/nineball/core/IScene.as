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
		 * @return 次のシーン。無い場合、null
		 */
		function get nextScene():IScene;
		
		////////// METHODS //////////
		
		/**
		 * デストラクタ。
		 * シーンの解放処理を記述します。
		 */
		function dispose():void;

		/**
		 * シーンの動作処理を記述します。
		 * |next ＼戻値|true|false |
		 * |nextScene有|call| goto |
		 * |nextScene無|next|return|
		 * 
		 * @return 次のフレームもシーンが存続する場合、true
		 */
		function update():Boolean;
	}
}
