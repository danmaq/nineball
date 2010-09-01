////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.data
{
	import danmaq.nineball.misc.math.*;

	/**
	 * グラデーション情報を格納するクラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CGradation
	{

		////////// FIELDS //////////

		/**	初期値が格納されます。 */
		public var start:Number;
		
		/**	最終値が格納されます。 */
		public var end:Number;
		
		/**	限界値1が格納されます。 */
		public var limit1:Number;
		
		/**	限界値2が格納されます。 */
		public var limit2:Number;

		////////// PROPERTIES //////////
		
		/**
		 * グラデーション計算をせず、デフォルトの値を取得します。
		 * 
		 * @return デフォルト値
		 */
		public function get value():Number
		{
			if(limit1 == limit2)
			{
				return limit1;
			}
			return CMisc.clamp(start, Math.min(limit1, limit2), Math.max(limit1, limit2));
		}

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param fStart 初期値
		 * @param fEnd 最終値
		 * @param fLimit1 限界値1
		 * @param fLimit2 限界値2
		 */
		public function CGradation(
			fStart:Number, fEnd:Number = Number.NaN,
			fLimit1:Number = Number.NEGATIVE_INFINITY,
			fLimit2:Number = Number.POSITIVE_INFINITY):void
		{
			initialize(fStart, fEnd, fLimit1, fLimit2);
		}

		/**
		 * グラデーション計算に必要な値を設定します。
		 * 
		 * @param fStart 初期値
		 * @param fEnd 最終値
		 * @param fLimit1 限界値1
		 * @param fLimit2 限界値2
		 */
		public function initialize(
			fStart:Number, fEnd:Number = Number.NaN,
			fLimit1:Number = Number.NEGATIVE_INFINITY,
			fLimit2:Number = Number.POSITIVE_INFINITY):void
		{
			start = fStart;
			end = isNaN(fEnd) ? fStart : fEnd;
			limit1 = fLimit1;
			limit2 = fLimit2;
		}

		/**
		 * グラデーション計算をし、補完値を算出します。
		 * 
		 * @param nNow 現在値
		 * @param uSize 分割数
		 * @return 補完値
		 */
		public function smooth(nNow:int, uSize:uint):Number
		{
			if(limit1 == limit2)
			{
				return limit1;
			}
			return CMisc.clamp(CInterpolate.smooth(start, end, nNow, uSize),
				Math.min(limit1, limit2), Math.max(limit1, limit2));
		}

		/**
		 * グラデーションの合計全長の計算をします。
		 * 
		 * @param uSize 分割数
		 * @return 全長
		 */
		public function getLength(uSize:uint):Number
		{
			var fResult:Number = 0;
			for(var i:int = 0; i < uSize; fResult += smooth(i++, uSize));
			return fResult;
		}
	}
}