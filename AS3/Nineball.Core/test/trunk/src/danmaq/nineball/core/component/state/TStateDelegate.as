package danmaq.nineball.core.component.state
{

	import danmaq.nineball.core.component.context.CContext;
	import danmaq.nineball.core.component.context.CContextBody;
	import danmaq.nineball.core.component.context.IContext;
	
	import flexunit.framework.Assert;
	
	public class TStateDelegate
	{

		private var body:CContextBody;
		
		[Before]
		public function setUp():void
		{
			body = new CContextBody(new CContext(), null);
		}
		
		[After]
		public function tearDown():void
		{
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
		public function testCStateDelegate():void
		{
			var state:IState;
			try
			{
				state = new CStateDelegate();
			}
			catch(e:Error)
			{
			}
			Assert.assertNull(state);
		}
		
		[Test]
		public function testonSetup():void
		{
			CStateDelegate.onSetup = null;
			Assert.assertNotNull(CStateDelegate.onSetup);
		}
		
		[Test]
		public function testonTeardown():void
		{
			CStateDelegate.onTeardown = null;
			Assert.assertNotNull(CStateDelegate.onSetup);
		}
		
		[Test]
		public function testonUpdate():void
		{
			CStateDelegate.onUpdate = null;
			Assert.assertNotNull(CStateDelegate.onSetup);
		}
		
		[Test]
		public function testSetup():void
		{
			var passed:Boolean = false;
			CStateDelegate.onSetup = function(o:CContextBody):void
			{
				passed = true;
				Assert.assertObjectEquals(body, o);
			};
			CStateDelegate.instance.setup(body);
			Assert.assertTrue(passed);
		}
		
		[Test]
		public function testTeardown():void
		{
			var passed:Boolean = false;
			CStateDelegate.onTeardown = function(o:CContextBody):void
			{
				passed = true;
				Assert.assertObjectEquals(body, o);
			};
			CStateDelegate.instance.teardown(body);
			Assert.assertTrue(passed);
		}
		
		[Test]
		public function testUpdate():void
		{
			var passed:Boolean = false;
			CStateDelegate.onUpdate = function(o:CContextBody):void
			{
				passed = true;
				Assert.assertObjectEquals(body, o);
			};
			CStateDelegate.instance.update(body);
			Assert.assertTrue(passed);
		}
		
		[Test]
		public function testDispose():void
		{
			Assert.assertObjectEquals(
				CStateDelegate.onSetup, CStateDelegate.onUpdate, CStateDelegate.onTeardown);
		}
	}
}
