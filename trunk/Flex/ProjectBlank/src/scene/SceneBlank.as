package scene{

	import danmaq.nineball.task.scene.CSceneBase;

	/**
	 * ブランクなシーンです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class SceneBlank extends CSceneBase{

		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 */
		public function SceneBlank(){
			super();

			/// insert any functions... ///

		}

		/**
		 * 解放時に管理クラスから呼び出される処理です。
		 * オーバーライドする際はメソッドの最後に継承元メソッドも呼び出してください。
		 */
		public override function dispose():void{

			/// insert any functions... ///

			super.dispose();
		}
		
		/**
		 * シーンを1フレーム分動かします。
		 * オーバーライドする際はメソッドの最後に継承元メソッドを呼び出してください。
		 * 次のシーンが登録されるとfalseが返ります。もしGOSUBしたい場合、
		 * このメソッドがfalseを返したときに継承先でtrueを返してください。
		 * 
		 * @return 次のフレームもシーンが存続する場合、true
		 */
		public override function update():Boolean{

			/// insert any functions... ///

			return super.update();
		}
	}
}
