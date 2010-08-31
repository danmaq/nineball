////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.data
{

	import danmaq.nineball.old.core.IDisposed;
	import danmaq.nineball.old.data.CDisplayObject;
	
	import flash.display.*;
	import flash.events.Event;
	import flash.geom.Point;
	
	import mx.containers.Canvas;
	import mx.core.UIComponent;

	/**
	 * 画面オブジェクト管理クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CScreen implements IDisposed
	{

		////////// CONSTANTS //////////

		/**	登録されている表示物体のレイヤ一覧が格納されます。 */
		private const displayObjectList:Vector.<CDisplayObject> = new Vector.<CDisplayObject>();

		/**	画面サイズが格納されます。 */
		private static const posSize:Point = new Point();

		////////// FIELDS //////////

		/**	ルート管理クラスが格納されます。 */
		private static var m_root:CScreen = null;

		/**	解放されたかどうかが格納されます。 */
		private var m_bDisposed:Boolean = false;

		/**	画面オブジェクトが格納されます。 */
		private var m_screen:UIComponent = null;
		
		/**	ルート管理クラスかどうかが格納されます。 */
		private var m_bRoot:Boolean = false;
		
		/**	親管理クラスが格納されます。 */
		private var m_parent:CScreen = null;
		
		////////// PROPERTIES //////////

		/**
		 * ルート管理クラスを取得します。
		 * 
		 * @return ルート管理クラス
		 */
		public static function get root():CScreen
		{
			if(m_root == null)
			{
				m_root = new CScreen(int.MIN_VALUE, null, true, new CScreenRoot());
			}
			return m_root;
		}

		/**
		 * メイン描画領域を取得します。
		 * 
		 * @return メイン描画領域
		 */
		public static function get stage():Stage
		{
			return root.screen.stage;
		}
		
		/**
		 * 画面サイズを取得します。
		 * 注意：CScreenクラスを初めて使用する際、既に画面上に何か描画していると
		 * 正常に値が取得できない場合があります。
		 * 
		 * @return 画面サイズ
		 */
		public static function get size():Point
		{
			return posSize.clone();
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
		 * 画面オブジェクトを取得します。
		 * 
		 * @return 登録オブジェクト
		 */
		public function get screen():UIComponent
		{
			return m_screen;
		}

		/**
		 * 登録オブジェクトの総数を取得します。
		 * 孫以下に登録されているオブジェクトはカウントされません。
		 * 
		 * @return 登録オブジェクトの総数
		 */
		public function get total():uint
		{
			return displayObjectList.length;
		}

		/**
		 * 親管理クラスであるかどうかを取得します。
		 * 
		 * @return 親管理クラスである場合、true
		 */
		public function get isRoot():Boolean
		{
			return m_bRoot;
		}
		
		/**
		 * 直属の親管理クラスを取得します。
		 * 
		 * @return 直属の親管理クラス インスタンス。rootである場合、自分自身のインスタンス。
		 */
		public function get parent():CScreen
		{
			return m_parent == null ? root : m_parent;
		}

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * <p>
		 * 通常時はSingletonなルート管理クラスの子として生成されますが、
		 * parentObjectを指定した場合、そのオブジェクトの子となります。
		 * mx.controlsオブジェクトを配置したい場合、bUseCanvasをtrueに設定します。
		 * _parentは常時nullで構いません。
		 * </p>
		 * 
		 * @param nLayer (省略可:負の最大値)レイヤ番号
		 * @param parent (省略可:null)親コンテナ
		 * @param bUseCanvas (省略可:false)子にmxコントロールを設置するかどうか
		 * @param _parent (省略可:null)親管理クラス作成に必要な値
		 */
		public function CScreen(
			nLayer:int = int.MIN_VALUE, parent:CScreen = null,
			bUseCanvas:Boolean = false, _root:CScreenRoot = null)
		{
			m_bRoot = (_root != null);
			if(isRoot)
			{
				m_screen = new Canvas();
				m_screen.addEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
			}
			else
			{
				m_parent = parent;
				m_screen = bUseCanvas ? new Canvas() : new UIComponent();
				if(parent == null)
				{
					root.add(m_screen, nLayer);
				}
				else
				{
					parent.add(m_screen, nLayer);
				}
			}
		}

		/**
		 * この管理クラスを解放します。
		 * ルート管理クラスでこのメソッドを実行した場合無視されます。
		 * (disposedプロパティがtrueになりません)
		 */
		public function dispose():void
		{
			if(parent != this)
			{
				parent.remove(screen);
				m_bDisposed = true;
			}
		}

		/**
		 * 画面に表示オブジェクトを配置します。
		 * 
		 * @param child 配置するオブジェクト
		 * @param nLayer レイヤ番号
		 */
		public function add(child:DisplayObject, nLayer:int = int.MIN_VALUE):void
		{
			var struct:CDisplayObject = new CDisplayObject(child, nLayer);
			var uLength:uint = displayObjectList.length;
			for(var i:int = 0; i < uLength; i++)
			{
				if(displayObjectList[i].layer > nLayer)
				{
					displayObjectList.splice(i, 0, struct);
					addChildAtReverse(child, i);
					return;
				}
			}
			displayObjectList.push(struct);
			screen.addChildAt(child, 0);
		}

		/**
		 * 画面から表示オブジェクトを外します。
		 * 
		 * @param child 配置するオブジェクト
		 * @param nLayer レイヤ番号
		 */
		public function remove(child:DisplayObject):void
		{
			screen.removeChild(child);
			var uLength:uint = displayObjectList.length;
			for(var i:int = 0; i < uLength; i++)
			{
				if(displayObjectList[i].obj === child)
				{
					displayObjectList.splice(i, 1);
					return;
				}
			}
			trace("warn:add()で登録されていないオブジェクトが削除されました。");
		}

		/**
		 * この DisplayObjectContainer インスタンスに子 DisplayObject インスタンスを追加します。
		 * addChildAt()の逆順でインデックスを追っていきます。
		 * 
		 * @param child 配置するオブジェクト
		 * @param index 子を追加するインデックス位置
		 */
		public function addChildAtReverse(child:DisplayObject, index:uint):DisplayObject
		{
			return screen.addChildAt(child, screen.numChildren - index);
		}

		/**
		 * ルート管理クラスがステージ上の表示リストに追加されたときに実行されます。
		 * 
		 * @param e イベントパラメータ
		 */
		private function onAddedToStage(e:Event):void
		{
			m_screen.removeEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
			posSize.x = stage.width;
			posSize.y = stage.height;
		}
	}
}

/**
 * 親管理クラス作成に必要なクラスです。
 */
class CScreenRoot
{
}
