package danmaq.nineball.core.component.task
{
	import danmaq.nineball.core.component.context.CContextBody;
	import danmaq.nineball.core.component.context.IContext;
	
	/**
	 * タスク管理クラスと状態との橋渡しをするクラス。
	 *
	 * @author Mc(danmaq)
	 */
	public final class CTaskManagerBody extends CContextBody
	{

		//* constants ──────────────────────────────-*

		/** 現在稼働中のタスク一覧。 */
		public const tasks:Vector.<ITask> = new Vector.<ITask>();
		
		/** 追加予約されているタスク一覧。 */
		public const add:Vector.<ITask> = new Vector.<ITask>();
		
		/** 削除予約されているタスク一覧。 */
		public const remove:Vector.<ITask> = new Vector.<ITask>();
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 *
		 * @param context タスク管理クラス。
		 * @param owner 親とするオブジェクト。
		 */
		public function CTaskManagerBody(context:CTaskManager, owner:Object)
		{
			super(context, owner);
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * タスク管理クラスを取得します。
		 *
		 * @return タスク管理クラス。
		 */
		public function get taskManager():CTaskManager
		{
			return CTaskManager(context);
		}
		
		//* instance methods ───────────────────────────*

		/**
		 * 登録タスクに対し、1フレーム分の更新処理を実行するためのコールバックです。
		 *
		 * @param item タスク。
		 * @param index インデックス番号。
		 * @param vector タスク一覧。
		 */
		public static function updateDelegate(item:ITask, index:int, vector:Vector.<ITask>):void
		{
			item.update();
		}
		
		/**
		 * @inheritDoc
		 */
		override public function dispose():void
		{
			clear();
			super.dispose();
		}
		
		/**
		 * 登録タスクをすべて即時削除します。
		 */
		public function clear():void
		{
			commitReserve();
			tasks.forEach(disposeDelegate);
			tasks.splice(0, tasks.length);
		}
		
		/**
		 * タスク登録・削除の予約を確定します。
		 */
		public function commitReserve():void
		{
			remove.forEach(removeDelegate);
			remove.splice(0, remove.length);
			add.forEach(addDelegate);
			add.splice(0, add.length);
		}

		/**
		 * 登録タスクに対し、明示的に解放可能な状態にするためのコールバックです。
		 *
		 * @param item タスク。
		 * @param index インデックス番号。
		 * @param vector タスク一覧。
		 */
		private function disposeDelegate(item:ITask, index:int, vector:Vector.<ITask>):void
		{
			item.dispose();
		}
		
		/**
		 * 追加予約されているタスクを登録するためのコールバックです。
		 *
		 * @param item タスク。
		 * @param index インデックス番号。
		 * @param vector タスク一覧。
		 */
		private function addDelegate(item:ITask, index:int, vector:Vector.<ITask>):void
		{
			tasks.push(item);
		}
		
		/**
		 * 削除予約されているタスクを削除するためのコールバックです。
		 *
		 * @param item タスク。
		 * @param index インデックス番号。
		 * @param vector タスク一覧。
		 */
		private function removeDelegate(item:ITask, index:int, vector:Vector.<ITask>):void
		{
			var i:int = tasks.indexOf(item);
			if(i >= 0)
			{
				tasks.splice(i, 1);
			}
		}
	}
}
