package danmaq.nineball.test.struct{

	import danmaq.nineball.struct.CAlign;
	
	import org.libspark.as3unit.test;
	import org.libspark.as3unit.assert.*;
	
	use namespace test;
	
	/**
	 * 端寄せ情報定義リストのテストクラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CAlignTest{

		/**
		 * CAlign.validメソッドのテストを開始します。
		 */
		test function test_IsValid():void{
			assertTrue( CAlign.isValid( CAlign.BOTTOM_RIGHT ), "BOTTOM_RIGHT" );
			assertTrue( CAlign.isValid( CAlign.TOP_LEFT ), "TOP_LEFT" );
			assertTrue( CAlign.isValid( CAlign.CENTER ), "CENTER" );
			assertFalse( CAlign.isValid( -2 ), "outvalue" );
			assertFalse( CAlign.isValid( 2 ), "outvalue" );
		}

	}
}
