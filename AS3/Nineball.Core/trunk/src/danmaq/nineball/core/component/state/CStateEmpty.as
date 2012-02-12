package danmaq.nineball.core.component.state
{

	import danmaq.nineball.core.component.context.CContextProxy;
	import danmaq.nineball.core.util.object.blockDuplicate;

	/**
	 * 空の状態。
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
		 * Singletonクラスのため、このクラスの生成は許可されません。
		 * 静的メンバinstanceを使用してください。
		 */
		public function CStateEmpty()
		{
			blockDuplicate(this, instance);
		}

		//* instance methods ───────────────────────────*

		/**
		 * 状態が開始された際に呼び出されます。
		 * 
		 * @param proxy 実体へのアクセサ。
		 */
		public function setup(proxy:CContextProxy):void
		{
		}

		/**
		 * 1フレーム分の更新処理を実行します。
		 * 
		 * @param proxy 実体へのアクセサ。
		 */
		public function update(proxy:CContextProxy):void
		{
		}

		/**
		 * 別の状態へと移行される際に呼び出されます。
		 * 
		 * @param proxy 実体のアクセサ。
		 */
		public function teardown(proxy:CContextProxy):void
		{
		}
	}
}
