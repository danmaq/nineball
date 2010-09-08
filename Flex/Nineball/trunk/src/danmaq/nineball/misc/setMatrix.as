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
	 * 表示オブジェクトに行列を設定します。
	 * 
	 * @param obj 表示オブジェクト
	 * @param scale 拡大率 
	 * @param rotate 角度(度)
	 * @param pos 中心座標
	 */
	public function setMatrix(obj:DisplayObject, scale:Point, rotate:Number, pos:Point):void
	{
		obj.transform.matrix = new Matrix();
		var result:Matrix = new Matrix();
		result.tx = -obj.width / 2;
		result.ty = -obj.height / 2;
		result.scale(scale.x, scale.y);
		result.rotate(Math.PI * rotate / 180);
		result.tx += pos.x;
		result.ty += pos.y;
		obj.transform.matrix = result;
	}
}
