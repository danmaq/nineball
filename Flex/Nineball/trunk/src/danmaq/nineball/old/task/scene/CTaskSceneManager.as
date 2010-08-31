////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.old.task.scene
{

	import mx.utils.StringUtil;
	import danmaq.nineball.old.task.CTaskBase;

	/**
	 * シーン進行を管理するクラスです。
	 * 
	 * <p>
	 * このクラスにシーンを登録し、そしてこのクラスを通じ実行させます。
	 * 複数シーンのスタックを積むことも出来ます。
	 * (この場合、一番若いシーンが実行されます)
	 * </p>
	 * 
	 * @see danmaq.nineball.core.IScene
	 * @author Mc(danmaq)
	 */
	public final class CTaskSceneManager extends CTaskBase
	{

		////////// CONSTANTS //////////

		/**	シーンのスタックが格納されます。 */
		private static const stack:Vector.<IScene> = new Vector.<IScene>();

		////////// PROPERTIES //////////

		/**
		 * 現在アクティブなシーンを取得します。
		 * 
		 * @return 現在アクティブなシーン。無い場合、null
		 */
		public function get scene():IScene
		{
			var nLength:uint = stack.length;
			return nLength > 0 ? stack[nLength - 1] : null;
		}
		
		/**
		 * 現在シーンのスタック段数を取得します。
		 * 0が帰ってきた場合、何も登録されていないことを意味します。
		 * 
		 * @return スタック段数
		 */
		public function get total():uint
		{
			return stack.length;
		}
		
		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param uLayer レイヤ番号
		 */
		public function CTaskSceneManager(uLayer:uint = 0)
		{
			layer = uLayer;
		}
		
		/**
		 * デストラクタ。
		 */
		public override function dispose():void
		{
			while(stack.length > 0)
			{
				erase();
			}
			super.dispose();
		}

		/**
		 * シーンを追加します。現在アクティブなシーンは
		 * 即座にスリープ状態となりスタックに蓄積されます。
		 * 
		 * @param scene シーン
		 */
		public function add(scene:IScene):void
		{
			stack.push(scene);
		}
		
		/**
		 * 現在アクティブなシーンを強制的に終了・削除します。
		 */
		public function erase():void
		{
			if(stack.length > 0)
			{
				scene.dispose();
				stack.pop();
			}
		}
		
		/**
		 * シーンを1フレーム分動かします。
		 * 
		 * @return 次フレームまでの間生存し続ける場合、true
		 */
		public override function update():Boolean
		{
			var _scene:IScene = scene;
			if(_scene != null)
			{
				var bContinue:Boolean = _scene.update(); 
				var next:IScene = _scene.nextScene;
				if(!bContinue)
				{
					erase();
				}
				if(next != null)
				{
					_scene.nextScene = null;
					add(next);
				}
			}
			return super.update();
		}

		/**
		 * フェーズなどこのクラスの状態を文字列で取得します。
		 * 
		 * @return オブジェクトのストリング表現
		 */
		public function toString():String
		{
			return StringUtil.substitute("Active:{0},Stack:{1}", total, scene);
		}
	}
}
