package danmaq.nineball.core.util.math
{
	import danmaq.nineball.core.util.object.blockStatic;
	
	/**
	 * 値を指定の分解能に合わせて値を丸め込む関数をまとめたクラスです。
	 * 
	 * <p>
	 * このクラスは静的クラスです。このクラスのインスタンスを生成することはできません。
	 * </p>
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CQuantize
	{
		
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
