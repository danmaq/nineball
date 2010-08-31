////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.misc.math
{

	/**
	 * 汎用的な演算関数群クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CMisc
	{

		////////// METHODS //////////

		/**
		 * 値が奇数かどうかを取得します。
		 * 
		 * @param nExpr 対象の値
		 * @return 値が奇数である場合、true
		 */
		public static function isOdd(nExpr:int):Boolean
		{
			return (nExpr & 1) == 1;
		}

		/**
		* 数値符号を返します。
		*
		* @param fExpr 対象値
		* @return 負数の場合-1、正の整数の場合1、0の場合0
		*/
		public static function getSign(fExpr:Number):int
		{
			return fExpr > 0 ? 1 : (fExpr < 0 ? -1 : 0);
		}

		/**
		* 対象値の符号を他方に付けて返します。
		*
		* @param fDst 対象値1(変更される値)
		* @param fSrc 対象値2
		* @return 対象値2の符号が付けられた対象値1
		*/
		public static function copySign(fDst:Number, fSrc:Number):Number
		{
			var nSignDst:int = getSign(fDst);
			return fDst * ((getSign(fSrc) < 0 ? nSignDst < 0 : nSignDst >= 0) ? 1 : -1);
		}

		/**
		* 二値の誤差が一定以内かどうかを判定します。
		*
		* @param fExpr1 対象値1
		* @param fExpr2 対象値2
		* @param fLength 誤差
		* @return 二つの対象値が誤差の範囲内ならtrue
		*/
		public static function isNear(fExpr1:Number, fExpr2:Number, fLength:Number):Boolean
		{
			return Math.abs(fExpr1 - fExpr2) < fLength;
		}

		/**
		* 値を特定単位で切り捨てます。
		*
		* @param fExpr 対象値
		* @param fUnit 切り捨てる単位数値
		* @return 指定単位で切り捨てられた対象値
		*/
		public static function truncate(fExpr:Number, fUnit:Number = 0):Number
		{
			return int(fExpr / fUnit) * fUnit;
		}
		
		/**
		 * 指定桁以上を切り捨てます。
		 *
		 * @param fExpr 対象値
		 * @param nGrade 切り捨てる桁(負数で小数位も指定出来ます)
		 * @return 指定桁以上を切り捨てられた対象値
		 */
		public static function truncateOverhead(fExpr:Number, nGrade:int):Number
		{
			var fUnit:Number = 10 ^ int(nGrade - 1);
			return fExpr - int(fExpr / fUnit) * fUnit;
		}
		
		/**
		 * 値を指定された範囲内に制限します。
		 * 最小値と最大値を逆さに設定しても内部で自動的に認識・交換しますが、
		 * 無駄なオーバーヘッドが増えるだけなので極力避けてください。
		 * 
		 * @param fExpr 対象の値
		 * @param fMin 制限値(最小) 
		 * @param fMax 制限値(最大)
		 * @return 制限された値
		 */
		public static function clamp(fExpr:Number, fMin:Number, fMax:Number):Number
		{
			if(fMin == fMax)
			{
				return fMin;
			}
			else if(fMin > fMax)
			{
				fMin ^= fMax;
				fMax ^= fMin;
				fMin ^= fMax;
			}
			return Math.min(fMax, Math.max(fMin, fExpr));
		}

		/**
		 * 値を指定された範囲内に制限します。
		 * 最小値と最大値を逆さに設定しても内部で自動的に認識・交換しますが、
		 * 無駄なオーバーヘッドが増えるだけなので極力避けてください。
		 * 
		 * @param fExpr 対象の値
		 * @param fMin 制限値(最小) 
		 * @param fMax 制限値(最大)
		 * @return 制限された値
		 */
		public static function clampLoop(fExpr:Number, fMin:Number, fMax:Number):Number{
			if(fMin == fMax)
			{
				return fMin;
			}
			else if(fMin > fMax)
			{
				fMin ^= fMax;
				fMax ^= fMin;
				fMin ^= fMax;
			}
			while(fExpr >= fMax || fExpr < fMin)
			{
				if(fExpr >= fMax)
				{
					fExpr = fMin + fExpr - fMax;
				}
				if(fExpr < fMin){
					fExpr = fMax - Math.abs(fExpr - fMin);
				}
			}
			return Math.min(fMax, Math.max(fMin, fExpr));
		}
		
		/**
		 * 乱数によって誤差を発生させます。
		 *
		 * @param fExpr 誤差の幅(±exprとなります)
		 * @return 0を中心とした誤差
		 */
		public static function randBlur(fExpr:Number):Number
		{
			return Math.random() * fExpr * 2 - fExpr;
		}

		/**
		 * 指定の確率で真偽を返します。
		 *
		 * @param fPercentage 百分率
		 * @return 指定した確率でtrue
		 */
		public static function randPercentage(fPercentage:Number):Boolean
		{
			return Math.random() * 100 < fPercentage;
		}
		
		/**
		 * int型整数をビットリストに分解します。
		 * 注意：31bit以降は無視され、それ以上ビット数を増やしてもfalseが増えるだけです。
		 * 
		 * @param nExpr 分解する値
		 * @param uBitLength ビット長
		 * @return 分解されたリスト
		 */
		public static function createBitList(nExpr:int, uBitLength:uint):Vector.<Boolean>
		{
			var anResult:Vector.<Boolean> = new Vector.<Boolean>();
			var nBit:int = 1;
			while(anResult.length < uBitLength)
			{
				anResult.splice(0, 0, (nExpr & nBit) > 0);
				nBit <<= 1;
			}
			return anResult;
		}
	}
}
