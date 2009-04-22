package danmaq.ball.task{

	import danmaq.ball.struct.CVKData;
	import danmaq.nineball.core.*;

	/**
	 * キー入力を管理するタスクです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskInput implements ITask{

		////////// FIELDS //////////

		/**	レイヤ値が格納されます。 */
		private var m_uLayer:uint = 0;

		/**	仮想ボタン割り当て構造体が格納されます。 */
		private var m_aVKData:Vector.<CVKData>;

		////////// PROPERTIES //////////

		/**
		 * レイヤ値を取得します。
		 * 
		 * @return レイヤ値
		 */
		public function get layer():uint{ return m_uLayer; }

		/**
		 * タスク管理クラスを設定します。
		 * このクラスでは特に必要ないので何も設定しません。
		 * 
		 * @param value タスク管理クラス
		 */
		public function set manager( value:CTaskManager ):void{}
		
		/**
		 * 一時停止に対応しているかどうかを取得します。
		 * 
		 * @return 無条件にfalse
		 */
		public function get isAvailablePause():Boolean{ return false; }
		
		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param uLayer レイヤ番号
		 */
		public function CTaskInput( uLayer:uint = 0 ){
			m_uLayer = uLayer;
			resetVK();
		}

		/**
		 * コンストラクタの後、タスクが管理クラスに登録された直後に、
		 * 1度だけ自動的に呼ばれます。
		 */
		public function initialize():void{}
		
		/**
		 * 解放時に管理クラスから呼び出される処理です。
		 */
		public function dispose():void{}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return 無条件にtrue
		 */
		public function update():Boolean{ return true; }
		
		/**
		 * 仮想ボタンを初期状態に戻します。
		 */
		public function resetVK():void{ m_aVKData = new Vector.<CVKData>(); }
		
		/**
		 * 仮想ボタンを追加します。
		 * 
		 * @param vkData 仮想ボタン情報構造体
		 * @return 仮想ボタンID
		 */
		public function addVK( vkData:CVKData ):uint{
			m_aVKData.add( vkData );
			return m_aVKData.length;
		}

		/**
		 * 仮想ボタンを取得します。
		 * 
		 * @param uVKID 仮想ボタンID
		 * @return 仮想ボタン情報構造体
		 */
		public function getVK( uVKID:uint ):CVKData{ return m_aVKData[ uVKID ]; }
	}
}
