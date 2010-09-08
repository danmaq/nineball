package danmaq.nineball.state
{
	import danmaq.nineball.constant.CSentence;
	import danmaq.nineball.data.CScreen;
	import danmaq.nineball.data.CVirtualInput;
	import danmaq.nineball.entity.*;
	
	import flash.events.*;
	
	/**
	 * キー入力管理クラスの既定の状態。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CStateVirtualInput implements IState
	{
		
		////////// CONSTANTS //////////

		/**	インスタンス。 */
		public static const instance:CStateVirtualInput = new CStateVirtualInput();

		////////// METHODS //////////

		/**
		 * 状態が開始された時に呼び出されます。
		 * このメソッドは、遷移元のteardownよりも後に呼び出されます。
		 * 
		 * @param entity この状態を適用されたオブジェクト。
		 * @param privateMembers オブジェクトと状態クラスのみがアクセス可能なフィールド。
		 */
		public function setup(entity:IEntity, privateMembers:Object):void
		{
			if(CScreen.stage == null)
			{
				throw new Error(CSentence.ILLEGAL_SCREEN);
			}
			var vim:CVirtualInputManager = CVirtualInputManager(entity);
			CScreen.stage.addEventListener(KeyboardEvent.KEY_DOWN, vim.onKeyDown);
			CScreen.stage.addEventListener(KeyboardEvent.KEY_UP, vim.onKeyUp);
			CScreen.stage.addEventListener(MouseEvent.MOUSE_DOWN, vim.onMouseDown);
			CScreen.stage.addEventListener(MouseEvent.MOUSE_UP, vim.onMouseUp);
			privateMembers.addedEventListener = true;
		}

		/**
		 * 1フレーム分の更新処理を実行します。
		 * 
		 * @param entity この状態を適用されたオブジェクト。
		 * @param privateMembers オブジェクトと状態クラスのみがアクセス可能なフィールド。
		 */
		public function update(entity:IEntity, privateMembers:Object):void
		{
			var vim:CVirtualInputManager = CVirtualInputManager(entity);
			var prev:Vector.<Boolean> = vim.inputTable;
			while(privateMembers.buffer.length > 0)
			{
				var data:Object = privateMembers.buffer.pop();
				prev[data.id] = data.push;
			}
			var uLength:uint = privateMembers.viData.length;
			var viData:Vector.<CVirtualInput> = privateMembers.viData;
			for(var i:uint = 0; i < uLength; i++)
			{
				viData[i].update(prev[i]);
			}
		}
		
		/**
		 * オブジェクトが別の状態へ移行する時に呼び出されます。
		 * このメソッドは、遷移先のsetupよりも先に呼び出されます。
		 * 
		 * @param entity この状態を適用されたオブジェクト。
		 * @param privateMembers オブジェクトと状態クラスのみがアクセス可能なフィールド。
		 * @param nextState オブジェクトが次に適用する状態。
		 */
		public function teardown(
			entity:IEntity, privateMembers:Object, nextState:IState):void
		{
			if(privateMembers.addedEventListener)
			{
				var vim:CVirtualInputManager = CVirtualInputManager(entity);
				CScreen.stage.removeEventListener(KeyboardEvent.KEY_DOWN, vim.onKeyDown);
				CScreen.stage.removeEventListener(KeyboardEvent.KEY_UP, vim.onKeyUp);
				CScreen.stage.removeEventListener(MouseEvent.MOUSE_DOWN, vim.onMouseDown);
				CScreen.stage.removeEventListener(MouseEvent.MOUSE_UP, vim.onMouseUp);
				privateMembers.addedEventListener = false;
			}
		}
		
	}
}
