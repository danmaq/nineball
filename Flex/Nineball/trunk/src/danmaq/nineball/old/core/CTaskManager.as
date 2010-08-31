package danmaq.nineball.old.core
{

	import mx.utils.StringUtil;

	/**
	 * タスク管理クラスです。
	 * 
	 * @see danmaq.nineball.core.ITask
	 * @author Mc(danmaq)
	 */
	public final class CTaskManager
	{

		////////// FIELDS //////////

		/**	タスクのリストが格納されます。 */
		private var m_list:Vector.<ITask> = new Vector.<ITask>();
		
		/**	一時停止中かどうかが格納されます。 */
		public var pause:Boolean = false;

		////////// PROPERTIES //////////
		
		/**
		 * 登録されているタスクの数を取得します。
		 * 
		 * @return 登録されているタスクの数
		 */
		public function get num():uint
		{
			return m_list.length;
		}

		////////// METHODS //////////
		
		/**
		 * 登録されている全てのタスクを解放します。
		 * 
		 * <p>
		 * タスク管理を終了するときに必ず実行してください。
		 * </p>
		 */
		public function dispose():void
		{
			eraseAll();
		}
		
		/**
		 * タスクを追加します。
		 * 
		 * @param task 登録するタスク
		 * @param bSkipInitialize (省略可:false)初期化をスキップするかどうか
		 */
		public function add(task:ITask, bSkipInitialize:Boolean = false):void
		{
			task.manager = this;
			var uLength:uint = m_list.length;
			var bInserted:Boolean = false;
			for(var i:int = 0; i < uLength; i++)
			{
				if(m_list[i].layer < task.layer)
				{
					m_list.splice(i, 0, task);
					bInserted = true;
					break;
				}
			}
			if(!bInserted)
			{
				m_list.push(task);
			}
			if(!bSkipInitialize)
			{
				task.initialize();
			}
		}
		
		/**
		 * 登録されているすべてのタスクを抹消します。
		 * 
		 * @return 抹消した数
		 */
		public function eraseAll():uint
		{
			var uResult:uint = m_list.length;
			for each(var task:ITask in m_list){
				task.dispose();
			}
			m_list = new Vector.<ITask>();
			return uResult;
		}
		
		/**
		 * 特定のタスクを検索し抹消します。
		 * 
		 * @param task 抹消対象のタスク オブジェクト
		 * @return 抹消出来た(見つかった)場合、true
		 */
		public function eraseTask(task:ITask):Boolean
		{
			var nIndex:int = m_list.indexOf(task);
			var bResult:Boolean = (nIndex >= 0);
			if(bResult)
			{
				task.dispose();
				m_list.splice(nIndex, 1);
			}
			return bResult;
		}
		
		/**
		 * 登録されているタスクのうち、指定の
		 * レイヤに属するものを抹消します。
		 * 
		 * @param nLayer 抹消させるレイヤ番号
		 * @return 抹消した数
		 */
		public function eraseLayer(uLayer:uint):uint
		{
			var uResult:uint = 0;
			for(var i:int = 0; i < m_list.length; i++)
			{
				// ! TODO : 指定レイヤを通過したら脱出するようにする
				if(m_list[i].layer == uLayer){
					m_list.splice(i, 1);
					uResult++;
				}
			}
			return uResult;
		}

		/**
		 * 登録されているタスクのうち、指定の
		 * レイヤの範囲に属するものを抹消します。
		 * 
		 * @param uLayerLimit1 抹消させるレイヤ番号の範囲1
		 * @param uLayerLimit2 抹消させるレイヤ番号の範囲2
		 * @return 抹消した数
		 */
		public function eraseLayerBand(uLayerLimit1:uint, uLayerLimit2:uint):uint
		{
			var uResult:uint = 0;
			var nEnd:int = Math.max(uLayerLimit1, uLayerLimit2);
			for(var i:int = Math.min(uLayerLimit1, uLayerLimit2);
				i <= nEnd; uResult += eraseLayer(i++));
			return uResult;
		}
		
		/**
		 * 指定レイヤのタスク一覧を検索し、取得します。
		 * 
		 * @param uLayer 検索対象のレイヤ番号
		 * @return 指定レイヤのタスク一覧配列
		 */
		public function find(uLayer:uint):Array
		{
			var aResult:Array = new Array();
			for each(var task:ITask in m_list)
			{
				if(task.layer == uLayer){ aResult.push(task); }
			}
			return aResult;
		}
		
		/**
		 * 登録されている全タスクに1フレーム分の更新処理をさせます。
		 * 
		 * <p>
		 * その結果タスクよりfalseが返ってきた場合、
		 * そのタスクは終了・抹消されます。
		 * </p>
		 */
		public function update():void
		{
			var i:int = 0;
			var task:ITask;
			while(i < m_list.length)
			{
				task = m_list[i];
				if((pause && task.isAvailablePause) || task.update())
				{
					i++;
				}
				else
				{
					task.dispose();
					m_list.splice(i, 1);
				}
			}
		}
		
		/**
		 * このクラスの状態を文字列で取得します。
		 * 
		 * @return オブジェクトのストリング表現
		 */
		public function toString():String
		{
			return StringUtil.substitute("TaskNum:{0}", num);
		}
	}
}