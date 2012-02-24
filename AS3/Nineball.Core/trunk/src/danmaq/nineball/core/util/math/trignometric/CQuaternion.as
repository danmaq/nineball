package danmaq.nineball.core.util.math.trignometric
{
	import danmaq.nineball.core.util.math.CQuantize;
	
	import flash.geom.Point;
	import flash.geom.Vector3D;
	
	/**
	 * ベクトル回転に使用する四元数を計算するクラスです。
	 * 
	 * <p>
	 * 三角関数演算をオブジェクト作成時に先んじて行うため、高速軽量な回転演算が可能となります。
	 * また、要素が4個だけのため、回転のみしかできない代わりに、行列よりも高速な演算が行えます。
	 * </p>
	 * 
	 * @author Mc(danmaq)
	 * @example 下記の例では、2D座標(10, 0)をZ軸回転します。
	 * <listing version="3.0">
	 * var q:CQuaternion = CQuaternion.createFromAxis(0, 0, 1, Math.PI * 0.5);
	 * var pos = q.tranform2D(new Point(10, 0));
	 * trace(pos) // (x=0, y=10)
	 * 
	 * q.tranform2D(pos, pos)
	 * trace(pos) // (x=-10, y=0)
	 * 
	 * q.tranform2D(pos, pos)
	 * trace(pos) // (x=0, y=-10)
	 * 
	 * q.tranform2D(pos, pos)
	 * trace(pos) // (x=10, y=0)
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
		 * 四元数同士を乗算します。
		 * 
		 * <p>
		 * このメソッドでは内部的に下記のような演算を行っています。
		 * </p>
		 * <pre>
		 * q1 * q2 = (w1w2 - V1・V2; w1V2 + w2V1 + V1×V2)
		 * </pre>
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
			result.w = v1.w * v2.w - v1.x * v2.x - v1.y * v2.y - v1.z * v2.z;
			result.x = v1.w * v2.x + v2.w * v1.x + v1.y * v2.z - v2.y * v1.z;
			result.y = v1.w * v2.y + v2.w * v1.y + v1.z * v2.x - v2.z * v1.x;
			result.z = v1.w * v2.z + v2.w * v1.z + v1.x * v2.y - v2.x * v1.y;
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
		 * 指定した三次元ベクトルを回転します。
		 * 
		 * <p>このメソッドでは引数のw要素は変化しません。</p>
		 * 
		 * @param vector 回転したい三次元ベクトル。
		 * @param result 格納先。<code>null</code>の場合、新規にオブジェクトを作成します。
		 * 上書きしたい場合は、<code>point</code>と同じ引数を指定します。
		 * @return 回転された二次元ベクトル。<code>result</code>が<code>null</code>
		 * でない場合、<code>result</code>と同一のオブジェクトを取得します。
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
			src.w = 0;
			transform();
			result.x = src.x;
			result.y = src.y;
			result.z = src.z;
			return result;
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
