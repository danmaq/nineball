package danmaq.ball.task{

	import danmaq.ball.resource.*;
	import danmaq.nineball.core.*;
	import danmaq.nineball.misc.CInterpolate;
	import danmaq.nineball.struct.CScreen;
	
	import flash.display.Shape;
	
	import mx.utils.ColorUtil;

	/**
	 * 玉を表示するタスクです。
	 * 
	 * @author Mc(danmaq)
	 */
	public class CTaskBall implements ITask, IDisposed{

		////////// CONSTANTS //////////

		/**	フェーズ・カウンタ管理クラスが格納されます。 */
		protected const phaseManager:CPhaseManager = new CPhaseManager();

		/**	玉グラフィックの大きさが格納されます。 */
		private static const SIZE:uint = 32;
		
		/**	玉グラフィックが格納されます。 */
		private const ball:Shape = new Shape();
		
		/**	移動キューが格納されます。 */
		private const moveQueueList:Vector.<uint> = new Vector.<uint>();

		////////// FIELDS //////////

		/**	Y座標が格納されます。 */
		protected var y:Number = 0;

		/**	加速度グラフ情報が格納されます。 */
		private static var m_afAccelerate:Vector.<Number> = null; 
		
		/**	玉の描画情報が格納されます。 */
		private static var m_aShapeInfo:Vector.<CBallShapeInfo> = null; 
		
		/**	初期化済みかどうかが格納されます。 */
		private static var m_bInitialized:Boolean = false;

		/**	タスクが終了したかどうかが格納されます。 */
		private var m_bDisposed:Boolean = false;

		/**	現在速度が格納されます。 */
		private var m_fSpeed:Number = 0;

		////////// PROPERTIES //////////

		/**
		 * レイヤ値を取得します。
		 * 
		 * @return レイヤ値
		 */
		public function get layer():uint{ return CONST.LAYER_BALL; }

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
		 * @return 一時停止に対応している場合、true
		 */
		public function get isAvailablePause():Boolean{ return true; }

		/**
		 * 解放したかどうかを取得します。
		 * 
		 * @return 解放している場合、true
		 */
		public function get disposed():Boolean{ return m_bDisposed; }

		/**
		 * X座標を取得します。
		 * 
		 * @return X座標
		 */
		public function get x():Number{ return ball.x; }

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param uColor 色
		 */
		public function CTaskBall( uColor:uint ){
			if( !m_bInitialized ){
				initializeShapeInfo();
				initializeAccelerateGraph();
				m_bInitialized = true;
			}
			for each( var info:CBallShapeInfo in m_aShapeInfo ){
				ball.graphics.beginFill( ColorUtil.adjustBrightness( uColor, info.brite ) );
				ball.graphics.drawCircle( info.offset, info.offset, info.radius );
				ball.graphics.endFill();
			}
			ball.cacheAsBitmap = true;
			ball.x = SIZE / 2;
			ball.visible = false;
			CResource.screen.add( ball, int.MIN_VALUE );
		}

		/**
		 * コンストラクタの後、タスクが管理クラスに登録された直後に、
		 * 1度だけ自動的に呼ばれます。
		 */
		public function initialize():void{ ball.visible = true; }
		
		/**
		 * 解放時に管理クラスから呼び出される処理です。
		 */
		public function dispose():void{
			CResource.screen.remove( ball );
			m_bDisposed = true;
		}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return ゴールにたどり着くまでの間、true
		 */
		public function update():Boolean{
			calcSpeed();
			ball.y = y;
			ball.x += m_fSpeed;
			phaseManager.count++;
			return x < CScreen.size.x;
		}
		
		/**
		 * 玉を移動します。
		 */
		protected function move():void{ moveQueueList.push( phaseManager.count ); }
		
		/**
		 * 描画情報の初期化をします。
		 */
		private static function initializeShapeInfo():void{
			m_aShapeInfo = new Vector.<CBallShapeInfo>();
			var uLimit:uint = 256;
			for( var i:uint = 0; i <= uLimit; i++ ){
				m_aShapeInfo.push( new CBallShapeInfo(
					CInterpolate.accelerate( 0, 255, i, uLimit ),
					CInterpolate.accelerate( SIZE / 2, 0.1, i, uLimit ),
					CInterpolate.accelerate( 0, -SIZE / 5.5, i, uLimit ) ) );
			}
		}

		/**
		 * 速度変化グラフの初期化をします。
		 */
		private static function initializeAccelerateGraph():void{
			m_afAccelerate = new Vector.<Number>();
			var auPhaseLimit:Vector.<uint> = Vector.<uint>( [ 5, 5, 10 ] );
			var fPrevSpeed:Number = 0;
			for(
				var mgrPhase:CPhaseManager = new CPhaseManager();
				mgrPhase.phase < 3; mgrPhase.count++
			){
				var uPCount:uint = mgrPhase.phaseCount;
				var uPLimit:uint = auPhaseLimit[ mgrPhase.phase ];
				var fSpeed:Number = CONST.BALL_SPEED;
				switch( mgrPhase.phase ){
				case 0:
					fSpeed = CInterpolate.splineSFS( 0, CONST.BALL_SPEED, uPCount, uPLimit );
					break;
				case 2:
					fSpeed = CInterpolate.accelerate( CONST.BALL_SPEED, 0, uPCount, uPLimit );
					break;
				}
				m_afAccelerate.push( fSpeed - fPrevSpeed );
				fPrevSpeed = fSpeed;
				mgrPhase.isReserveNextPhase = uPCount >= uPLimit;
			}
		}

		/**
		 * 現在速度を更新します。
		 */
		private function calcSpeed():void{
			var i:uint = 0;
			while( i < moveQueueList.length ){
				var c:uint = phaseManager.count - moveQueueList[ i ];
				if( c >= m_afAccelerate.length ){ moveQueueList.splice( i, 1 ); }
				else{
					m_fSpeed += m_afAccelerate[ c ];
					i++;
				}
			}
			if( m_fSpeed < 0 ){ m_fSpeed = 0; }
		}
	}
}

/**
 * 玉の描画情報構造体です。
 * 
 * @author Mc(danmaq)
 */
final class CBallShapeInfo{

	/**	輝度が格納されます。 */
	public var brite:Number;

	/**	半径が格納されます。 */
	public var radius:Number;

	/**	中心座標からの誤差が格納されます。 */
	public var offset:Number;

	/**
	 * コンストラクタ。
	 * 
	 * @param _brite 輝度
	 * @param _radius 半径
	 * @param _offset 中心座標からの誤差
	 */
	public function CBallShapeInfo( _brite:Number, _radius:Number, _offset:Number ){
		brite = _brite;
		radius = _radius;
		offset = _offset;
	}
}
