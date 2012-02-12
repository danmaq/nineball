package danmaq.nineball.core.util.list.random
{
	import danmaq.nineball.core.util.list.iterator.IIterator;
	import danmaq.nineball.core.util.math.interpolate.lerpLinear;
	import danmaq.nineball.core.util.math.onBit;
	
	import flash.utils.getTimer;
	
	import flexunit.framework.Assert;
	
	public class TRandomAll
	{

		private var rnds:Vector.<IRandom> = new Vector.<IRandom>();
		
		[Before]
		public function setUp():void
		{
			rnds.splice(0, rnds.length);
			rnds.push(new CLCG(), new CXORShift(), new CSFMT());
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
		public function testGet_iterator():void
		{
			var length:int = 1000;
			var tmp:Vector.<uint> = new Vector.<uint>(length);
			for(var i:int = rnds.length; --i >= 0; )
			{
				var rnd:IRandom = rnds[i];
				rnd.reset(length);
				var j:int;
				for(j = 0; j < length; j++)
				{
					Assert.assertEquals(j, rnd.counter);
					tmp[j] = rnd.next;
				}
				
				rnd.reset(length);
				var it:IIterator = rnd.iterator;
				for(j = 0; j < length; j++)
				{
					Assert.assertTrue(it.hasNext);
					Assert.assertEquals(j, rnd.counter);
					Assert.assertEquals(tmp[j], it.next);
				}
				Assert.assertFalse(it.reset());
			}
		}
		
		[Test]
		public function testGet_max():void
		{
			for(var i:int = rnds.length; --i >= 0; )
			{
				var rnd:IRandom = rnds[i];
				for(var j:int = 1000; --j >= 0; )
				{
					var v:Number = rnd.next;
					Assert.assertTrue(v < rnd.max);
				}
			}
		}
		
		[Test]
		public function testGet_next():void
		{
			// TODO : The Poker TestやThe Runs Test、The Long Run Testにも対応する。
			for(var i:int = rnds.length; --i >= 0; )
			{
				var rnd:IRandom = rnds[i];
				var monobit:int = 0;
				for(var j:int = 625; --j >= 0; )
				{
					monobit += onBit(lerpLinear(0, uint.MAX_VALUE, rnd.next, rnd.max));
				}
//				Assert.assertTrue(monobit >= 9654 && monobit <= 10364);	// NIST FIPS140-1 The Monobit Test
				Assert.assertTrue(monobit >= 9500 && monobit <= 10364);
			}
		}
		
		[Test]
		public function testReset():void
		{
			var seed:int = 100;
			var timer:int = getTimer();
			for(var i:int = rnds.length; --i >= 0; )
			{
				var rnd:IRandom = rnds[i];
				rnd.reset(seed);
				Assert.assertEquals(rnd.seed, seed);
				rnd.reset(-1);
				Assert.assertTrue(rnd.seed >= timer);
			}
		}
	}
}
