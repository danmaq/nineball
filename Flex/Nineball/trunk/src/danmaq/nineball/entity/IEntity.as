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

	/**
	 * 状態を持つオブジェクトのインターフェイス。
	 * これを実装するか、または<c>CEntity</c>を継承することで、
	 * 状態を持つオブジェクトを作ることができます。
	 * 
	 * @author Mc(danmaq)
	 */
	public interface IEntity
	{
		
		////////// PROPERTIES //////////

		/**
		 * 最後に変化する前の状態を取得します。
		 * 
		 * @return 最後に変化する前の状態。初期値はCState.empty。
		 */
		function get previousState():IState;
		
		/**
		 * 現在の状態を取得します。
		 * 
		 * @return 現在の状態。初期値はCState.empty。
		 */
		function get currentState():IState;
		
		/**
		 * 次に変化する状態を設定します。
		 * 
		 * @return 次に変化する状態。
		 */
		function set nextState(value:IState):void;

		////////// METHODS //////////

		/**
		 * 1フレーム分の更新処理を実行します。
		 */
		function update():void;
	}
}
