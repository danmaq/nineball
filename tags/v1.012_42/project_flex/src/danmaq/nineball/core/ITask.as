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
		 * <p>
		 * このタスクを管理クラスに登録すると、
		 * 自動的にこのプロパティに代入されます。
		 * </p>
		 * 
		 * @param value タスク管理クラス
		 */
		function set manager( value:CTaskManager ):void;

		/**
		 * レイヤ値を取得します。
		 * 
		 * <p>
		 * レイヤ値の若い方から順に処理されます。
		 * 同一値が複数ある場合、登録された順に処理されます。
		 * </p>
		 * <p>
		 * !!注意!!：管理クラス登録後はレイヤ変更しないでください。
		 * </p>
		 * 
		 * @return レイヤ値
		 */
		function get layer():uint;
		
		////////// METHODS //////////
		
		/**
		 * タスクが管理クラスに登録された直後に、1度だけ自動的に呼ばれます。
		 * 
		 * <p>
		 * 直前にmanagerプロパティが自動的に代入されるので、
		 * タスク管理クラスが必要な初期化処理などの用途に便利です。
		 * </p>
		 */
		function initialize():void;
		
		/**
		 * タスクの解放処理を記述してください。
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
		 * @return 次のフレームもこのタスクが存続する場合、true
		 */
		function update():Boolean;
	}
}
