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
	 * 加速変化する内分カウンタです。
	 * 
	 * @param fStart targetが0と等しい場合の値
	 * @param fEnd targetがfLimitと等しい場合の値
	 * @param target 現在時間
	 * @param fLimit fEndに到達する時間
	 * @return 0からfLimitまでのtargetに相当するfStartからfEndまでの値
	 */
	public function accelerate(fStart:Number, fEnd:Number, target:Number, fLimit:Number):Number
	{
		if(target <= 0){ return fStart; }
		if(target >= fLimit){ return fEnd; }
		return lerp(fStart, fEnd, amountAccelerate(target, fLimit));
	}
}
