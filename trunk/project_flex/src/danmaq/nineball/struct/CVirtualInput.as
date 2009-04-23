package danmaq.nineball.struct{

	import flash.geom.*;
	
	import mx.utils.StringUtil;

	/**
	 * 仮想ボタン割り当てのための構造体です。
	 * CTaskVirtualInputタスクで使用します。
	 * 1仮想ボタンにつき1オブジェクトを使用します。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CVirtualInput{

		////////// FIELDS //////////

		/**	押しっぱなしループ時間が格納されます。 */
		public static var keyLoop:uint = 10;

		/**	マウスクリックが割り当てられる領域一覧が格納されます。 */
		private var m_arcMouseArea:Vector.<Rectangle>;

		/**	割り当てられるキーコード一覧が格納されます。 */
		private var m_auKeyCode:Vector.<uint>;

		/**	現在ボタンが押されているかどうかが格納されます。 */
		private var m_bHold:Boolean;
		
		/**	最後にボタンの状態が更新されてからの時間が格納されます。 */
		private var m_uCount:uint;

		////////// PROPERTIES //////////

		/**
		 * マウスクリックが割り当てられる領域一覧を取得します。
		 * 
		 * @return マウスクリックが割り当てられる領域一覧
		 */
		public function get assignMouseAreaList():Vector.<Rectangle>{ return m_arcMouseArea; }

		/**
		 * 割り当てられるキーコード一覧を取得します。
		 * 
		 * @return 割り当てられるキーコード一覧
		 */
		public function get assignKeyCodeList():Vector.<uint>{ return m_auKeyCode; }

		/**
		 * 現在ボタンが押されているかどうかを取得します。
		 * 
		 * @return 現在ボタンが押されている場合、true
		 */
		public function get hold():Boolean{ return m_bHold; }

		/**
		 * 最後にボタンの状態が更新されてからの時間を取得します。
		 * 
		 * @return 最後にボタンの状態が更新されてからのフレーム時間
		 */
		public function get count():uint{ return m_uCount; }

		/**
		 * 最後にボタンの状態が更新されてからの時間を取得します。
		 * 押しっぱなしループ対応型です。
		 * 
		 * @return 最後にボタンの状態が更新されてからのフレーム時間
		 */
		public function get countLoop():uint{ return m_uCount % keyLoop; }

		/**
		 * 現在のフレームでボタンが押されたかどうかを取得します。
		 * 
		 * @return 現在のフレームでボタンが押された場合、true
		 */
		public function get push():Boolean{ return ( hold && count == 0 ); }

		/**
		 * 現在のフレームでボタンが離されたかどうかを取得します。
		 * 
		 * @return 現在のフレームでボタンが離された場合、true
		 */
		public function get pull():Boolean{ return ( !hold && count == 0 ); }

		/**
		 * 現在のフレームでボタンが押されたかどうかを取得します。
		 * 押しっぱなしループ対応型です。
		 * 
		 * @return 現在のフレームでボタンが押された場合、true
		 */
		public function get pushLoop():Boolean{ return ( hold && countLoop == 0 ); }

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 */
		public function CVirtualInput(){ reset(); }

		/**
		 * データを初期状態に戻します。
		 */
		public function reset():void{
			m_arcMouseArea = new Vector.<Rectangle>();
			m_auKeyCode = new Vector.<uint>();
			m_bHold = false;
			m_uCount = 0;
		}
		
		/**
		 * ボタン状態の更新をします。
		 * キー入力管理クラスより毎フレーム呼び出されます。
		 * 
		 * @param bState 最新のボタン状態
		 */
		public function update( bState:Boolean ):void{
			if( m_bHold == bState ){ m_uCount++; }
			else{
				m_bHold = bState;
				m_uCount = 0;
			}
		}
		
		/**
		 * 指定したキーコードが割り当てられているかどうかを取得します。
		 * 
		 * @param uKeyCode キーコード
		 * @return 割り当てられている場合、true
		 */
		public function findKey( uKeyCode:uint ):Boolean{
			var f:Function = function( item:uint, index:int, vector:Vector.<uint> ):Boolean{
				return item == uKeyCode;
			};
			return assignKeyCodeList.some( f );
		}

		/**
		 * 指定した座標が割り当てエリアに入っているかどうかを取得します。
		 * 
		 * @param pos 座標
		 * @return 割り当てエリアに入っている場合、true
		 */
		public function isHitArea( pos:Point ):Boolean{
			var f:Function = function( item:Rectangle, index:int, vector:Vector.<Rectangle> ):Boolean{
				return item.containsPoint( pos );
			};
			return assignMouseAreaList.some( f );
		}

		/**
		 * このクラスの状態を文字列で取得します。
		 * 
		 * @return オブジェクトのストリング表現
		 */
		public function toString():String{
			var strStat:String;
			var strStatLoop:String;
			if( hold ){
				strStat = push ? "PUSH" : "HOLD";
				strStatLoop = pushLoop ? "PUSH" : "HOLD";
			}
			else{
				strStat = pull ? "PULL" : "FREE";
				strStatLoop = strStat;
			}
			return StringUtil.substitute(
				"Stat:{0},Count:{1},Loop[Stat:{2},Count:{3}]",
				strStat, count, strStatLoop, countLoop );
		}
	}
}
