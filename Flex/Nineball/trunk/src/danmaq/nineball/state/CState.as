////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.state
{
	import danmaq.nineball.entity.IEntity;

	/**
	 * 状態表現のための空の基底クラス。
	 * これを実装するか、IStateを実装することで、状態を表現することが出来ます。
	 * 
	 * @author Mc(danmaq)
	 */
	public class CState implements IState
	{
		
		////////// CONSTANTS //////////

		/**	実装された、既定の空の状態。 */
		public static const empty:CState = new CState();

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
		}

		/**
		 * 1フレーム分の更新処理を実行します。
		 * 
		 * @param entity この状態を適用されたオブジェクト。
		 * @param privateMembers オブジェクトと状態クラスのみがアクセス可能なフィールド。
		 */
		public function update(entity:IEntity, privateMembers:Object):void
		{
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
		}
	}
}
