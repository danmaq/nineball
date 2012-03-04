package danmaq.nineball.core.util.string
{
	import danmaq.nineball.core.component.IDisposable;
	
	/**
	 * バージョン情報を保持、比較するためのクラス。
	 * 
	 * <p>
	 * 4桁のドット10進表記のみをサポートしています。例：1.0.12.150
	 * </p>
	 *
	 * @author Mc(danmaq)
	 */
	public final class CVersion implements IDisposable
	{
		
		//* fields ────────────────────────────────*
		
		/**	メジャー バージョン番号。 */
		public var major:int;
		
		/**	マイナ バージョン番号。 */
		public var minor:int;
		
		/**	リビジョン バージョン番号。 */
		public var revision:int;
		
		/**	ビルド バージョン番号。 */
		public var build:int;

		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 
		 * @param major メジャー バージョン番号。
		 * @param minor マイナ バージョン番号。
		 * @param revision リビジョン バージョン番号。
		 * @param build ビルド バージョン番号。
		 */
		public function CVersion(major:int = 0, minor:int = 0, revision:int = 0, build:int = 0)
		{
			this.major = major;
			this.minor = minor;
			this.revision = revision;
			this.build = build;
		}
		
		//* class methods ────────────────────────────-*
		
		/**
		 * 値をパースしてバージョン情報を生成します。
		 * 
		 * <p>
		 * パース可能な文字列は4桁以内のドット10進数表記の文字列
		 * ("1.0.12.150"または"1.0.12_150"、或いは"1.0.12")、
		 * または整数および第3位までの小数に限ります。
		 * </p>
		 * 
		 * @param version パースするデータ。
		 * @return build ビルド バージョン番号。
		 */
		public static function parse(version:*):CVersion
		{
			var result:CVersion = null;
			var str:String = version as String;
			if(str == null)
			{
				if(!isNaN(Number(version)))
				{
					
				}
			}
			else
			{
				result = parseString(str);
			}
			return result;
		}
		
		/**
		 * 文字列値をパースしてバージョン情報を生成します。
		 * 
		 * @param version パースする文字列値。
		 * @return build ビルド バージョン番号。
		 */
		private static function parseString(version:String):CVersion
		{
			var vlist:Array = version.split(/\._/);
			while(vlist.length < 4)
			{
				vlist.push(0);
			}
			for(var i:int = 4; --i >= 0; )
			{
				var v:Number = Number(vlist[i]);
				vlist[i] = isNaN(v) ? 0 : v;
			}
			return new CVersion(vlist[0], vlist[1], vlist[2], vlist[3]);
		}
		
		/**
		 * 小数値をパースしてバージョン情報を生成します。
		 * 
		 * @param version パースする小数値。
		 * @return build ビルド バージョン番号。
		 */
		private static function parseNumber(version:Number):CVersion
		{
			var result:CVersion = new CVersion();
			result.major = version >> 0;
			result.minor = (version * 10 >> 0) % 10;
			result.revision = (version * 100 >> 0) % 10;
			result.build = (version * 1000 >> 0) % 10;
			return result;
		}
		
		//* instance methods ───────────────────────────*
		
		public function compare():int
		{
			/* ... */
			return 0;
		}
		
		/**
		 * オブジェクトの文字列表現を取得します。
		 *
		 * @return オブジェクトの文字列表現。
		 */
		public function toString():String
		{
			// TODO : フォーマットを任意に変更可能にする
			return sprintf("%d.%d.%d.%d", major, minor, revision, build);
		}
		
		/**
		 * 情報を破棄します。
		 */
		public function dispose():void
		{
			major = 0;
			minor = 0;
			revision = 0;
			build = 0;
		}
	}
}
