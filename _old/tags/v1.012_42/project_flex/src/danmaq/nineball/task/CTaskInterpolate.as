package danmaq.nineball.task{
	
	import danmaq.nineball.core.*;
	import danmaq.nineball.misc.CInterpolate;
	
	import flash.errors.IllegalOperationError;

	/**
	 * 内分カウンタタスクです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskInterpolate implements ITask, IDisposed{

		////////// CONSTANTS //////////

		public static const TYPE_SMOOTH:uint		= 0;
		public static const TYPE_ACCELERATE:uint	= 1;
		public static const TYPE_SLOWDOWN:uint		= 2;
		public static const TYPE_SFS:uint			= 3;
		public static const TYPE_FSF:uint			= 4;

		////////// FIELDS //////////

		/**	内分カウンタ値を設定する関数が格納されます。 */
		public var setter:Function = null;

		/**	タスク管理クラスが格納されます。 */
		private var m_taskManager:CTaskManager = null;

		/**	レイヤ番号が格納されます。 */
		private var m_uLayer:uint;

		/**	タスクが終了したかどうかが格納されます。 */
		private var m_bDisposed:Boolean = false;

		/**	次に連結されるタスクが格納されます。 */
		private var m_taskNextChain:CTaskInterpolate = null;

		/**	簡易フレーム時間カウンタが格納されます。 */
		private var m_uCount:uint = 0;

		/**	内分カウンタの寿命が格納されます。 */
		private var m_uLimit:uint;

		/**	内分カウンタの種別が格納されます。 */
		private var m_fnInterpolate:Function;

		/** 始値が格納されます。 */
		private var m_fStart:Number;

		/**	終値が格納されます。 */
		private var m_fEnd:Number;

		/**	現在の内分カウンタ値が格納されます。 */
		private var m_fValue:Number;

		////////// PROPERTIES //////////

		/**
		 * レイヤ値を取得します。
		 * 
		 * @return レイヤ値
		 */
		public function get layer():uint{ return m_uLayer; }

		/**
		 * タスク管理クラスを設定します。
		 * 
		 * @param value タスク管理クラス
		 */
		public function set manager( value:CTaskManager ):void{ m_taskManager = value; }

		/**
		 * 解放したかどうかを取得します。
		 * 
		 * @return 解放している場合、true
		 */
		public function get disposed():Boolean{ return m_bDisposed; }

		/**
		 * 次に連結されるタスクを取得します。
		 * 
		 * @return 次に連結されるタスク。存在しない場合、null
		 */
		public function get nextChain():CTaskInterpolate{ return m_taskNextChain; }

		/**
		 * 次に連結されるタスクを設定します。
		 * 
		 * @param value 次に連結されるタスク。
		 */
		public function set nextChain( value:CTaskInterpolate ):void{
			if( value == this ){
				throw new IllegalOperationError( "自分自身を次タスクとして連結することは出来ません。" );
			}
			m_taskNextChain = value;
		}

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param fStart 始値
		 * @param fEnd 終値
		 * @param uLimit カウンタ寿命
		 * @param uType 内分カウンタ種類ID
		 * @param uLayer レイヤ値
		 */
		public function CTaskInterpolate(
			fStart:Number, fEnd:Number, uLimit:uint, uType:uint = TYPE_SMOOTH, uLayer:uint = 0
		){
			m_fValue = fStart;
			m_fStart = fStart;
			m_fEnd = fEnd;
			m_uLimit = uLimit;
			m_uLayer = uLayer;
			switch( uType ){
			case TYPE_SMOOTH:		m_fnInterpolate = CInterpolate.smooth;		break;
			case TYPE_ACCELERATE:	m_fnInterpolate = CInterpolate.accelerate;	break;
			case TYPE_SLOWDOWN:		m_fnInterpolate = CInterpolate.slowdown;	break;
			case TYPE_SFS:			m_fnInterpolate = CInterpolate.splineSFS;	break;
			case TYPE_FSF:			m_fnInterpolate = CInterpolate.splineFSF;	break;
			default:
				throw new IllegalOperationError(
					"不正な内分カウンタ種類IDが指定されました。" );
				break;
			}
		}

		/**
		 * コンストラクタの後、タスクが管理クラスに登録された直後に、
		 * 1度だけ自動的に呼ばれます。
		 */
		public function initialize():void{}

		/**
		 * 解放時に管理クラスから呼び出される処理です。
		 */
		public function dispose():void{
			if( nextChain != null ){ m_taskManager.add( nextChain ); }
			m_bDisposed = true;
		}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return アニメーションが有効な間、true
		 */
		public function update():Boolean{
			m_fValue = m_fnInterpolate( m_fStart, m_fEnd, m_uCount, m_uLimit );
			if( setter != null ){ setter( m_fValue ); }
			m_uCount++;
			return m_uCount < m_uLimit;
		}
	}
}
