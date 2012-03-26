package danmaq.nineball.core.util.math
{

	import danmaq.nineball.core.util.string.sprintf;

	import flash.geom.Point;
	import flash.geom.Vector3D;

	/**
	 * ベクトル回転に使用する四元数を計算するクラスです。
	 *
	 * <p>
	 * 三角関数演算をオブジェクト作成時に先んじて行うため、高速軽量な回転演算が可能となります。
	 * また、要素が4個だけのため、回転のみしかできない代わりに、行列よりも高速な演算が行えます。
	 * </p>
	 * <p>
	 * このクラスには二次元および三次元ベクトルの回転機能だけでなく
	 * 四次元ベクトルとしての基本的な計算機能も実装されています。
	 * </p>
	 *
	 * @author Mc(danmaq)
	 * @example 下記の例では、2D座標(10, 0)をZ軸回転します。
	 * <listing version="3.0">
	 * var q:CQuaternion = CQuaternion.createFromAxis(0, 0, 1, Math.PI * 0.5);
	 * var pos = q.tranform2D(new Point(10, 0));
	 * trace(pos); // (x=0, y=10)
	 * q.tranform2D(pos, pos);
	 * trace(pos); // (x=-10, y=0)
	 * q.tranform2D(pos, pos);
	 * trace(pos); // (x=0, y=-10)
	 * q.tranform2D(pos, pos);
	 * trace(pos); // (x=10, y=0)
	 * </listing>
	 */
	public final class CQuaternion extends Vector3D
	{

		//* constants ──────────────────────────────-*

		/** 切り捨てる誤差。 */
		private static const GAP:Number = 0.000000000000002;

		/** 入出力情報保持用四次元ベクトル。 */
		private const src:Vector3D = new Vector3D();

		/** 負の四元数を設定するバッファ。 */
		private const negBuffer:Vector3D = new Vector3D();

		/** 一時的な結果を設定するバッファ。 */
		private const tempBuffer:Vector3D = new Vector3D();

		//* constructor & destructor ───────────────────────*

		/**
		 * コンストラクタ。
		 *
		 * <p>
		 * 既定の引数を使用すると、回転を行わない四元数(identity)が作成されます。
		 * </p>
		 *
		 * @param x 四元数を構成するベクトル要素のX値。
		 * @param y 四元数を構成するベクトル要素のY値。
		 * @param z 四元数を構成するベクトル要素のZ値。
		 * @param w 四元数の回転要素。
		 */
		public function CQuaternion(x:Number = 0, y:Number = 0, z:Number = 0, w:Number = 1)
		{
			super(x, y, z, w);
		}

		//* instance properties ─────────────────────────-*

		/**
		 * 四次元ベクトルの長さを取得します。
		 *
		 * <p>
		 * このプロパティは、内部で計算処理に時間のかかる<code>Math.sqrt()</code>メソッドを使用
		 * しています。可能であれば、<code>lengthSquared</code>プロパティの使用を強く推奨します。
		 * </p>
		 *
		 * @return 長さ。
		 * @see #lengthSquared
		 * @see Math.sqrt()
		 */
		override public function get length():Number
		{
			return Math.sqrt(lengthSquared);
		}

		/**
		 * 四次元ベクトルの長さの平方を取得します。
		 *
		 * @return 長さ。
		 * @see #length
		 */
		override public function get lengthSquared():Number
		{
			return x * x + y * y + z * z + w * w;
		}

		/**
		 * 軸ベクトルを取得します。
		 *
		 * @return 軸ベクトル。
		 */
		public function get axis():Vector3D
		{
			var s:Number = Math.sin(Math.acos(w) * 2.0 * 0.5);
			return new Vector3D(x / s, y / s, z / s);
		}

		//* class methods ────────────────────────────-*

		/**
		 * 軸ベクトルから四元数を作成します。
		 *
		 * <p>
		 * 軸ベクトルは予め正規化しておく必要があります。そうでない場合、正しい結果を取得することが
		 * できません。正規化には<code>Vector3D.normalize()</code>メソッドが便利です。
		 * </p>
		 *
		 * @param x 軸ベクトルを構成するX要素。
		 * @param y 軸ベクトルを構成するY要素。
		 * @param z 軸ベクトルを構成するZ要素。
		 * @param radian 回転量(ラジアン)。
		 * @param result 格納先。<code>null</code>の場合、新規にオブジェクトを作成します。
		 * @return 四元数。<code>result</code>が<code>null</code>
		 * でない場合、<code>result</code>と同一のオブジェクトを取得します。
		 * @see flash.geom.Vector3D#normalize()
		 */
		public static function createFromAxis(
			x:Number, y:Number, z:Number, radian:Number, result:CQuaternion = null):CQuaternion
		{
			if(result == null)
			{
				result = new CQuaternion();
			}
			var s:Number = Math.sin(radian * 0.5);
			result.w = Math.cos(radian * 0.5);
			result.x = s * x;
			result.y = s * y;
			result.z = s * z;
			return result;
		}

		/**
		 * 軸ベクトルから四元数を作成します。
		 *
		 * <p>
		 * 軸ベクトルは予め正規化しておく必要があります。
		 * そうでない場合、正しい結果を取得することができません。
		 * </p>
		 *
		 * @param axis 軸ベクトル。
		 * @param radian 回転量(ラジアン)。
		 * @param result 格納先。<code>null</code>の場合、新規にオブジェクトを作成します。
		 * @return 四元数。<code>result</code>が<code>null</code>
		 * でない場合、<code>result</code>と同一のオブジェクトを取得します。
		 */
		public static function createFromVectorAxis(
			axis:Vector3D, radian:Number, result:CQuaternion = null):CQuaternion
		{
			return createFromAxis(axis.x, axis.y, axis.z, radian, result);;
		}

		/**
		 * ヨー、ピッチ、ロールから四元数を作成します。
		 *
		 * @param yaw ヨー回転量(ラジアン)。
		 * @param pitch ピッチ回転量(ラジアン)。
		 * @param roll ロール回転量(ラジアン)。
		 * @param result 格納先。<code>null</code>の場合、新規にオブジェクトを作成します。
		 * @return 四元数。<code>result</code>が<code>null</code>
		 * でない場合、<code>result</code>と同一のオブジェクトを取得します。
		 * @see flash.geom.Vector3D#normalize()
		 */
		public static function createFromYawPitchRoll(
			yaw:Number, pitch:Number, roll:Number, result:CQuaternion = null):CQuaternion
		{
			if(result == null)
			{
				result = new CQuaternion();
			}
			var sy:Number = Math.sin(yaw * 0.5);
			var cy:Number = Math.cos(yaw * 0.5);
			var sp:Number = Math.sin(pitch * 0.5);
			var cp:Number = Math.cos(pitch * 0.5);
			var sr:Number = Math.sin(roll * 0.5);
			var cr:Number = Math.cos(roll * 0.5);
			result.x = sr * cp * cy - cr * sp * sy;
			result.y = cr * sp * cy + sr * cp * sy;
			result.z = cr * cp * sy - sr * sp * cy;
			result.w = cr * cp * cy + sr * sp * sy;
			return result;
		}

		/**
		 * 四元数同士のクォータニオン積を計算します。
		 *
		 * @param v1 四元数を構成する四次元ベクトル。
		 * @param v2 四元数を構成する四次元ベクトル。
		 * @param result 格納先。<code>null</code>の場合、新規にオブジェクトを作成します。
		 * @return 四元数。<code>result</code>が<code>null</code>
		 * でない場合、<code>result</code>と同一のオブジェクトを取得します。
		 */
		public static function multiply(
			v1:Vector3D, v2:Vector3D, result:Vector3D = null):Vector3D
		{
			if(result == null)
			{
				result = new Vector3D();
			}
			result.x = v1.w * v2.x + v2.w * v1.x + v1.y * v2.z - v2.y * v1.z;
			result.y = v1.w * v2.y + v2.w * v1.y + v1.z * v2.x - v2.z * v1.x;
			result.z = v1.w * v2.z + v2.w * v1.z + v1.x * v2.y - v2.x * v1.y;
			result.w = v1.w * v2.w - v1.x * v2.x - v1.y * v2.y - v1.z * v2.z;
			return result;
		}

		/**
		 * 四次元ベクトルの線形補完をします。
		 *
		 * @param v1 四元数を構成する四次元ベクトル。
		 * @param v2 四元数を構成する四次元ベクトル。
		 * @param amount 重みを0.0～1.0で指定します。0に近づくほど<code>v1</code>
		 * に近い値となり、1に近づくほど<code>v2</code>に近い値となります。
		 * @param result 格納先。<code>null</code>の場合、新規にオブジェクトを作成します。
		 * @return 四元数。<code>result</code>が<code>null</code>
		 * でない場合、<code>result</code>と同一のオブジェクトを取得します。
		 */
		public static function lerp(
			v1:Vector3D, v2:Vector3D, amount:Number, result:Vector3D = null):Vector3D
		{
			if(result == null)
			{
				result = new CQuaternion();
			}
			result.x = (v2.x - v1.x) * amount + v1.x;
			result.y = (v2.y - v1.y) * amount + v1.y;
			result.z = (v2.z - v1.z) * amount + v1.z;
			result.w = (v2.w - v1.w) * amount + v1.w;
			return result;
		}

		//* instance methods ───────────────────────────*

		/**
		 * 指定した二次元ベクトルを回転します。
		 *
		 * @param point 回転したい二次元ベクトル。
		 * @param result 格納先。<code>null</code>の場合、新規にオブジェクトを作成します。
		 * 上書きしたい場合は、<code>point</code>と同じ引数を指定します。
		 * @return 回転された二次元ベクトル。<code>result</code>が<code>null</code>
		 * でない場合、<code>result</code>と同一のオブジェクトを取得します。
		 */
		public function tranform2D(point:Point, result:Point = null):Point
		{
			if(result == null)
			{
				result = new Point();
			}
			src.x = point.x;
			src.y = point.y;
			src.z = 0;
			src.w = 0;
			transform();
			result.x = src.x;
			result.y = src.y;
			return result;
		}

		/**
		 * 指定した三次元、または四次元ベクトルを回転します。
		 *
		 * @param vector 回転したい三次元、または四次元ベクトル。
		 * @param result 格納先。<code>null</code>の場合、新規にオブジェクトを作成します。
		 * 上書きしたい場合は、<code>point</code>と同じ引数を指定します。
		 * @return 回転された三次元、または四次元ベクトル。<code>result</code>
		 * が<code>null</code>でない場合、<code>result</code>と同一のオブジェクトを取得します。
		 */
		public function tranform3D(vector:Vector3D, result:Vector3D = null):Vector3D
		{
			if(result == null)
			{
				result = new Vector3D();
			}
			src.x = vector.x;
			src.y = vector.y;
			src.z = vector.z;
			src.w = vector.w;
			transform();
			result.x = src.x;
			result.y = src.y;
			result.z = src.z;
			result.w = src.w;
			return result;
		}

		/**
		 * 四次元ベクトル同士のドット積(内積)を計算します。
		 *
		 * @param a 計算対象の4次元ベクトル。
		 * @return ドット積(内積)。
		 */
		override public function dotProduct(a:Vector3D):Number
		{
			return x * a.x + y * a.y + z * a.z + w * a.w;
		}

		/**
		 * 四次元ベクトルを反転します。
		 */
		override public function negate():void
		{
			super.negate();
			w *= -1;
		}

		/**
		 * 現在の四次元ベクトルを単位ベクトルに変換(正規化)します。
		 *
		 * <ul>
		 * <li>単位ベクトルの長さは、<code>1</code>です。</li>
		 * <li>
		 * 長さが0の四次元ベクトルを正規化した場合、全ての要素が<code>NaN</code>となります。
		 * </li>
		 * </ul>
		 *
		 * @return 単位ベクトルに変換される直前の長さ。
		 */
		override public function normalize():Number
		{
			var result:Number = this.lengthSquared;
			var l:Number = NaN;
			if(result > 0)
			{
				result = Math.sqrt(result);
				l = 1.0 / result;
			}
			x *= l;
			y *= l;
			z *= l;
			w *= l;
			return result;
		}

		/**
		 * 四次元ベクトル値の文字列表現を取得します。
		 *
		 * @return 値の文字列表現。
		 */
		override public function toString():String
		{
			return sprintf("Quaternion(4D Vector)[X=%f, Y=%f, Z=%f, W=%f]", x, y, z, w);
		}

		/**
		 * 指定したベクトルを回転します。
		 */
		private function transform():void
		{
			negBuffer.x = -x;
			negBuffer.y = -y;
			negBuffer.z = -z;
			negBuffer.w = w;
			multiply(this, src, tempBuffer);
			multiply(tempBuffer, negBuffer, src);
			if(src.x < GAP && src.x > -GAP)
			{
				src.x = 0;
			}
			if(src.y < GAP && src.y > -GAP)
			{
				src.y = 0;
			}
			if(src.z < GAP && src.z > -GAP)
			{
				src.z = 0;
			}
		}
	}
}
