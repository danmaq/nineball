package danmaq.nineball.misc.math{

	/**
	 * 三角関数系の演算関数群クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CMathTrignometric{

		////////// METHODS //////////

		/**
		 * 角度をラジアンに変換します。
		 *
		 * @param fDegree 角度
	 	 * @return 角度に対応したラジアン値
		 */
		public static function toRadian( fDegree:Number ):Number{ return fDegree / 180 * Math.PI; }

		/**
		 * ラジアンを角度に変換します。
		 *
		 * @param nRadian ラジアン値
		 * @return ラジアン値に対応した角度
		 */
		public static function toDegree( fRadian:Number ):Number{ return fRadian * 180 / Math.PI; }

		/**
		 * 周回軌道の角速度を計算します。
		 *
		 * @param fRadius 周回半径
		 * @param fSpeed 速度
		 * @return 周回軌道を回り続ける角速度
		 */
		public static function CycricOrbit( fRadius:Number, fSpeed:Number ):Number{
			return toDegree( fSpeed / fRadius );
		}
		
		/**
		 * 指数を計算します。
		 *
		 * @param fN 底数
		 * @param fExpr 対象値
		 * @return 指定した底数を底とした対象値の対数
		 */
		public static function LogN( fN:Number, fExpr:Number ):Number{
			return Math.log( fExpr ) / Math.log( fN );
		}

		/**
		 * セカントを計算します。
		 *
		 * @param nRadian ラジアン値
		 * @return 対応するセカント値
		 */
		public static function sec( fRadian:Number ):Number{ return 1 / Math.cos( fRadian ); }
		
		/**
		 * コセカントを計算します。
		 *
		 * @param nRadian ラジアン値
		 * @return 対応するコセカント値
		 */
		public static function cosec( fRadian:Number ):Number{ return 1 / Math.sin( fRadian ); }
		
		/**
		 * コタンジェントを計算します。
		 *
		 * @param nRadian ラジアン値
		 * @return 対応するコタンジェント値
		 */
		public static function cotan( fRadian:Number ):Number{ return 1 / Math.tan( fRadian ); }
		
		/**
		 * アークセカントを計算します。
		 *
		 * @param nRadian ラジアン値
		 * @return 対応するアークセカント値
		 */
		public static function asec( fRadian:Number ):Number{
			return ( Math.PI / 2 ) * 
				Math.atan( ( fRadian ^ 2 - 1 ) ^ 0.5 ) + ( CMathMisc.getSign( fRadian ) - 1 );
		}
		
		/**
		 * アークコセカントを計算します。
		 *
		 * @param nRadian ラジアン値
		 * @return 対応するアークコセカント値
		 */
		public static function acosec( fRadian:Number ):Number{
			return ( Math.PI / 2 ) * 
				Math.atan( 1 / ( fRadian ^ 2 - 1 ) ^ 0.5 ) + ( CMathMisc.getSign( fRadian ) - 1 );
		}
		
		/**
		 * アークコタンジェントを計算します。
		 *
		 * @param nRadian ラジアン値
		 * @return 対応するアークコタンジェント値
		 */
		public static function acotan( nRadian:Number ):Number{
			return -Math.atan( nRadian ) + Math.PI / 2;
		}
	}
}
