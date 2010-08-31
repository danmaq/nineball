////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.old.data.font
{

	import danmaq.nineball.misc.math.CMisc;
	
	import flash.geom.Point;
	
	import mx.utils.StringUtil;

	/**
	 * フォントの描画調整情報を保持する構造体です。
	 * 
	 * <p>
	 * 通常、ユーザはこのクラスよりもサブクラスである
	 * CFontTransformを使用することの方が多いでしょう。
	 * </p>
	 * 
	 * @see danmaq.nineball.struct.CFontTransform
	 * @see danmaq.nineball.task.CTaskFont
	 * @author Mc(danmaq) 
	 */
	public class CFontTransformBit
	{

		////////// CONSTANTS //////////

		/**	座標が格納されます。 */
		public const pos:Point = new Point();
		
		/**	拡大率が格納されます。 */
		public const scale:Point = new Point(1, 1);

		////////// FIELDS //////////
		
		/**	アンチエイリアスを施すかどうかが格納されます。 */
		public var smoothing:Boolean;
		
		/**	回転角度が格納されます。 */
		private var m_fRotate:Number;

		/**	透明度が格納されます。 */
		private var m_fAlpha:Number;

		/**	乗算する色が格納されます。 */
		private var m_uColor:uint;

		////////// PROPERTIES //////////

		/**
		 * 回転角度を取得します。
		 *
		 * @return 回転角度
		 */
		public function get rotate():Number
		{
			return m_fRotate;
		}

		/**
		 * 回転角度を設定します。
		 *
		 * @param value 回転角度
		 * @throws flash.errors.IllegalOperationError 引数にNaNを設定した場合
		 */
		public function set rotate(value:Number):void
		{
			m_fRotate = isNaN(value) ? 0 : value;
		}

		/**
		 * 色を取得します。
		 *
		 * @return 色情報(赤8bit、緑8bit、青8bit)
		 */
		public function get color():uint
		{
			return m_uColor;
		}

		/**
		 * 色を設定します。
		 * 範囲外の値を設定すると自動的に丸められます。
		 *
		 * @param value 色情報(赤8bit、緑8bit、青8bit)
		 */
		public function set color(value:uint):void
		{
			m_uColor = (value & 0xFFFFFF);
		}

		/**
		 * 透明度を取得します。
		 *
		 * @return 透明度(透明0～1不透明)
		 */
		public function get alpha():Number
		{
			return m_fAlpha;
		}

		/**
		 * 透明度を設定します。
		 * 範囲外の値を設定すると自動的に丸められます。
		 *
		 * @param value 透明度(透明0～1不透明)
		 */
		public function set alpha(value:Number):void
		{
			m_fAlpha = CMisc.clamp(value, 0, 1);
		}

		/**
		 * オブジェクトのコピーを取得します。
		 *
		 * @return オブジェクトのコピー
		 */
		public function get clone():CFontTransformBit
		{
			return new CFontTransformBit(pos, scale, rotate, alpha, color, smoothing);
		}

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 *
		 * @param pos 座標
		 * @param scale 拡大率
		 * @param rotate 回転角度
		 * @param alpha 透過度
		 * @param color 乗算するカラーコード
		 * @param smoothing アンチエイリアスを施すかどうか
		 */
		public function CFontTransformBit(
			pos:Point = null, scale:Point = null, rotate:Number = 0,
			alpha:Number = 1, color:uint = 0xFFFFFF, smoothing:Boolean = false)
		{
			if(pos != null)
			{
				this.pos.x = pos.x;
				this.pos.y = pos.y;
			}
			if(scale != null)
			{
				this.scale.x = scale.x;
				this.scale.y = scale.y;
			}
			this.rotate = rotate;
			this.alpha = alpha;
			this.color = color;
			this.smoothing = smoothing;
		}

		/**
		 * オブジェクトのストリング表現を取得します。
		 * このクラスでは、設定された値の一覧を文字列形式で習得します。
		 *
		 * @param 設定された値の一覧文字列
		 */
		public function toString():String
		{
			return StringUtil.substitute("POS:{0},SCALE:{1},ALPHA:{2},COLOR:[R:{3},G:{4},B:{5}]",
				pos, scale, alpha, (color >> 4) & 0xFF, (color >> 2) & 0xFF, color & 0xFF);
		}
	}
}
