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

	import danmaq.nineball.state.IState;
	
	import flash.display.Sprite;

	/**
	 * 状態を持った、スプライト クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public class CSpriteEntity extends Sprite implements IEntity
	{

		////////// FIELDS //////////

		/**	AIのためのオブジェクト。 */
		protected var ai:CEntity = null;

		/**	AIのためのプライベート メンバの控え。 */
		protected var privateMembers:Object = null;

		////////// PROPERTIES //////////
		
		/**
		 * 最後に変化する前の状態を取得します。
		 * 
		 * @return 最後に変化する前の状態。初期値はCState.empty。
		 */
		public function get previousState():IState
		{
			return ai.previousState;
		}
		
		/**
		 * 現在の状態を取得します。
		 * 
		 * @return 現在の状態。初期値はCState.empty。
		 */
		public function get currentState():IState
		{
			return ai.currentState;
		}
		
		/**
		 * 予約されている次の状態を取得します。
		 * 
		 * @return 次の状態。まだ予約されていない場合、null。
		 */
		public function get nextState():IState
		{
			return ai.nextState;
		}
		
		/**
		 * 次の状態を設定します。
		 * IButtonStateを実装している必要があります。
		 * 
		 * @param value 次の状態。
		 */
		public function set nextState(value:IState):void
		{
			ai.nextState = value;
		}

		/**
		 * 汎用フレームカウンタを取得します。
		 * 
		 * @return 汎用フレームカウンタ。
		 */
		public function get counter():uint
		{
			return ai.counter;
		}
		
		////////// METHODS //////////
		
		/**
		 * コンストラクタ。
		 * privateMembersには、自動的にownerプロパティにこのインスタンスが設定されます。
		 * 
		 * @param firstState 最初のAIの状態。
		 * @param privateMembers AIのためのプライベート メンバ。
		 */
		public function CSpriteEntity(firstState:IState = null, privateMembers:Object = null)
		{
			super();
			if(privateMembers == null)
			{
				privateMembers = new Object();
			}
			privateMembers.owner = this;
			this.privateMembers = privateMembers;
			ai = new CEntity(firstState, privateMembers);
		}
		
		/**
		 * 1フレーム分の更新処理をします。
		 */
		public function update():void
		{
			ai.update();
		}
		
		/**
		 * AIを初期化します。
		 */
		public function release():void
		{
			ai.release();
		}
	}
}
