package danmaq.nineball.data
{

	import mx.utils.StringUtil;

	/**
	 * フェーズ進行・カウンタ進行の管理をするクラス。
	 * 
	 * <p>
	 * 使用するためにはcount++を毎フレーム呼び出してください。
	 * </p>
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CPhase
	{

		////////// FIELDS //////////

		/**	現在のフェーズ値が格納されます。 */		
		private var m_uPhase:uint;

		/**	現在のカウント値が格納されます。 */		
		private var m_uCount:uint;

		/**	現在のフェーズが開始された時のカウント値が格納されます。 */		
		private var m_uPhaseStartTime:uint;

		/**	カウント変化時に進むフェーズが格納されます。 */		
		private var m_nNextPhase:int;

		/**	前回のフェーズ値が格納されます。 */		
		private var m_uPrevPhase:uint;

		////////// PROPERTIES //////////

		/**
		 * 現在のフェーズ値を取得します。
		 * 
		 * @return フェーズ値
		 */
		public function get phase():uint
		{
			return m_uPhase;
		}

		/**
		 * 現在のフェーズ値を設定します。
		 * 
		 * @param value フェーズ値
		 */
		public function set phase(value:uint):void
		{
			m_uPrevPhase = phase;
			m_uPhase = value;
			m_uPhaseStartTime = count;
		}

		/**
		 * 現在のカウント値を取得します。
		 * 
		 * @return カウント値
		 */
		public function get count():uint
		{
			return m_uCount;
		}

		/**
		 * 現在のカウント値を設定します。
		 * 
		 * <p>
		 * 次のフェーズへ行く予約が入っている場合、
		 * ここで次のフェーズへと移行します。
		 * </p>
		 * 
		 * @param value カウント値
		 */
		public function set count(value:uint):void
		{
			m_uCount = value;
			if(isReserveNextPhase){
				phase = nextPhase;
				isReserveNextPhase = false;
			}
		}
		
		/**
		 * 現在のフェーズが開始された時のカウント値を取得します。
		 * 
		 * @return 現在のフェーズが開始された時のカウント値
		 */
		public function get phaseStartTime():uint
		{
			return m_uPhaseStartTime;
		}
		
		/**
		 * 現在のフェーズ内のカウント値を取得します。
		 * 
		 * @return 現在のフェーズ内のカウント値
		 */
		public function get phaseCount():uint{
			return count - phaseStartTime;
		}

		/**
		 * カウント変化時にフェーズを進めるかどうかを取得します。
		 * 
		 * @return カウント変化時にフェーズを進める場合、true
		 */
		public function get isReserveNextPhase():Boolean
		{
			return (nextPhase >= 0);
		}

		/**
		 * カウント変化時にフェーズを進めるかどうかを設定します。
		 * 
		 * @param value カウント変化時にフェーズを進めるかどうか
		 */
		public function set isReserveNextPhase(value:Boolean):void
		{
			nextPhase = (value ? phase + 1 : -1);
		}

		/**
		 * カウント変化時に進むフェーズ値を取得します。
		 * 
		 * @return カウント変化時に進むフェーズ値
		 */
		public function get nextPhase():int
		{
			return m_nNextPhase;
		}

		/**
		 * カウント変化時に進むフェーズ値を設定します。
		 * 
		 * @param value カウント変化時に進むフェーズ値
		 */
		public function set nextPhase(value:int):void
		{
			if(phase != value)
			{
				m_nNextPhase = value;
			}
		}

		/**
		 * 前回のフェーズ値を取得します。
		 * 
		 * @return 前回のフェーズ値
		 */
		public function get prevPhase():uint
		{
			return m_uPrevPhase;
		}

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 * 内部データのリセットを行います。
		 */
		public function CPhase()
		{
			reset();
		}
		
		/**
		 * フェーズやカウンタなど、内部データのリセットを行います。
		 */
		public function reset():void
		{
			m_uCount = 0;
			m_uPhase = 0;
			m_uPrevPhase = 0;
			m_uPhaseStartTime = 0;
			isReserveNextPhase = false;
		}
		
		/**
		 * このクラスの状態を文字列で取得します。
		 * 
		 * @return オブジェクトのストリング表現
		 */
		public function toString():String
		{
			return StringUtil.substitute(
				"Phase[Now:{0},Next:{1},Prev:{2}],Count[Total:{3},Phase:{4}]",
				phase, nextPhase, prevPhase, count, phaseCount);
		}
	}
}
