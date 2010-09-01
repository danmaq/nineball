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
	public class CEntity implements IEntity
	{

		////////// FIELDS //////////
		
		/**	最後に変化する前の状態。 */
		protected var m_previousState:IState = CState.empty;

		/**	現在の状態。 */
		protected var m_currentState:IState = CState.empty;

		/**	汎用フレームカウンタ。 */
		protected var m_counter:uint = 0;

		/**	次の状態。 */
		private var m_nextState:IState = null;

		////////// CONSTRUCTOR //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param firstState 最初の状態。
		 */
		public function CEntity(firstState:IState = null)
		{
			nextState = firstState;
		}

		////////// PROPERTIES //////////
		
		/**
		 * 最後に変化する前の状態を取得します。
		 * 
		 * @return 最後に変化する前の状態。初期値はCState.empty。
		 */
		public function get previousState():IState
		{
			return m_previousState;
		}
		
		/**
		 * 現在の状態を取得します。
		 * 
		 * @return 現在の状態。初期値はCState.empty。
		 */
		public function get currentState():IState
		{
			return m_currentState;
		}
		
		/**
		 * 予約されている次の状態を取得します。
		 * 
		 * @return 次の状態。まだ予約されていない場合、null。
		 */
		public function get nextState():IState
		{
			return m_nextState;
		}
		
		/**
		 * 次の状態を設定します。
		 * 
		 * @param value 次の状態。
		 */
		public function set nextState(value:IState):void
		{
			m_nextState = value;
		}
		
		/**
		 * 汎用フレームカウンタを取得します。
		 * 
		 * @return 汎用フレームカウンタ。
		 */
		public function get counter():uint
		{
			return m_counter;
		}
		
		/**
		 * オブジェクトと状態クラスのみがアクセス可能なフィールドを取得します。
		 * 特に何も定義していない場合、nullを取得します。
		 * 
		 * @return オブジェクトと状態クラスのみがアクセス可能なフィールド。
		 */
		protected function get privateMembers():Object
		{
			return null;
		}
		
		////////// METHODS //////////
		
		/**
		 * 1フレーム分の更新処理をします。
		 */
		public function update():void
		{
			if(nextState != null)
			{
				commitNextState();
			}
			currentState.update(this, privateMembers);
			m_counter++;
		}

		/**
		 * オブジェクトを初期化します。
		 */
		public function release():void
		{
			nextState = CState.empty;
			commitNextState();
			m_previousState = CState.empty;
			m_counter = 0;
		}

		/**
		 * 予約された次の状態を確定します。
		 * 本来update開始時に予約が確認されたら自動でこの処理に入るため、通常このメソッドを
		 * 意図して呼ぶ必要はありませんが、あるタイミングで強制的に予約を確定したい場合などに、
		 * この処理を呼ぶことで実現できます。
		 */
		protected function commitNextState():void
		{
			if(!(nextState == null || currentState == nextState))
			{
				var _next:IState = nextState;
				nextState = null;
				currentState.teardown(this, privateMembers, _next);
				var oldCurrent:IState = currentState;
				m_previousState = currentState;
				m_currentState = _next;
				currentState.setup(this, privateMembers);
			}
			nextState = null;
		}
	}
}
