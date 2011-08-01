package danmaq.nineball.misc.math{

	/**
	 * 双曲線関数系の演算関数群クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CMathHyperbolic{

		////////// METHODS //////////

		/**
		 * ハイパーボリック サインを計算します。
		 *
		 * @param fRadian ラジアン値
		 * @return 対応するハイパーボリック サイン値
		 */
		public static function Hsin( fRadian:Number ):Number{
			return ( Math.exp( fRadian ) - Math.exp( -fRadian ) ) / 2;
		}
		
		/**
		 * ハイパーボリック コサインを計算します。
		 *
		 * @param fRadian ラジアン値
		 * @return 対応するハイパーボリック コサイン値
		*/
		public static function Hcos( fRadian:Number ):Number{
			return ( Math.exp( fRadian ) + Math.exp( -fRadian ) ) / 2;
		}
		
		/**
		 * ハイパーボリック タンジェントを計算します。
		 *
		 * @param fRadian ラジアン値
		 * @return 対応するハイパーボリック タンジェント値
		 */
		public static function Htan( fRadian:Number ):Number{
			return ( Math.exp( fRadian ) - Math.exp( -fRadian ) ) /
				( Math.exp( fRadian ) + Math.exp( -fRadian ) );
		}
		
		/**
		 * ハイパーボリック セカントを計算します。
		 *
		 * @param fRadian ラジアン値
		 * @return 対応するハイパーボリック セカント値
		 */
		public static function Hsec( fRadian:Number ):Number{
			return 2 / ( Math.exp( fRadian ) +  Math.exp( -fRadian ) );
		}
		
		/**
		 * ハイパーボリック コセカントを計算します。
		 *
		 * @param fRadian ラジアン値
		 * @return 対応するハイパーボリック コセカント値
		 */
		public static function Hcosec( fRadian:Number ):Number{
			return 2 / ( Math.exp( fRadian ) - Math.exp( -fRadian ) );
		}
		
		/**
		 * ハイパーボリック コタンジェントを計算します。
		 *
		 * @param fRadian ラジアン値
		 * @return 対応するハイパーボリック コタンジェント値
		 */
		public static function Hcotan( fRadian:Number ):Number{
			return Math.exp( -fRadian ) / ( Math.exp( fRadian ) - Math.exp( -fRadian ) ) * 2 + 1;
		}
		
		/**
		 * ハイパーボリック アークサインを計算します。
		 *
		 * @param fRadian ラジアン値
		 * @return 対応するハイパーボリック アークサイン値
		 */
		public static function Hasin( fRadian:Number ):Number{
			return Math.log( fRadian + ( fRadian ^ 2 + 1 ) ^ 0.5 );
		}
		
		/**
		 * ハイパーボリック アークコサインを計算します。
		 *
		 * @param fRadian ラジアン値
		 * @return 対応するハイパーボリック アークコサイン値
		 */
		public static function Hacos( fRadian:Number ):Number{
			return Math.log( fRadian + ( fRadian ^ 2 - 1 ) ^ 0.5 );
		}
		
		/**
		 * ハイパーボリック アークタンジェントを計算します。
		 *
		 * @param fRadian ラジアン値
		 * @return 対応するハイパーボリック アークタンジェント値
		 */
		public static function Hatan( fRadian:Number ):Number{
			return Math.log( ( 1 + fRadian ) / ( 1 - fRadian ) ) / 2;
		}
		
		/**
		 * ハイパーボリック アークセカントを計算します。
		 *
		 * @param fRadian ラジアン値
		 * @return 対応するハイパーボリック アークセカント値
		 */
		public static function Hasec( fRadian:Number ):Number{
			return Math.log( ( ( -fRadian * fRadian + 1 ) ^ 0.5 + 1 ) / fRadian );
		}
		
		/**
		 * ハイパーボリック アークコセカントを計算します。
		 *
		 * @param fRadian ラジアン値
		 * @return 対応するハイパーボリック アークコセカント値
		 */
		public static function Hacosec( fRadian:Number ):Number{
			return Math.log(
				( CMathMisc.getSign( fRadian ) * ( fRadian ^ 2 + 1 ) ^ 0.5 + 1 ) / fRadian );
		}
		
		/**
		 * ハイパーボリック アークコタンジェントを計算します。
		 *
		 * @param fRadian ラジアン値
		 * @return 対応するハイパーボリック アークコタンジェント値
		 */
		public static function Hacotan( fRadian:Number ):Number{
			return Math.log( ( fRadian + 1 ) / ( fRadian - 1 ) ) / 2;
		}
	}
}
