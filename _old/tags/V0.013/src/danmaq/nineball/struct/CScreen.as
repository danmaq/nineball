package danmaq.nineball.struct{

	import flash.display.*;
	
	import mx.containers.Canvas;
	import mx.core.UIComponent;

	/**
	 * 画面オブジェクト管理クラスです。
	 * 画面オブジェクトとして親の場合はCanvasクラス、
	 * 子の場合はUIComponentクラスを内部的に使用します。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CScreen{

		////////// CONSTANTS //////////

		/**	登録されている表示物体のレイヤ一覧が格納されます。 */
		private const displayObjectList:Vector.<CDisplayObject> = new Vector.<CDisplayObject>();

		////////// FIELDS //////////

		/**	親管理クラスが格納されます。 */
		private static var m_parent:CScreen = null;

		/**	画面オブジェクトが格納されます。 */
		private var m_screen:UIComponent = null;
		
		/**	親管理クラスかどうかが格納されます。 */
		private var m_bParent:Boolean = false;
		
		////////// PROPERTIES //////////

		/**
		 * 画面オブジェクトを取得します。
		 * 
		 * @return 登録オブジェクト
		 */
		public function get screen():UIComponent{ return m_screen; }

		/**
		 * 登録オブジェクトの総数を取得します。
		 * 孫以下に登録されているオブジェクトはカウントされません。
		 * 
		 * @return 登録オブジェクトの総数
		 */
		public function get total():uint{ return displayObjectList.length; }

		/**
		 * 親管理クラスを取得します。
		 * 
		 * @return 親管理クラス
		 */
		public function get parent():CScreen{ return m_parent; }

		/**
		 * 親管理クラスであるかどうかを取得します。
		 * 
		 * @return 親管理クラスである場合、true
		 */
		public function get isParent():Boolean{ return m_bParent; }

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 通常時はSingletonな親管理クラスの子として生成されますが、
		 * parentObjectを指定した場合、そのオブジェクトの子となります。
		 * _parentは常時nullで構いません。
		 * 
		 * @param nLayer (省略可:負の最大値)レイヤ番号
		 * @param parentObject (省略可:null)親コンテナ
		 * @param _parent (省略可:null)親管理クラス作成に必要な値
		 */
		public function CScreen(
			nLayer:int = int.MIN_VALUE, parentScreen:CScreen = null, _parent:CScreenParent = null
		){
			m_bParent = ( _parent != null );
			if( isParent ){ m_screen = new Canvas(); }
			else{
				if( m_parent == null ){
					m_parent = new CScreen( int.MIN_VALUE, null, new CScreenParent() );
				}
				m_screen = new UIComponent();
				if( parentScreen == null ){ parent.add( m_screen, nLayer ); }
				else{ parentScreen.add( m_screen, nLayer ); }
			}
		}

		/**
		 * 画面に表示オブジェクトを配置します。
		 * 
		 * @param child 配置するオブジェクト
		 * @param nLayer レイヤ番号
		 */
		public function add( child:DisplayObject, nLayer:int ):void{
			var struct:CDisplayObject = new CDisplayObject( child, nLayer );
			var uLength:uint = displayObjectList.length;
			for( var i:int = 0; i < uLength; i++ ){
				if( displayObjectList[ i ].layer > nLayer ){
					displayObjectList.splice( i, 0, struct );
					addChildAtReverse( child, i );
					return;
				}
			}
			displayObjectList.push( struct );
			m_screen.addChildAt( child, 0 );
		}

		/**
		 * 画面から表示オブジェクトを外します。
		 * 
		 * @param child 配置するオブジェクト
		 * @param nLayer レイヤ番号
		 */
		public function remove( child:DisplayObject ):void{
			m_screen.removeChild( child );
			var uLength:uint = displayObjectList.length;
			for( var i:int = 0; i < uLength; i++ ){
				if( displayObjectList[ i ].obj === child ){
					displayObjectList.splice( i, 1 );
					return;
				}
			}
			trace( "warn:add()で登録されていないオブジェクトが削除されました。" );
		}

		/**
		 * この DisplayObjectContainer インスタンスに子 DisplayObject インスタンスを追加します。
		 * addChildAt()の逆順でインデックスを追っていきます。
		 * 
		 * @param child 配置するオブジェクト
		 * @param index 子を追加するインデックス位置
		 */
		public function addChildAtReverse( child:DisplayObject, index:int ):DisplayObject{
			return m_screen.addChildAt( child, m_screen.numChildren - index );
		}
	}
}

class CScreenParent{}
