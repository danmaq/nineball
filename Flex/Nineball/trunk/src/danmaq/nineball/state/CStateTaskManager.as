package danmaq.nineball.state
{

	import danmaq.nineball.entity.*;

	/**
	 * タスク管理クラスの既定の状態。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CStateTaskManager implements IState
	{
		
		////////// CONSTANTS //////////

		/**	インスタンス。 */
		public static const instance:CStateTaskManager = new CStateTaskManager();

		////////// METHODS //////////

		/**
		 * 状態が開始された時に呼び出されます。
		 * このメソッドは、遷移元のteardownよりも後に呼び出されます。
		 * 
		 * @param entity この状態を適用されたオブジェクト。
		 * @param privateMembers オブジェクトと状態クラスのみがアクセス可能なフィールド。
		 */
		public function setup(entity:IEntity, privateMembers:Object):void
		{
		}

		/**
		 * 1フレーム分の更新処理を実行します。
		 * 
		 * @param entity この状態を適用されたオブジェクト。
		 * @param privateMembers オブジェクトと状態クラスのみがアクセス可能なフィールド。
		 */
		public function update(entity:IEntity, privateMembers:Object):void
		{
			var mgrTask:CTaskManager = CTaskManager(entity);
			mgrTask.commit();
			privateMembers.tasks.forEach(updateTasks, null);
			mgrTask.commit();
		}
		
		/**
		 * オブジェクトが別の状態へ移行する時に呼び出されます。
		 * このメソッドは、遷移元のteardownよりも後に呼び出されます。
		 * 
		 * @param entity この状態を適用されたオブジェクト。
		 * @param privateMembers オブジェクトと状態クラスのみがアクセス可能なフィールド。
		 * @param nextState オブジェクトが次に適用する状態。
		 */
		public function teardown(
			entity:IEntity, privateMembers:Object, nextState:IState):void
		{
		}
		
		/**
		 * タスクを実行します。
		 * 
		 * @param item 実行対象のタスク
		 * @param index 優先度順にソートされたタスクの登録番号
		 * @param vector 登録タスク一覧
		 */
		private function updateTasks(item:Object, index:int, vector:Vector.<Object>):void
		{
			item.item.update();
		}
	}
}
