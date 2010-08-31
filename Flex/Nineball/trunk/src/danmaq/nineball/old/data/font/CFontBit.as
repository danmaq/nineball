////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.old.data.font
{

	import danmaq.nineball.data.CScreen;
	import danmaq.nineball.misc.CMisc;
	import danmaq.nineball.old.core.IDisposed;
	
	import flash.display.*;
	import flash.errors.IllegalOperationError;
	import flash.geom.*;

	/**
	 * 単文字フォントクラスです。
	 * 
	 * <p>
	 * クラスCTaskFont内部で自動的に使用します。
	 * 通常ユーザが直接使用する必要はありません。
	 * </p>
	 * 
	 * @see danmaq.nineball.task.CTaskFont
	 * @author Mc(danmaq)
	 */
	public class CFontBit implements IDisposed
	{

		////////// FIELDS //////////
		
		/**	レイヤ番号が格納されます。 */
		private var m_uLayer:uint;

		/**	画像を格納する画面管理クラスが格納されます。 */
		private var m_screen:CScreen = null;

		/**	画像を格納する画面管理クラスが格納されます。 */
		private var m_doc:DisplayObjectContainer = null;

		/**	解放されたかどうかが格納されます。 */
		private var m_bDisposed:Boolean = false;
		
		/**	表示するテキストが格納されます。 */
		private var m_strText:String = "";

		/**	現在表示されているかどうかが格納されます。 */
		private var m_bView:Boolean = false;

		/**	画像が格納されます。 */
		private var m_image:Bitmap = null;

		/**	初期サイズが格納されます。 */
		private var m_size:Point = new Point();
		
		////////// PROPERTIES //////////

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
		 * 現在表示されているかどうかを取得します。
		 * 
		 * @return 現在表示されているいる場合、true
		 */
		public function get view():Boolean
		{
			return m_bView;
		}

		/**
		 * 現在表示されているかどうかを設定します。
		 * 
		 * @param value 現在表示されているかどうか
		 */
		public function set view(value:Boolean):void
		{
			if(m_image == null)
			{
				throw new IllegalOperationError("表示すべき画像が準備されていません。");
			}
			if(m_bView != value)
			{
				m_bView = value;
				if(value)
				{
					add(m_image, m_uLayer);
				}
				else
				{
					remove(m_image);
				}
			}
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
		 * 拡縮しない状態での画像サイズを取得します。
		 * 
		 * @return 画像サイズ
		 */
		public function get size():Point
		{
			return m_size.clone();
		}

		/**
		 * 画面管理クラスに画像を貼り付けるメソッドを取得します。
		 * 
		 * @return 画面管理クラスに画像を貼り付けるメソッド
		 */
		private function get add():Function
		{
			return m_doc == null ? m_screen.add :
				function(img:DisplayObject, layer:int):void
				{
					m_doc.addChild(img);
				};
		}

		/**
		 * 画面管理クラスから画像を除去するメソッドを取得します。
		 * 
		 * @return 画面管理クラスから画像を除去するメソッド
		 */
		private function get remove():Function
		{
			return m_doc == null ? m_screen.remove : m_doc.removeChild;
		}

		/**
		 * 現画面管理クラスを設定します。
		 * 
		 * @param value 画面管理クラス
		 * @throws flash.errors.IllegalOperationError 画面管理クラスが
		 * DisplayObjectContainerのサブクラスかCScreenクラスのインスタンスで無かった場合
		 */
		private function set screen(value:Object):void
		{
			if(CMisc.isRelate(CScreen, value))
			{
				m_screen = value as CScreen;
			}
			else if(CMisc.isRelate(DisplayObjectContainer, value))
			{
				m_doc = value as DisplayObjectContainer;
			}
			else
			{
				throw new IllegalOperationError(
					"画面管理クラスはDisplayObjectContainerのサブクラス、または" + 
					"CScreenクラスのインスタンスで無ければなりません。");
			}
		}

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param fontResource フォントリソース
		 * @param strByte 単文字
		 * @param screen 格納する画面管理クラス
		 * @param uLayer レイヤ番号
		 */
		public function CFontBit(
			fontResource:CFontResource, strByte:String, screen:Object, uLayer:uint = 0)
		{
			this.screen = screen;
			m_uLayer = uLayer;
			if(text != strByte)
			{
				var imgByte:Bitmap = fontResource.getImage(strByte);
				if(imgByte == null)
				{
					throw new IllegalOperationError("指定の文字は割り当てられていません。:" + strByte);
				}
				m_image = imgByte;
				m_size = new Point(m_image.width, m_image.height);
				m_strText = strByte;
			}
		}

		/**
		 * 解放時に管理クラスから呼び出されます。
		 */
		public function dispose():void
		{
			view = false;
			m_bDisposed = true;
		}

		/**
		 * レンダリングします。
		 * 
		 * @param info フォント描画調整情報
		 */
		public function render(info:CFontTransformBit):void
		{
			var color:ColorTransform = new ColorTransform();
			color.color = info.color;
			m_image.transform.colorTransform = color;
			m_image.alpha = info.alpha;
			m_image.smoothing = info.smoothing;
			CMisc.setMatrix(m_image, info.scale, info.rotate, info.pos);
		}
	}
}
