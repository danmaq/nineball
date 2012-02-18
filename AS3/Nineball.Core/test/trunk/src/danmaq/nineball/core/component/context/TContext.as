package danmaq.nineball.core.component.context
{
	import danmaq.nineball.core.component.state.CStateDelegate;
	import danmaq.nineball.core.component.state.CStateEmpty;
	
	import flash.utils.getTimer;
	
	import flexunit.framework.Assert;
	
	public class TContext
	{

		private var context:CContext;
		
		[Before]
		public function setUp():void
		{
			context = new CContext();
		}
		
		[After]
		public function tearDown():void
		{
			context.dispose();
			context = null;
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
		public function testCContext():void
		{
			Assert.assertObjectEquals(context.previousState, CStateEmpty.instance);
			Assert.assertObjectEquals(context.currentState, CStateEmpty.instance);
			Assert.assertNull(context.nextState);
		}
		
		[Test]
		public function testCommitState():void
		{
			// 状態遷移
			context.nextState = CStateDelegate.instance;
			context.commitState();
			Assert.assertObjectEquals(context.previousState, CStateEmpty.instance);
			Assert.assertObjectEquals(context.currentState, CStateDelegate.instance);
			Assert.assertNull(context.nextState);

			// 状態遷移のキャンセル
			CStateDelegate.onTeardown = function(body:CContextBody):void
			{
				body.context.nextState = null;
			};
			context.nextState = CStateEmpty.instance;
			context.commitState();
			Assert.assertObjectEquals(context.previousState, CStateEmpty.instance);
			Assert.assertObjectEquals(context.currentState, CStateDelegate.instance);
			Assert.assertNull(context.nextState);
			
			// さらに状態遷移
			CStateDelegate.onTeardown = null;
			context.nextState = CStateEmpty.instance;
			context.commitState();
			Assert.assertObjectEquals(context.previousState, CStateDelegate.instance);
			Assert.assertObjectEquals(context.currentState, CStateEmpty.instance);
		}
		
		[Test]
		public function testDispose():void
		{
			var passed:Boolean = false;
			CStateDelegate.onTeardown = function(body:CContextBody):void
			{
				passed = true;
				body.context.nextState = null;	// 状態遷移キャンセルはできない
			};
			context.nextState = CStateDelegate.instance;
			context.commitState();
			context.dispose();
			Assert.assertTrue(passed);
		}
		
		[Test]
		public function testnextState():void
		{
			Assert.assertNull(context.nextState);
			context.nextState = CStateDelegate.instance;
			Assert.assertObjectEquals(context.nextState, CStateDelegate.instance);
		}
		
		[Test]
		public function testUpdate():void
		{
			var now:int = getTimer();
			var passed:Boolean = false;
			CStateDelegate.onUpdate = function(o:Object):void
			{
				passed = true;
			};
			context.nextState = CStateDelegate.instance;
			context.update();
			Assert.assertFalse(passed);
			Assert.assertNull(context.nextState);
			context.update();
			Assert.assertTrue(passed);
			Assert.assertEquals(2, context.counter);
			Assert.assertEquals(1, context.stateChangedCounter);
			Assert.assertTrue(now <= context.stateChangedTime);
		}
	}
}
