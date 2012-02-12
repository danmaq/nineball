package danmaq.nineball.core.component.task
{
	import danmaq.nineball.core.component.context.CContextProxy;
	import danmaq.nineball.core.component.state.IState;
	import danmaq.nineball.core.util.object.blockDuplicate;
	
	/**
	 * タスク管理クラスの既定の状態。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CStateTaskManager implements IState
	{
		
		//* constants ──────────────────────────────-*
		
		/** クラス インスタンス。 */
		public static const instance:IState = new CStateTaskManager();
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 
		 * Singletonクラスのため、このクラスの生成は許可されません。
		 * 静的メンバinstanceを使用してください。
		 */
		public function CStateTaskManager()
		{
			blockDuplicate(this, instance);
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * 登録タスクに対し、1フレーム分の更新処理を実行するためのコールバックです。
		 * 
		 * @param item タスク。
		 * @param index インデックス番号。
		 * @param vector タスク一覧。
		 */
		private static function updateDelegate(item:ITask, index:int, vector:Vector.<ITask>):void
		{
			item.update();
		}
		
		/**
		 * 状態が開始された際に呼び出されます。
		 * 
		 * @param proxy 実体へのアクセサ。
		 */
		public function setup(proxy:CContextProxy):void
		{
		}
		
		/**
		 * 1フレーム分の更新処理を実行します。
		 * 
		 * @param proxy 実体へのアクセサ。
		 */
		public function update(proxy:CContextProxy):void
		{
			var tproxy:CTaskManagerProxy = proxy as CTaskManagerProxy;
			if(tproxy != null)
			{
				tproxy.commitReserve();
				tproxy.tasks.forEach(updateDelegate);
			}
		}
		
		/**
		 * 別の状態へと移行される際に呼び出されます。
		 * 
		 * @param proxy 実体のアクセサ。
		 */
		public function teardown(proxy:CContextProxy):void
		{
		}
	}
}
