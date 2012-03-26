package danmaq.nineball.core.util.math
{
	
	import danmaq.nineball.core.util.object.blockStatic;

	/**
	 * 三角関数系の算術演算関数をまとめた静的クラスです。
	 *
	 * <p>
	 * このクラスは静的クラスです。このクラスのインスタンスを生成することはできません。
	 * </p>
	 *
	 * @author Mc(danmaq)
	 */
	public final class CTrignometric
	{
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 *
		 * <p>
		 * このクラスは静的クラスです。このクラスのインスタンスを生成することはできません。
		 * </p>
		 */
		public function CTrignometric()
		{
			blockStatic();
		}
		
		//* class methods ────────────────────────────-*
		
		/**
		 * ラジアンを角度に変換します。
		 *
		 * @param radians ラジアン値。
		 * @return ラジアン値に対応した角度。
		 */
		public static function toDegrees(radians:Number):Number
		{
			return radians * 180 / Math.PI;
		}
		
		/**
		 * 角度をラジアンに変換します。
		 *
		 * @param degrees 角度。
		 * @return 角度に対応したラジアン値。
		 */
		public static function toRadians(degrees:Number):Number
		{
			return degrees / 180 * Math.PI;
		}

		/**
		 * 指数を計算します。
		 *
		 * @param n 底数。
		 * @param expr 対象値。
		 * @return 指定した底数を底とした対象値の対数。
		 */
		public static function LogN(n:Number, expr:Number):Number
		{
			return Math.log(expr) / Math.log(n);
		}
		
		/**
		 * セカントを計算します。
		 *
		 * @param radians ラジアン値。
		 * @return 対応するセカント値。
		 */
		public static function sec(radians:Number):Number
		{
			return 1 / Math.cos(radians);
		}
		
		/**
		 * コセカントを計算します。
		 *
		 * @param radians ラジアン値。
		 * @return 対応するコセカント値。
		 */
		public static function cosec(radians:Number):Number
		{
			return 1 / Math.sin(radians);
		}
		
		/**
		 * コタンジェントを計算します。
		 *
		 * @param radians ラジアン値。
		 * @return 対応するコタンジェント値。
		 */
		public static function cotan(radians:Number):Number
		{
			return 1 / Math.tan(radians);
		}
		
		/**
		 * アークセカントを計算します。
		 *
		 * @param radians ラジアン値。
		 * @return 対応するアークセカント値。
		 */
		public static function asec(radians:Number):Number
		{
			return (Math.PI * 0.5) * 
				Math.atan((radians ^ 2 - 1) ^ 0.5) + (CMathHelper.sign(radians) - 1);
		}
		
		/**
		 * アークコセカントを計算します。
		 *
		 * @param radians ラジアン値。
		 * @return 対応するアークコセカント値。
		 */
		public static function acosec(radians:Number):Number
		{
			return (Math.PI * 0.5) * 
				Math.atan(1 / (radians ^ 2 - 1) ^ 0.5) + (CMathHelper.sign(radians) - 1);
		}
		
		/**
		 * アークコタンジェントを計算します。
		 *
		 * @param radians ラジアン値。
		 * @return 対応するアークコタンジェント値。
		 */
		public static function acotan(radians:Number):Number
		{
			return -Math.atan(radians) + Math.PI * 0.5;
		}
	}
}
