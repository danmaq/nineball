package danmaq.nineball.flex.components
{
	import flash.events.ProgressEvent;
	
	import mx.preloaders.SparkDownloadProgressBar;
	
	/**
	 * 強制的に表示するプログレスバー。
	 * 
	 * <p>
	 * <code>SparkDownloadProgressBar</code>クラスは読み込み時間が短いと非表示のまま
	 * 進捗を終わらせてしまうことがありますが、このクラスは進捗終了の間まで強制的に表示します。
	 * </p>
	 * 
	 * @author Mc(danmaq)
	 */
	public class CForcePreloader extends SparkDownloadProgressBar
	{
		
		//* instance methods ───────────────────────────*
		
		/**
		 * ダウンロード中にダウンロードプログレスバーを
		 * 表示するかどうかを決めるアルゴリズムを定義します。 
		 * 
		 * @param elapsedTime ダウンロード段階が開始してから経過した時間（ミリ秒）。 
		 * @param event <code>bytesLoaded</code>プロパティおよび<code>bytesTotal</code>
		 * プロパティのある<code>ProgressEvent</code> オブジェクトです。
		 * @return 常に<code>true</code>。 
		 */
		override protected function showDisplayForDownloading(
			elapsedTime:int, event:ProgressEvent):Boolean
		{
			return super.showDisplayForDownloading(elapsedTime, event) || true;
		}
		
		/**
		 * ダウンロードプログレスバーが現在表示されていないことを前提として、初期化段階時に
		 * ダウンロードプログレスバーを表示するかどうかを決めるアルゴリズムを定義します。
		 * 
		 * @param elapsedTime ダウンロード段階が開始してから経過した時間（ミリ秒）。
		 * @param count アプリケーションからの<code>initProgress</code>イベントを受け取った回数です。
		 * @return 常に<code>true</code>。 
		 */
		override protected function showDisplayForInit(elapsedTime:int, count:int):Boolean
		{
			return super.showDisplayForInit(elapsedTime, count) || true;
		}
	}
}
