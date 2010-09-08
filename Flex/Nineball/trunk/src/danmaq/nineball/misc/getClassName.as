////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.misc
{

	import flash.utils.describeType;
	
	/**
	 * オブジェクトのクラス名を取得します。
	 * 
	 * @param obj オブジェクト
	 * @return クラス名文字列
	 */
	public function getClassName(obj:*):String
	{
		var strResult:String = "null";
		try
		{
			if(obj == null)
			{
				throw new Error();
			}
			var strName:String = describeType(obj).@name;
			if(strName == null)
			{
				throw new Error();
			}
			strResult = strName.match(/::(.*)/)[1];
		}
		catch(e:Error)
		{
		}
		return strResult;
	}
}
