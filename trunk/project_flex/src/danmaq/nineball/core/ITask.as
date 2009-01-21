package danmaq.nineball.core{

	/**
	 * タスクの基底となるインターフェイスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public interface ITask{

		////////// PROPERTIES //////////

		/**
		 * タスク管理クラスを設定します。
		 * 
		 * @param value タスク管理クラス
		 */
		function set manager( value:CTaskManager ):void;

		/**
		 * レイヤ値を取得します。
		 * レイヤ値の若い方から順に処理されます。
		 * 同一値が複数ある場合、登録された順に処理されます。
		 * !!注意!!：管理クラス登録後はレイヤ変更しないでください。
		 * 
		 * @return レイヤ値
		 */
		function get layer():uint;
		
		////////// METHODS //////////
		
		/**
		 * タスクが管理クラスに登録された直後に、1度だけ自動的に
		 * 呼ばれます。直前にmanagerプロパティが自動的に代入されるので、
		 * タスク管理クラスが必要な初期化処理などの用途に便利です。
		 */
		function initialize():void;
		
		/**
		 * デストラクタ。
		 * シーンの解放処理を記述します。
		 */
		function dispose():void;

		/**
		 * シーンの動作処理を記述します。
		 * 
		 * @return 次のフレームもシーンが存続する場合、true
		 */
		function update():Boolean;
	}
}
