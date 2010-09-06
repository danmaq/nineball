package danmaq.nineball.state
{
	import danmaq.nineball.data.*;
	import danmaq.nineball.entity.*;

	/**
	 * プログラマブル内分カウンタ クラスの既定の状態。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CStateInterpolate implements IState
	{
		
		////////// CONSTANTS //////////

		/**	インスタンス。 */
		public static const instance:CStateInterpolate = new CStateInterpolate();

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
			var cpi:CProgrammableInterpolate = CProgrammableInterpolate(entity);
			var phase:CPhase = cpi.phase;
			if(phase.phase < cpi.program.length)
			{
				var info:CInterpolateInfo = cpi.program[phase.phase];
				privateMembers.result =
					info.interpolate(info.start, info.end, phase.phaseCount, info.interval);
				phase.isReserveNextPhase = (phase.phaseCount >= info.interval);
			}
			else
			{
				if(cpi.loop)
				{
					phase.nextPhase = 0;
				}
				else
				{
					cpi.release();
				}
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
		}
	}
}
