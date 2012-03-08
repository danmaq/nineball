package danmaq.nineball.core.component.manager.sound
{

	import flash.media.Sound;

	/**
	 * 音声シーケンス用クラス。
	 * 
	 * <p>
	 * シーケンスの繋ぎ目におけるフェード処理には対応していません。
	 * フェードを行う場合、別個のシーケンスにしてください。
	 * </p>
	 *
	 * @author Mc(danmaq)
	 */
	public final class CSoundSequence
	{
		
		//* fields ────────────────────────────────*
		
		/** サウンド本体。 */
		private var _sound:Sound;
		
		/** 再生終了後に遷移するインデックス加算値。 */
		private var _next:int;
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 *
		 * @param sound サウンド本体。
		 * @param next 再生終了後に遷移するインデックス加算値。
		 */
		public function CSoundSequence(sound:Sound, next:int = -1)
		{
			_sound = sound;
			_next = next;
		}
		
		//* class methods ────────────────────────────-*
		
		/**
		 * ループ シーケンスを作成します。
		 * 
		 * @param loop ループ再生したい音声オブジェクト。
		 * @param introduction イントロ再生したい音声オブジェクト。
		 */
		public static function createLoop(
			loop:Sound, introduction:Sound = null):Vector.<CSoundSequence>
		{
			var result:Vector.<CSoundSequence> = new Vector.<CSoundSequence>();
			if(introduction != null)
			{
				result.push(new CSoundSequence(introduction, 1));
			}
			result.push(new CSoundSequence(loop, 0));
			return result;
		}

		//* instance properties ─────────────────────────-*
		
		/**
		 * サウンド本体を取得します。
		 */
		public function get sound():Sound
		{
			return _sound;
		}
		
		/**
		 * 再生終了後に遷移するインデックス加算値を取得します。
		 */
		public function get next():int
		{
			return _next;
		}

		//* instance methods ───────────────────────────*
		
		/**
		 * 同一の内容かどうかを取得します。
		 * 
		 * @param expr 対象。
		 * @return 同一である場合、<code>true</code>。
		 */
		public function equals(expr:CSoundSequence):Boolean
		{
			return sound == expr.sound && next == expr.next;
		}
	}
}
