package danmaq.nineball.core.component.state
{
	
	import danmaq.nineball.core.component.IDisposable;
	import danmaq.nineball.core.component.context.CContextBody;
	import danmaq.nineball.core.util.object.blockDuplicate;
	
	import flash.events.Event;
	import flash.events.EventDispatcher;
	
	/**
	 * 状態実行時に任意のメソッドを実行する状態です。
	 * 
	 * <p>
	 * このクラスにはこの状態が開始された時と別の状態へ移行される時、あるいはこの状態が
	 * 適用中に<code>CContext</code>インスタンスからupdateが呼び出された時の合わせて
	 * 3種類のコールバックを登録できます。主に一時的なデバッグ用途に役立つでしょう。
	 * </p>
	 * <p>
	 * ただし、<code>CContext</code>インスタンスの数にかかわらず、コールバックは常に1つで
	 * あることに注意してください。新しいコールバックを登録すると、古いものは上書きされます。
	 * </p>
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CStateDelegate implements IState, IDisposable
	{
		
		//* constants ──────────────────────────────-*
		
		/** クラス インスタンス。 */
		public static const instance:CStateDelegate = new CStateDelegate();

		//* fields ────────────────────────────────*
		
		/** 状態が開始された際に呼び出されます。 */
		private static var _onSetup:Function;
		
		/** updateメソッドが呼ばれた際に呼び出されます。 */
		private static var _onUpdate:Function;
		
		/** 別の状態へと移行される際に呼び出されます。 */
		private static var _onTeardown:Function;
		
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
		public function CStateDelegate()
		{
			blockDuplicate(this, instance);
			dispose();
		}
		
		//* class properties ───────────────────────────*

		/**
		 * 状態が開始された際に呼び出されるメソッドを取得します。
		 * 
		 * @default no-op function
		 * @return <code>function(body:CContextBody):void</code>
		 */
		public static function get onSetup():Function
		{
			return _onSetup;
		}
		
		/**
		 * 状態が開始された際に呼び出されるメソッドを設定します。
		 * <code>null</code>を設定することで、空のメソッドを登録することもできます。
		 * 
		 * @param value <code>function(body:CContextBody):void</code>
		 */
		public static function set onSetup(value:Function):void
		{
			_onSetup = value == null ? noop : value;
		}
		
		/**
		 * 毎フレームの更新メソッドが呼び出された際に呼び出されるメソッドを取得します。
		 * 
		 * @default no-op function
		 * @return <code>function(body:CContextBody):void</code>
		 */
		public static function get onUpdate():Function
		{
			return _onUpdate;
		}
		
		/**
		 * 毎フレームの更新メソッドが呼び出された際に呼び出されるメソッドを設定します。
		 * <code>null</code>を設定することで、空のメソッドを登録することもできます。
		 * 
		 * @param value <code>function(body:CContextBody):void</code>
		 */
		public static function set onUpdate(value:Function):void
		{
			_onUpdate = value == null ? noop : value;
		}
		
		/**
		 * 別の状態へと移行される際に呼び出されるメソッドを取得します。
		 * 
		 * @default no-op function
		 * @return <code>function(body:CContextBody):void</code>
		 */
		public static function get onTeardown():Function
		{
			return _onTeardown;
		}
		
		/**
		 * 別の状態へと移行される際に呼び出されるメソッドを設定します。
		 * <code>null</code>を設定することで、空のメソッドを登録することもできます。
		 * 
		 * @param value <code>function(body:CContextBody):void</code>
		 */
		public static function set onTeardown(value:Function):void
		{
			_onTeardown = value == null ? noop : value;
		}
		
		//* class methods ────────────────────────────-*
		
		/**
		 * 何もしません。
		 * 
		 * @param body 実体のアクセサ。
		 */
		private static function noop(body:CContextBody):void
		{
		}
		
		//* instance methods ───────────────────────────*

		/**
		 * 状態が開始された際に呼び出されます。
		 * 
		 * @param body 実体へのアクセサ。
		 */
		public function setup(body:CContextBody):void
		{
			onSetup(body);
		}
		
		/**
		 * 1フレーム分の更新処理を実行します。
		 * 
		 * @param body 実体へのアクセサ。
		 */
		public function update(body:CContextBody):void
		{
			onUpdate(body);
		}
		
		/**
		 * 別の状態へと移行される際に呼び出されます。
		 * 
		 * @param body 実体のアクセサ。
		 */
		public function teardown(body:CContextBody):void
		{
			onTeardown(body);
		}
		
		/**
		 * 明示的にオブジェクトを解放可能な状態にします。
		 */
		public function dispose():void
		{
			var func:Function = noop;
			onSetup = func;
			onUpdate = func;
			onTeardown = func;
		}
	}
}
