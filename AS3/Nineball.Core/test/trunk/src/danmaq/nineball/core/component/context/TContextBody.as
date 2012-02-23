package danmaq.nineball.core.component.context
{

	import danmaq.nineball.core.component.state.CStateDelegate;
	
	import flexunit.framework.Assert;
	
	public class TContextBody
	{		

		private var context:IContext;
		private var body:CContextBody;
		
		[Before]
		public function setUp():void
		{
			context = new CContext(CStateDelegate.instance);
			body = new CContextBody(context, null);
		}
		
		[After]
		public function tearDown():void
		{
			context.dispose();
			context = null;
			body.dispose();
			body = null;
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
			Assert.assertObjectEquals(body.context, context);
		}
		
		[Test]
		public function testUpdate():void
		{
			var passed:Boolean = false;
			CStateDelegate.onUpdate = function(o:Object):void
			{
				passed = true;
			};
			Assert.assertEquals(body.counter, 0);
			body.update();
			Assert.assertTrue(passed);
			Assert.assertEquals(body.counter, 1);
		}
		
		[Test]
		public function testDispose():void
		{
			body.update();
			body.dispose();
			Assert.assertObjectEquals(body.context, context);
			Assert.assertEquals(body.counter, 0);
		}
	}
}
