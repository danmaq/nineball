package danmaq.nineball.core.util.math
{
	
	import danmaq.nineball.core.util.object.blockStatic;
	
	/**
	 * 双曲線関数系の算術演算関数をまとめた静的クラスです。
	 *
	 * <p>
	 * このクラスは静的クラスです。このクラスのインスタンスを生成することはできません。
	 * </p>
	 *
	 * @author Mc(danmaq)
	 */
	public final class CHyperbolic
	{
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 *
		 * <p>
		 * このクラスは静的クラスです。このクラスのインスタンスを生成することはできません。
		 * </p>
		 */
		public function CHyperbolic()
		{
			blockStatic();
		}
		
		//* class methods ────────────────────────────-*
		
		/**
		 * ハイパーボリック サインを計算します。
		 *
		 * @param radians ラジアン値
		 * @return 対応するハイパーボリック サイン値
		 */
		public static function Hsin(radians:Number):Number
		{
			return (Math.exp(radians) - Math.exp(-radians)) * 0.5;
		}
		
		/**
		 * ハイパーボリック コサインを計算します。
		 *
		 * @param radians ラジアン値
		 * @return 対応するハイパーボリック コサイン値
		 */
		public static function Hcos(radians:Number):Number
		{
			return (Math.exp(radians) + Math.exp(-radians)) * 0.5;
		}
		
		/**
		 * ハイパーボリック タンジェントを計算します。
		 *
		 * @param radians ラジアン値
		 * @return 対応するハイパーボリック タンジェント値
		 */
		public static function Htan(radians:Number):Number
		{
			return (Math.exp(radians) - Math.exp(-radians)) /
				(Math.exp(radians) + Math.exp(-radians));
		}
		
		/**
		 * ハイパーボリック セカントを計算します。
		 *
		 * @param radians ラジアン値
		 * @return 対応するハイパーボリック セカント値
		 */
		public static function Hsec(radians:Number):Number
		{
			return 2 / (Math.exp(radians) +  Math.exp(-radians));
		}
		
		/**
		 * ハイパーボリック コセカントを計算します。
		 *
		 * @param radians ラジアン値
		 * @return 対応するハイパーボリック コセカント値
		 */
		public static function Hcosec(radians:Number):Number
		{
			return 2 / (Math.exp(radians) - Math.exp(-radians));
		}
		
		/**
		 * ハイパーボリック コタンジェントを計算します。
		 *
		 * @param radians ラジアン値
		 * @return 対応するハイパーボリック コタンジェント値
		 */
		public static function Hcotan(radians:Number):Number
		{
			return Math.exp(-radians) / (Math.exp(radians) - Math.exp(-radians)) * 2 + 1;
		}
		
		/**
		 * ハイパーボリック アークサインを計算します。
		 *
		 * @param radians ラジアン値
		 * @return 対応するハイパーボリック アークサイン値
		 */
		public static function Hasin(radians:Number):Number
		{
			return Math.log(radians + (radians ^ 2 + 1) ^ 0.5);
		}
		
		/**
		 * ハイパーボリック アークコサインを計算します。
		 *
		 * @param radians ラジアン値
		 * @return 対応するハイパーボリック アークコサイン値
		 */
		public static function Hacos(radians:Number):Number
		{
			return Math.log(radians + (radians ^ 2 - 1) ^ 0.5);
		}
		
		/**
		 * ハイパーボリック アークタンジェントを計算します。
		 *
		 * @param radians ラジアン値
		 * @return 対応するハイパーボリック アークタンジェント値
		 */
		public static function Hatan(radians:Number):Number
		{
			return Math.log((1 + radians) / (1 - radians)) * 0.5;
		}
		
		/**
		 * ハイパーボリック アークセカントを計算します。
		 *
		 * @param radians ラジアン値
		 * @return 対応するハイパーボリック アークセカント値
		 */
		public static function Hasec(radians:Number):Number
		{
			return Math.log(((-radians * radians + 1) ^ 0.5 + 1) / radians);
		}
		
		/**
		 * ハイパーボリック アークコセカントを計算します。
		 *
		 * @param radians ラジアン値
		 * @return 対応するハイパーボリック アークコセカント値
		 */
		public static function Hacosec(radians:Number):Number
		{
			return Math.log(
				(CMathHelper.sign(radians) * (radians ^ 2 + 1) ^ 0.5 + 1) / radians);
		}
		
		/**
		 * ハイパーボリック アークコタンジェントを計算します。
		 *
		 * @param radians ラジアン値
		 * @return 対応するハイパーボリック アークコタンジェント値
		 */
		public static function Hacotan(radians:Number):Number
		{
			return Math.log((radians + 1) / (radians - 1)) * 0.5;
		}
	}
}
