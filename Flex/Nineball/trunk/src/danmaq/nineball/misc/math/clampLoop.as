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
	 * 値を指定された範囲内に制限します。
	 * 最小値と最大値を逆さに設定しても内部で自動的に認識・交換しますが、
	 * 無駄なオーバーヘッドが増えるだけなので極力避けてください。
	 * 
	 * @param fExpr 対象の値
	 * @param fMin 制限値(最小) 
	 * @param fMax 制限値(最大)
	 * @return 制限された値
	 */
	public function clampLoop(fExpr:Number, fMin:Number, fMax:Number):Number{
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
				fExpr = fMax - abs(fExpr - fMin);
			}
		}
		return Math.min(fMax, Math.max(fMin, fExpr));
	}
}
