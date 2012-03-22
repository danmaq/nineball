package danmaq.nineball.core.util.math
{
	import flexunit.framework.Assert;

	public class TMathHelper
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
		public function testCMathHelper():void
		{
			var m:CMathHelper;
			try
			{
				m = new CMathHelper();
			}
			catch(e:Error)
			{
			}
			Assert.assertNull(m);
		}
		
		[Test]
		public function testOnBit():void
		{
			var bits:Vector.<uint> = Vector.<uint>([
				0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4 ]);
			for(var i:int = bits.length; --i >= 0; )
			{
				Assert.assertEquals(CMathHelper.onBit(i), bits[i]);
			}
			Assert.assertEquals(0, CMathHelper.onBit(0));
			Assert.assertEquals(4, CMathHelper.onBit(204));
			Assert.assertEquals(8, CMathHelper.onBit(0xFF));
			Assert.assertEquals(16, CMathHelper.onBit(0xFFFF));
			Assert.assertEquals(24, CMathHelper.onBit(0xFFFFFF));
			Assert.assertEquals(32, CMathHelper.onBit(uint.MAX_VALUE));
		}
		
		[Test]
		public function testContains():void
		{
			var a:Number = -5.25;
			var b:Number = 3.1;
			Assert.assertTrue(CMathHelper.contains(1, a, b));
			Assert.assertFalse(CMathHelper.contains(-7, a, b));
			Assert.assertFalse(CMathHelper.contains(Number.NEGATIVE_INFINITY, a, b));
			Assert.assertFalse(CMathHelper.contains(4, a, b));
			Assert.assertFalse(CMathHelper.contains(Number.POSITIVE_INFINITY, a, b));
			Assert.assertTrue(CMathHelper.contains(1, b, a));
			Assert.assertFalse(CMathHelper.contains(-7, b, a));
			Assert.assertFalse(CMathHelper.contains(Number.NEGATIVE_INFINITY, b, a));
			Assert.assertFalse(CMathHelper.contains(4, b, a));
			Assert.assertFalse(CMathHelper.contains(Number.POSITIVE_INFINITY, b, a));
		}
		
		[Test]
		public function testClamp():void
		{
			var a:Number = -5.25;
			var b:Number = 3.1;
			Assert.assertEquals(1, CMathHelper.clamp(1, a, b));
			Assert.assertEquals(-5.25, CMathHelper.clamp(-7, a, b));
			Assert.assertEquals(-5.25, CMathHelper.clamp(Number.NEGATIVE_INFINITY, a, b));
			Assert.assertEquals(3.1, CMathHelper.clamp(4, a, b));
			Assert.assertEquals(3.1, CMathHelper.clamp(Number.POSITIVE_INFINITY, a, b));
			Assert.assertEquals(1, CMathHelper.clamp(1, b, a));
			Assert.assertEquals(-5.25, CMathHelper.clamp(-7, b, a));
			Assert.assertEquals(-5.25, CMathHelper.clamp(Number.NEGATIVE_INFINITY, b, a));
			Assert.assertEquals(3.1, CMathHelper.clamp(4, b, a));
			Assert.assertEquals(3.1, CMathHelper.clamp(Number.POSITIVE_INFINITY, b, a));
		}
		
		[Test]
		public function testSign():void
		{
			Assert.assertStrictlyEquals(0, CMathHelper.sign(0));
			Assert.assertStrictlyEquals(0, CMathHelper.sign(-0));
			Assert.assertStrictlyEquals(0, CMathHelper.sign(Number.NaN));
			Assert.assertStrictlyEquals(-1, CMathHelper.sign(-1));
			Assert.assertStrictlyEquals(1, CMathHelper.sign(1));
			Assert.assertStrictlyEquals(-1, CMathHelper.sign(-0.01));
			Assert.assertStrictlyEquals(1, CMathHelper.sign(0.01));
			Assert.assertStrictlyEquals(-1, CMathHelper.sign(-Number.MAX_VALUE));
			Assert.assertStrictlyEquals(1, CMathHelper.sign(Number.MAX_VALUE));
			Assert.assertStrictlyEquals(-1, CMathHelper.sign(Number.NEGATIVE_INFINITY));
			Assert.assertStrictlyEquals(1, CMathHelper.sign(Number.POSITIVE_INFINITY));
		}
	}
}
