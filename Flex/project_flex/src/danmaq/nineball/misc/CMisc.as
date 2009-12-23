package danmaq.nineball.misc{
	
	import flash.display.*;
	import flash.geom.*;
	import flash.utils.*;
	
	/**
	 * 汎用関数群クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CMisc{

		////////// METHODS //////////

		/**
		 * 表示オブジェクトに行列を設定します。
		 * 
		 * @param obj 表示オブジェクト
		 * @param scale 拡大率 
		 * @param rotate 角度(度)
		 * @param pos 中心座標
		 */
		public static function setMatrix(
			obj:DisplayObject, scale:Point, rotate:Number, pos:Point
		):void{
			obj.transform.matrix = new Matrix();
			var result:Matrix = new Matrix();
			result.tx = -obj.width / 2;
			result.ty = -obj.height / 2;
			result.scale( scale.x, scale.y );
			result.rotate( Math.PI * rotate / 180 );
			result.tx += pos.x;
			result.ty += pos.y;
			obj.transform.matrix = result;
		}
		
		/**
		 * オブジェクトの中心座標を基準に描画先座標を設定します。
		 * 
		 * @param obj 表示オブジェクト
		 * @param pos 座標
		 */
		public static function setPosCenter( obj:DisplayObject, pos:Point ):void{
			obj.transform.matrix = new Matrix();
			var result:Matrix = new Matrix();
			result.tx = pos.x - obj.width / 2;
			result.ty = pos.y - obj.height / 2;
			obj.transform.matrix = result;
		}
		
		/**
		 * ベクトルを作成します。
		 * 
		 * @param fDeg 角度(度)
		 * @param fSpeed 速度
		 * @return ベクトル
		 */
		public static function createVector( fDeg:Number, fSpeed:Number ):Point{
			var fRad:Number = Math.PI * fDeg / 180;
			return new Point(
				Math.sin( fRad ) * fSpeed, -Math.cos( fRad ) * fSpeed );
		}

		/**
		 * オブジェクトのクラス名を取得します。
		 * 
		 * @param obj オブジェクト
		 * @return クラス名文字列
		 */
		public static function getClassName( obj:* ):String{
			var strResult:String = "null";
			try{
				if( obj == null ){ throw new Error(); }
				var strName:String = describeType( obj ).@name;
				if( strName == null ){ throw new Error(); }
				strResult = strName.match( /::(.*)/ )[ 1 ];
			}
			catch( e:Error ){}
			return strResult;
		}
		
		/**
		 * クラスのインスタンス オブジェクトのクラス オブジェクトを取得します。
		 * 
		 * @param obj クラスのインスタンス オブジェクト
		 * @return クラス オブジェクト
		 */
		public static function getClassObject( obj:Object ):Class{
			return getDefinitionByName( getQualifiedClassName( obj ) ) as Class;
		}
		
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
		public static function isRelate( cls:Class, objInstance:Object ):Boolean{
			var bResult:Boolean = true;
			try{
				if( objInstance as cls == null ){ throw new Error(); }
			}
			catch( e:Error ){ bResult = false; }
			return bResult;
		}
	}
}
