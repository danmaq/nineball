package danmaq.nineball.core
{

	import danmaq.nineball.core.data.CVersion;
	import danmaq.nineball.core.data.IVersion;
	import danmaq.nineball.core.util.object.blockStatic;

	/**
	 * 定数をまとめた静的クラス。
	 */
	public final class CConstants
	{

		//* constants ──────────────────────────────-*
		
		/** Nineball.Core バージョン情報。 */
		public static const VERSION:IVersion = new CVersion(1, 3, 0, 0).asReadonly;
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 *
		 * <p>
		 * このクラスは静的クラスです。このクラスのインスタンスを生成することはできません。
		 * </p>
		 */
		public function CConstants()
		{
			blockStatic();
		}
	}
}
