package danmaq.nineball.flex.data
{
	import danmaq.nineball.core.component.IDisposable;
	
	/**
	 * JSONデータを貯蔵するクラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public class CJSONStore implements IDisposable
	{
		
		//* fields ────────────────────────────────*
		
		/**	展開されたJSONオブジェクト。 */
		private var _store:Object = new Object();
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * 展開されたJSONオブジェクトを取得します。
		 */
		public function get store():Object
		{
			return _store;
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * JSONをパースして格納します。
		 *
		 * @param json JSON文字列。
		 */
		public function decode(json:String):void
		{
			_store = JSON.parse(json);
		}
		
		/**
		 * 現在の展開されているJSONオブジェクトをJSON文字列に変換します。
		 *
		 * @param json JSON文字列。
		 */
		public function encode():String
		{
			return JSON.stringify(store);
		}

		/**
		 * JSONオブジェクトを結合します。
		 * 
		 * <p>
		 * キーが競合している場合、<code>object</code>の値で上書きされます。
		 * </p>
		 *
		 * @param object JSONオブジェクト。
		 * @return 取り込んだペアの数。上書き分もカウントされます。
		 */
		public function merge(object:Object):int
		{
			var counter:int = 0;
			for(var key:String in object)
			{
				store[key] = object[key];
				counter++;
			}
			return counter;
		}
		
		/**
		 * 明示的にオブジェクトを解放可能な状態にします。
		 */
		public function dispose():void
		{
			_store = new Object();
		}
	}
}
