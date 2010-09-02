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
	
	import danmaq.nineball.data.CPhase;
	import danmaq.nineball.state.*;
	
	import flash.utils.Timer;

	/**
	 * フレームレート制御クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CFPSTimer extends CEntity
	{

		////////// CONSTANTS //////////

		/**	オブジェクトと状態クラスのみがアクセス可能なフィールド。 */
		private const m_privateMembers:Object = 
		{
			phase: new CPhase(),
			variable: 0,
			prevSeconds: 0,
			real: 0,
			slowdownCount: 0,
			penaltyCount: 0
		};

		////////// FIELDS //////////

		/**	FPS理論値が格納されます。 */
		[Bindable]
		public var theoretical:uint = 60;

		/**	FPS更新フレーム間隔が格納されます。 */
		[Bindable]
		public var refleshInterval:uint = 0;
		
		/**	実測内部FPSの最低許容値が格納されます。 */
		[Bindable]
		public var slowdownLimit:uint = 0;
		
		/**	実測内部FPSの最低許容値を下回る許容回数が格納されます。 */
		[Bindable]
		public var slowdownCountLimit:uint = 0;

		////////// PROPERTIES //////////

		/**
		 * ループ用タイマを取得します。
		 * 
		 * @return タイマ
		 */
		public function get timer():Timer
		{
			return new Timer(1000 / privateMembers.variable, refleshInterval);
		}
		
		/**
		 * 実測FPSを取得します。
		 * 
		 * @return 実測FPS
		 */
		public function get realFPS():uint
		{
			return privateMembers.real;
		}

		/**
		 * オブジェクトと状態クラスのみがアクセス可能なフィールドを取得します。
		 * 
		 * @return オブジェクトと状態クラスのみがアクセス可能なフィールド。
		 */
		protected override function get privateMembers():Object
		{
			return m_privateMembers;
		}
		
		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 最初の状態が設定できますが、何も指定しない場合は既定の状態
		 * (CStateFPSTimer.instance)となります。
		 * 
		 * @param firstState 最初の状態。
		 */
		public function CFPSTimer(firstState:IState = null)
		{
			if(firstState == null)
			{
				firstState = CStateFPSTimer.instance;
			}
			super(firstState);
			resetCalibration();
		}
		
		/**
		 * FPS補正をリセットします。
		 * 
		 * <p>
		 * 急激な負荷の変化が予想される時に実行してください。
		 * 注意：あまり頻繁に呼び出すと補正の効果が薄れます。
		 * </p>
		 */
		public function resetCalibration():void
		{
			privateMembers.variable = theoretical;
		}

		/**
		 * プロパティが変化されたときに呼び出されます。
		 * 
		 * @param e イベント情報
		 */
		private function onPropertyChange(e:PropertyChangeEvent):void
		{
			resetCalibration();
		}
	}
}
