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
	 * 状態表現のためのインターフェイス。
	 * これを実装するか、CStateを継承することで、状態を表現することが出来ます。
	 * 
	 * @author Mc(danmaq)
	 */
	public interface IState
	{
		
		////////// METHODS //////////
		
		/**
		 * 状態が開始された時に呼び出されます。
		 * このメソッドは、遷移元のteardownよりも後に呼び出されます。
		 * 
		 * @param entity この状態を適用されたオブジェクト。
		 * @param privateMembers オブジェクトと状態クラスのみがアクセス可能なフィールド。
		 */
		function setup(entity:IEntity, privateMembers:Object):void;
		
		/**
		 * 1フレーム分の更新処理を実行します。
		 * 
		 * @param entity この状態を適用されたオブジェクト。
		 * @param privateMembers オブジェクトと状態クラスのみがアクセス可能なフィールド。
		 */
		function update(entity:IEntity, privateMembers:Object):void;

		/**
		 * オブジェクトが別の状態へ移行する時に呼び出されます。
		 * このメソッドは、遷移元のteardownよりも後に呼び出されます。
		 * 
		 * @param entity この状態を適用されたオブジェクト。
		 * @param privateMembers オブジェクトと状態クラスのみがアクセス可能なフィールド。
		 * @param nextState オブジェクトが次に適用する状態。
		 */
		function teardown(entity:IEntity, privateMembers:Object, nextState:IState):void;
	}
}
