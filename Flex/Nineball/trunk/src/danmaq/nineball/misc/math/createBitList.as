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
	 * int型整数をビットリストに分解します。
	 * 注意：31bit以降は無視され、それ以上ビット数を増やしてもfalseが増えるだけです。
	 * 
	 * @param nExpr 分解する値
	 * @param uBitLength ビット長
	 * @return 分解されたリスト
	 */
	public function createBitList(nExpr:int, uBitLength:uint):Vector.<Boolean>
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
