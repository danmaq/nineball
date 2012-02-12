package danmaq.nineball.core.util.object
{

	/**
	 * Singletonクラスなどで、インスタンスが外部からの操作により余計に生成されることを阻止します。
	 * 
	 * ロジックとしては引数がすべてnullでない場合にSingleton違反と見なし例外を発生させます。
	 * 原理的に内部における余計な生成の場合、対応しきれない場合があります。
	 * 
	 * @param expr 現在のインスタンス。
	 * @param args Singletonオブジェクト一覧。
	 * @throws ArgumentError 引数が不足しているか、またはSingleton違反を検出した場合。
	 */
	public function blockDuplicate(expr:Object, ...args:Array):void
	{
		var length:int = args.length;
		if(expr == null || length == 0)
		{
			throw new ArgumentError("引数が不足しています。");
		}
		var isNull:Boolean = false;
		for(var i:int = length; --i >= 0; )
		{
			isNull = args[i] == null;
			if(isNull)
			{
				break;
			}
		}
		if(!isNull)
		{
			throw new ArgumentError(
				"このオブジェクトはSingletonクラスです。直接生成は許可されていません。");
		}
	}
}
