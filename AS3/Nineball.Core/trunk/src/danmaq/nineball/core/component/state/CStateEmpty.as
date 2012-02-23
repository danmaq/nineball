package danmaq.nineball.core.component.state
{

	import danmaq.nineball.core.component.context.CContextBody;
	import danmaq.nineball.core.util.object.blockDuplicate;

	/**
	 * 抜け殻の状態。
	 * 
	 * <p>
	 * 実体に何もしてほしくない場合、状態として<code>null</code>の代わりにこのオブジェクトを
	 * 設定します。抜け殻のガチャピンが動かないのと同様に、この状態が適用されている間は
	 * <code>CContext.update()</code>メソッドを呼び出しても何の処理も行いません。
	 * </p>
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CStateEmpty implements IState
	{

		//* constants ──────────────────────────────-*

		/** クラス インスタンス。 */
		public static const instance:IState = new CStateEmpty();

		//* constructor & destructor ───────────────────────*

		/**
		 * コンストラクタ。
		 * 
		 * <p>
		 * Singletonクラスのため、このクラスの生成は許可されません。
		 * 静的メンバ<code>instance</code>を使用してください。
		 * </p>
		 * 
		 * @see #instance
		 */
		public function CStateEmpty()
		{
			blockDuplicate(this, instance);
		}

		//* instance methods ───────────────────────────*

		/**
		 * 状態が開始された際に呼び出されます。
		 * 
		 * @param body 実体へのアクセサ。
		 */
		public function setup(body:CContextBody):void
		{
		}

		/**
		 * 1フレーム分の更新処理を実行します。
		 * 
		 * @param body 実体へのアクセサ。
		 */
		public function update(body:CContextBody):void
		{
		}

		/**
		 * 別の状態へと移行される際に呼び出されます。
		 * 
		 * @param body 実体のアクセサ。
		 */
		public function teardown(body:CContextBody):void
		{
		}
	}
}
