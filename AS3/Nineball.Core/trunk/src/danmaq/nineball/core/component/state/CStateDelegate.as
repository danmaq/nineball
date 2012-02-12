package danmaq.nineball.core.component.state
{
	
	import danmaq.nineball.core.component.IDisposable;
	import danmaq.nineball.core.component.context.CContextProxy;
	import danmaq.nineball.core.util.object.blockDuplicate;
	
	import flash.events.Event;
	import flash.events.EventDispatcher;
	
	/**
	 * 状態実行時に任意のメソッドを実行する状態です。
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
		
		/** 状態が開始された際に呼び出されます。 */
		private static var _onUpdate:Function;
		
		/** 別の状態へと移行される際に呼び出されます。 */
		private static var _onTeardown:Function;
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 
		 * Singletonクラスのため、このクラスの生成は許可されません。
		 * 静的メンバinstanceを使用してください。
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
		 * @return function(value:Function):void
		 */
		public static function get onSetup():Function
		{
			return _onSetup;
		}
		
		/**
		 * 状態が開始された際に呼び出されるメソッドを設定します。
		 * 
		 * @param value function(value:Function):void
		 */
		public static function set onSetup(value:Function):void
		{
			_onSetup = value == null ? noop : value;
		}
		
		/**
		 * 毎フレームの更新メソッドが呼び出された際に呼び出されるメソッドを取得します。
		 * 
		 * @default no-op function
		 * @return function(value:Function):void
		 */
		public static function get onUpdate():Function
		{
			return _onUpdate;
		}
		
		/**
		 * 毎フレームの更新メソッドが呼び出された際に呼び出されるメソッドを設定します。
		 * 
		 * @param value function(value:Function):void
		 */
		public static function set onUpdate(value:Function):void
		{
			_onUpdate = value == null ? noop : value;
		}
		
		/**
		 * 別の状態へと移行される際に呼び出されるメソッドを取得します。
		 * 
		 * @default no-op function
		 * @return function(value:Function):void
		 */
		public static function get onTeardown():Function
		{
			return _onTeardown;
		}
		
		/**
		 * 別の状態へと移行される際に呼び出されるメソッドを設定します。
		 * 
		 * @param value function(value:Function):void
		 */
		public static function set onTeardown(value:Function):void
		{
			_onTeardown = value == null ? noop : value;
		}
		
		//* class methods ────────────────────────────-*
		
		/**
		 * 何もしません。
		 * 
		 * @param proxy 実体のアクセサ。
		 */
		private static function noop(proxy:CContextProxy):void
		{
		}
		
		//* instance methods ───────────────────────────*

		/**
		 * 状態が開始された際に呼び出されます。
		 * 
		 * @param proxy 実体へのアクセサ。
		 */
		public function setup(proxy:CContextProxy):void
		{
			onSetup(proxy);
		}
		
		/**
		 * 1フレーム分の更新処理を実行します。
		 * 
		 * @param proxy 実体へのアクセサ。
		 */
		public function update(proxy:CContextProxy):void
		{
			onUpdate(proxy);
		}
		
		/**
		 * 別の状態へと移行される際に呼び出されます。
		 * 
		 * @param proxy 実体のアクセサ。
		 */
		public function teardown(proxy:CContextProxy):void
		{
			onTeardown(proxy);
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
