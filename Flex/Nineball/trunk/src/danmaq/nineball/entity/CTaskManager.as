package danmaq.nineball.entity
{

	import danmaq.nineball.state.*;

	/**
	 * タスク管理クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskManager extends CEntity
	{

		////////// PROPERTIES //////////
		
		/**
		 * 登録されているタスクの数を取得します。
		 * 
		 * @return 登録されているタスクの数
		 */
		public function get length():uint
		{
			return privateMembers.tasks.length;
		}

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 * 最初の状態が設定できますが、何も指定しない場合は既定の状態
		 * (CStateTaskManager.instance)となります。
		 * 
		 * @param firstState 最初の状態。
		 */
		public function CTaskManager(firstState:IState = null)
		{
			var privateMembers:Object = 
			{
				tasks: new Vector.<Object>(),
				addList: new Vector.<Object>(),
				delList: new Vector.<Object>()
			};
			if(firstState == null)
			{
				firstState = CStateTaskManager.instance;
			}
			super(firstState, privateMembers);
		}
		
		/**
		 * タスク追加の予約を追加します。
		 * 
		 * @param task 登録するタスク
		 * @param _priority 処理優先度
		 */
		public function add(task:ITask, _priority:uint = 0):void
		{
			var item:Object =
			{
				priority: _priority,
				item: task
			};
			privateMembers.addList.push(item);
		}
		
		/**
		 * 登録されているすべてのタスクを即時抹消します。
		 * このメソッドは予約ではなく、即時実行であることに注意してください。
		 * 
		 * @param callback 抹消前にVector.forEachによって一括実行される関数。
		 * 	function callback(item:IEntity):void;
		 */
		public function eraseImmediately(callback:Function = null):void
		{
			if(callback != null)
			{
				var func:Function = function(item:Object, index:int, vector:Vector.<Object>):void
				{
					callback(item.item);
				};
				privateMembers.tasks.forEach(func, null);
			}
			privateMembers.tasks = new Vector.<Object>();
		}
		
		/**
		 * 特定のタスク抹消の予約を追加します。
		 * 
		 * @param task 抹消対象のタスク オブジェクト
		 */
		public function eraseTask(task:ITask):void
		{
			var item:Object =
			{
				item: task,
				func: null
			};
			privateMembers.delList.push(item);
		}
		
		/**
		 * 登録されているタスクのうち、指定の優先度に属するものを抹消するための予約を追加します。
		 * 
		 * @param nLayer 抹消させる優先度
		 * @param callback 抹消前にVector.forEachによって一括実行される関数。
		 * 	function callback(item:IEntity):void;
		 * @return 検出したタスク一覧。 
		 */
		public function erasePriority(
			_priority:uint, callback:Function = null):Vector.<ITask>
		{
			var result:Vector.<ITask> = find(_priority);
			for each(var task:ITask in result)
			{
				var item:Object =
				{
					item: task,
					func: callback
				};
				privateMembers.delList.push(item);
			}
			return result;
		}

		/**
		 * 登録されているタスクのうち、指定の優先度に属するものを抹消するための予約を追加します。
		 * 
		 * @param priorityMin 抹消させる優先度の範囲1
		 * @param priorityMax 抹消させる優先度の範囲2
		 * @param callback 抹消前にVector.forEachによって一括実行される関数。
		 * 	function callback(item:IEntity):void;
		 * @return 抹消した数
		 */
		public function eraseLayerBand(
			priorityMin:uint, priorityMax:uint, callback:Function = null):uint
		{
			var uResult:uint = 0;
			var nEnd:int = Math.max(priorityMin, priorityMax);
			for(var i:int = Math.min(priorityMin, priorityMax);
				i <= nEnd; uResult += erasePriority(i++, callback).length);
			return uResult;
		}
		
		/**
		 * 指定レイヤのタスク一覧を検索し、取得します。
		 * 
		 * @param uLayer 検索対象のレイヤ番号
		 * @return 指定レイヤのタスク一覧配列
		 */
		public function find(_priority:uint):Vector.<ITask>
		{
			var test:Function = function(item:Object, index:int, vector:Vector.<Object>):Boolean
			{
				return item.priority == _priority;
			};
			var target:Vector.<Object> = privateMembers.tasks.filter(test, this);
			var result:Vector.<ITask> = new Vector.<ITask>();
			for each(var item:Object in target)
			{
				result.push(item.item);
			}
			return result;
		}
		
		/**
		 * タスク追加および削除の予約を確定します。
		 */
		public function commit():void
		{
			commitExecute(privateMembers.delList, commitErase);
			commitExecute(privateMembers.addList, commitAdd);
		}
		
		/**
		 * タスク追加および削除の予約を確定します。
		 * 
		 * @param list 予約一覧。
		 * @param func 予約を確定させるためのコールバック関数。
		 */
		private function commitExecute(list:Vector.<Object>, func:Function):void
		{
			var _length:uint = list.length;
			if(_length > 0)
			{
				list.forEach(func, null);
				list.splice(0, _length);
			}
		}
		
		/**
		 * タスク追加の予約を確定します。
		 * 
		 * @param item 予約アイテム
		 * @param index 予約番号
		 * @param vector 予約リスト一覧
		 */
		private function commitErase(item:Object, index:int, vector:Vector.<Object>):void
		{
			var i:int = privateMembers.tasks.indexOf(item.item);
			if(i >= 0)
			{
				item.func(item.item);
				privateMembers.tasks.splice(i, 1);
			}
		}
		
		/**
		 * タスク削除の予約を確定します。
		 * 
		 * @param item 予約アイテム
		 * @param index 予約番号
		 * @param vector 予約リスト一覧
		 */
		private function commitAdd(item:Object, index:int, vector:Vector.<Object>):void
		{
			var tasks:Vector.<Object> = privateMembers.tasks;
			var length:uint = tasks.length;
			var inserted:Boolean = false;
			for(var i:int = 0; i < length && !inserted; i++)
			{
				if(tasks[i].priority < item.priority)
				{
					tasks.splice(i, 0, item);
					inserted = true;
				}
			}
			if(!inserted)
			{
				tasks.push(item);
			}
		}
	}
}
