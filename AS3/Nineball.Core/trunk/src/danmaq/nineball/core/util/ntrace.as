package danmaq.nineball.core.util
{

	import danmaq.nineball.core.util.string.sprintf;

	import flash.external.ExternalInterface;
	import flash.system.Capabilities;

	/**
	 * デバッグ中に式の結果をコンソール表示します。
	 *
	 * <p>
	 * 通常はデバッグ コンソール出力しますが、ブラウザ上で動作しているかどうかを判定し、
	 * 必要に応じてJavascriptコンソールにも同様の出力をします。
	 * </p>
	 *
	 * @param args 出力する内容。
	 * @author Mc(danmaq)
	 */
	public function ntrace(...args:Array):void
	{
		var message:String = args.join(" ");
		var playerType:String = Capabilities.playerType;
		if(playerType == "ActiveX" || playerType == "PlugIn")
		{
			ExternalInterface.call(
				sprintf("( function(){ if (console != null){ console.log('%s'); } })()", message));
		}
		trace(message);
	}
}
