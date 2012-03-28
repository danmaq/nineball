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
	 * <p>
	 * 内分カウンタ機能は0.2.1.135より、従来のLuna(http://www.twin-tail.jp/)ベースから、
	 * Tweener(http://code.google.com/p/tweener/)ベースへと変更します。
	 * なるべく従来と互換性を残すつもりですが、数値レベルの完全な互換性は失われます。
	 * </p>
	 *
	 * <pre> 
	 * Disclaimer for Robert Penner's Easing Equations license:
	 * 
	 * TERMS OF USE - EASING EQUATIONS
	 * 
	 * Open source under the BSD License.
	 * 
	 * Copyright © 2001 Robert Penner
	 * All rights reserved.
	 * 
	 * Redistribution and use in source and binary forms, with or without modification,
	 * are permitted provided that the following conditions are met:
	 * 
	 *     * Redistributions of source code must retain the above copyright notice,
	 *       this list of conditions and the following disclaimer.
	 *     * Redistributions in binary form must reproduce the above copyright notice,
	 *       this list of conditions and the following disclaimer in the documentation
	 *       and/or other materials provided with the distribution.
	 *     * Neither the name of the author nor the names of contributors may be used to
	 *       endorse or promote products derived from this software without specific
	 *       prior written permission.
	 * 
	 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
	 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
	 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
	 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
	 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
	 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
	 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
	 * POSSIBILITY OF SUCH DAMAGE.
	 * </pre>
	 *
	 * @author Mc(danmaq)
	 */
	public final class CInterpolate
	{
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 *
		 * <p>
		 * このクラスは静的クラスです。このクラスのインスタンスを生成することはできません。
		 * </p>
		 */
		public function CInterpolate()
		{
			blockStatic();
		}
		
		//* class methods ────────────────────────────-*

		/**
		 * 直線形の重み計算をします。
		 *
		 * @param numerator 分子。
		 * @param denominator 分母。
		 * @return <code>numerator</code>～<code>denominator</code>に対応する、0.0～1.0の値。
		 */
		public static function amountLinear(numerator:Number, denominator:Number):Number
		{
			return validateParams(numerator, denominator) ? numerator / denominator : 0;
		}
		
		/**
		 * 曲線形の重み計算をします。
		 *
		 * @param numerator 分子。
		 * @param denominator 分母。
		 * @return <code>numerator</code>～<code>denominator</code>に対応する、0.0～1.0の値。
		 */
		public static function amountInQuad(numerator:Number, denominator:Number):Number
		{
			return validateParams(numerator, denominator) ?
				(numerator /= denominator) * numerator : 0;
		}

		/**
		 * 曲線形の重み計算をします。
		 *
		 * @param numerator 分子。
		 * @param denominator 分母。
		 * @return <code>numerator</code>～<code>denominator</code>に対応する、0.0～1.0の値。
		 */
		public static function amountOutQuad(numerator:Number, denominator:Number):Number
		{
			return validateParams(numerator, denominator) ?
				-(numerator /= denominator) * (numerator - 2) : 0;
		}
		
		/**
		 * 曲線形の重み計算をします。
		 *
		 * @param numerator 分子。
		 * @param denominator 分母。
		 * @return <code>numerator</code>～<code>denominator</code>に対応する、0.0～1.0の値。
		 */
		public static function amountInCubic(numerator:Number, denominator:Number):Number
		{
			return validateParams(numerator, denominator) ?
				(numerator /= denominator) * numerator * numerator : 0;
		}
		
		/**
		 * 曲線形の重み計算をします。
		 *
		 * @param numerator 分子。
		 * @param denominator 分母。
		 * @return <code>numerator</code>～<code>denominator</code>に対応する、0.0～1.0の値。
		 */
		public static function amountOutCubic(numerator:Number, denominator:Number):Number
		{
			return validateParams(numerator, denominator) ?
				(numerator = numerator / denominator - 1) * numerator * numerator + 1 : 0;
		}
		
		/**
		 * 曲線形の重み計算をします。
		 *
		 * @param numerator 分子。
		 * @param denominator 分母。
		 * @return <code>numerator</code>～<code>denominator</code>に対応する、0.0～1.0の値。
		 */
		public static function amountInQuart(numerator:Number, denominator:Number):Number
		{
			return validateParams(numerator, denominator) ?
				(numerator /= denominator) * numerator * numerator * numerator : 0;
		}
		
		/**
		 * 曲線形の重み計算をします。
		 *
		 * @param numerator 分子。
		 * @param denominator 分母。
		 * @return <code>numerator</code>～<code>denominator</code>に対応する、0.0～1.0の値。
		 */
		public static function amountOutQuart(numerator:Number, denominator:Number):Number
		{
			return validateParams(numerator, denominator) ?
				-((numerator = numerator / denominator - 1) * numerator * numerator * numerator - 1) :
				0;
		}
		
		/**
		 * 曲線形の重み計算をします。
		 *
		 * @param numerator 分子。
		 * @param denominator 分母。
		 * @return <code>numerator</code>～<code>denominator</code>に対応する、0.0～1.0の値。
		 */
		public static function amountInQuint(numerator:Number, denominator:Number):Number
		{
			return validateParams(numerator, denominator) ?
				(numerator /= denominator) * numerator * numerator * numerator * numerator : 0;
		}
		
		/**
		 * 曲線形の重み計算をします。
		 *
		 * @param numerator 分子。
		 * @param denominator 分母。
		 * @return <code>numerator</code>～<code>denominator</code>に対応する、0.0～1.0の値。
		 */
		public static function amountOutQuint(numerator:Number, denominator:Number):Number
		{
			return validateParams(numerator, denominator) ?
				(numerator = numerator / denominator - 1) *
					numerator * numerator * numerator * numerator + 1 : 0;
		}
		
		/**
		 * 曲線形の重み計算をします。
		 *
		 * @param numerator 分子。
		 * @param denominator 分母。
		 * @return <code>numerator</code>～<code>denominator</code>に対応する、0.0～1.0の値。
		 */
		public static function amountOutSin(numerator:Number, denominator:Number):Number
		{
			return validateParams(numerator, denominator) ?
				Math.sin(numerator / denominator * Math.PI * 0.5) : 0;
		}
		
		/**
		 * 2つの数値間で線形補完の計算をします。
		 *
		 * @param expr1 対象値1。
		 * @param expr2 対象値2。
		 * @param amount 重み。
		 * @return amountの0～1に対応する、expr1～expr2の値。
		 */
		public static function lerp(expr1:Number, expr2:Number, amount:Number):Number
		{
			return expr1 + (expr2 - expr1) * amount;
		}

		/**
		 * 線形補完の直線変換をします。
		 *
		 * @param expr1 対象値1。
		 * @param expr2 対象値2。
		 * @param numerator 重みを決定する分子。
		 * @param denominator 重みを決定する分母。
		 * @return numeratorの0～denominatorに対応する、expr1～expr2の値。
		 */
		public static function lerpLinear(
			expr1:Number, expr2:Number, numerator:Number, denominator:Number):Number
		{
			return lerp(expr1, expr2, amountLinear(numerator, denominator));
		}
		
		/**
		 * 引数の検証を行います。
		 *
		 * @param numerator 分子。
		 * @param denominator 分母。
		 * @return 引数が正当な値である場合、<code>true</code>。
		 */
		private static function validateParams(numerator:Number, denominator:Number):Boolean
		{
			return !(denominator == 0 || isNaN(numerator) || isNaN(denominator));
		}
	}
}
