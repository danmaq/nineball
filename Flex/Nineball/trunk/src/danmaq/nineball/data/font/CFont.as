package danmaq.nineball.data.font
{
	import danmaq.nineball.constant.CSentence;
	
	import flash.display.Sprite;
	import flash.errors.IllegalOperationError;
	import flash.geom.Point;
	
	import mx.events.PropertyChangeEvent;
	
	/**
	 * フォントクラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CFont extends Sprite
	{
		////////// CONSTANTS //////////

		/**	単文字フォントタスクが格納されます。 */
		private const bitList:Vector.<CFontBit> = new Vector.<CFontBit>();

		////////// FIELDS //////////
		
		/** テキスト入力時に自動レンダリングするかどうかが格納されます。 */
		public var autoRender:Boolean = false;

		/**	現在表示されているかどうかが格納されます。 */
		public var view:Boolean = false;

		/**	フォントリソースが格納されます。 */
		private var m_fontResource:CFontResource;

		/**	表示するテキストが格納されます。 */
		private var m_strText:String = "";

		/**	最後に使用した描画調整情報が格納されます。 */
		private var m_transform:CFontTransform = new CFontTransform();

		////////// PROPERTIES //////////

		/**
		 * 表示するテキストを取得します。
		 * 
		 * @return 表示するテキスト
		 */
		public function get text():String
		{
			return m_strText;
		}

		/**
		 * 表示するテキストを設定します。
		 * 
		 * @param value 表示するテキスト
		 */
		[Bindable]
		public function set text(value:String):void
		{
			if(value == null)
			{
				throw new IllegalOperationError(CSentence.NOT_NULL);
			}
			if(m_strText != value)
			{
				m_strText = value;
				createChild();
			}
		}

		/**
		 * 文字列画像のサイズを取得します。
		 * 
		 * @return 文字列画像のサイズ
		 */
		public function get size():Point
		{
			var posResult:Point = new Point();
			for each(var bit:CFontBit in bitList)
			{
				posResult.x += bit.size.x * m_transform.scale.x * m_transform.kerning;
				posResult.y = Math.max(posResult.y, bit.size.y * m_transform.scale.y);
			}
			return posResult;
		}

		/**
		 * 描画調整情報を取得します。
		 * 
		 * @return 描画調整情報
		 */
		public function get fontTransform():CFontTransform
		{
			return m_transform;
		}

		/**
		 * 描画調整情報を設定します。
		 * 
		 * @param value 描画調整情報
		 */
		[Bindable]
		public function set fontTransform(value:CFontTransform):void
		{
			if(value == null)
			{
				value = new CFontTransform();
			}
			m_transform = value;
		}

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param fontResource フォントリソース
		 */
		public function CFont(fontResource:CFontResource)
		{
			m_fontResource = fontResource;
			addEventListener(PropertyChangeEvent.PROPERTY_CHANGE, onPropertyChange);
		}

		/**
		 * 指定したパラメータどおりにレンダリングします。
		 * 
		 * @param info フォント描画調整情報
		 */
		public function render(info:CFontTransform = null):void
		{
			if(info != null)
			{
				fontTransform = info;
			}
			else
			{
				info = fontTransform;
			}
			var posSize:Point = size;
			var fX:Number;
			var fY:Number;
			switch(info.halign)
			{
				case CFontTransform.TOP_LEFT:			fX = info.pos.x;					break;
				case CFontTransform.BOTTOM_RIGHT:		fX = info.pos.x - posSize.x;		break;
				case CFontTransform.CENTER:	default:	fX = info.pos.x - posSize.x / 2;	break;
			}
			switch(info.valign)
			{
				case CFontTransform.TOP_LEFT:			fY = info.pos.y + posSize.y / 2;	break;
				case CFontTransform.BOTTOM_RIGHT:		fY = info.pos.y - posSize.y / 2;	break;
				case CFontTransform.CENTER:	default:	fY = info.pos.y;					break;
			}
			for each(var bit:CFontBit in bitList)
			{
				var fHWidth:Number = bit.size.x * info.scale.x / 2;
				var infoBit:CFontTransformBit = info.clone;
				fX += fHWidth * info.kerning;
				infoBit.pos.x = fX;
				infoBit.pos.y = fY;
				bit.render(infoBit);
				bit.view = view;
				fX += fHWidth * info.kerning;
			}
		}

		/**
		 * 子タスクを抹消します。
		 */
		private function deleteChild():void
		{
			while(bitList.length > 0)
			{
				bitList.pop().dispose();
			}
		}

		/**
		 * 子タスクを生成します。
		 */
		private function createChild():void
		{
			deleteChild();
			try
			{
				var uLen:uint = m_strText.length;
				for(var i:uint = 0; i < uLen; i++)
				{
					var bit:CFontBit =
						new CFontBit(m_fontResource, m_strText.charAt(i), this, 0);
					bitList.push(bit);
				}
			}
			catch(e:Error)
			{
				deleteChild();
				m_strText = "";
				throw e;
			}
		}

		/**
		 * プロパティが変化されたときに呼び出されます。
		 * 
		 * @param e イベント情報
		 */
		private function onPropertyChange(e:PropertyChangeEvent):void
		{
			if(autoRender)
			{
				render();
			}
		}
	}
}
