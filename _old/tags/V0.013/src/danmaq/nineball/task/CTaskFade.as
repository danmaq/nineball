package danmaq.nineball.task{
	
	import danmaq.nineball.core.*;
	import danmaq.nineball.misc.CInterpolate;
	
	import flash.display.*;
	import flash.geom.*;

	/**
	 * フェードイン・アウトタスクです。
	 * 現時点では白ベタまたは黒ベタへのフェードイン・アウトのみサポートしています。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskFade implements ITask, IDisposed{

		////////// CONSTANTS //////////

		/**	塗りつぶし用シェイプが格納されます。 */
		private const fill:Shape = new Shape();

		////////// FIELDS //////////

		/**	解放されたかどうかが格納されます。 */
		private var m_bDisposed:Boolean = false;

		/**	簡易カウンタが格納されます。 */
		private var m_uCount:uint = 0;

		/**	レイヤ番号が格納されます。 */
		private var m_uLayer:uint;

		/**	画面番号が格納されます。 */
		private var m_uScreen:uint;

		/**	フェードインかどうかが格納されます。 */
		private var m_bIn:Boolean;

		/**	ホワイトフェードかどうかが格納されます。 */
		private var m_bWhite:Boolean;

		/**	フェードの所要時間が格納されます。 */
		private var m_uLimit:uint;

		////////// PROPERTIES //////////

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
		 * 解放したかどうかを取得します。
		 * 
		 * @return 解放している場合、true
		 */
		public function get disposed():Boolean{ return m_bDisposed; }

		/**
		 * 残り時間を取得します。
		 * 
		 * @return 残り時間
		 */
		public function get amount():uint{ return m_uLimit - m_uCount; }

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param rect フェード対象区域座標
		 * @param bIn フェードインかどうか
		 * @param bWhite ホワイトフェードかどうか
		 * @param uTime フェード時間
		 * @param uScreen 画面番号
		 * @param uLayer レイヤ番号
		 */
		public function CTaskFade(
			rect:Rectangle, bIn:Boolean = true, bWhite:Boolean = false,
			uTime:uint = 50, uScreen:uint = 0, uLayer:uint = 0
		){
			m_bIn = bIn;
			m_bWhite = bWhite;
			m_uLimit = uTime;
			m_uScreen = uScreen;
			m_uLayer = uLayer;
			fill.graphics.beginFill( 0xFFFFFF );
			fill.graphics.drawRect( rect.x, rect.y, rect.width, rect.height );
			fill.blendMode = bWhite ? BlendMode.ADD : BlendMode.SUBTRACT;
			var fDepth:Number = m_bIn ? 1 : 0;
			fill.transform.colorTransform = new ColorTransform( fDepth, fDepth, fDepth );
			CMainLoop.instance.screenList[ uScreen ].add( fill, uLayer );
		}

		/**
		 * コンストラクタの後、タスクが管理クラスに登録された直後に、
		 * 1度だけ自動的に呼ばれます。
		 */
		public function initialize():void{}

		/**
		 * デストラクタ。
		 */
		public function dispose():void{
			CMainLoop.instance.screenList[ m_uScreen ].remove( fill );
			m_bDisposed = true;
		}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return アニメーションが有効な限りtrue
		 */
		public function update():Boolean{
			var fDepth:Number = m_bIn ?
				CInterpolate.accelerate( 1, 0, m_uCount, m_uLimit ) :
				CInterpolate.slowdown( 0, 1, m_uCount, m_uLimit );
			fill.transform.colorTransform = new ColorTransform( fDepth, fDepth, fDepth );
			m_uCount++;
			return ( m_uCount <= m_uLimit );
		}
	}
}
