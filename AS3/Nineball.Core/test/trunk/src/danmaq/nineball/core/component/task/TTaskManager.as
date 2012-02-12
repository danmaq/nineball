package danmaq.nineball.core.component.task
{
	import danmaq.nineball.core.component.context.CContext;
	import danmaq.nineball.core.component.context.CContextProxy;
	import danmaq.nineball.core.component.state.CStateDelegate;
	import danmaq.nineball.core.util.list.iterator.IIterator;
	
	import flexunit.framework.Assert;
	
	public class TTaskManager
	{

		private var A:CContext;
		private var B:CContext;
		private var taskManager:CTaskManager;
		private var proxy:CTaskManagerProxy;
		
		[Before]
		public function setUp():void
		{
			CStateDelegate.onSetup = function(o:CContextProxy):void
			{
				proxy = CTaskManagerProxy(o);
			};
			taskManager = new CTaskManager(CStateDelegate.instance);
			CStateDelegate.instance.dispose();
			taskManager.dispose();
			A = new CContext(CStateDelegate.instance);
			B = new CContext(CStateDelegate.instance);
		}
		
		[After]
		public function tearDown():void
		{
			taskManager.dispose();
			taskManager = null;
			A.dispose();
			A = null;
			B.dispose();
			B = null;
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
		public function testCTaskManager():void
		{
			Assert.assertObjectEquals(CStateTaskManager.instance, taskManager.currentState);
		}
		
		[Test]
		public function testAddReserve():void
		{
			Assert.assertEquals(0, proxy.add.length);
			taskManager.addReserve(A);
			Assert.assertEquals(1, proxy.add.length);
			taskManager.addReserve(B);
			Assert.assertEquals(2, proxy.add.length);
			taskManager.addReserve(A);
			Assert.assertEquals(2, proxy.add.length);
			taskManager.commitReserve();
			Assert.assertEquals(0, proxy.add.length);
			taskManager.addReserve(B);
			Assert.assertEquals(0, proxy.add.length);

			taskManager.clear();
			taskManager.addReserve(A);
			taskManager.commitReserve();
			taskManager.addReserve(B);
			Assert.assertEquals(1, proxy.add.length);
			taskManager.removeReserve(A);
			Assert.assertEquals(1, proxy.add.length);
			taskManager.removeReserve(B);
			Assert.assertEquals(0, proxy.add.length);
		}
		
		[Test]
		public function testClear():void
		{
			taskManager.addReserve(A);
			taskManager.commitReserve();
			taskManager.addReserve(B);
			taskManager.removeReserve(A);
			Assert.assertEquals(1, proxy.tasks.length);
			Assert.assertEquals(1, proxy.add.length);
			Assert.assertEquals(1, proxy.remove.length);
			taskManager.clear();
			Assert.assertEquals(0, proxy.tasks.length);
			Assert.assertEquals(0, proxy.add.length);
			Assert.assertEquals(0, proxy.remove.length);
		}
		
		[Test]
		public function testCommitReserve():void
		{
			Assert.assertEquals(0, proxy.tasks.length);
			taskManager.addReserve(A);
			taskManager.addReserve(B);
			Assert.assertEquals(0, proxy.tasks.length);
			taskManager.commitReserve();
			Assert.assertEquals(2, proxy.tasks.length);
			taskManager.removeReserve(A);
			Assert.assertEquals(2, proxy.tasks.length);
			taskManager.commitReserve();
			Assert.assertEquals(1, proxy.tasks.length);
		}
		
		[Test]
		public function testContains():void
		{
			taskManager.addReserve(A);
			Assert.assertFalse(taskManager.contains(A));
			Assert.assertFalse(taskManager.contains(B));
			taskManager.commitReserve();
			Assert.assertTrue(taskManager.contains(A));
			Assert.assertFalse(taskManager.contains(B));
		}
		
		[Test]
		public function testGet_defaultState():void
		{
			Assert.assertObjectEquals(CStateTaskManager.instance, taskManager.defaultState);
		}
		
		[Test]
		public function testGet_iterator():void
		{
			taskManager.addReserve(A);
			taskManager.addReserve(B);
			taskManager.commitReserve();
			var it:IIterator = taskManager.iterator;
			Assert.assertTrue(it.hasNext);
			Assert.assertObjectEquals(A, it.next);
			Assert.assertTrue(it.hasNext);
			Assert.assertObjectEquals(B, it.next);
			Assert.assertFalse(it.hasNext);
			try
			{
				var obj:Object = it.next;
				Assert.fail("値を取得できてしまってはいけません。");
			}
			catch(e:Error)
			{
			}
			Assert.assertTrue(it.reset());
			Assert.assertTrue(it.hasNext);
			Assert.assertObjectEquals(A, it.next);
		}
		
		[Test]
		public function testGet_length():void
		{
			taskManager.addReserve(A);
			Assert.assertEquals(0, taskManager.length);
			taskManager.commitReserve();
			Assert.assertEquals(1, taskManager.length);
			taskManager.removeReserve(A);
			Assert.assertEquals(1, taskManager.length);
			taskManager.commitReserve();
			Assert.assertEquals(0, taskManager.length);
		}
		
		[Test]
		public function testRemoveReserve():void
		{
			taskManager.addReserve(A);
			taskManager.addReserve(B);
			taskManager.commitReserve();
			Assert.assertEquals(0, proxy.remove.length);
			taskManager.removeReserve(A);
			Assert.assertEquals(1, proxy.remove.length);
			taskManager.removeReserve(A);
			Assert.assertEquals(1, proxy.remove.length);
			taskManager.removeReserve(B);
			Assert.assertEquals(2, proxy.remove.length);
			taskManager.commitReserve();
			Assert.assertEquals(0, proxy.remove.length);
			taskManager.removeReserve(B);
			Assert.assertEquals(0, proxy.remove.length);

			taskManager.clear();
			taskManager.addReserve(A);
			taskManager.commitReserve();
			taskManager.removeReserve(A);
			Assert.assertEquals(1, proxy.remove.length);
			taskManager.addReserve(A);
			Assert.assertEquals(0, proxy.remove.length);
			Assert.assertEquals(0, proxy.add.length);
		}
		
		[Test]
		public function testUpdate():void
		{
			var passed:int = 0;
			CStateDelegate.onUpdate = function(o:CContextProxy):void
			{
				passed++;
			};
			taskManager.addReserve(A);
			taskManager.addReserve(B);
			Assert.assertEquals(0, proxy.tasks.length);
			taskManager.update();
			Assert.assertEquals(2, passed);
			passed = 0;
			taskManager.update();
			Assert.assertEquals(2, passed);
			passed = 0;
			taskManager.removeReserve(A);
			taskManager.removeReserve(B);
			taskManager.update();
			Assert.assertEquals(0, passed);
		}
	}
}
