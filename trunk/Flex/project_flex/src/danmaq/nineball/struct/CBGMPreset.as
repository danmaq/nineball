package danmaq.nineball.struct{

	import danmaq.nineball.misc.CMisc;	

	import flash.media.Sound;

	import mx.utils.StringUtil;

	/**
	 * BGMプリセット構造体です。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CBGMPreset{

		////////// FIELDS //////////
		
		/**	BGMが格納されます。 */
		private var m_bgm:Sound;

		/**	ループ開始ポイント(ミリ秒)が格納されます。 */
		private var m_uLoopStart:uint;

		/**	ループ終了ポイント(ミリ秒)が格納されます。 */
		private var m_uLoopEnd:uint;

		////////// PROPERTIES //////////

		/**
		 * BGMを取得します。
		 * 
		 * @return BGM
		 */
		public function get bgm():Sound{ return m_bgm; }
		
		/**
		 * ループ開始ポイントを取得します。
		 * 
		 * @return ループ開始ポイント(ミリ秒)
		 */
		public function get loopStart():uint{ return m_uLoopStart; }
		
		/**
		 * ループ終了ポイントを取得します。
		 * 
		 * @return ループ終了ポイント(ミリ秒)
		 */
		public function get loopEnd():uint{ return m_uLoopEnd; }
		
		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 * 
		 * @param _bgm BGM
		 * @param uLoopStart ループ開始ポイント(ミリ秒)
		 * @param uLoopEnd ループ終了ポイント(ミリ秒)
		 */
		public function CBGMPreset( _bgm:Sound, uLoopStart:uint, uLoopEnd:uint ){
			if( uLoopStart > uLoopEnd ){
				uLoopStart ^= uLoopEnd;
				uLoopEnd ^= uLoopStart;
				uLoopStart ^= uLoopEnd;
			}
			m_bgm = _bgm;
			m_uLoopStart = uLoopStart;
			m_uLoopEnd = uLoopEnd;
		}

		/**
		 * このクラスの状態を文字列で取得します。
		 * 
		 * @return オブジェクトのストリング表現
		 */
		public function toString():String{
			return StringUtil.substitute( "BGM:{0},Loop[Start:{1},End:{2}]",
				CMisc.getClassName( bgm ), loopStart, loopEnd );
		}
	}
}
