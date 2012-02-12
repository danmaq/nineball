package danmaq.nineball.core.util.list.random
{
	import flexunit.framework.Assert;
	
	public class TRandom
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
		public function testCRandom():void
		{
			var rnd:CRandom;
			try
			{
				rnd = new CRandom();
			}
			catch(e:Error)
			{
			}
			Assert.assertNull(rnd);
		}
	}
}