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
	 * 内分カウンタ機能の関数集クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CInterpolate
	{

		////////// METHODS //////////

		/**
		 * 2つの値の間を線形補間します。
		 * 
		 * @param fStart ソース値1
		 * @param fEnd ソース値2
		 * @param fRate 重みを示す0～1の範囲の値。
		 * @return 補間された値。
		 */
		public static function lerp(fStart:Number, fEnd:Number, fRate:Number):Number
		{
			return fStart + (fEnd - fStart) * fRate;
		}

		/**
		 * 等速変化する内分カウンタです。
		 * 
		 * @param fStart fNowが0と等しい場合の値
		 * @param fEnd fNowがfLimitと等しい場合の値
		 * @param fNow 現在時間
		 * @param fLimit fEndに到達する時間
		 * @return 0からfLimitまでのfNowに相当するfStartからfEndまでの値
		 */
		public static function smooth(fStart:Number, fEnd:Number, fNow:Number, fLimit:Number):Number
		{
			if(fNow <= 0){ return fStart; }
			if(fNow >= fLimit){ return fEnd; }
			return lerp(fStart, fEnd, fNow / fLimit);
		}

		/**
		 * 減速変化する内分カウンタです。
		 * 
		 * @param fStart fNowが0と等しい場合の値
		 * @param fEnd fNowがfLimitと等しい場合の値
		 * @param fNow 現在時間
		 * @param fLimit fEndに到達する時間
		 * @return 0からfLimitまでのfNowに相当するfStartからfEndまでの値
		 */
		public static function slowdown(fStart:Number, fEnd:Number, fNow:Number, fLimit:Number):Number
		{
			if(fNow <= 0){ return fStart; }
			if(fNow >= fLimit){ return fEnd; }
			return lerp(fStart, fEnd, 1 - Math.pow(1 - fNow / fLimit, 2));
		}

		/**
		 * 加速変化する内分カウンタです。
		 * 
		 * @param fStart fNowが0と等しい場合の値
		 * @param fEnd fNowがfLimitと等しい場合の値
		 * @param fNow 現在時間
		 * @param fLimit fEndに到達する時間
		 * @return 0からfLimitまでのfNowに相当するfStartからfEndまでの値
		 */
		public static function accelerate(fStart:Number, fEnd:Number, fNow:Number, fLimit:Number):Number
		{
			if(fNow <= 0){ return fStart; }
			if(fNow >= fLimit){ return fEnd; }
			return lerp(fStart, fEnd, Math.pow(fNow / fLimit, 2));
		}
		
		/**
		 * 減速変化→加速変化を組み合わせスプラインのような動きを模する内分カウンタです。
		 * 高速→低速→高速と変化します。
		 * 
		 * @param fStart fNowが0と等しい場合の値
		 * @param fEnd fNowがfLimitと等しい場合の値
		 * @param fNow 現在時間
		 * @param fLimit fEndに到達する時間
		 * @return 0からfLimitまでのfNowに相当するfStartからfEndまでの値
		 */
		public static function splineFSF(fStart:Number, fEnd:Number, fNow:Number, fLimit:Number):Number
		{
			if(fNow <= 0){ return fStart; }
			if(fNow >= fLimit){ return fEnd; }
			var fCenter:Number = smooth(fStart, fEnd, 1, 2);
			var fHalfLimit:Number = fLimit / 2;
			return fNow < fHalfLimit ?
				slowdown(fStart, fCenter, fNow, fHalfLimit) :
				accelerate(fCenter, fEnd, fNow - fHalfLimit, fHalfLimit);
		}
		
		/**
		 * 加速変化→減速変化を組み合わせスプラインのような動きを模する内分カウンタです。
		 * 低速→高速→低速と変化します。
		 * 
		 * @param fStart fNowが0と等しい場合の値
		 * @param fEnd fNowがfLimitと等しい場合の値
		 * @param fNow 現在時間
		 * @param fLimit fEndに到達する時間
		 * @return 0からfLimitまでのfNowに相当するfStartからfEndまでの値
		 */
		public static function splineSFS(fStart:Number, fEnd:Number, fNow:Number, fLimit:Number):Number
		{
			if(fNow <= 0){ return fStart; }
			if(fNow >= fLimit){ return fEnd; }
			var fCenter:Number = smooth(fStart, fEnd, 1, 2);
			var fHalfLimit:Number = fLimit / 2;
			return fNow < fHalfLimit ?
				accelerate(fStart, fCenter, fNow, fHalfLimit) :
				slowdown(fCenter, fEnd, fNow - fHalfLimit, fHalfLimit);
		}

		/**
		* ネヴィル・スプラインのシミュレータです。
		*
		* @param nStart 現在時間==0の時の初期値
		* @param nMiddle 制御点(中間値)
		* @param nEnd 最終値
		* @param nNow 現在時間
		* @param nLimit 最終値に到達する時間
		* @return 初期値～(中間値)～最終値に対し0～到達時間の現在時間に相当する値
		*/
		public static function neville(fStart:Number, fMiddle:Number, fEnd:Number, fNow:Number, fLimit:Number):Number
		{
			if(fNow >= fLimit || fStart == fEnd || fLimit <= 0){ return fEnd; }
			if(fNow <= 0){ return fStart; }
			var fTimePoint:Number = fNow / fLimit * 2;
			fMiddle = fEnd + (fEnd - fMiddle) * (fTimePoint - 2);
			return fMiddle + (fMiddle - (fMiddle + (fMiddle - fStart) * (fTimePoint - 1))) * (fTimePoint - 2) * 0.5;
		}

		/**
		* ベジェ・スプラインのシミュレータです。
		*
		* @param nStart 現在時間==0の時の初期値
		* @param nMiddle 制御点(中間値)
		* @param nEnd 最終値
		* @param nNow 現在時間
		* @param nLimit 最終値に到達する時間
		* @return 初期値～(中間値)～最終値に対し0～到達時間の現在時間に相当する値
		*/
		public static function bezier(fStart:Number, fMiddle:Number, fEnd:Number, fNow:Number, fLimit:Number):Number
		{
			if(fNow >= fLimit || fStart == fEnd || fLimit <= 0){ return fEnd; }
			if(fNow <= 0){ return fStart; }
			var fTimePoint:Number = fNow / fLimit * 2;
			var fResidual:Number = (1 - fTimePoint);
			return (fResidual ^ 2 * fStart) + (2 * fResidual * fTimePoint * fMiddle) + (fTimePoint ^ 2 * fEnd);
		}
	}
}
