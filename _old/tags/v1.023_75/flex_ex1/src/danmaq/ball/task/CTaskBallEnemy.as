package danmaq.ball.task{

	import danmaq.nineball.misc.CInterpolate;

	/**
	 * 敵機玉タスクです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskBallEnemy extends CTaskBall{

		////////// FIELDS //////////

		/**	難易度が格納されます。 */
		private var m_uLevel:uint;

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 */
		public function CTaskBallEnemy( uLevel:uint ){
			super( 0xC00000 );
			m_uLevel = uLevel;
			y = 260;
		}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return ゴールにたどり着くまでの間、true
		 */
		public override function update():Boolean{
			var uCount:uint = phaseManager.count;
			switch( m_uLevel ){
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
				if( uCount % uint( CInterpolate.slowdown( 40, 6, m_uLevel, 6 ) ) == 0 ){ move(); }
				break;
			case 7:
				if( uCount > 200 ){ move(); }
				break;
			case 8:
				if( uCount > 30 || ( uCount & 1 ) == 0 ){ move(); }
				break;
			}
			return super.update();
		}
	}
}
