package danmaq.nineball.core{

	import __AS3__.vec.Vector;
	
	import mx.utils.StringUtil;

	/**
	 * シーン進行を管理するクラスです。
	 * このクラスにシーンを登録し、そしてこのクラスを通じ実行させます。
	 * 複数シーンのスタックを積むことも出来ます。
	 * (この場合、一番若いシーンが実行されます)
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CSceneManager{

		////////// CONSTANTS //////////

		/**	シーンのスタックが格納されます。 */
		private static const stack:Vector.<IScene> = new Vector.<IScene>();

		////////// PROPERTIES //////////

		/**
		 * 現在アクティブなシーンを取得します。
		 * 
		 * @return 現在アクティブなシーン。無い場合、null
		 */
		public function get scene():IScene{
			var nLength:uint = stack.length;
			return nLength > 0 ? stack[ nLength - 1 ] : null;
		}
		
		/**
		 * 現在シーンのスタック段数を取得します。
		 * 0が帰ってきた場合、何も登録されていないことを意味します。
		 * 
		 * @return スタック段数
		 */
		public function get total():uint{ return stack.length; }
		
		////////// METHODS //////////

		/**
		 * デストラクタ。
		 */
		public function dispose():void{
			while( stack.length > 0 ){ erase(); }
		}

		/**
		 * シーンを追加します。現在アクティブなシーンは
		 * 即座にスリープ状態となりスタックに蓄積されます。
		 * 
		 * @param scene シーン
		 */
		public function add( scene:IScene ):void{ stack.push( scene ); }
		
		/**
		 * 現在アクティブなシーンを強制的に終了・削除します。
		 */
		public function erase():void{
			if( stack.length > 0 ){
				scene.dispose();
				stack.pop();
			}
		}
		
		/**
		 * シーンを1フレーム分動かします。
		 */
		public function update():void{
			var _scene:IScene = scene;
			if( _scene != null ){
				var bContinue:Boolean = _scene.update(); 
				var next:IScene = _scene.nextScene;
				if( !bContinue ){ erase(); }
				if( next != null ){ add( next ); }
			}
		}

		/**
		 * フェーズなどこのクラスの状態を文字列で取得します。
		 * 
		 * @return オブジェクトのストリング表現
		 */
		public function toString():String{
			return StringUtil.substitute( "Active:{0},Stack:{1}", total, scene );
		}
	}
}
