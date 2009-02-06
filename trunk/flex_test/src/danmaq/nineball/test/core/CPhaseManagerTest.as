package danmaq.nineball.test.core{

	import danmaq.nineball.core.CPhaseManager;
	
	import org.libspark.as3unit.assert.*;
	import org.libspark.as3unit.test;
	
	use namespace test;

	/**
	 * フェーズ管理クラスのテストクラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CPhaseManagerTest{

		/**
		 * コンストラクタのテストを開始します。
		 */
		test function test_ctor():void{
			var obj:CPhaseManager = new CPhaseManager();
			assertSame( 0, obj.phase, "phase" );
			assertSame( 0, obj.count, "count" );
			assertSame( 0, obj.phaseStartTime, "phaseStartTime" );
			assertSame( 0, obj.phaseCount, "phaseCount" );
			assertSame( 0, obj.prevPhase, "prevPhase" );
			assertSame( -1, obj.nextPhase, "nextPhase" );
			assertFalse( obj.isReserveNextPhase, "isReserveNextPhase" );
		}

		/**
		 * phaseプロパティのテストを開始します。
		 */
		test function test_phase():void{
			var obj:CPhaseManager = new CPhaseManager();
			assertSame( 0, obj.phase, "phase" );
			obj.phase = 1;
			assertSame( 1, obj.phase, "phase" );
			assertSame( 0, obj.prevPhase, "prevPhase" );
			obj.phase = 2;
			assertSame( 2, obj.phase, "phase" );
			assertSame( 1, obj.prevPhase, "prevPhase" );
		}

		/**
		 * countプロパティのテストを開始します。
		 */
		test function test_count():void{
			var obj:CPhaseManager = new CPhaseManager();
			assertSame( 0, obj.count );
			obj.count++;
			assertSame( 1, obj.count );
			obj.count += 5;
			assertSame( 2, obj.count );
			obj.count = 0;
			assertSame( 3, obj.count );
		}

		/**
		 * phaseStartTimeプロパティのテストを開始します。
		 */
		test function test_phaseStartTime():void{
			var obj:CPhaseManager = new CPhaseManager();
			assertSame( 0, obj.phaseStartTime );
			obj.count++;
			obj.phase = 10;
			assertSame( 1, obj.phaseStartTime );
		}

		/**
		 * phaseCountプロパティのテストを開始します。
		 */
		test function test_phaseCount():void{
			var obj:CPhaseManager = new CPhaseManager();
			assertSame( 0, obj.phaseCount );
			obj.count++;
			assertSame( 1, obj.phaseCount );
			obj.count += 5;
			assertSame( 2, obj.phaseCount );
			obj.count = 0;
			assertSame( 3, obj.phaseCount );
			obj.phase = 1;
			assertSame( 0, obj.phaseCount );
			obj.count += 5;
			assertSame( 1, obj.phaseCount );
		}

		/**
		 * Resetのテストを開始します。
		 */
		test function test_reset():void{
			var obj:CPhaseManager = new CPhaseManager();
			var stat:String = obj.toString();
			obj.count++;
			obj.phase = 1;
			obj.isReserveNextPhase = true;
			assertNotSame( stat, obj.toString() );
			obj.reset();
			assertSame( stat, obj.toString() );
		}

		/**
		 * toStringのテストを開始します。
		 */
		test function test_toString():void{
			var obj:CPhaseManager = new CPhaseManager();
			assertSame( "Phase[Now:0,Next:-1,Prev:0],Count[Total:0,Phase:0]", obj.toString() );
			obj.count++;
			assertSame( "Phase[Now:0,Next:-1,Prev:0],Count[Total:1,Phase:1]", obj.toString() );
			obj.phase = 1;
			assertSame( "Phase[Now:1,Next:-1,Prev:0],Count[Total:1,Phase:0]", obj.toString() );
			obj.count++;
			assertSame( "Phase[Now:1,Next:-1,Prev:0],Count[Total:2,Phase:1]", obj.toString() );
			obj.nextPhase = 3;
			assertSame( "Phase[Now:1,Next:3,Prev:0],Count[Total:2,Phase:1]", obj.toString() );
			obj.count++;
			assertSame( "Phase[Now:3,Next:-1,Prev:1],Count[Total:3,Phase:0]", obj.toString() );
			obj.count++;
			assertSame( "Phase[Now:3,Next:-1,Prev:1],Count[Total:4,Phase:1]", obj.toString() );
			obj.isReserveNextPhase = true;
			assertSame( "Phase[Now:3,Next:4,Prev:1],Count[Total:4,Phase:1]", obj.toString() );
			obj.count++;
			assertSame( "Phase[Now:4,Next:-1,Prev:3],Count[Total:5,Phase:0]", obj.toString() );
			obj.count++;
			assertSame( "Phase[Now:4,Next:-1,Prev:3],Count[Total:6,Phase:1]", obj.toString() );
			obj.reset();
			assertSame( "Phase[Now:0,Next:-1,Prev:0],Count[Total:0,Phase:0]", obj.toString() );
		}
	}
}
