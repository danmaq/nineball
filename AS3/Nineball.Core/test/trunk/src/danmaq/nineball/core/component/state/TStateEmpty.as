package danmaq.nineball.core.component.state
{
	
	import flexunit.framework.Assert;
	
	import mx.utils.StringUtil;
	
	public class TStateEmpty
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
		public function testCStateEmpty():void
		{
			var state:IState;
			try
			{
				state = new CStateEmpty();
			}
			catch(e:Error)
			{
			}
			Assert.assertNull(state);
		}
		
		[Test]
		public function testSetup():void
		{
			try
			{
				CStateEmpty.instance.setup(null);
			}
			catch(e:Error)
			{
				Assert.fail(StringUtil.substitute(
					"CStateEmpty.setupクラスは何もしてはいけません。: {0}", e));
			}
		}
		
		[Test]
		public function testTeardown():void
		{
			try
			{
				CStateEmpty.instance.teardown(null);
			}
			catch(e:Error)
			{
				Assert.fail(StringUtil.substitute(
					"CStateEmpty.teardownクラスは何もしてはいけません。: {0}", e));
			}
		}
		
		[Test]
		public function testUpdate():void
		{
			try
			{
				CStateEmpty.instance.update(null);
			}
			catch(e:Error)
			{
				Assert.fail(StringUtil.substitute(
					"CStateEmpty.updateメソッドは何もしてはいけません。: {0}", e));
			}
		}
	}
}