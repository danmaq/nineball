package danmaq.nineball.core{

	import danmaq.nineball.core.*;
	import danmaq.nineball.constant.CSentence;
	
	import flash.errors.IllegalOperationError;

	/**
	 * タスクの基底となるクラスです。
	 * タスク管理クラスCTaskManagerに登録するタスクを作成するためには、
	 * このクラスを継承するか、ITaskを実装します。
	 * 
	 * @author Mc(danmaq)
	 */
	public class CTaskBase implements ITask{

		////////// CONSTANTS //////////

		/**
		 * フェーズ・カウンタ管理クラスが格納されます。
		 * カウンタはupdateメソッドで自動的にインクリメントされます。
		 */
		protected const phaseManager:CPhaseManager = new CPhaseManager();

		////////// FIELDS //////////

		/**	次フレームまで生存するかどうかが格納されます。 */
		protected var isAlive:Boolean = true;

		/**	タスク管理クラスが格納されます。 */
		private var m_taskManager:CTaskManager = null;

		/**	レイヤ番号が格納されます。 */
		private var m_uLayer:uint = 0;

		/**	レイヤ番号を固定するかどうかが格納されます。 */
		private var m_bLockedLayer:Boolean = false;

		/**	一時停止に対応しているかどうかが格納されます。 */
		private var m_bAvailablePause:Boolean = true;

		////////// PROPERTIES //////////
		
		/**
		 * レイヤ値を取得します。
		 * 
		 * @return レイヤ値
		 */
		public function get layer():uint{ return m_uLayer; }
		
		/**
		 * 一時停止に対応しているかどうかを取得します。
		 * 
		 * <p>
		 * 一時停止に対応しているタスクは、登録されている管理クラスにおいて
		 * pauseプロパティがtrueの間、updateメソッドに制御が移りません。
		 * </p>
		 * 
		 * @return 一時停止に対応している場合、true
		 */
		public function get isAvailablePause():Boolean{ return m_bAvailablePause; }

		/**
		 * タスク管理クラスを取得します。
		 * 
		 * @return タスク管理クラス
		 */
		public function get manager():CTaskManager{ return m_taskManager; }

		/**
		 * レイヤ番号が固定済みかどうかを取得します。
		 * 
		 * @return レイヤ番号が固定済みである場合、true
		 */
		public function get isLockedLayer():Boolean{ return m_bLockedLayer; }

		/**
		 * レイヤ値を設定します。
		 * 
		 * <p>
		 * レイヤ値の若い方から順に処理されます。
		 * 同一値が複数ある場合、登録された順に処理されます。
		 * </p>
		 * 
		 * @param value レイヤ値
		 * @throws flash.errors.IllegalOperationError
		 * 管理クラス登録後にレイヤ番号を設定しようとした場合
		 */
		public function set layer( value:uint ):void{
			if( m_bLockedLayer ){ throw new IllegalOperationError( CSentence.LAYER_CHANGE ); }
			m_uLayer = value;
		}
		
		/**
		 * 一時停止に対応しているかどうかレイヤ値を設定します。
		 * 
		 * <p>
		 * 一時停止に対応しているタスクは、登録されている管理クラスにおいて
		 * pauseプロパティがtrueの間、updateメソッドに制御が移りません。
		 * </p>
		 * 
		 * @param value 一時停止に対応しているかどうか
		 */
		public function set isAvailablePause( value:Boolean ):void{
			m_bAvailablePause = value;
		}
		
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
		public function set manager( value:CTaskManager ):void{
			m_taskManager = value;
		}
		
		////////// METHODS //////////
		
		/**
		 * コンストラクタの後、タスクが管理クラスに登録された直後に、
		 * 1度だけ自動的に呼ばれます。
		 * オーバーライドする際はメソッドの最初に継承元メソッドも呼び出してください。
		 */
		public function initialize():void{ m_bLockedLayer = true; }
		
		/**
		 * 解放時に管理クラスから呼び出される処理です。
		 * オーバーライドする際はメソッドの最後に継承元メソッドも呼び出してください。
		 */
		public function dispose():void{ phaseManager.reset(); }
		
		/**
		 * タスクを1フレーム分動かします。
		 * オーバーライドする際はメソッドの最後に継承元メソッドを呼び出してください。
		 * isAliveプロパティと同等の値が戻り値となります。
		 * 
		 * @return 次フレームまでの間生存し続ける場合、true
		 */
		public function update():Boolean{
			phaseManager.count++;
			return isAlive;
		}
	}
}
