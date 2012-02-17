package danmaq.nineball.core.util.math.integer
{
	import danmaq.nineball.core.util.math.contains;
	
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
			Assert.assertTrue(danmaq.nineball.core.util.math.integer.contains(1, a, b));
			Assert.assertFalse(danmaq.nineball.core.util.math.integer.contains(-7, a, b));
			Assert.assertFalse(danmaq.nineball.core.util.math.integer.contains(4, a, b));
			Assert.assertTrue(danmaq.nineball.core.util.math.integer.contains(1, b, a));
			Assert.assertFalse(danmaq.nineball.core.util.math.integer.contains(-7, b, a));
			Assert.assertFalse(danmaq.nineball.core.util.math.integer.contains(4, b, a));
			var tn:int = getTimer();
			for(i = 500000; --i >= 0; )
			{
				danmaq.nineball.core.util.math.contains(1, 0, 5);
			}
			tn = getTimer() - tn;
			var ti:int = getTimer();
			for(i = 500000; --i >= 0; )
			{
				danmaq.nineball.core.util.math.integer.contains(1, 0, 5);
			}
			ti = getTimer() - ti;
			Assert.assertTrue(tn > ti);
		}
	}
}
