package danmaq.nineball.core.component.context
{

	import danmaq.nineball.core.component.state.CStateDelegate;
	
	import flexunit.framework.Assert;
	
	public class TContextProxy
	{		

		private var context:IContext;
		private var proxy:CContextProxy;
		
		[Before]
		public function setUp():void
		{
			context = new CContext(CStateDelegate.instance);
			proxy = new CContextProxy(context);
		}
		
		[After]
		public function tearDown():void
		{
			context.dispose();
			context = null;
			proxy.dispose();
			proxy = null;
			CStateDelegate.instance.dispose();
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
		public function testGet_context():void
		{
			Assert.assertObjectEquals(proxy.context, context);
		}
		
		[Test]
		public function testUpdate():void
		{
			var passed:Boolean = false;
			CStateDelegate.onUpdate = function(o:Object):void
			{
				passed = true;
			};
			Assert.assertEquals(proxy.counter, 0);
			proxy.update();
			Assert.assertTrue(passed);
			Assert.assertEquals(proxy.counter, 1);
		}
		
		[Test]
		public function testDispose():void
		{
			proxy.update();
			proxy.dispose();
			Assert.assertObjectEquals(proxy.context, context);
			Assert.assertEquals(proxy.counter, 0);
		}
	}
}
