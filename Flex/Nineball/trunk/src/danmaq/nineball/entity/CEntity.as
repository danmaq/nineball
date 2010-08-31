////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.entity
{
	import danmaq.nineball.state.*;

	import flash.events.EventDispatcher;

	/**
	 * 状態を持つオブジェクトのインターフェイス。
	 * これを実装するか、またはIEntityを実装することで、
	 * 状態を持つオブジェクトを作ることができます。
	 * 
	 * また、このクラスに直接状態を持たせて使用することもできます。
	 * 
	 * @author Mc(danmaq)
	 */
	public class CEntity extends EventDispatcher implements IEntity
	{

		////////// FIELDS //////////
		
		/**	最後に変化する前の状態を取得します。 */
		protected var m_previousState:IState = CState.empty;

		/**	現在の状態を取得します。 */
		protected var m_currentState:IState = CState.empty;

		/**	このオブジェクトを所有する親オブジェクト。 */
		private var m_owner:IEntity = null;

		////////// CONSTRUCTOR //////////

		/**
		 * コンストラクタ。
		 */
		public function CEntity(firstState:IState = null, owner:IEntity = null)
		{
			if(firstState != null)
			{
				nextState = firstState;
			}
			m_owner = owner;
			super(null);
		}

		////////// PROPERTIES //////////
		
		public function get owner():IEntity
		{
			//TODO: implement function
			return null;
		}
		
		public function get previousState():IState
		{
			//TODO: implement function
			return null;
		}
		
		public function get currentState():IState
		{
			//TODO: implement function
			return null;
		}
		
		public function set nextState(value:IState):void
		{
			//TODO: implement function
		}
		
		////////// METHODS //////////
		
		public function update():void
		{
			currentState.update(this, null);
			//TODO: implement function
		}
		
	}
}
