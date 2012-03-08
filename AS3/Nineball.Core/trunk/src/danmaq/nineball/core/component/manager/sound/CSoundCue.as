package danmaq.nineball.core.component.manager.sound
{
	
	import danmaq.nineball.core.component.IDisposable;
	import danmaq.nineball.core.util.math.clamp;
	
	import flash.events.Event;
	import flash.events.IEventDispatcher;
	import flash.media.SoundChannel;
	import flash.media.SoundTransform;
	import flash.utils.getTimer;

	/**
	 * 音声キュー。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CSoundCue implements IDisposable
	{
		
		//* constants ──────────────────────────────-*
		
		/** 音量調整用のオブジェクト。 */
		private const transform:SoundTransform = new SoundTransform();
		
		//* fields ────────────────────────────────*

		/** 音声シーケンスのリスト。 */
		private var _sequence:Vector.<CSoundSequence>;

		/** 現在再生中の音声。 */
		private var _currentChannel:SoundChannel;
		
		/** 親となる音声管理オブジェクト。 */
		private var _parent:CSoundManager;
		
		/** 再生開始時間。 */
		private var _startTime:int;
		
		/** 現在のインデックス。 */
		private var _index:int;
		
		/** 音量。 */
		private var _volume:Number;
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 
		 * @param sequence 音声シーケンスのリスト。
		 */
		public function CSoundCue(sequence:Vector.<CSoundSequence>)
		{
			_sequence = sequence;
			volume = 1;
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * 音声シーケンスのリストを取得します。
		 */
		public function get sequence():Vector.<CSoundSequence>
		{
			return _sequence;
		}
		
		/**
		 * 現在再生中の音声を取得します。
		 */
		public function get currentChannel():SoundChannel
		{
			return _currentChannel;
		}
		
		/**
		 * 親となる音声管理オブジェクトを取得します。
		 */
		public function get parent():CSoundManager
		{
			return _parent;
		}
		
		/**
		 * 再生開始時間を取得します。
		 */
		public function get startTime():int
		{
			return _startTime;
		}
		
		/**
		 * 現在のインデックスを取得します。
		 */
		public function get index():int
		{
			return _index > 0 ? _index : 0;
		}
		
		/**
		 * 現在有効なシーケンスを取得します。
		 */
		public function get currentSequence():CSoundSequence
		{
			return sequence[index];
		}
		
		/**
		 * 音量を取得します。
		 * 
		 * <p>
		 * 音量は0(無音)から1(最大)までの範囲内の値です。
		 * 範囲外の値が設定された場合、自動的に範囲内に丸められます。
		 * <p>
		 * </p>
		 * 最終的にマスター音量と掛け合わされた値が音量として使用されます。
		 * </p>
		 */
		public function get volume():Number
		{
			return _volume;
		}
		
		/**
		 * @private
		 */
		public function set volume(v:Number):void
		{
			if(_volume != v)
			{
				_volume = clamp(v, 0.0, 1.0);
				refreshVolume();
			}
		}

		//* instance methods ───────────────────────────*
		
		/**
		 * 同一の内容かどうかを取得します。
		 * 
		 * @param expr 対象。
		 * @return 同一である場合、<code>true</code>。
		 */
		public function equals(expr:CSoundCue):Boolean
		{
			var result:Boolean = this == expr || sequence == expr.sequence;
			if(!result)
			{
				var length:int = sequence.length;
				if(result = (length == expr.sequence.length))
				{
					for(var i:int = length; result && --i >= 0; )
					{
						result = sequence[i].equals(expr.sequence[i]);
					}
				}
			}
			return result;
		}
		
		/**
		 * 再生を開始します。
		 * 
		 * @param parent 親となる音声管理オブジェクト。
		 */
		public function play(parent:CSoundManager):void
		{
			_parent = parent;
			innerPlay();
			_startTime = getTimer();
		}
		
		/**
		 * 停止します。
		 */
		public function stop():void
		{
			// TODO : 最後に停止したところから再開
			if(_currentChannel != null)
			{
				_currentChannel.removeEventListener(Event.SOUND_COMPLETE, onSoundComplete);
				_currentChannel.stop();
				_currentChannel = null;
			}
		}
		
		/**
		 * 音量を即時適用します。
		 */
		public function refreshVolume():void
		{
			if(parent != null)
			{
				transform.volume = parent.mute ? 0 : volume * parent.masterVolume;
			}
		}
		
		/**
		 * 明示的にオブジェクトを解放可能な状態にします。
		 */
		public function dispose():void
		{
			stop();
			_index = 0;
			_sequence = null;
			_parent = null;
		}

		/**
		 * 音声再生完了時のイベントによって呼び出されます。
		 * 
		 * @param event イベント情報。
		 */
		private function onSoundComplete(event:Event):void
		{
			if((_index += currentSequence.next) >= 0)
			{
				innerPlay();
				IEventDispatcher(event.currentTarget).removeEventListener(
					Event.SOUND_COMPLETE, onSoundComplete);
			}
			else
			{
				stop();
			}
		}
		
		/**
		 * 再生を開始します。
		 */
		private function innerPlay():void
		{
			refreshVolume();
			var sequence:CSoundSequence = currentSequence;
			var loop:Boolean = sequence.next == 0;
			var channel:SoundChannel = currentSequence.sound.play(
				parent.mp3Delay, loop ? int.MAX_VALUE : 0, transform);
			if(!loop)
			{
				channel.addEventListener(Event.SOUND_COMPLETE, onSoundComplete);
			}
			_currentChannel = channel;
		}
	}
}
