package danmaq.nineball.flex.components
{

	import danmaq.nineball.core.component.manager.CFPSCalculator;
	
	import flash.events.Event;
	
	import mx.utils.StringUtil;
	
	import spark.components.Label;
	
	/**
	 * FPSを計測し、表示します。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CFPSViewer extends Label
	{
		
		//* constants ──────────────────────────────-*
		
		/** FPS計測クラス。 */
		private const calclulator:CFPSCalculator = new CFPSCalculator();

		//* fields ────────────────────────────────*
		
		[Bindable(event="change")]

		/** 接頭辞。 */
		private var _prefix:String = "";

		[Bindable(event="change")]
		
		/** 接尾辞。 */
		private var _suffix:String = " FPS";
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 */
		public function CFPSViewer()
		{
			addChild(calclulator);
			calclulator.addEventListener(Event.CHANGE, onChangedFPS);
		}
		
		
		//* instance properties ─────────────────────────-*

		[Inspectable(category="Other", defaultValue="")]
		
		/**
		 * 接頭辞を取得します。
		 *
		 * @default ""
		 */
		public function get prefix():String
		{
			return _prefix;
		}
		
		/**
		 * @private
		 */
		public function set prefix(value:String):void
		{
			_prefix = value == null ? "" : value;
		}
		
		[Inspectable(category="Other", defaultValue=" FPS")]
		
		/**
		 * 接尾辞を取得します。
		 *
		 * @default ""
		 */
		public function get suffix():String
		{
			return _suffix;
		}
		
		/**
		 * @private
		 */
		public function set suffix(value:String):void
		{
			_suffix = value == null ? "" : value;
		}

		//* instance methods ───────────────────────────*
		
		/**
		 * FPS計測値が変更されたときに呼び出されます。
		 *
		 * @param event イベント情報。
		 */
		private function onChangedFPS(event:Event = null):void
		{
			text = StringUtil.substitute("{0}{1}{2}", prefix, calclulator.fps, suffix);
		}
	}
}
