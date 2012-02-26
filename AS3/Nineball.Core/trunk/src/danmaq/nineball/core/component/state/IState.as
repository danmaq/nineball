package danmaq.nineball.core.component.state
{

	import danmaq.nineball.core.component.context.CContextBody;

	/**
	 * 状態としての振る舞いを定義するインターフェイス。
	 *
	 * @author Mc(danmaq)
	 * @see danmaq.nineball.core.component.context.CContext
	 */
	public interface IState
	{

		//* instance methods ───────────────────────────*

		/**
		 * 状態が開始された際に呼び出されます。
		 *
		 * @param body 実体へのアクセサ。
		 */
		function setup(body:CContextBody):void;

		/**
		 * 1フレーム分の更新処理を実行します。
		 *
		 * @param body 実体へのアクセサ。
		 */
		function update(body:CContextBody):void;

		/**
		 * 別の状態へと移行される直前に呼び出されます。
		 *
		 * この時点で、現在予約されている状態を書き換えることによって、
		 * 状態の移行先を変更したり、或いは状態遷移そのものを取り消すことができます。
		 *
		 * @param body 実体のアクセサ。
		 */
		function teardown(body:CContextBody):void;
	}
}
