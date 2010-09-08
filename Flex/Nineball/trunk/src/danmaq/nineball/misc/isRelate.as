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
	
	/**
	 * インスタンスが指定クラスと関連性があるかどうかを取得します。
	 * 
	 * <p>
	 * インスタンスが指定クラスのインスタンスか、サブクラスの
	 * インスタンスである場合、関連性があると見なされます。
	 * </p>
	 * 
	 * @param cls クラス オブジェクト
	 * @param objInstance インスタンス オブジェクト
	 * @return 関連性がある場合、true
	 */
	public function isRelate(cls:Class, objInstance:Object):Boolean
	{
		return !(objInstance == null || cls == null || (objInstance as cls) == null);
	}
}
