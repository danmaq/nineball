package danmaq.nineball.core.util.list.random
{
	import flexunit.framework.Assert;
	
	public class TRandomUtil
	{

		private var _rnd:CRandom;
		private var _rndUtil:CRandomUtil;
		
		[Before]
		public function setUp():void
		{
			_rnd = new CSFMT(0);
			_rndUtil = new CRandomUtil(new CSFMT(0));
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
		public function testTRandomUtil():void
		{
			Assert.assertNotNull(new CRandomUtil().random);
			Assert.assertObjectEquals(_rnd, new CRandomUtil(_rnd).random);
			Assert.assertStrictlyEquals(0, _rnd.seed, _rndUtil.seed);
		}
		
		[Test]
		public function testGet_max():void
		{
			Assert.assertEquals(_rnd.max, _rndUtil.max);
		}
		
		[Test]
		public function testGet_next():void
		{
			for(var i:int = 1000; --i >= 0; )
			{
				Assert.assertStrictlyEquals(i, _rnd.counter, _rndUtil.counter);
				Assert.assertStrictlyEquals(_rnd.next, _rndUtil.next);
			}
		}
		
		[Test]
		public function testNextBlur():void
		{
			for(var i:int = 0; i < 1000; i++)
			{
				Assert.assertStrictlyEquals(i, _rndUtil.counter);
				var value:Number = _rndUtil.nextBlur(i);
				Assert.assertTrue(value >= -i && value <= i);
			}
		}
		
		[Test]
		public function testNextLimit():void
		{
			for(var i:int = 0; i < 1000; i++)
			{
				Assert.assertStrictlyEquals(i, _rndUtil.counter);
				var value:Number = _rndUtil.nextLimit(i, i + 512.34);
				Assert.assertTrue(value >= i && value <= i + 512.34);
			}
		}
		
		[Test]
		public function testGet_nextNumber():void
		{
			for(var i:int = 0; i < 1000; i++)
			{
				Assert.assertStrictlyEquals(i, _rndUtil.counter);
				var value:Number = _rndUtil.nextNumber;
				Assert.assertTrue(value >= 0 && value <= 1);
			}
		}
		
		[Test]
		public function testReset():void
		{
			var value:Number;
			value = _rnd.next;
			value = _rndUtil.next;
			Assert.assertStrictlyEquals(1, _rnd.counter, _rndUtil.counter);
			var seed:int = 100;
			_rnd.reset(seed);
			_rndUtil.reset(seed);
			Assert.assertStrictlyEquals(0, _rnd.counter, _rndUtil.counter);
			Assert.assertStrictlyEquals(seed, _rnd.seed, _rndUtil.seed);
		}
	}
}
