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
	 * 減速変化する内分カウンタです。
	 * 
	 * @param fStart fNowが0と等しい場合の値
	 * @param fEnd fNowがfLimitと等しい場合の値
	 * @param fNow 現在時間
	 * @param fLimit fEndに到達する時間
	 * @return 0からfLimitまでのfNowに相当するfStartからfEndまでの値
	 */
	public function slowdown(fStart:Number, fEnd:Number, fNow:Number, fLimit:Number):Number
	{
		if(fNow <= 0){ return fStart; }
		if(fNow >= fLimit){ return fEnd; }
		return lerp(fStart, fEnd, 1 - Math.pow(1 - fNow / fLimit, 2));
	}
}
