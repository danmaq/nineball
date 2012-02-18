package danmaq.nineball.core.util.math
{
	import flash.utils.getTimer;
	
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
			Assert.assertEquals(8, onBit(0xFF));
			Assert.assertEquals(16, onBit(0xFFFF));
			Assert.assertEquals(24, onBit(0xFFFFFF));
			Assert.assertEquals(32, onBit(uint.MAX_VALUE));
		}
		
		[Test]
		public function testContains():void
		{
			var a:Number = -5.25;
			var b:Number = 3.1;
			Assert.assertTrue(contains(1, a, b));
			Assert.assertFalse(contains(-7, a, b));
			Assert.assertFalse(contains(Number.NEGATIVE_INFINITY, a, b));
			Assert.assertFalse(contains(4, a, b));
			Assert.assertFalse(contains(Number.POSITIVE_INFINITY, a, b));
			Assert.assertTrue(contains(1, b, a));
			Assert.assertFalse(contains(-7, b, a));
			Assert.assertFalse(contains(Number.NEGATIVE_INFINITY, b, a));
			Assert.assertFalse(contains(4, b, a));
			Assert.assertFalse(contains(Number.POSITIVE_INFINITY, b, a));
		}
		
		[Test]
		public function testClamp():void
		{
			var a:Number = -5.25;
			var b:Number = 3.1;
			Assert.assertEquals(1, clamp(1, a, b));
			Assert.assertEquals(-5.25, clamp(-7, a, b));
			Assert.assertEquals(-5.25, clamp(Number.NEGATIVE_INFINITY, a, b));
			Assert.assertEquals(3.1, clamp(4, a, b));
			Assert.assertEquals(3.1, clamp(Number.POSITIVE_INFINITY, a, b));
			Assert.assertEquals(1, clamp(1, b, a));
			Assert.assertEquals(-5.25, clamp(-7, b, a));
			Assert.assertEquals(-5.25, clamp(Number.NEGATIVE_INFINITY, b, a));
			Assert.assertEquals(3.1, clamp(4, b, a));
			Assert.assertEquals(3.1, clamp(Number.POSITIVE_INFINITY, b, a));
		}
	}
}
