package danmaq.nineball.core.util.math
{

	import danmaq.nineball.core.util.object.blockStatic;
	
	/**
	 * 算術演算関数をまとめた静的クラスです。
	 *
	 * <p>
	 * このクラスは静的クラスです。このクラスのインスタンスを生成することはできません。
	 * </p>
	 *
	 * @author Mc(danmaq)
	 */
	public final class CMathHelper
	{

		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 *
		 * <p>
		 * このクラスは静的クラスです。このクラスのインスタンスを生成することはできません。
		 * </p>
		 */
		public function CMathHelper()
		{
			blockStatic();
		}

		//* class methods ────────────────────────────-*
		
		/**
		 * 値を範囲内に丸めます。
		 *
		 * <p>
		 * <code>limit1</code>に最小値、<code>limit2</code>に最大値を設定することを強く推奨します。
		 * 逆にしても内部で自動認識して置換しますが、その分の余計な負荷がかかってしまいます。
		 * </p>
		 *
		 * @param expr 対象値。
		 * @param limit1 限界値。
		 * @param limit2 限界値。
		 * @return 丸められた値。
		 */
		public static function clamp(expr:Number, limit1:Number, limit2:Number):Number
		{
			if (limit1 > limit2)
			{
				var tmp:Number = limit1;
				limit1 = limit2;
				limit2 = tmp;
			}
			if(limit1 > expr)
			{
				expr = limit1;
			}
			else if(limit2 < expr)
			{
				expr = limit2;
			}
			return expr;
		}
		
		/**
		 * 値が範囲内に含まれているかどうかを取得します。
		 *
		 * <p>
		 * <code>limit1</code>に最小値、<code>limit2</code>に最大値を設定することを強く推奨します。
		 * 逆にしても内部で自動認識して置換しますが、その分の余計な負荷がかかってしまいます。
		 * </p>
		 *
		 * @param expr 対象値。
		 * @param limit1 限界値。
		 * @param limit2 限界値。
		 * @return 値が範囲内に含まれている場合、true。
		 */
		public static function contains(expr:Number, limit1:Number, limit2:Number):Boolean
		{
			if (limit1 > limit2)
			{
				var tmp:Number = limit1;
				limit1 = limit2;
				limit2 = tmp;
			}
			return expr >= limit1 && expr <= limit2;
		}
		
		/**
		 * 対象値を最大32個のビット配列とみなし、1となっているビット数を計算します。
		 *
		 * <p>
		 * 例えば10進法の数値204は、2進法表記の場合11001100となり、
		 * 1となっているビット数は4個のため、この関数の戻り値は4となります。
		 * </p>
		 *
		 * @param expr 対象値。
		 * @return 1となっているビット数(0～32)。
		 */
		public static function onBit(expr:uint):uint
		{
			expr = (expr & 0x55555555) + (expr >> 1 & 0x55555555);
			expr = (expr & 0x33333333) + (expr >> 2 & 0x33333333);
			expr = (expr & 0x0f0f0f0f) + (expr >> 4 & 0x0f0f0f0f);
			expr = (expr & 0x00ff00ff) + (expr >> 8 & 0x00ff00ff);
			return (expr & 0x0000ffff) + (expr >> 16 & 0x0000ffff);
		}
		
		/**
		 * 数値の符号を取得します。
		 *
		 * @param expr 対象の値。
		 * @return exprが正数である場合、1。負数である場合、-1。NaN含めいずれにも該当しない場合、0。
		 */
		public static function sign(expr:Number):int
		{
			var result:int = 0;
			if (!(isNaN(expr) || expr == 0))
			{
				result = expr > 0 ? 1 : -1;
			}
			return result;
		}
		
		/**
		 * 値を指定された範囲内に制限し、超過した場合は範囲内でループします。
		 *
		 * <p>
		 * 注意：最小値と最大値を逆さに設定しても内部で自動的に認識・交換しますが、
		 * 無駄なオーバーヘッドが増えるだけなので極力避けてください。
		 * </p>
		 *
		 * @param expr 対象の値。
		 * @param min 制限値1(最小)。
		 * @param max 制限値1(最大)。
		 * @param allowMin 制限値1(最小)と同値の状態を許容するかどうか。
		 * @param allowMax 制限値2(最大)と同値の状態を許容するかどうか。
		 * @return 制限された値。
		 */
		public static function wrap(expr:Number, min:Number, max:Number,
							 allowMin:Boolean = true, allowMax:Boolean = true):Number
		{
			var result:Number = min;
			if (min != max)
			{
				if (min > max)
				{
					var tmp:Number = max;
					max = min;
					min = tmp;
				}
				var cmpMore:Function;
				var cmpLess:Function;
				cmpMore = allowMax ? wrapMore : wrapEqMore;
				cmpLess = allowMin ? wrapLess : wrapEqLess;
				var moreMax:Boolean = false;
				var lessMin:Boolean = false;
				do
				{
					// TODO : ちょっと作りがダサイ。それに値によっては無限ループになり得る。
					moreMax = cmpMore(expr, max);
					lessMin = cmpLess(expr, min);
					if (moreMax)
					{
						expr = min + expr - max - 1;
					}
					if (lessMin)
					{
						expr = max - Math.abs(expr - min) + 1;
					}
				}
				while (moreMax || lessMin);
				result = clamp(expr, min, max);
			}
			return result;
		}

		/**
		 * 値が閾値未満かどうかを取得します。
		 *
		 * @param expr 対象の値。
		 * @param threshold 閾値。
		 * @return 値が閾値未満である場合、true。
		 */
		private static function wrapLess(expr:Number, threshold:Number):Boolean
		{
			return expr < threshold;
		}
		
		/**
		 * 値が閾値以下かどうかを取得します。
		 *
		 * @param expr 対象の値。
		 * @param threshold 閾値。
		 * @return 値が閾値以下である場合、true。
		 */
		private static function wrapEqLess(expr:Number, threshold:Number):Boolean
		{
			return expr <= threshold;
		}
		
		/**
		 * 値が閾値を超過しているかどうかを取得します。
		 *
		 * @param expr 対象の値。
		 * @param threshold 閾値。
		 * @return 値が閾値を超過している場合、true。
		 */
		private static function wrapMore(expr:Number, threshold:Number):Boolean
		{
			return expr > threshold;
		}
		
		/**
		 * 値が閾値以上かどうかを取得します。
		 *
		 * @param expr 対象の値。
		 * @param threshold 閾値。
		 * @return 値が閾値以下である場合、true。
		 */
		private static function wrapEqMore(expr:Number, threshold:Number):Boolean
		{
			return expr >= threshold;
		}
	}
}
