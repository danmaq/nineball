package danmaq.nineball.core.util.math.integer
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
		public function testMax():void
		{
		}
		
		[Test]
		public function testContains():void
		{
			var a:int = -5;
			var b:int = 3;
			var i:int;
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
			var tn:int = getTimer();
			for(i = 300000; --i >= 0; )
			{
				contains(1, 0, 5)
			}
			tn = getTimer() - tn;
			var ti:int = getTimer();
			for(i = 300000; --i >= 0; )
			{
				contains(1, 0, 5)
			}
			ti = getTimer() - ti;
		}
	}
}
