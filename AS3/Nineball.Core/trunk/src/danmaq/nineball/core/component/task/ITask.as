package danmaq.nineball.core.component.task
{
	import danmaq.nineball.core.component.IDisposable;

	/**
	 * フレームごとの更新メソッドを定義した、シンプルなタスク インターフェイス。
	 * 
	 * @author Mc(danmaq)
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