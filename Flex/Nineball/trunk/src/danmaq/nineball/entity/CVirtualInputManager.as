package danmaq.nineball.entity
{
	import danmaq.nineball.data.*;
	import danmaq.nineball.state.*;
	
	import flash.events.*;
	import flash.geom.Point;

	/**
	 * 仮想ボタン入力を管理するクラスです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CVirtualInputManager extends CEntity
	{

		////////// FIELDS //////////

		/**	オブジェクトと状態クラスのみがアクセス可能なフィールド。 */
		private var m_privateMembers:Object = 
		{
			addedEventListener: false,
			viData: null,
			buffer: new Array()
			
		};

		////////// PROPERTIES //////////

		/**
		 * 仮想ボタン入力状態の一覧を取得します。
		 * 
		 * @return 仮想ボタン入力状態の一覧
		 */
		public function get inputTable():Vector.<Boolean>
		{
			var result:Vector.<Boolean> = new Vector.<Boolean>();
			for each(var data:CVirtualInput in privateMembers.viData)
			{
				result.push(data.hold);
			}
			return result;
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
		 * 
		 * @param firstState 最初の状態。
		 */
		public function CVirtualInputManager(firstState:IState = null)
		{
			if(firstState == null)
			{
				firstState = CStateVirtualInput.instance;
			}
			super(firstState);
		}
		
		/**
		 * 仮想ボタンを初期状態に戻します。
		 */
		public function resetVI():void
		{
			privateMembers.viData = new Vector.<CVirtualInput>();
			privateMembers.buffer = new Array();
		}
		
		/**
		 * 仮想ボタンを追加します。
		 * 
		 * @param vkData 仮想ボタン情報構造体
		 * @return 仮想ボタンID
		 */
		public function addVI(viData:CVirtualInput):uint
		{
			privateMembers.viData.push(viData);
			return privateMembers.viData.length;
		}

		/**
		 * 仮想ボタンを取得します。
		 * 
		 * @param uVIID 仮想ボタンID
		 * @return 仮想ボタン情報構造体
		 */
		public function getVI(uVIID:uint):CVirtualInput
		{
			return privateMembers.viData[uVIID];
		}

		/**
		 * 強制的にキーを押下または解放させます。
		 * 
		 * @param uKeyCode キーコード
		 * @param bPush 押下かどうか
		 */
		public function forceKeyChange(uKeyCode:uint, bPush:Boolean):void
		{
			var uLength:uint = privateMembers.viData.length;
			for(var i:uint = 0; i < uLength; i++)
			{
				if(privateMembers.viData[i].findKey(uKeyCode))
				{
					forceVIChange(i, bPush);
				}
			}
		}

		/**
		 * 強制的に所定位置にマウスボタンを押下または解放させます。
		 * カーソル自体は移動しません。
		 * 
		 * @param pos 座標
		 * @param bPush 押下かどうか
		 */
		public function forceMouseChange(pos:Point, bPush:Boolean):void
		{
			var uLength:uint = privateMembers.viData.length;
			for(var i:uint = 0; i < uLength; i++)
			{
				if(privateMembers.viData[i].isHitArea(pos))
				{
					forceVIChange(i, bPush);
				}
			}
		}

		/**
		 * 強制的に仮想ボタンを押下または解放させます。
		 * 
		 * @param uVIID 仮想ボタンID
		 * @param bPush 押下かどうか
		 */
		public function forceVIChange(uVIID:uint, bPush:Boolean):void
		{
			var data:Object = 
			{
				id: uVIID,
				push: bPush
			};
			privateMembers.buffer.push(data);
		}

		/**
		 * マウスボタンを押すイベントが発生した時にコールバックされます。
		 * 
		 * @param event キーボードイベント パラメータ
		 */
		public function onMouseDown(event:MouseEvent):void
		{
			forceMouseChange(new Point(event.stageX, event.stageY), true); 
		}

		/**
		 * マウスボタンを離すイベントが発生した時にコールバックされます。
		 * 
		 * @param event キーボードイベント パラメータ
		 */
		public function onMouseUp(event:MouseEvent):void
		{
			forceMouseChange(new Point(event.stageX, event.stageY), false); 
		}

		/**
		 * キーを押すイベントが発生した時にコールバックされます。
		 * 
		 * @param event キーボードイベント パラメータ
		 */
		public function onKeyDown(event:KeyboardEvent):void
		{
			forceKeyChange(event.keyCode, true);
		}

		/**
		 * キーを離すイベントが発生した時にコールバックされます。
		 * 
		 * @param event キーボードイベント パラメータ
		 */
		public function onKeyUp(event:KeyboardEvent):void
		{
			forceKeyChange(event.keyCode, false);
		}
	}
}
