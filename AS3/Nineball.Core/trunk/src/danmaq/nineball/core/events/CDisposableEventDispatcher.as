package danmaq.nineball.core.events
{
	import danmaq.nineball.core.component.IDisposable;
	
	import flash.events.EventDispatcher;
	import flash.events.IEventDispatcher;
	
	/**
	 * イベント リスナの一括削除に対応した、イベント送出クラス。
	 * 
	 * <p>
	 * このクラスをEventDispatcherの代わりに使用することで、dispose()メソッドを
	 * 呼び出すことにより登録されているイベントをすべて抹消します。
	 * </p>
	 * 
	 * @author Mc(danmaq)
	 * @see flash.events.EventDispatcher
	 * @see #dispose
	 */
	public class CDisposableEventDispatcher extends EventDispatcher implements IDisposable
	{

		//* constants ──────────────────────────────-*
		
		/** イベント リスナ一覧。 */
		private const listeners:Vector.<CPair> = new Vector.<CPair>();
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 
		 * @param target EventDispatcher オブジェクトに送出されるイベントのターゲットオブジェクト。
		 */
		public function CDisposableEventDispatcher(target:IEventDispatcher = null)
		{
			super(target);
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * イベントリスナーオブジェクトを EventDispatcher オブジェクトに
		 * 登録し、リスナーがイベントの通知を受け取るようにします。
		 * 
		 * イベントリスナーは、特定のタイプのイベント、段階、および
		 * 優先度に関する表示リスト内のすべてのノードに登録できます。
		 * 
		 * @param type イベントのタイプ。
		 * @param listener イベントを処理するリスナー関数。
		 * @param useCapture リスナーが、キャプチャ段階で動作するかどうか。
		 * @param priority イベントリスナーの優先度レベル。
		 * @param useWeakReference リスナーへの参照が弱参照であるかどうか。
		 */
		override public function addEventListener(
			type:String, listener:Function, useCapture:Boolean=false,
			priority:int=0, useWeakReference:Boolean=false):void
		{
			listeners.push(new CPair(type, listener));
			super.addEventListener(type, listener, useCapture, priority, useWeakReference);
		}

		/**
		 * 全てのイベント リスナを削除します。
		 */
		public function clearEventListener():void
		{
			var length:int = listeners.length;
			for(var i:int = length; --i >= 0; )
			{
				var item:CPair = listeners[i];
				removeEventListener(item.type, item.listener);
			}
			listeners.splice(0, length);
		}
		
		/**
		 * 明示的にオブジェクトを解放可能な状態にします。
		 * 
		 * <p>
		 * このメソッドはclearEventListener()メソッドの別名として機能します。
		 * </p>
		 * 
		 * @see #clearEventListener
		 */
		public function dispose():void
		{
			clearEventListener();
		}
	}
}

/**
 * イベントのタイプとリスナ関数とのペア。
 */
class CPair
{

	//* fields ────────────────────────────────*

	/** イベントのタイプ。 */
	public var type:String;

	/** イベントを処理するリスナー関数。 */
	public var listener:Function;
	
	//* constructor & destructor ───────────────────────*
	
	/**
	 * コンストラクタ。
	 * 
	 * @param type イベントのタイプ。
	 * @param listener イベントを処理するリスナー関数。
	 */
	public function CPair(type:String, listener:Function)
	{
		this.type = type;
		this.listener = listener;
	}
}
