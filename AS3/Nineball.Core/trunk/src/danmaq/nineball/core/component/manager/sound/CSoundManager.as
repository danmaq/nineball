package danmaq.nineball.core.component.manager.sound
{

	import danmaq.nineball.core.component.IDisposable;
	import danmaq.nineball.core.util.math.clamp;
	
	import flash.media.Sound;
	
	/**
	 * 音声再生管理クラス。
	 *
	 * @author Mc(danmaq)
	 */
	public final class CSoundManager implements IDisposable
	{

		// 優先度 v
		// TODO : 5 同サウンドは排他再生かどうか
		// TODO : 4 別サウンドは排他再生かどうか
		// TODO : 3 同サウンド排他再生のQuantize
		// TODO : 3 別サウンド排他再生のQuantize
		// TODO : 2 フェードアウト時間
		// TODO : 2 フェードイン時間
		// TODO : 2 フェードアウトのイージング
		// TODO : 2 フェードインのイージング
		// TODO : 2 クロスフェード時間
		// TODO : 1 最後に停止したところから再開
		// TODO : 0 SoundSplitterを実装する(要Flash11→Flex)
		
		//* constants ──────────────────────────────-*

		/** 再生リスト。 */
		private const cues:Vector.<CSoundCue> = new Vector.<CSoundCue>();
		
		/** 予約リスト。 */
		private const reserved:Vector.<Vector.<CSoundSequence>> =
			new Vector.<Vector.<CSoundSequence>>();
		
		//* fields ────────────────────────────────*
		
		/** ミュートかどうか。 */
		private var _mute:Boolean;
		
		/**
		 * 音声の先頭無音区間(ミリ秒)。
		 * 
		 * <p>
		 * この値を設定した場合、先頭をスキップして再生します。
		 * 無音区間をスキップしてギャップレス再生するのに有効です。
		 * </p>
		 */
		public var mp3Delay:uint = 25;
		
		/** マスター ボリューム。 */
		private var _masterVolume:Number;

		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 */
		public function CSoundManager()
		{
			masterVolume = 1;
		}

		/**
		 * マスター音量を取得、および設定します。
		 * 
		 * <p>
		 * マスター音量は0(無音)から1(最大)までの範囲内の値です。
		 * </p>
		 */
		public function get masterVolume():Number
		{
			return _masterVolume;
		}
		
		/**
		 * @private
		 */
		public function set masterVolume(v:Number):void
		{
			if(_masterVolume != v)
			{
				_masterVolume = clamp(v, 0.0, 1.0);
				cues.forEach(refleshVolume);
			}
		}
		
		/**
		 * ミュートかどうかを取得、および設定します。
		 */
		public function get mute():Boolean
		{
			return _mute;
		}
		
		/**
		 * @private
		 */
		public function set mute(v:Boolean):void
		{
			if(_mute != v)
			{
				_mute = v;
				cues.forEach(refleshVolume);
			}
		}
		
		//* instance methods ───────────────────────────*

		/**
		 * 再生の予約をします。
		 * 
		 * @param sound 再生したい音声オブジェクト。
		 */
		public function play(sound:Sound):void
		{
			playSequence(Vector.<CSoundSequence>([new CSoundSequence(sound)]));
		}
		
		/**
		 * 再生の予約をします。
		 * 
		 * @param loop ループ再生したい音声オブジェクト。
		 * @param sequence 音声シーケンス定義。
		 */
		public function playSequence(sequence:Vector.<CSoundSequence>):void
		{
			reserved.push(sequence);
		}
		
		/**
		 * 明示的に解放可能な状態にします。
		 */
		public function dispose():void
		{
			mute = false;
			mp3Delay = 25;
			_masterVolume = 1;
			reserved.splice(0, reserved.length);
			cues.forEach(disposeCue);
			cues.splice(0, cues.length);
		}
		
		/**
		 * 各キューの再生を止め、明示的に解放可能な状態にします。
		 * 
		 * @param item キュー本体。
		 * @param index キュー一覧のインデックス。
		 * @param vector キュー一覧。
		 */
		private function disposeCue(item:CSoundCue, index:int, vector:Vector.<CSoundCue>):void
		{
			item.dispose();
		}
		
		/**
		 * 各キューにマスタ音量を反映させます。
		 * 
		 * @param item キュー本体。
		 * @param index キュー一覧のインデックス。
		 * @param vector キュー一覧。
		 */
		private function refleshVolume(item:CSoundCue, index:int, vector:Vector.<CSoundCue>):void
		{
			item.refreshVolume();
		}
	}
}
