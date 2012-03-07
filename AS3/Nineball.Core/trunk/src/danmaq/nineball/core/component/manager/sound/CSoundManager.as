package danmaq.nineball.core.component.manager.sound
{

	import danmaq.nineball.core.component.IDisposable;
	import danmaq.nineball.core.component.task.ITask;
	import danmaq.nineball.core.events.CDisposableEventDispatcher;
	import danmaq.nineball.core.util.math.clamp;
	
	import flash.events.Event;
	import flash.media.Sound;
	import flash.utils.ByteArray;
	
	/**
	 * 音声再生管理クラス。
	 * 
	 * <p>
	 * 使用するためには、毎フレーム<code>update()</code>メソッドを呼び出してください。
	 * イベント呼び出しのための<code>updateFromEvent()</code>ラッパー メソッドも用意してあります。
	 * </p>
	 *
	 * @author Mc(danmaq)
	 */
	public final class CSoundManager extends CDisposableEventDispatcher implements ITask
	{

		// TODO : SoundSplitterを実装する(要Flash11→Flex)
		// TODO : 同サウンドは排他再生かどうか
		// TODO : 別サウンドは排他再生かどうか
		// TODO : 同サウンド排他再生のQuantize
		// TODO : 別サウンド排他再生のQuantize
		// TODO : フェードアウト時間
		// TODO : フェードイン時間
		// TODO : フェードアウトのイージング
		// TODO : フェードインのイージング
		// TODO : クロスフェード時間
		// TODO : 最後に停止したところから再開
		
		//* constants ──────────────────────────────-*
		
		/** 予約リスト。 */
		private const reserved:Vector.<Vector.<CSoundSequence>> =
			new Vector.<Vector.<CSoundSequence>>();
		
		//* fields ────────────────────────────────*
		
		/** ミュートかどうか。 */
		public var _mute:Boolean;
		
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
		 * マスター ボリュームを取得します。
		 *
		 * @return マスター ボリューム(0～1)。
		 */
		public function get masterVolume():Number
		{
			return _masterVolume;
		}
		
		/**
		 * マスター ボリュームを設定します。
		 *
		 * @param v マスター ボリューム(0～1)。
		 */
		public function set masterVolume(v:Number):void
		{
			_masterVolume = clamp(v, 0.0, 1.0);
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
		 * 1フレーム分の更新処理を実行します。
		 */
		public function update():void
		{
		}
		
		/**
		 * <code>update()</code>メソッドのラッパーです。
		 *
		 * @param evt イベント情報。(無視されます)
		 * @see #update()
		 */
		public function updateFromEvent(evt:Event = null):void
		{
			update();
		}
		
		/**
		 * @inheritDoc
		 */
		override public function dispose():void
		{
			_mute = false;
			_masterVolume = 1;
			super.dispose();
		}
	}
}
