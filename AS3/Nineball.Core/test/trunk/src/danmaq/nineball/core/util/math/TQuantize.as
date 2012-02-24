package danmaq.nineball.core.util.math
{
	import flexunit.framework.Assert;
	
	public class TQuantize
	{

		private const GAP:Number = 0.0000000000000005;
		
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
		public function testCQuantize():void
		{
			var q:CQuantize;
			try
			{
				q = new CQuantize();
			}
			catch(e:Error)
			{
			}
			Assert.assertNull(q);
		}
		
		[Test]
		public function testCeil():void
		{
			var e:Number = Math.E; // 2.71828182845905
			Assert.assertTrue(Math.abs(CQuantize.ceil(e, 1000) - 1000) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.ceil(e, 100) - 100) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.ceil(e, 10) - 10) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.ceil(e, 6) - 6) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.ceil(e, 5) - 5) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.ceil(e, 2) - 4) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.ceil(e, 1) - 3) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.ceil(e, 0.5) - 3) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.ceil(e, 0.1) - 2.8) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.ceil(e, 0.05) - 2.75) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.ceil(e, 0.01) - 2.72) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.ceil(e, 0.001) - 2.719) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.ceil(e, 0.0001) - 2.7183) < GAP);
			Assert.assertStrictlyEquals(e, CQuantize.ceil(e, 0));
			Assert.assertStrictlyEquals(e, CQuantize.ceil(e, -1));
		}
		
		[Test]
		public function testFloor():void
		{
			var e:Number = Math.E; // 2.71828182845905
			Assert.assertTrue(Math.abs(CQuantize.floor(e, 10) - 0) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.floor(e, 3) - 0) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.floor(e, 2) - 2) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.floor(e, 1) - 2) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.floor(e, 0.5) - 2.5) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.floor(e, 0.1) - 2.7) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.floor(e, 0.05) - 2.70) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.floor(e, 0.01) - 2.71) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.floor(e, 0.001) - 2.718) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.floor(e, 0.0001) - 2.7182) < GAP);
			Assert.assertStrictlyEquals(e, CQuantize.floor(e, 0));
			Assert.assertStrictlyEquals(e, CQuantize.floor(e, -1));
		}
		
		[Test]
		public function testRound():void
		{
			var e:Number = Math.E; // 2.71828182845905
			Assert.assertTrue(Math.abs(CQuantize.round(e, 10) - 0) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.round(e, 6) - 0) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.round(e, 5) - 5) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.round(e, 2) - 2) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.round(e, 1) - 3) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.round(e, 0.5) - 2.5) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.round(e, 0.1) - 2.7) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.round(e, 0.05) - 2.70) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.round(e, 0.01) - 2.72) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.round(e, 0.001) - 2.718) < GAP);
			Assert.assertTrue(Math.abs(CQuantize.round(e, 0.0001) - 2.7183) < GAP);
			Assert.assertStrictlyEquals(e, CQuantize.round(e, 0));
			Assert.assertStrictlyEquals(e, CQuantize.round(e, -1));
		}
	}
}