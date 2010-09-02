package danmaq.nineball.state
{
	import danmaq.nineball.data.CScreen;
	import danmaq.nineball.entity.*;

	/**
	 * フレームレート管理クラスの既定の状態。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CStateFPSTimer implements IState
	{
		
		////////// CONSTANTS //////////

		/**	インスタンス。 */
		public static const instance:CStateFPSTimer = new CStateFPSTimer();

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
			var timer:CFPSTimer = CFPSTimer(entity);
			var bFirst:Boolean = (privateMembers.phase.count == 0); 
			var nSeconds:int = new Date().seconds;
			if(privateMembers.prevSeconds != nSeconds)
			{
				privateMembers.prevSeconds = nSeconds;
				privateMembers.real = privateMembers.phase.phaseCount;
				if(privateMembers.real < timer.slowdownLimit)
				{
					privateMembers.slowdownCount++;	// あんまり重いようだと描画FPSを半分にする
					if(privateMembers.slowdownCount == timer.slowdownCountLimit)
					{
						privateMembers.penaltyCount++;
						if(CScreen.root.screen.stage != null)
						{
							CScreen.root.screen.stage.frameRate =
								Math.min(60, timer.theoretical) / (privateMembers.penaltyCount + 1);
						}
					}
				}
				if(bFirst)
				{
					if(CScreen.root.screen.stage != null)
					{
						CScreen.root.screen.stage.frameRate = Math.min(60, timer.theoretical);
					}
				}
				else
				{
					privateMembers.variable = Math.max(1, privateMembers.variable +
							int((timer.theoretical - int(privateMembers.real)) * 0.8));
				}
				privateMembers.phase.isReserveNextPhase = true;
			}
			privateMembers.phase.count++;
		}
		
		/**
		 * オブジェクトが別の状態へ移行する時に呼び出されます。
		 * このメソッドは、遷移元のteardownよりも後に呼び出されます。
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
