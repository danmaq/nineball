package danmaq.nineball.test.struct{

	import danmaq.nineball.struct.CAlign;
	
	import org.libspark.as3unit.test;
	import org.libspark.as3unit.assert.*;
	
	use namespace test;
	
	public final class CAlignTest{

		test function testIsValid():void{
			assertTrue( CAlign.isValid( CAlign.BOTTOM_RIGHT ), "BOTTOM_RIGHT" );
			assertTrue( CAlign.isValid( CAlign.TOP_LEFT ), "TOP_LEFT" );
			assertTrue( CAlign.isValid( CAlign.CENTER ), "CENTER" );
			assertFalse( CAlign.isValid( -2 ), "outvalue" );
			assertFalse( CAlign.isValid( 2 ), "outvalue" );
		}

	}
}
