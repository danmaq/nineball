package danmaq.nineball.core.component.task
{

	import danmaq.nineball.core.component.IDisposable;

	/**
	 * タスクに対し、動作のための最低限のメソッドを定義します。
	 *
	 * <p>
	 * このインターフェイスを実装することで、CTaskManagerによるタスク管理が行えるようになります。
	 * また、CContextクラスは既にITaskを実装しているため、そちらを活用すると一層便利です。
	 * </p>
	 *
	 * @author Mc(danmaq)
	 * @see danmaq.nineball.core.component.task.CTaskManager
	 * @see danmaq.nineball.core.component.context.CContext
	 */
	public interface ITask extends IDisposable
	{

		//* instance methods ───────────────────────────*

		/**
		 * 1フレーム分の更新処理を実行します。
		 */
		function update():void;
	}
}
