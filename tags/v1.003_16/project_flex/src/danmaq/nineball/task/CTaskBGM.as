package danmaq.nineball.task{

	import danmaq.nineball.core.*;
	import danmaq.nineball.misc.CInterpolate;
	import danmaq.nineball.struct.CBGMPreset;
	
	import flash.events.TimerEvent;
	import flash.media.*;
	import flash.utils.Timer;

	/**
	 * 部分ループポイント対応BGMタスクです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskBGM implements ITask, IDisposed{

		////////// CONSTANTS //////////

		/**	終了フェーズ番号が格納されます。 */
		private static const PHASE_END:uint = 255;

		/**	フェーズ管理クラスが格納されます。 */
		private const phaseManager:CPhaseManager = new CPhaseManager();

		////////// FIELDS //////////

		/**
		 * 解放命令を無視するかどうかが格納されます。
		 * 解除忘れ防止のため、一度無視するたびにオフになります。
		 */
		public var isDisposeCancel:Boolean = false;

		/**	現在再生されているかどうかが格納されます。 */
		private static var s_bPlay:Boolean = false;

		/**	タスクが終了したかどうかが格納されます。 */
		private var m_bDisposed:Boolean = false;

		/**	レイヤ番号が格納されます。 */
		private var m_uLayer:uint;

		/** フェードに要するフレーム時間が格納されます。 */
		private var m_uFadeTime:uint;

		/**	BGM用サウンドチャンネルが格納されます。 */
		private var m_bgmch:SoundChannel = null;
		
		/**	BGMプリセット構造体が格納されます。 */
		private var m_preset:CBGMPreset;

		/**	BGM再生開始時間が格納されます。 */
		private var m_nStartTime:int;
		
		/**	タイマが設定されているかどうかが格納されます。 */
		private var m_bSetTimer:Boolean;
		
		/**	ミュート中かどうかが格納されます。 */
		private var m_bMute:Boolean = false;
		
		/**	最大音量値が格納されます。 */
		private var m_fMaxVolume:Number = 1;
		
		////////// PROPERTIES //////////

		/**
		 * 現在再生中かどうかを取得します。
		 * 
		 * @return 現在再生中である場合、true
		 */
		public static function get isPlay():Boolean{ return s_bPlay; }

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
		 * タスクが終了したかどうかを取得します。
		 * 
		 * @return タスクが終了している場合、true
		 */
		public function get disposed():Boolean{ return m_bDisposed; }

		/**
		 * ミュート中かどうかを取得します。
		 * 
		 * @return ミュートしている場合、true
		 */
		public function get mute():Boolean{ return m_bMute; }

		/**
		 * ミュートするかどうかを設定します。
		 * 
		 * @param value ミュートするかどうか
		 */
		public function set mute( value:Boolean ):void{
			if( mute != value ){
				m_bMute = value;
				applyMute();
			}
		}

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 * 
		 * @param preset BGMプリセット
		 * @param uFadeTime フェード時間
		 * @param uLayer レイヤ番号
		 */
		public function CTaskBGM( preset:CBGMPreset, uFadeTime:uint, uLayer:uint = 0 ){
			m_preset = preset;
			uFadeTime = uFadeTime;
			m_uLayer = uLayer;
		}

		/**
		 * コンストラクタの後、タスクが管理クラスに登録された直後に、
		 * 1度だけ自動的に呼ばれます。
		 */
		public function initialize():void{ playLoop(); }

		/**
		 * デストラクタ。
		 */
		public function dispose():void{
			if( isDisposeCancel ){ isDisposeCancel = false; }
			else{
				if( m_bgmch != null ){ m_bgmch.stop(); }
				m_bgmch = null;
				m_bDisposed = true;
				s_bPlay = false;
			}
		}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return フェードするまでの間、true
		 */
		public function update():Boolean{
			var uPhase:uint = phaseManager.phase; 
			var uPCount:uint = phaseManager.phaseCount;
			var nAmount:int; 
			switch( uPhase ){
			case 0:
				if(
					!phaseManager.isReserveNextPhase &&
					setTimer( m_preset.loopEnd - ( new Date().time - m_nStartTime ) )
				){ phaseManager.isReserveNextPhase = true; }
				break;
			case 1:
				setTimer( ( m_preset.loopEnd - m_preset.loopStart ) -
					( new Date().time - m_nStartTime ) ); 
				break;
			case PHASE_END:
				phaseManager.isReserveNextPhase = ( uPhase == m_uFadeTime );
				m_fMaxVolume = CInterpolate.slowdown( 1, 0, uPCount, m_uFadeTime );
				m_bgmch.soundTransform = new SoundTransform( m_fMaxVolume );
				break;
			}
			phaseManager.count++;
			return uPhase <= PHASE_END;
		}

		/**
		 * フェードアウトを開始します。
		 */
		public function fadeAndStop():void{
			if( !disposed && phaseManager.phase < PHASE_END ){
				phaseManager.nextPhase = PHASE_END;
			}
		}
		
		/**
		 * ミュート中かどうかを判断して音量を設定します。
		 */
		private function applyMute():void{
			if( m_bgmch != null ){
				m_bgmch.soundTransform = new SoundTransform( mute ? 0 : m_fMaxVolume );
			}
		}

		/**
		 * タイマを設定します。
		 * 
		 * @param nAmount タイマ発動時間
		 * @return タイマを設定できた場合、true
		 */
		private function setTimer( nAmount:int ):Boolean{
			var bResult:Boolean = ( !m_bSetTimer && nAmount < 100 );
			if( bResult ){	// 長引くほど精度が落ちるので直前(0.1秒前)に仕掛ける
				m_bSetTimer = true;
				var t:Timer = new Timer( Math.max( nAmount, 0 ), 1 );
				t.addEventListener( "timer", playLoop );
				t.start();
			}
			return bResult;
		}
		
		/**
		 * 再生を開始します。
		 * 
		 * @param event タイマイベント
		 */
		private function playLoop( event:TimerEvent = null ):void{
			if( phaseManager.phase < PHASE_END ){
				var schOld:SoundChannel = m_bgmch;
				m_bgmch = m_preset.bgm.play(
					event == null ? 0 : m_preset.loopStart, int.MAX_VALUE );
				m_nStartTime = new Date().time;
				applyMute();
				if( schOld != null ){ schOld.stop(); }
				s_bPlay = true;
				m_bSetTimer = false;
			}
		}
	}
}
