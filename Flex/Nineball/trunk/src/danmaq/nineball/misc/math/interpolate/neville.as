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
	 * ネヴィル補間のシミュレータです。
	 *
	 * @param nStart 現在時間==0の時の初期値
	 * @param nMiddle 制御点(中間値)
	 * @param nEnd 最終値
	 * @param nNow 現在時間
	 * @param nLimit 最終値に到達する時間
	 * @return 初期値～(中間値)～最終値に対し0～到達時間の現在時間に相当する値
	 */
	public function neville(
		fStart:Number, fMiddle:Number, fEnd:Number, target:Number, fLimit:Number):Number
	{
		if(target >= fLimit || fStart == fEnd || fLimit <= 0){ return fEnd; }
		if(target <= 0){ return fStart; }
		var fTimePoint:Number = target / fLimit * 2;
		fMiddle = fEnd + (fEnd - fMiddle) * (fTimePoint - 2);
		return fMiddle + (fMiddle - (fMiddle + (fMiddle - fStart) * (fTimePoint - 1))) *
			(fTimePoint - 2) * 0.5;
	}
}
