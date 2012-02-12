package danmaq.nineball.core.component.task
{
	import danmaq.nineball.core.component.context.CContext;
	import danmaq.nineball.core.component.context.CContextProxy;
	import danmaq.nineball.core.component.state.IState;
	import danmaq.nineball.core.util.list.iterator.IAggregate;
	import danmaq.nineball.core.util.list.iterator.IIterator;
	
	/**
	 * タスク管理クラス。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskManager extends CContext implements IAggregate
	{

		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 
		 * @param firstState 初回の状態。
		 */
		public function CTaskManager(firstState:IState=null)
		{
			super(firstState);
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * @inheritDoc
		 */
		override public function get defaultState():IState
		{
			return CStateTaskManager.instance;
		}
		
		/**
		 * 集合を反復処理する列挙子を取得します。
		 * 
		 * @return 列挙子。
		 */
		public function get iterator():IIterator
		{
			return new CTaskManagerIterator(CTaskManagerProxy(proxy));
		}
		
		/**
		 * 登録タスク数を取得します。
		 * 
		 * @return 登録タスク数。
		 */
		public function get length():int
		{
			return tasks.length;
		}
		
		/**
		 * 現在稼働中タスク一覧を取得します。
		 * 
		 * @return 現在稼働中タスク一覧。
		 */
		private function get tasks():Vector.<ITask>
		{
			return CTaskManagerProxy(proxy).tasks;
		}
		
		/**
		 * 追加予約されているタスク一覧を取得します。
		 * 
		 * @return 追加予約されているタスク一覧。
		 */
		private function get add():Vector.<ITask>
		{
			return CTaskManagerProxy(proxy).add;
		}
		
		/**
		 * 削除予約されているタスク一覧を取得します。
		 * 
		 * @return 追加予約されているタスク一覧。
		 */
		private function get remove():Vector.<ITask>
		{
			return CTaskManagerProxy(proxy).remove;
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * タスク追加の予約を入れます。
		 * 
		 * @param task タスク。
		 */
		public function addReserve(task:ITask):void
		{
			var add:Vector.<ITask> = this.add;
			var remove:Vector.<ITask> = this.remove;
			var index:int = remove.indexOf(task);
			if(index >= 0)
			{
				remove.splice(index, 1);
			}
			if(add.indexOf(task) < 0 && !contains(task))
			{
				add.push(task);
			}
		}
		
		/**
		 * タスク削除の予約を入れます。
		 * 
		 * @param task タスク。
		 */
		public function removeReserve(task:ITask):void
		{
			var add:Vector.<ITask> = this.add;
			var remove:Vector.<ITask> = this.remove;
			var index:int = add.indexOf(task);
			if(index >= 0)
			{
				add.splice(index, 1);
			}
			if(remove.indexOf(task) < 0 && contains(task))
			{
				remove.push(task);
			}
		}
		
		/**
		 * 登録タスクをすべて即時削除します。
		 * 
		 * このメソッドに限り、予約ではなく即時実行であることに注意してください。
		 */
		public function clear():void
		{
			CTaskManagerProxy(proxy).clear();
		}
		
		/**
		 * タスクが登録されているかどうかを取得します。
		 * 
		 * @param task タスク。
		 * @return タスクが登録されている場合、true。
		 */
		public function contains(task:ITask):Boolean
		{
			return tasks.indexOf(task) >= 0;
		}
		
		/**
		 * 予約されているタスクの登録・削除は、既定の状態によってupdate処理中に
		 * 確定されますが、このメソッドを使用することで強制的に即時確定します。
		 */
		public function commitReserve():void
		{
			CTaskManagerProxy(proxy).commitReserve();
		}
		
		/**
		 * @inheritDoc
		 */
		override protected function createProxy():CContextProxy
		{
			return new CTaskManagerProxy(this);
		}
	}
}
