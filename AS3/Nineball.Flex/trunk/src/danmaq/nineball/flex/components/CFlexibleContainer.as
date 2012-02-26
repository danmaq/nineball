package danmaq.nineball.flex.components
{

	import flash.display.DisplayObject;
	import flash.events.Event;
	
	import mx.events.ResizeEvent;
	
	import spark.components.Group;
	
	/**
	 * リサイズしても内部コンテナをスケーリングすることでピクセル数を
	 * 維持し続ける、アスペクト比の固定されたコンテナ クラス。
	 * 
	 * <ul>
	 * 	<li>
	 * 	このクラスを有効に動作させるために、<code>minWidth</code>および
	 * 	<code>minHeight</code>を設定してください。
	 * 	</li>
	 * 	<li>
	 * 	現在のバージョンでは、1つの最大化されたビジュアル エレメントのみをサポートします。
	 * 	複数の、または最大化されていないビジュアル エレメントを
	 * 	追加した場合、全てが重なった状態で最大化されて表示されます。
	 * 	</li>
	 * 	<li>
	 * 	また、ビジュアル エレメントは<code>IVisualElement</code>を実装した、
	 * 	<code>DisplayObject</code>のサブクラスである必要があります。
	 * 	それ以外のビジュアル エレメントは正しくスケーリングされません。
	 * 	</li>
	 * </ul>
	 * 
	 * @author Mc(danmaq)
	 * @see mx.core.IVisualElement
	 * @see flash.display.DisplayObject
	 */
	public class CFlexibleContainer extends Group
	{

		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 */
		public function CFlexibleContainer()
		{
			addEventListener(ResizeEvent.RESIZE, onStageResized, false, 0, true);
			addEventListener(Event.ADDED_TO_STAGE, onStageResized, false, 0, true);
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * リサイズされた際のイベントによって呼び出されます。
		 * 
		 * @param evt イベント情報。
		 */
		private function onStageResized(evt:Event):void
		{
			var scaleX:Number = (width = height * (minWidth / minHeight)) / minWidth;
			var scaleY:Number = height / minHeight;
			for(var i:int = numElements; --i >= 0; )
			{
				var element:DisplayObject = getElementAt(i) as DisplayObject;
				element.scaleX = scaleX; 
				element.scaleY = scaleY;
			}
		}
	}
}
