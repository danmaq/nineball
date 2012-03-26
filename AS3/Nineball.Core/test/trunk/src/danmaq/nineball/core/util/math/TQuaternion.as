package danmaq.nineball.core.util.math
{
	import flash.geom.Point;
	import flash.geom.Vector3D;
	
	import flexunit.framework.Assert;
	
	public class TQuaternion
	{

		private const GAP:Number = 0.000000000000006;
		
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
		public function testCQuaternion():void
		{
			var q:CQuaternion = new CQuaternion(1, 2, 3, 4);
			Assert.assertStrictlyEquals(1, q.x);
			Assert.assertStrictlyEquals(2, q.y);
			Assert.assertStrictlyEquals(3, q.z);
			Assert.assertStrictlyEquals(4, q.w);
		}
		
		[Test]
		public function testEquals():void
		{
			var q1:CQuaternion = CQuaternion.createFromAxis(0, 0, 1, Math.PI);
			var q2:CQuaternion = CQuaternion.createFromVectorAxis(Vector3D.Z_AXIS, Math.PI);
			Assert.assertTrue(q1.equals(q2));
			Assert.assertTrue(q2.equals(q1));
		}
		
		[Test]
		public function testTranform2D():void
		{
			var q:CQuaternion = CQuaternion.createFromVectorAxis(Vector3D.Z_AXIS, Math.PI * 0.5);
			var got:Point = q.tranform2D(new Point(10, 0));
			Assert.assertTrue(Math.abs(got.x - 0) < GAP);
			Assert.assertTrue(Math.abs(got.y - 10) < GAP);
			q.tranform2D(got, got);
			Assert.assertTrue(Math.abs(got.x + 10) < GAP);
			Assert.assertTrue(Math.abs(got.y - 0) < GAP);
			q.tranform2D(got, got);
			Assert.assertTrue(Math.abs(got.x - 0) < GAP);
			Assert.assertTrue(Math.abs(got.y + 10) < GAP);
			q.tranform2D(got, got);
			Assert.assertTrue(Math.abs(got.x - 10) < GAP);
			Assert.assertTrue(Math.abs(got.y - 0) < GAP);
		}
		
		[Test]
		public function testTranform3D():void
		{
			var q:CQuaternion = CQuaternion.createFromVectorAxis(Vector3D.Z_AXIS, Math.PI * 0.5);
			var got:Vector3D = q.tranform3D(new Vector3D(10, 0, 10));
			Assert.assertTrue(Math.abs(got.x - 0) < GAP);
			Assert.assertTrue(Math.abs(got.y - 10) < GAP);
			Assert.assertTrue(Math.abs(got.z - 10) < GAP);
			q.tranform3D(got, got);
			Assert.assertTrue(Math.abs(got.x + 10) < GAP);
			Assert.assertTrue(Math.abs(got.y - 0) < GAP);
			Assert.assertTrue(Math.abs(got.z - 10) < GAP);
			q.tranform3D(got, got);
			Assert.assertTrue(Math.abs(got.x - 0) < GAP);
			Assert.assertTrue(Math.abs(got.y + 10) < GAP);
			Assert.assertTrue(Math.abs(got.z - 10) < GAP);
			q.tranform3D(got, got);
			Assert.assertTrue(Math.abs(got.x - 10) < GAP);
			Assert.assertTrue(Math.abs(got.y - 0) < GAP);
			Assert.assertTrue(Math.abs(got.z - 10) < GAP);
		}
		
		[Test]
		public function testAxis():void
		{
			var q:CQuaternion = CQuaternion.createFromVectorAxis(Vector3D.Z_AXIS, Math.PI * 0.5);
			Assert.assertTrue(Vector3D.Z_AXIS.equals(q.axis));
		}
		
		[Test]
		public function testNegate():void
		{
			var q:CQuaternion = new CQuaternion(1, 2, 3, 4);
			q.negate();
			Assert.assertStrictlyEquals(-1, q.x);
			Assert.assertStrictlyEquals(-2, q.y);
			Assert.assertStrictlyEquals(-3, q.z);
			Assert.assertStrictlyEquals(-4, q.w);
		}
		
		[Test]
		public function testLerp():void
		{
			var q1:CQuaternion = new CQuaternion(0, 0, 0, 0);
			var q2:CQuaternion = new CQuaternion(2, 4, 6, 8);
			var qg:CQuaternion = new CQuaternion();
			CQuaternion.lerp(q1, q2, 0, qg);
			Assert.assertStrictlyEquals(0, qg.x);
			Assert.assertStrictlyEquals(0, qg.y);
			Assert.assertStrictlyEquals(0, qg.z);
			Assert.assertStrictlyEquals(0, qg.w);
			CQuaternion.lerp(q1, q2, 1, qg);
			Assert.assertStrictlyEquals(2, qg.x);
			Assert.assertStrictlyEquals(4, qg.y);
			Assert.assertStrictlyEquals(6, qg.z);
			Assert.assertStrictlyEquals(8, qg.w);
			CQuaternion.lerp(q1, q2, 0.5, qg);
			Assert.assertStrictlyEquals(1, qg.x);
			Assert.assertStrictlyEquals(2, qg.y);
			Assert.assertStrictlyEquals(3, qg.z);
			Assert.assertStrictlyEquals(4, qg.w);
		}
	}
}