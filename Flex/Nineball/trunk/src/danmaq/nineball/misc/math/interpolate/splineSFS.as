////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.misc.math.interpolate
{

	/**
	 * 加速変化→減速変化を組み合わせスプラインのような動きを模する内分カウンタです。
	 * 低速→高速→低速と変化します。
	 * 
	 * @param fStart targetが0と等しい場合の値
	 * @param fEnd targetがfLimitと等しい場合の値
	 * @param target 現在時間
	 * @param fLimit fEndに到達する時間
	 * @return 0からfLimitまでのtargetに相当するfStartからfEndまでの値
	 */
	public function splineSFS(fStart:Number, fEnd:Number, target:Number, fLimit:Number):Number
	{
		if(target <= 0){ return fStart; }
		if(target >= fLimit){ return fEnd; }
		var fCenter:Number = smooth(fStart, fEnd, 1, 2);
		var fHalfLimit:Number = fLimit / 2;
		return target < fHalfLimit ?
			accelerate(fStart, fCenter, target, fHalfLimit) :
			slowdown(fCenter, fEnd, target - fHalfLimit, fHalfLimit);
	}
}
