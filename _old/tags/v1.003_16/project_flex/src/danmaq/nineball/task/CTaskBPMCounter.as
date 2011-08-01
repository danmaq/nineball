package danmaq.nineball.task{

	import danmaq.nineball.core.*;
	
	import flash.errors.IllegalOperationError;

	/**
	 * BPMカウンタタスクです。
	 * 指定したBPMに合わせビートを刻みます。
	 * BPMのデフォルト値は60です。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskBPMCounter implements ITask{
		// ! TODO : 現在精度が及ばない分は切り捨てとなっているが、四捨五入するように改良。

		////////// CONSTANTS //////////

		/**	フェーズ管理クラスが格納されます。 */
		private const phaseManager:CPhaseManager = new CPhaseManager();

		////////// FIELDS //////////

		/**	BPMが格納されます。 */
		private var m_fBPM:Number = 60;

		/**	レイヤ番号が格納されます。 */
		private var m_uLayer:uint;

		/**	カウント開始時間が格納されます。 */
		private var m_fStartTime:Number;
		
		/**	前フレームの時間が格納されます。 */
		private var m_fPrevTime:Number;
		
		/**	次のビートを刻む時間が格納されます。 */
		private var m_fNextBeatTime:Number;

		/**	次のビートを刻むまでの残り時間が格納されます。 */
		private var m_fNextBeatAmount:Number;

		/**	次のビートが来るまでの予想フレーム時間が格納されます。 */
		private var m_uNextBeatCount:uint = uint.MAX_VALUE;

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
		 * BPMを取得します。
		 * 
		 * @return BPM
		 */
		public function get bpm():Number{ return m_fBPM; }

		/**
		 * BPMを設定します。
		 * 負数や0、NaNを設定すると例外が発生します。
		 * 
		 * @param value BPM
		 */
		public function set bpm( value:Number ):void{
			if( value <= 0 || isNaN( value ) ){
				throw new IllegalOperationError( "BPMとしておかしい値は設定出来ません。" );
			}
			m_fBPM = value;
			reset();
		}

		/**
		 * 次のビートが来るまでの予想フレーム時間を取得します。
		 * 
		 * @return 次のビートが来るまでの予想フレーム時間
		 */
		public function get nextBeatCount():uint{ return m_uNextBeatCount; }

		/**
		 * 次のビートが来るまでの残り時間を取得します。
		 * 
		 * @return 次のビートが来るまでの残り時間
		 */
		public function get nextBeatAmount():uint{ return m_fNextBeatAmount; }

		/**
		 * このフレームでビートを刻んだかどうかを取得します。
		 * 
		 * @return このフレームでビートを刻んだ場合、true
		 */
		public function get beat():Boolean{ return ( phaseManager.phaseCount == 0 ); }

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param uLayer レイヤ番号
		 */
		public function CTaskBPMCounter( uLayer:uint = 0 ){
			m_uLayer = uLayer;
			reset();
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
		 * @return 無条件でtrue
		 */
		public function update():Boolean{
			var fNow:Number = ( new Date() ).time - m_fStartTime;
			var fGap:Number = fNow - m_fPrevTime;
			if( fNow > m_fNextBeatTime ){
				phaseManager.isReserveNextPhase = true;
				m_fNextBeatTime = ( 60000 / bpm ) * ( phaseManager.phase + 2 );
			}
			m_fNextBeatAmount = m_fNextBeatTime - fNow;
			m_uNextBeatCount = int( m_fNextBeatAmount / fGap );
			m_fPrevTime = fNow;
			phaseManager.count++;
			return true;
		}
		
		/**
		 * タイマおよびカウンタをリセットします。
		 * 初期化時やBPMを変更した時に呼び出されます。
		 */
		private function reset():void{
			phaseManager.reset();
			m_fStartTime = ( new Date() ).time;
			m_fNextBeatTime = 60000 / bpm;
			m_fPrevTime = m_fStartTime;
		}
	}
}
