package danmaq.nineball.task{

	import danmaq.nineball.core.*;
	import danmaq.nineball.misc.CMisc;
	
	import flash.errors.IllegalOperationError;
	import flash.media.*;
	import flash.utils.Dictionary;
	
	/**
	 * 排他的に効果音を再生するタスク。
	 * 同一の効果音を連続して再生しようとすると前の効果音はキャンセルされます。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskExclusiveSE implements ITask{

		////////// CONSTANTS //////////

		/**
		 * 効果音のリストが格納されます。
		 * [ "key", [ SOUND, SOUND_CH ] ]
		 */
		private const dicSE:Dictionary = new Dictionary();

		/**	再生キューが格納されます。 */
		private const QList:Array = new Array();

		////////// FIELDS //////////

		/**	レイヤ番号が格納されます。 */
		private var m_uLayer:uint;

		/**
		 * 分解能がフレーム時間単位で格納されます。
		 * 再生メソッドを投げてから実際に再生されるまでの最大遅延時間です。
		 */
		private var m_uResolution:uint;

		/**	現在のフレーム時間が格納されます。 */
		private var m_uCount:uint = 0;
		
		////////// PROPERTIES //////////

		/**
		 * レイヤ値を取得します。
		 * 
		 * @return レイヤ値
		 */
		public function get layer():uint{ return 0; }

		/**
		 * タスク管理クラスを設定します。
		 * このクラスでは特に必要ないので何も設定しません。
		 * 
		 * @param value タスク管理クラス
		 */
		public function set manager( value:CTaskManager ):void{}

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 効果音再生フレーム解像度には1未満の値は設定出来ません。
		 * (設定すると例外が発生します)
		 * 
		 * @param uLayer レイヤ番号
		 * @param uResolution 効果音再生フレーム解像度
		 */
		public function CTaskExclusiveSE( uLayer:uint = 0, uResolution:uint = 1 ){
			m_uLayer = uLayer;
			m_uResolution = uResolution;
			if( uResolution == 0 ){
				throw new IllegalOperationError( "効果音解像度は1以上である必要があります。" );
			}
		}

		/**
		 * コンストラクタの後、タスクが管理クラスに登録された直後に、
		 * 1度だけ自動的に呼ばれます。
		 */
		public function initialize():void{}

		/**
		 * デストラクタ。
		 */
		public function dispose():void{}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return 無条件にtrue
		 */
		public function update():Boolean{
			m_uCount++;
			if( m_uCount % m_uResolution == 0 ){
				while( QList.length > 0 ){
					var strKey:String = QList.pop();
					if( dicSE[ strKey ] != null ){
						var aItem:Array = dicSE[ strKey ];
						if( aItem[ 1 ] != null ){ ( aItem[ 1 ] as SoundChannel ).stop(); }
						aItem[ 1 ] = ( aItem[ 0 ] as Sound ).play();
					}
				}
			}
			return true;
		}
		
		/**
		 * 効果音再生の予約を入れます。
		 * 
		 * @param se 効果音
		 */
		public function play( se:Sound ):void{
			var strClass:String = CMisc.getClassName( se );
			if( dicSE[ strClass ] == null ){ dicSE[ strClass ] = [ se, null ]; }
			if( QList.indexOf( strClass ) < 0 ){ QList.push( strClass ); }
		}
	}
}
