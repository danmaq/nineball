package danmaq.nineball.core.data
{

	import danmaq.nineball.core.component.IDisposable;
	import danmaq.nineball.core.util.string.sprintf;
	
	/**
	 * バージョン情報を保持、比較するためのクラス。
	 * 
	 * <p>
	 * 4桁のドット10進表記のみをサポートしています。例：1.0.12.150
	 * </p>
	 *
	 * @author Mc(danmaq)
	 * @see #parse()
	 */
	public final class CVersion implements IVersion, IDisposable
	{
		
		//* constants ──────────────────────────────-*
		
		/**	バージョン情報の文字表現のために使用する既定のフォーマット。 */
		public static const DEFAULT_FORMAT:String = "%d.%d.%d.%d";
		
		//* fields ────────────────────────────────*
		
		/**	メジャー バージョン番号。 */
		private var _major:int;
		
		/**	マイナ バージョン番号。 */
		private var _minor:int;
		
		/**	リビジョン バージョン番号。 */
		private var _revision:int;
		
		/**	ビルド バージョン番号。 */
		private var _build:int;

		/**	バージョン情報の文字表現のために使用するフォーマット。 */
		private var _format:String = DEFAULT_FORMAT;
		
		/**	書き換え不能インスタンス。 */
		private var _asReadonly:IVersion;
		
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
			_asReadonly = new CVersionWrapper(this);
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * メジャー バージョン番号を取得します。
		 */
		public function get major():int
		{
			return _major;
		}
		
		/**	@private */
		public function set major(value:int):void
		{
			_major = value;
		}
		
		/**
		 * マイナ バージョン番号を取得します。
		 */
		public function get minor():int
		{
			return _minor;
		}
		
		/**	@private */
		public function set minor(value:int):void
		{
			_minor = value;
		}
		
		/**
		 * リビジョン バージョン番号を取得します。
		 */
		public function get revision():int
		{
			return _revision;
		}
		
		/**	@private */
		public function set revision(value:int):void
		{
			_revision = value;
		}
		
		/**
		 * ビルド バージョン番号を取得します。
		 */
		public function get build():int
		{
			return _build;
		}
		
		/**	@private */
		public function set build(value:int):void
		{
			_build = value;
		}
		
		/**
		 * バージョン情報の文字表現のために使用するフォーマットを取得および設定します。
		 * 
		 * <p>フォーマットには、<code>sprintf</code>形式を使用します。</p>
		 *
		 * @default CVersion.DEFAULT_FORMAT
		 * @see #DEFAULT_FORMAT
		 * @see danmaq.nineball.core.util.string.#sprintf()
		 * @see http://ja.wikipedia.org/wiki/Printf
		 */
		public function get format():String
		{
			return _format;
		}
		
		/**
		 * @private
		 */
		public function set format(value:String):void
		{
			createVersionString(value);
			_format = value;
		}
		
		/**
		 * 読み込み専用のラッパー インスタンスを取得します。
		 * 
		 * <p>
		 * このプロパティは、インスタンスごとに常に同一のオブジェクトを取得します。
		 * </p>
		 */
		public function get asReadonly():IVersion
		{
			return _asReadonly;
		}
		
		//* class methods ────────────────────────────-*
		
		/**
		 * 値をパースしてバージョン情報を生成します。
		 * 
		 * <p>
		 * パース可能なデータは下記のとおりです。
		 * </p>
		 * <table>
		 * <tr>
		 * 	<th>例</th><th>備考</th>
		 * </tr>
		 * <tr>
		 * 	<td>"1.0.21.150"</td><td>ドット10進数表記。</td>
		 * </tr>
		 * <tr>
		 * 	<td>"1.0.21_150"</td><td>アンダースコアもドットと認識します。</td>
		 * </tr>
		 * <tr>
		 * 	<td>"1.0.21.112_150"</td><td>4桁以降は切り捨てられ、1.0.21.112と認識されます。</td>
		 * </tr>
		 * <tr>
		 * 	<td>"1.0.5"</td><td>内部で1.0.5.0と変換されます。</td>
		 * </tr>
		 * <tr>
		 * 	<td>"2.77"</td><td>内部で2.77.0.0と変換されます。</td>
		 * </tr>
		 * <tr>
		 * 	<td>1.21</td><td>内部で1.2.1.0と変換されます。</td>
		 * </tr>
		 * <tr>
		 * 	<td>1.2155</td><td>小数第4位以降は切り捨てられ、内部で1.2.1.5と変換されます。</td>
		 * </tr>
		 * <tr>
		 * 	<td>11</td><td>内部で11.0.0.0と変換されます。</td>
		 * </tr>
		 * </table>
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
				var num:Number = Number(version);
				if(!isNaN(num))
				{
					result = parseNumber(num);
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
			var vlist:Array = version.split(/[\._]/);
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
		
		/**
		 * バージョンを比較します。
		 *
		 * @param target 比較対象のバージョン。
		 * @return <code>target</code>よりバージョンが高い場合、正数値。逆に低い場合負数値。
		 * 同一である場合、<code>0</code>。
		 */
		public function compare(target:IVersion):int
		{
			var result:int = major - target.major;
			if(result == 0)
			{
				result = minor - target.minor;
				if(result == 0)
				{
					result = revision - target.revision;
					if(result == 0)
					{
						result = build - target.build;
					}
				}
			}
			return result;
		}
		
		/**
		 * バージョン情報の文字列表現を取得します。
		 * 
		 * @return バージョン情報の文字列表現。
		 * @see #format
		 */
		public function toString():String
		{
			return createVersionString(format);
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
			_format = DEFAULT_FORMAT;
		}
		
		/**
		 * 指定したフォーマットでバージョン情報の文字列表現を取得します。
		 *
		 * <p>フォーマットには、<code>sprintf</code>形式を使用します。</p>
		 * 
		 * @param format バージョン情報の文字表現のために使用するフォーマット。
		 * @return バージョン情報の文字列表現。
		 * @see danmaq.nineball.core.util.string.#sprintf()
		 * @see http://ja.wikipedia.org/wiki/Printf
		 */
		private function createVersionString(format:String):String
		{
			return sprintf(format, major, minor, revision, build);
		}
	}
}
