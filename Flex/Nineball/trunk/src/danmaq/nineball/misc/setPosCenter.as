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

	import flash.display.DisplayObject;
	import flash.geom.*;

	/**
	 * オブジェクトの中心座標を基準に描画先座標を設定します。
	 * 
	 * @param obj 表示オブジェクト
	 * @param pos 座標
	 */
	public function setPosCenter(obj:DisplayObject, pos:Point):void
	{
		obj.transform.matrix = new Matrix();
		var result:Matrix = new Matrix();
		result.tx = pos.x - obj.width / 2;
		result.ty = pos.y - obj.height / 2;
		obj.transform.matrix = result;
	}
}
