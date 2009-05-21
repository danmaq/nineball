package danmaq.nineball.test{

	import danmaq.nineball.test.core.*;
	import danmaq.nineball.test.task.*;
	
	import org.libspark.as3unit.runners.Suite;

	/**
	 * テストスイート定義クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class AllTests{

		////////// CONSTANTS //////////

		public static const RunWith:Class = Suite;

		public static const SuiteClasses:Array =[
			CPhaseManagerTest,
		]; 
	}
}
