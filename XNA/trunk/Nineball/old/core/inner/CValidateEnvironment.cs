////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using danmaq.nineball.util.caps;

namespace danmaq.nineball.old.core.inner
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>システム環境検証クラス。</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete]
	class CValidateEnvironment : IDisposable
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>システム環境検証を通過出来たかどうか。</summary>
		public readonly bool AVALIABLE;

		/// <summary>エラーメッセージ。</summary>
		public readonly string ERROR_MESSAGE;

		/// <summary>.NET Framework 2.0のバージョン番号。</summary>
		private readonly Version DOTNET_VERSION_20 = new Version(2, 0, 50727, 42);

		/// <summary>.NET Framework 2.0 SP1のバージョン番号。</summary>
		private readonly Version DOTNET_VERSION_20SP1 = new Version(2, 0, 50727, 1433);

		/// <summary>.NET Framework 3.0のバージョン番号。</summary>
		private readonly Version DOTNET_VERSION_30 = new Version(3, 0, 4506, 30);

#if WINDOWS
		/// <summary>Windows XPのバージョン番号。</summary>
		private readonly Version WINDOWS_VERSION_XP = new Version(5, 1, 2600);

#endif

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CValidateEnvironment()
		{
			try
			{
				AVALIABLE = (
#if WINDOWS
Environment.UserInteractive &&
#endif
 isAvaliablePlatform && isAvaliableDotNet20SP1);
				if(!AVALIABLE)
				{
					ERROR_MESSAGE =
						"OSのバージョン、または.NET Frameworkのバージョンが古いため、実行することが出来ません。" + Environment.NewLine + Environment.NewLine;
				}
				ERROR_MESSAGE += createErrorMessage(isAvaliablePlatform, "Microsoft Windows XP SP3以降 または Microsoft WindowsVista SP1以降 または Microsoft XBOX360");
				ERROR_MESSAGE += createErrorMessage(isAvaliableDotNet20,
					".NET Framework 2.0", "Microsoft .NET Framework 2.0以降");
				ERROR_MESSAGE += createErrorMessage(isAvaliableDotNet20SP1,
					".NET Framework 2.0 SP1", "Microsoft .NET Framework 2.0以降");
#if WINDOWS
				if(!Environment.UserInteractive)
				{
					ERROR_MESSAGE += "× ユーザ非対話モードで起動しようとしました。このアプリケーションはユーザ対話モードでのみ実行可能です。";
				}
#endif
			}
			catch(Exception e)
			{
				ERROR_MESSAGE += e.ToString();
				AVALIABLE = false;
			}
			ERROR_MESSAGE += string.Format("この環境で {0} を動かすことが{1}。",
				Properties.Resources.NAME, AVALIABLE ? "可能です" : "このままでは不可能です");
		}

		//* -----------------------------------------------------------------------*
		/// <summary>デストラクタ。</summary>
		~CValidateEnvironment()
		{
			Dispose();
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>実行中の.NET Frameworkのバージョンが2.0以降かどうか。</summary>
		public bool isAvaliableDotNet20
		{
			get
			{
				return Environment.Version >= DOTNET_VERSION_20;
			}
		}

		/// <summary>実行中の.NET Frameworkのバージョンが2.0 SP1以降かどうか。</summary>
		public bool isAvaliableDotNet20SP1
		{
			get
			{
				return Environment.Version >= DOTNET_VERSION_20SP1;
			}
		}

		/// <summary>実行中の.NET Frameworkのバージョンが3.0以降かどうか。</summary>
		public bool isAvaliableDotNet30
		{
			get
			{
				return Environment.Version >= DOTNET_VERSION_30;
			}
		}

		/// <summary>
		/// 実行中のOSがこのフレームワークの実行に対応しているかかどうか。
		/// </summary>
		public bool isAvaliablePlatform
		{
			get
			{
#if WINDOWS
				string strSP = Environment.OSVersion.ServicePack;
				return Environment.OSVersion.Version >= WINDOWS_VERSION_XP;
#else
				return ( Environment.OSVersion.Platform == PlatformID.Xbox );
#endif
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// アンマネージ リソースの解放およびリセットに関連付けられている
		/// アプリケーション定義のタスクを実行します。
		/// </summary>
		public void Dispose()
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>システムの環境レポートを作成します。</summary>
		/// 
		/// <returns>システムの環境レポート 文字列</returns>
		public override string ToString()
		{
			string strResult = "◆◆◆ 基本環境情報" + Environment.NewLine;
			strResult += "  OS バージョン         : " + Environment.OSVersion.ToString() + Environment.NewLine;
#if WINDOWS
			strResult += "  DNL対応OS             : " + isAvaliablePlatform.ToStringOX() + Environment.NewLine;
#endif
			strResult += "  デバッグ バージョン   : ";
#if DEBUG
			strResult += true.ToStringOX() + Environment.NewLine;
#else
			strResult += false.ToStringOX() + Environment.NewLine;
#endif
			strResult += "  .NET FW バージョン    : " + Environment.Version.ToString() + Environment.NewLine;
			strResult += "  .NET FW 2.0以降       : " + isAvaliableDotNet20.ToStringOX() + Environment.NewLine;
			strResult += "  .NET FW 2.0 SP1以降   : " + isAvaliableDotNet20SP1.ToStringOX() + Environment.NewLine;
			strResult += "  .NET FW 3.0以降       : " + isAvaliableDotNet30.ToStringOX() + Environment.NewLine;
			strResult += "  演算プロセッサ数      : " + Environment.ProcessorCount + " 個" + Environment.NewLine;
#if WINDOWS
			strResult += "  ApplicationData       : " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Environment.NewLine;
			strResult += "  Favorites             : " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Environment.NewLine;
			strResult += "  Programs              : " + Environment.GetFolderPath(Environment.SpecialFolder.Programs) + Environment.NewLine;
			strResult += "  StartMenu             : " + Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + Environment.NewLine;
			strResult += "  Startup               : " + Environment.GetFolderPath(Environment.SpecialFolder.Startup) + Environment.NewLine;
			strResult += "  Personal              : " + Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Environment.NewLine;
			strResult += "  CommonApplicationData : " + Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + Environment.NewLine;
			strResult += "  LocalApplicationData  : " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Environment.NewLine;
			strResult += "  Cookies               : " + Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + Environment.NewLine;
			strResult += "  Desktop               : " + Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Environment.NewLine;
			strResult += "  History               : " + Environment.GetFolderPath(Environment.SpecialFolder.History) + Environment.NewLine;
			strResult += "  InternetCache         : " + Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + Environment.NewLine;
			strResult += "  MyComputer            : " + Environment.GetFolderPath(Environment.SpecialFolder.MyComputer) + Environment.NewLine;
			strResult += "  MyMusic               : " + Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + Environment.NewLine;
			strResult += "  MyPictures            : " + Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + Environment.NewLine;
			strResult += "  Recent                : " + Environment.GetFolderPath(Environment.SpecialFolder.Recent) + Environment.NewLine;
			strResult += "  SendTo                : " + Environment.GetFolderPath(Environment.SpecialFolder.SendTo) + Environment.NewLine;
			strResult += "  Templates             : " + Environment.GetFolderPath(Environment.SpecialFolder.Templates) + Environment.NewLine;
			strResult += "  DesktopDirectory      : " + Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Environment.NewLine;
			strResult += "  MyDocuments           : " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Environment.NewLine;
			strResult += "  ProgramFiles          : " + Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + Environment.NewLine;
			strResult += "  CommonProgramFiles    : " + Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + Environment.NewLine;
			strResult += "  コマンドライン        : " + Environment.CommandLine + Environment.NewLine;
			strResult += "  システムフォルダパス  : " + Environment.SystemDirectory + Environment.NewLine;
			strResult += "  ログオン ユーザID     : " + Environment.UserName + Environment.NewLine;
			strResult += "  ユーザ対話モード      : " + Environment.UserInteractive.ToStringOX() + Environment.NewLine;
			strResult += "  PC名(NetBIOS)         : ";
			try
			{
				strResult += Environment.UserName + Environment.NewLine;
			}
			catch(Exception)
			{
				strResult += "取得出来ませんでした。" + Environment.NewLine;
			}
			strResult += "  ネットワーク ドメイン : ";
			try
			{
				strResult += Environment.UserDomainName + Environment.NewLine;
			}
			catch(Exception)
			{
				strResult += "取得出来ませんでした。" + Environment.NewLine;
			}
			strResult += "  論理ドライブ設定一覧  : ";
			try
			{
				strResult += string.Join(", ", Environment.GetLogicalDrives()) + Environment.NewLine;
			}
			catch(Exception)
			{
				strResult += "取得出来ませんでした。" + Environment.NewLine;
			}
			strResult += "  環境変数設定一覧      : ";
			try
			{
				string strEnv = Environment.NewLine;
				foreach(DictionaryEntry pair in Environment.GetEnvironmentVariables())
				{
					strEnv += "  | " + pair.Key + " = " + pair.Value + Environment.NewLine;
				}
				strResult += strEnv;
			}
			catch(Exception)
			{
				strResult += "取得出来ませんでした。" + Environment.NewLine;
			}
#endif
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ランタイムのインストール確認メッセージを作成します。</summary>
		/// 
		/// <param name="bExpr">ランタイムのインストール確認検証結果</param>
		/// <param name="strExprRuntime">検証したランタイム及びバージョン</param>
		/// <returns>ランタイムのインストール確認メッセージ 文字列</returns>
		private string createErrorMessage(bool bExpr, string strExprRuntime)
		{
			return createErrorMessage(bExpr, strExprRuntime, strExprRuntime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ランタイムのインストール確認メッセージを作成します。</summary>
		/// 
		/// <param name="bExpr">ランタイムのインストール確認検証結果</param>
		/// <param name="strExprRuntime">検証したランタイム及びバージョン</param>
		/// <param name="strRequireRuntime">必要なランタイム及びバージョン</param>
		/// <returns>ランタイムのインストール確認メッセージ 文字列</returns>
		private string createErrorMessage(
			bool bExpr, string strExprRuntime, string strRequireRuntime
		)
		{
			string strResult = string.Format("{0} この環境には {1} がインストールされて{2}。",
				bExpr.ToStringOX(), strExprRuntime, bExpr ? "います" : "いません");
			if(!bExpr)
			{
				strResult += Environment.NewLine + string.Format("{0} を実行するためには {1} が必要となります。",
					Properties.Resources.NAME, strRequireRuntime);
			}
			return (strResult + Environment.NewLine);
		}
	}
}
