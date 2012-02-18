package danmaq.nineball.core.data
{
	import flexunit.framework.Assert;
	
	public class TPhase
	{

		private var phase:CPhase;

		[Before]
		public function setUp():void
		{
			phase = new CPhase();
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
		public function testCPhase():void
		{
			Assert.assertEquals(0, phase.counter);
			Assert.assertEquals(0, phase.phase);
			Assert.assertEquals(0, phase.previousPhase);
			Assert.assertEquals(0, phase.phaseChangeCount);
			Assert.assertFalse(phase.reserveNextPhase);
			Assert.assertTrue(phase.nextPhase < 0);
		}
		
		[Test]
		public function testcounter():void
		{
			Assert.assertEquals(0, phase.counter);
			Assert.assertEquals(1, ++phase.counter);
			Assert.assertEquals(1, phase.counter++);
			Assert.assertEquals(2, phase.counter);
			phase.reserveNextPhase = true;
			Assert.assertEquals(2, phase.counter);
			Assert.assertTrue(phase.reserveNextPhase);
			Assert.assertEquals(2, phase.counter++);
			Assert.assertFalse(phase.reserveNextPhase);
		}
		
		[Test]
		public function testDispose():void
		{
			phase.counter = 2;
			phase.phase = 1;
			phase.phase = 2;
			phase.nextPhase = 3;
			phase.dispose();
			Assert.assertEquals(0, phase.counter);
			Assert.assertEquals(0, phase.phase);
			Assert.assertEquals(0, phase.previousPhase);
			Assert.assertEquals(0, phase.phaseChangeCount);
			Assert.assertFalse(phase.reserveNextPhase);
			Assert.assertTrue(phase.nextPhase < 0);
		}
		
		[Test]
		public function testSet_phase():void
		{
			Assert.assertEquals(0, phase.phase);
			Assert.assertEquals(1, ++phase.phase);
			Assert.assertEquals(1, phase.phase++);
			Assert.assertEquals(2, phase.phase);
		}
		
		[Test]
		public function testphaseChangeCount():void
		{
			Assert.assertEquals(0, phase.phaseChangeCount);
			phase.counter = 2;
			phase.phase = 1;
			Assert.assertEquals(2, phase.phaseChangeCount);
		}
		
		[Test]
		public function testphaseCounter():void
		{
			Assert.assertEquals(0, phase.phaseCounter);
			phase.counter = 2;
			Assert.assertEquals(2, phase.phaseCounter);
			phase.phase = 1;
			Assert.assertEquals(2, phase.counter);
			Assert.assertEquals(0, phase.phaseCounter);
		}
		
		[Test]
		public function testGet_previousPhase():void
		{
			phase.phase = 1;
			Assert.assertEquals(0, phase.previousPhase);
			phase.phase = 2;
			Assert.assertEquals(1, phase.previousPhase);
			phase.phase = 2;
			Assert.assertEquals(1, phase.previousPhase);
		}
		
		[Test]
		public function testreserveNextPhase():void
		{
			Assert.assertFalse(phase.reserveNextPhase);
			phase.nextPhase = 0;
			Assert.assertFalse(phase.reserveNextPhase);
			phase.nextPhase = 1;
			Assert.assertTrue(phase.reserveNextPhase);
			phase.nextPhase = -1;
			Assert.assertFalse(phase.reserveNextPhase);
			phase.reserveNextPhase = true;
			Assert.assertEquals(1, phase.nextPhase);
			phase.reserveNextPhase = false;
			Assert.assertEquals(-1, phase.nextPhase);
		}
	}
}