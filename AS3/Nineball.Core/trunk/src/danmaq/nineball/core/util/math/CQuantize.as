package danmaq.nineball.core.util.math
{
	import danmaq.nineball.core.util.object.blockStatic;
	
	/**
	 * 値を指定の分解能に合わせて値を丸め込む関数をまとめたクラスです。
	 * 
	 * <p>
	 * このクラスは静的クラスです。このクラスのインスタンスを生成することはできません。
	 * また、最大で±0.0000000000000005程度の誤差が出ることに注意してください。
	 * </p>
	 * 
	 * @author Mc(danmaq)
	 * @example 下記の例では、自然対数の底eを様々な単位で丸め込み、その動作の検証をします。
	 * <listing version="3.0">
	 * // Math.E == 2.71828182845905
	 * 
	 * CQuantize.ceil(Math.E, 10) // == 10.0±0.0000000000000005
	 * CQuantize.ceil(Math.E, 1) // == 3.0±0.0000000000000005
	 * CQuantize.ceil(Math.E, 0.5) // == 3.0±0.0000000000000005
	 * CQuantize.ceil(Math.E, 0.1) // == 2.8±0.0000000000000005
	 * 
	 * CQuantize.floor(Math.E, 3) // == 0.0±0.0000000000000005
	 * CQuantize.floor(Math.E, 1) // == 2.0±0.0000000000000005
	 * CQuantize.floor(Math.E, 0.5) // == 2.5±0.0000000000000005
	 * CQuantize.floor(Math.E, 0.1) // == 2.7±0.0000000000000005
	 * 
	 * CQuantize.round(Math.E, 5) // == 5.0±0.0000000000000005
	 * CQuantize.round(Math.E, 1) // == 3.0±0.0000000000000005
	 * CQuantize.round(Math.E, 0.5) // == 2.5±0.0000000000000005
	 * CQuantize.round(Math.E, 0.1) // == 2.7±0.0000000000000005
	 * </listing>
	 */
	public final class CQuantize
	{
		
		// TODO : 引数を逆数にして、乗算と除算を入れ替えると誤差が解消できるかもしれないっぽい
		// >>380 
		// 	こうだな 
		// 	var num:Number = 0.1233456; 
		// num = Math.round( num * 1000) / 1000;//小数点第三位まで 
		// num = Math.round( num * 100) / 100;//小数点第二位まで 
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 
		 * <p>
		 * このクラスは静的クラスです。このクラスのインスタンスを生成することはできません。
		 * </p>
		 */
		public function CQuantize()
		{
			blockStatic();
		}
		
		//* class methods ────────────────────────────-*
		
		/**
		 * 値を指定の分解能に合わせて切り捨てます。
		 *
		 * <p>最大で±0.0000000000000005程度の誤差が出ることに注意してください。</p>
		 * 
		 * @param expr 対象値。
		 * @param resolution 丸められる分解能。この数値が高いほど、exprは粗く丸められます。
		 * 既定値は1で、これはMath.floor()とほぼ同等(小数を切り捨てる)の効果を示します。
		 * @return 指定の分解能で丸められた対象値。分解能に0以下を指定した場合、exprを返します。
		 */
		public static function floor(expr:Number, resolution:Number = 1):Number
		{
			return resolution > 0 ? Math.floor(expr / resolution) * resolution : expr;
		}
		
		/**
		 * 値を指定の分解能に合わせて四捨五入ます。
		 *
		 * <p>最大で±0.0000000000000005程度の誤差が出ることに注意してください。</p>
		 *
		 * @param expr 対象値。
		 * @param resolution 丸められる分解能。この数値が高いほど、exprは粗く丸められます。
		 * 既定値は1で、これはMath.floor()とほぼ同等(小数を切り捨てる)の効果を示します。
		 * @return 指定の分解能で丸められた対象値。分解能に0以下を指定した場合、exprを返します。
		 */
		public static function round(expr:Number, resolution:Number = 1):Number
		{
			return resolution > 0 ? Math.round(expr / resolution) * resolution : expr;
		}
		
		/**
		 * 値を指定の分解能に合わせて切り上げます。
		 *
		 * <p>最大で±0.0000000000000005程度の誤差が出ることに注意してください。</p>
		 *
		 * @param expr 対象値。
		 * @param resolution 丸められる分解能。この数値が高いほど、exprは粗く丸められます。
		 * 既定値は1で、これはMath.floor()とほぼ同等(小数を切り捨てる)の効果を示します。
		 * @return 指定の分解能で丸められた対象値。分解能に0以下を指定した場合、exprを返します。
		 */
		public static function ceil(expr:Number, resolution:Number = 1):Number
		{
			return resolution > 0 ? Math.ceil(expr / resolution) * resolution : expr;
		}
	}
}
