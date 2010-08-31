////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.old.task
{

	import danmaq.nineball.old.core.*;
	import danmaq.nineball.old.data.font.*;
	
	import flash.errors.IllegalOperationError;
	import flash.geom.Point;

	/**
	 * フォント描画タスクです。
	 * タスクを殺すには管理クラスからeraseするか、
	 * またはタイマを0に設定します。
	 * 
	 * @author Mc(danmaq)
	 */
	public class CTaskFont implements ITask, IDisposed
	{

		////////// CONSTANTS //////////

		/**	単文字フォントタスクが格納されます。 */
		private const bitList:Vector.<CFontBit> = new Vector.<CFontBit>();

		////////// FIELDS //////////
		
		/**
		 * 生存タイマが格納されます。
		 * 負数にすることでタイマを切ることができます。
		 */
		public var timer:int = -1;

		/** テキスト入力時に自動レンダリングするかどうかが格納されます。 */
		public var autoRender:Boolean = false;

		/**	現在表示されているかどうかが格納されます。 */
		public var view:Boolean = false;

		/**	レイヤ番号が格納されます。 */
		private var m_uLayer:uint;

		/**	文字画像を格納する画面管理クラスが格納されます。 */
		private var m_screen:Object;

		/**	フォントリソースが格納されます。 */
		private var m_fontResource:CFontResource;

		/**	解放されたかどうかが格納されます。 */
		private var m_bDisposed:Boolean = false;
		
		/**	表示するテキストが格納されます。 */
		private var m_strText:String = "";

		/**	最後に使用した描画調整情報が格納されます。 */
		private var m_transform:CFontTransform = new CFontTransform();

		/**	一時停止に対応しているかどうかが格納されます。 */
		private var m_bAvailablePause:Boolean = true;

		////////// PROPERTIES //////////

		/**
		 * レイヤ値を取得します。
		 * 
		 * @return レイヤ値
		 */
		public function get layer():uint
		{
			return m_uLayer;
		}
		
		/**
		 * タスク管理クラスを設定します。
		 * このクラスでは特に必要ないので何も設定しません。
		 * 
		 * @param value タスク管理クラス
		 */
		public function set manager(value:CTaskManager):void
		{
		}

		/**
		 * 一時停止に対応しているかどうかを取得します。
		 * 
		 * @return 一時停止に対応している場合、true
		 */
		public function get isAvailablePause():Boolean
		{
			return m_bAvailablePause;
		}

		/**
		 * 一時停止に対応しているかどうかを設定します。
		 * 
		 * @return 一時停止に対応しているかどうか
		 */
		public function set isAvailablePause(value:Boolean):void
		{
			m_bAvailablePause = value;
		}

		/**
		 * 解放したかどうかを取得します。
		 * 
		 * @return 解放している場合、true
		 */
		public function get disposed():Boolean
		{
			return m_bDisposed;
		}

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
		public function set text(value:String):void
		{
			if(value == null)
			{
				throw new IllegalOperationError("引数にnullを設定出来ません。");
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
		public function get transform():CFontTransform
		{
			return m_transform;
		}

		/**
		 * 描画調整情報を設定します。
		 * 
		 * @param value 描画調整情報
		 */
		public function set transform(value:CFontTransform):void
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
		 * @param screen 格納する画面管理クラス(DisplayObjectContainerのサブクラスかCScreenクラスのインスタンス)
		 * @param uLayer レイヤ番号
		 */
		public function CTaskFont(fontResource:CFontResource, screen:Object, uLayer:uint = 0)
		{
			m_fontResource = fontResource;
			m_screen = screen;
			m_uLayer = uLayer;
		}

		/**
		 * コンストラクタの後、タスクが管理クラスに登録された直後に、
		 * 1度だけ自動的に呼ばれます。
		 */
		public function initialize():void
		{
		}

		/**
		 * 解放時に管理クラスから呼び出される処理です。
		 */
		public function dispose():void
		{
			deleteChild();
			m_bDisposed = true;
		}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return 生存タイマが0でない限りtrue
		 */
		public function update():Boolean
		{
			return (timer < 0 || timer-- > 0);
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
				transform = info;
			}
			else
			{
				info = transform;
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
						new CFontBit(m_fontResource, m_strText.charAt(i), m_screen, layer);
					bitList.push(bit);
				}
				if(autoRender)
				{
					render();
				}
			}
			catch(e:Error)
			{
				deleteChild();
				m_strText = "";
				throw e;
			}
		}
	}
}
