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
	
	import danmaq.nineball.data.*;
	import danmaq.nineball.state.*;
	
	/**
	 * 自在に組み合わせのできる内分カウンタ クラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CProgrammableInterpolate extends CEntity
	{
		
		////////// CONSTANTS //////////

		/**	内分カウンタ情報一覧。 */
		public const program:Vector.<CInterpolateInfo> = new Vector.<CInterpolateInfo>();
		
		/**	フェーズ・カウンタ管理クラス。 */
		public const phase:CPhase = new CPhase();
	
		////////// FIELDS //////////

		/**	ループするかどうか。 */
		public var loop:Boolean = false;

		////////// PROPERTIES //////////
		
		/**
		 * 内分カウンタの計算結果を取得します。
		 * 
		 * @return 計算結果
		 */
		public function get now():Number
		{
			return privateMembers.result;
		}

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 最初の状態が設定できますが、何も指定しない場合は既定の状態
		 * (CStateInterpolate.instance)となります。
		 * 
		 * @param firstState 最初の状態。
		 */
		public function CProgrammableInterpolate(firstState:IState = null)
		{
			var privateMembers:Object = 
			{
				result: 0.0	// 最新の計算結果
			};
			if(firstState == null)
			{
				firstState = CStateInterpolate.instance;
			}
			super(firstState, privateMembers);
		}
		
		/**
		 * 1フレーム分の更新処理をします。
		 */
		public override function update():void
		{
			super.update();
			phase.count++;
		}
		
		/**
		 * オブジェクトを初期化します。
		 */
		public override function release():void
		{
			program.splice(0, program.length);
			phase.reset();
			super.release();
		}
	}
}
