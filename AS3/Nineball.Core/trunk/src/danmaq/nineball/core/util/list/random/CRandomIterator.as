package danmaq.nineball.core.util.list.random
{

	import danmaq.nineball.core.util.list.iterator.IIterator;

	/**
	 * 疑似乱数ジェネレータの反復子クラス。
	 *
	 * @author Mc(danmaq)
	 */
	internal final class CRandomIterator implements IIterator
	{

		//* fields ────────────────────────────────*

		/** 疑似乱数ジェネレータ。 */
		private var _random:CRandom;

		//* constructor & destructor ───────────────────────*

		/**
		 * コンストラクタ。
		 *
		 * @param random 疑似乱数ジェネレータ。
		 */
		public function CRandomIterator(random:CRandom)
		{
			_random = random;
		}

		//* instance properties ─────────────────────────-*

		/**
		 * 次の値を取得します。
		 *
		 * @return 値。
		 */
		public function get next():*
		{
			return _random.next;
		}

		/**
		 * 次の値が取得可能かどうかを取得します。
		 *
		 * 乱数に終端は存在しないため、このメソッドは常にtrueを返します。
		 *
		 * @return 常にtrue。
		 */
		public function get hasNext():Boolean
		{
			return true;
		}

		//* instance methods ───────────────────────────*

		/**
		 * 反復子をリセットし、再度次の値を取得可能な状態にします。
		 *
		 * @return リセットに成功した場合、true。
		 */
		public function reset():Boolean
		{
			return false;
		}
	}
}
