package danmaq.ball.task{
	
	import danmaq.ball.resource.*;
	import danmaq.nineball.core.*;
	import danmaq.nineball.struct.font.CFontTransform;
	import danmaq.nineball.task.CTaskFont;
	
	import flash.geom.Point;
	
	import mx.utils.StringUtil;

	public final class CTaskScore implements ITask{

		////////// CONSTANTS //////////

		/**	スコア表示用フォントタスクが格納されます。 */
		private const taskFontScore:CTaskFont =
			new CTaskFont(CResource.font, CResource.screen, CONST.LAYER_TEXT);

		/**	ハイスコア表示用フォントタスクが格納されます。 */
		private const taskFontHiScore:CTaskFont =
			new CTaskFont(CResource.font, CResource.screen, CONST.LAYER_TEXT);

		////////// FIELDS //////////

		/**	タスク管理クラスが格納されます。 */
		private var m_taskManager:CTaskManager = null;

		/**	ハイスコアが格納されます。 */
		private var m_uHighScore:uint = 0;
		
		/**	最新のスコアが格納されます。 */
		private var m_uScore:uint = 0;
		
		////////// PROPERTIES //////////

		/**
		 * レイヤ値を取得します。
		 * 
		 * @return レイヤ値
		 */
		public function get layer():uint{ return CONST.LAYER_TEXT; }
		
		/**
		 * タスク管理クラスを設定します。
		 * 
		 * @param value タスク管理クラス
		 */
		public function set manager(value:CTaskManager):void{ m_taskManager = value; }
		
		/**
		 * 一時停止に対応しているかどうかレイヤ値を設定します。
		 * 
		 * @param value 一時停止に対応しているかどうか
		 */
		public function get isAvailablePause():Boolean{ return false; }

		/**
		 * 最新のスコア値を取得します。
		 * 
		 * @return 最新のスコア値
		 */
		public function get score():uint{ return m_uScore; }

		/**
		 * ハイスコア値を取得します。
		 * 
		 * @return ハイスコア値
		 */
		public function get hiScore():uint{ return m_uScore; }

		/**
		 * 最新のスコア文字列を取得します。
		 * 
		 * @return 最新のスコア文字列
		 */
		public function get scoreString():String{
			return StringUtil.substitute("SCORE    : {0}", score);
		}

		/**
		 * ハイスコア文字列を取得します。
		 * 
		 * @return ハイスコア文字列
		 */
		public function get hiScoreString():String{
			return StringUtil.substitute("HI-SCORE : {0}", hiScore);
		}

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 */
		public function CTaskScore(){
			taskFontScore.transform = new CFontTransform(
				null, null, 0, 1, 0x808080, false, 1,
				CFontTransform.TOP_LEFT, CFontTransform.TOP_LEFT);
			taskFontHiScore.transform = new CFontTransform(
				new Point(0, 16), null, 0, 1, 0x808080, false, 1,
				CFontTransform.TOP_LEFT, CFontTransform.TOP_LEFT);
		}
		
		/**
		 * コンストラクタの後、タスクが管理クラスに登録された直後に、
		 * 1度だけ自動的に呼ばれます。
		 */
		public function initialize():void{
			m_taskManager.add(taskFontScore);
			m_taskManager.add(taskFontHiScore);
			flush();
		}
		
		/**
		 * 解放時に管理クラスから呼び出される処理です。
		 */
		public function dispose():void{
			if(m_taskManager != null){
				m_taskManager.eraseTask(taskFontScore);
				m_taskManager.eraseTask(taskFontHiScore);
			}
		}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return 無条件にtrue
		 */
		public function update():Boolean{
			if(m_uHighScore < m_uScore){ m_uHighScore = m_uScore; }
			return true;
		}
		
		/**
		 * スコアを加算します。
		 * 
		 * @param uScore スコア加算値
		 */
		public function add(uScore:uint):void{
			if(uScore > 0){
				m_uScore += uScore;
				flush();
			}
		}
		
		/**
		 * フォントを強制的に更新します。
		 */
		public function flush():void{
			if(m_taskManager != null){
				taskFontScore.text = scoreString;
				taskFontScore.view = true;
				taskFontScore.render();
				taskFontHiScore.text = hiScoreString;
				taskFontHiScore.view = true;
				taskFontHiScore.render();
			}
		}

		/**
		 * このクラスの状態を文字列で取得します。
		 * 
		 * @return オブジェクトのストリング表現
		 */
		public function toString():String{
			return StringUtil.substitute("{0}\n{1}", scoreString, hiScoreString);
		}

	}
}
