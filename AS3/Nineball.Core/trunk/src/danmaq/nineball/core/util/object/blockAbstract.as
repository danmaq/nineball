package danmaq.nineball.core.util.object
{
	
	/**
	 * 抽象クラスのコンストラクタに記述することで、抽象クラスが直接生成されることを阻止します。
	 * 
	 * @param instance 現在のインスタンス。
	 * @param constructor コンストラクタ関数への参照。
	 * @throws ArgumentError 引数がnullである、または抽象クラスの直接生成を検出した場合。
	 */
	public function blockAbstract(instance:Object, constructor:Object):void
	{
		if(instance == null || constructor == null)
		{
			throw new ArgumentError("引数が不足しています。");
		}
		if(instance.constructor == constructor)
		{
			throw new ArgumentError(
				"このオブジェクトは抽象クラスです。直接生成は許可されていません。");
		}
	}
}
