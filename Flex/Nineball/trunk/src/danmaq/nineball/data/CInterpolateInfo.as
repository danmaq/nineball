////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.data
{
	
	import danmaq.nineball.misc.math.interpolate.smooth;

	/**
	 * プログラマブル内分カウンタ クラスの情報。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CInterpolateInfo
	{

		////////// FIELDS //////////

		/**	開始値。 */		
		public var start:Number;

		/**	終了値。 */		
		public var end:Number;

		/**	間隔。 */		
		public var interval:Number;

		/**	内分カウンタ。 */		
		public var interpolate:Function;

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param start 開始値。
		 * @param end 終了値。
		 * @param interval 間隔。
		 * @param interpolate 内分カウンタ。
		 */
		public function CInterpolateInfo(
			start:Number, end:Number, interval:Number, interpolate:Function = null)
		{
			this.start = start;
			this.end = end;
			this.interval = interval;
			this.interpolate = (interpolate == null ? smooth : interpolate);
		}
	}
}
