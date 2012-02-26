package danmaq.nineball.core.component.context
{

	import danmaq.nineball.core.component.task.ITask;
	import danmaq.nineball.core.component.state.IState;

	/**
	 * 状態を持つ実体に必要なメソッドを定義したインターフェイス。
	 *
	 * @author Mc(danmaq)
	 */
	public interface IContext extends ITask
	{

		//* instance properties ─────────────────────────-*

		/**
		 * 直前の状態を取得します。
		 *
		 * @return 直前の状態。
		 */
		function get previousState():IState;

		/**
		 * 現在の状態を取得します。
		 *
		 * @return 現在の状態。
		 */
		function get currentState():IState;

		/**
		 * 次の状態を取得します。
		 *
		 * @return 次の状態。未決定の場合、null。
		 */
		function get nextState():IState;

		/**
		 * 次の状態を設定、または取り消しします。
		 *
		 * @param v 次の状態。
		 */
		function set nextState(v:IState):void;

	}
}
