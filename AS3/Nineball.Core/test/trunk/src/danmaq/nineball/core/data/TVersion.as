package danmaq.nineball.core.data
{
	import flexunit.framework.Assert;

	public class TVersion
	{

		private var _version:CVersion;
		private var _readonly:IVersion;
		
		[Before]
		public function setUp():void
		{
			_version = new CVersion();
			_readonly = _version.asReadonly;
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
		public function testCVersion():void
		{
			Assert.assertStrictlyEquals(0, _version.major);
			Assert.assertStrictlyEquals(0, _version.minor);
			Assert.assertStrictlyEquals(0, _version.revision);
			Assert.assertStrictlyEquals(0, _version.build);
			Assert.assertStrictlyEquals(0, _readonly.major);
			Assert.assertStrictlyEquals(0, _readonly.minor);
			Assert.assertStrictlyEquals(0, _readonly.revision);
			Assert.assertStrictlyEquals(0, _readonly.build);
			var ver:CVersion = new CVersion(1, 2, 3, 4);
			Assert.assertStrictlyEquals(1, ver.major);
			Assert.assertStrictlyEquals(2, ver.minor);
			Assert.assertStrictlyEquals(3, ver.revision);
			Assert.assertStrictlyEquals(4, ver.build);
			Assert.assertStrictlyEquals(1, ver.asReadonly.major);
			Assert.assertStrictlyEquals(2, ver.asReadonly.minor);
			Assert.assertStrictlyEquals(3, ver.asReadonly.revision);
			Assert.assertStrictlyEquals(4, ver.asReadonly.build);
		}
		
		[Test]
		public function testFormat():void
		{
			Assert.assertStrictlyEquals("%d.%d.%d.%d", CVersion.DEFAULT_FORMAT);
			Assert.assertStrictlyEquals(CVersion.DEFAULT_FORMAT, _version.format);
			var format:String = "%d.%d%d";
			_version.format = format;
			Assert.assertStrictlyEquals(format, _version.format);
		}
		
		[Test]
		public function testToString():void
		{
			Assert.assertStrictlyEquals("0.0.0.0", _version.toString());
			var ver:CVersion = new CVersion(1, 2, 3, 40);
			Assert.assertStrictlyEquals("1.2.3.40", ver.toString());
			ver.format = "%d.%d%d";
			Assert.assertStrictlyEquals("1.23", ver.toString());
			Assert.assertStrictlyEquals("1.23", ver.asReadonly.toString());
		}
		
		[Test]
		public function testParse():void
		{
			Assert.assertStrictlyEquals("0.0.0.0", CVersion.parse("0.0.0.0").toString());
			Assert.assertStrictlyEquals("1.2.3.4", CVersion.parse("1.2.3.4").toString());
			Assert.assertStrictlyEquals("10.20.30.40", CVersion.parse("10.20.30.40").toString());
			Assert.assertStrictlyEquals("10.20.30.0", CVersion.parse("10.20.30").toString());
			Assert.assertStrictlyEquals("10.20.0.0", CVersion.parse("10.20").toString());
			Assert.assertStrictlyEquals("11.0.0.0", CVersion.parse("11").toString());
			Assert.assertStrictlyEquals("10.20.3.4", CVersion.parse("10.20.3_4").toString());
			Assert.assertStrictlyEquals("10.20.3.4", CVersion.parse("10_20.3_4").toString());
			Assert.assertStrictlyEquals("1.2.2.3", CVersion.parse(1.223).toString());
			Assert.assertStrictlyEquals("1.2.2.3", CVersion.parse(1.22345).toString());
			Assert.assertStrictlyEquals("4.2.0.0", CVersion.parse(4.2).toString());
			Assert.assertStrictlyEquals("6.0.0.0", CVersion.parse(6).toString());
			Assert.assertStrictlyEquals("0.0.0.0", CVersion.parse(0).toString());
			Assert.assertNull(CVersion.parse(new Object()));
			Assert.assertNull(CVersion.parse(_version));
		}
		
		[Test]
		public function testCompare():void
		{
			var ver:CVersion = new CVersion(1, 2, 3, 4);
			Assert.assertStrictlyEquals(0, _version.compare(new CVersion()));
			Assert.assertStrictlyEquals(0, ver.compare(ver));
			Assert.assertStrictlyEquals(0, ver.compare(new CVersion(1, 2, 3, 4)));
			Assert.assertTrue(-1, ver.compare(new CVersion(1, 2, 3, 5)));
			Assert.assertTrue(-1, ver.compare(new CVersion(1, 2, 4, 0)));
			Assert.assertTrue(-1, ver.compare(new CVersion(1, 3, 0, 0)));
			Assert.assertTrue(-1, ver.compare(new CVersion(2, 8, 8, 8)));
			Assert.assertTrue(1, ver.compare(new CVersion(1, 2, 3, 3)));
			Assert.assertTrue(1, ver.compare(new CVersion(1, 2, 2, 9)));
			Assert.assertTrue(1, ver.compare(new CVersion(1, 1, 8, 9)));
			Assert.assertTrue(1, ver.compare(new CVersion(0, 4, 5, 9)));
		}
	}
}
