package danmaq.nineball.core{

	/**
	 * シーンの基底となるインターフェイスです。
	 * シーン管理クラスCSceneManagerに登録するシーンを作成するためには、
	 * このクラスを継承するか、ISceneを実装します。
	 * 
	 * @author Mc(danmaq)
	 */
	public class CSceneBase implements IScene{

		////////// CONSTANTS //////////

		/**
		 * フェーズ・カウンタ管理クラスが格納されます。
		 * カウンタはupdateメソッドで自動的にインクリメントされます。
		 */
		protected const phaseManager:CPhaseManager = new CPhaseManager();

		/**	タスク管理クラスが格納されます。 */
		protected const taskManager:CTaskManager = new CTaskManager();

		////////// FIELDS //////////
		
		/**	次のシーン オブジェクトが格納されます。 */
		private var m_sceneNext:IScene = null;

		////////// PROPERTIES //////////

		/**
		 * 次のシーンを設定します。
		 * 
		 * <p>
		 * このプロパティにnull以外を設定すると、現在のシーンから次のシーンへ
		 * GOSUBします。その際、このプロパティには管理クラスによって
		 * nullが代入されます。(クリアしないとRETURN出来なくなるため)
		 * </p>
		 * 
 		 * @return 次のシーン。無い場合、null
 		 * 
 		 * @see #update()
		 */
		public function set nextScene( value:IScene ):void{ m_sceneNext = value; }
		
		/**
		 * 次のシーンを取得します。
		 * 
 		 * @return 次のシーン。無い場合、null
		 */
		public function get nextScene():IScene{ return m_sceneNext; }
		
		////////// METHODS //////////
		
		/**
		 * 解放時に管理クラスから呼び出される処理です。
		 * オーバーライドする際はメソッドの最後に継承元メソッドも呼び出してください。
		 */
		public function dispose():void{
			taskManager.dispose();
			phaseManager.reset();
		}
		
		/**
		 * シーンを1フレーム分動かします。
		 * オーバーライドする際はメソッドの最後に継承元メソッドを呼び出してください。
		 * 次のシーンが登録されるとfalseが返ります。もしGOSUBしたい場合、
		 * このメソッドがfalseを返したときに継承先でtrueを返してください。
		 * 
		 * @return 次のフレームもシーンが存続する場合、true
		 */
		public function update():Boolean{
			taskManager.update();
			phaseManager.count++;
			return nextScene != null;
		}
	}
}
