package danmaq.nineball.core.events
{
	import flash.events.Event;
	
	import flexunit.framework.Assert;
	
	public class TDisposableEventDispatcher
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
		public function testDispose():void
		{
			var obj:CDisposableEventDispatcher = new CDisposableEventDispatcher();
			Assert.assertFalse(obj.hasEventListener(Event.ACTIVATE));
			obj.addEventListener(Event.ACTIVATE, function(e:Event):void{});
			Assert.assertTrue(obj.hasEventListener(Event.ACTIVATE));
			obj.dispose();
			Assert.assertFalse(obj.hasEventListener(Event.ACTIVATE));
		}
	}
}