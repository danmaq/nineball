package danmaq.nineball.core.component.state
{

	import danmaq.nineball.core.component.context.CContext;
	import danmaq.nineball.core.component.context.CContextProxy;
	import danmaq.nineball.core.component.context.IContext;
	
	import flexunit.framework.Assert;
	
	public class TStateDelegate
	{

		private var proxy:CContextProxy;
		
		[Before]
		public function setUp():void
		{
			proxy = new CContextProxy(new CContext());
		}
		
		[After]
		public function tearDown():void
		{
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
			CStateDelegate.onSetup = function(o:CContextProxy):void
			{
				passed = true;
				Assert.assertObjectEquals(proxy, o);
			};
			CStateDelegate.instance.setup(proxy);
			Assert.assertTrue(passed);
		}
		
		[Test]
		public function testTeardown():void
		{
			var passed:Boolean = false;
			CStateDelegate.onTeardown = function(o:CContextProxy):void
			{
				passed = true;
				Assert.assertObjectEquals(proxy, o);
			};
			CStateDelegate.instance.teardown(proxy);
			Assert.assertTrue(passed);
		}
		
		[Test]
		public function testUpdate():void
		{
			var passed:Boolean = false;
			CStateDelegate.onUpdate = function(o:CContextProxy):void
			{
				passed = true;
				Assert.assertObjectEquals(proxy, o);
			};
			CStateDelegate.instance.update(proxy);
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
