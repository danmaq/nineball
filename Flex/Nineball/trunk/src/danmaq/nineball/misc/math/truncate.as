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
	 * 値を特定単位で切り捨てます。
	 *
	 * @param fExpr 対象値
	 * @param fUnit 切り捨てる単位数値
	 * @return 指定単位で切り捨てられた対象値
	 */
	public function truncate(fExpr:Number, fUnit:Number = 0):Number
	{
		return int(fExpr / fUnit) * fUnit;
	}
}
