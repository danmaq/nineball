package danmaq.nineball.core.util.math
{
	import flexunit.framework.Assert;

	public class TMath
	{		
		[Before]
		public function setUp():void
		{
		}
		
		[After]
		public function tearDown():void
		{
		}
		
		[BeforeClass]
		public static function setUpBeforeClass():void
		{
		}
		
		[AfterClass]
		public static function tearDownAfterClass():void
		{
		}
		
		[Test]
		public function testOnBit():void
		{
			var bits:Vector.<uint> = Vector.<uint>([
				0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4 ]); 
			for(var i:int = bits.length; --i >= 0; )
			{
				Assert.assertEquals(onBit(i), bits[i]);
			}
			Assert.assertEquals(0, onBit(0));
			Assert.assertEquals(8, onBit(255));
			Assert.assertEquals(16, onBit(65535));
			Assert.assertEquals(24, onBit(16777215));
			Assert.assertEquals(32, onBit(uint.MAX_VALUE));
		}
	}
}
