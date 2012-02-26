package danmaq.nineball.core.util.list.random
{

	import danmaq.nineball.core.util.object.blockStatic;

	/**
	 * 疑似乱数ジェネレータを静的に保持するクラス。
	 *
	 * @author Mc(danmaq)
	 */
	public final class CStaticRandom
	{

		//* fields ────────────────────────────────*

		/**
		 * 擬似乱数ジェネレータ。既定の状態ではSFMT法を用いた疑似乱数ジェネレータが格納されています。
		 */
		public static var random:IRandom = new CSFMT();

		//* constructor & destructor ───────────────────────*

		/**
		 * コンストラクタ。
		 *
		 * <p>
		 * このクラスは静的クラスです。このクラスのインスタンスを生成することはできません。
		 * </p>
		 */
		public function CStaticRandom()
		{
			blockStatic();
		}
	}
}
