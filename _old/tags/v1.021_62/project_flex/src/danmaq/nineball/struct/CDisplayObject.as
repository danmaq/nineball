package danmaq.nineball.struct{

	import danmaq.nineball.misc.CMisc;

	import flash.display.DisplayObject;

	import mx.utils.StringUtil;
	
	/**
	 * 画面オブジェクト管理クラス用表示物体構造体です。
	 * 
	 * <p>
	 * danmaq Nineball-Library内部で自動的に使用します。
	 * 通常ユーザが直接使用する必要はありません。
	 * </p>
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CDisplayObject{

		////////// FIELDS //////////
		
		/**	表示物体のオブジェクトが格納されます。 */
		private var m_obj:DisplayObject = null;
		
		/**	レイヤ値が格納されます。 */
		private var m_nLayer:int = int.MAX_VALUE;

		////////// PROPERTIES //////////

		/**
		 * 表示物体のオブジェクトを取得します。
		 * 
		 * @return 表示物体のオブジェクト
		 */
		public function get obj():DisplayObject{ return m_obj; }
		
		/**
		 * レイヤ値を取得します。
		 * 
		 * @return レイヤ値
		 */
		public function get layer():int{ return m_nLayer; }
		
		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param obj 表示物体のオブジェクト
		 * @param nLayer レイヤ値
		 */
		public function CDisplayObject( obj:DisplayObject, nLayer:int ){
			m_obj = obj;
			m_nLayer = nLayer;
		}

		/**
		 * このクラスの状態を文字列で取得します。
		 * 
		 * @return オブジェクトのストリング表現
		 */
		public function toString():String{
			return StringUtil.substitute( "Obj:{0},Layer:{1}",
				CMisc.getClassName( obj ), layer );
		}
	}
}
