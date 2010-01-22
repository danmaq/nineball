////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;
using danmaq.nineball.old.core.inner;
using danmaq.nineball.old.core.manager;
using danmaq.nineball.Properties;
using danmaq.nineball.util;
using danmaq.nineball.util.caps;

namespace danmaq.nineball.old.core.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>DNL動作初期設定用構造体。</summary>
	public struct SStarter<_T> where _T : new()
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>入力関係初期設定用構造体。</summary>
		public struct SInputInitializeData
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>連続入力となるまでのフレーム時間間隔。</summary>
			public ushort keyLoopStart;

			/// <summary>押しっぱなしで連続入力となるフレーム時間間隔。</summary>
			public ushort keyLoopInterval;

			/// <summary>十字キーを除くボタンの数。</summary>
			public byte buttons;

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>設定データ一覧を表示します。</summary>
			/// 
			/// <returns>設定データ一覧 文字列</returns>
			public override string ToString()
			{
				string strResult = "▽ 入力関連設定" + Environment.NewLine;
				strResult += "  ボタン数(方向キーを除く) : " + buttons + Environment.NewLine;
				strResult += "  ボタン連続入力間隔       : " + keyLoopInterval + Environment.NewLine;
				strResult += "  ボタン連続入力開始間隔   : " + keyLoopStart + Environment.NewLine;
				return strResult;
			}
		}

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>ファイル入力関係初期設定用構造体。</summary>
		public struct SFileIOInitializeData
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>Contentフォルダへのパス。</summary>
			public string dirContent;

			/// <summary>
			/// <para>設定ファイル名。</para>
			/// <para>
			/// パスを設定することは出来ません(自動的にEXEファイル直下となります)
			/// </para>
			/// </summary>
			public string fileConfigure;

			/// <summary>ログ ファイル名。</summary>
			public string fileLog;

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>設定データ一覧を表示します。</summary>
			/// 
			/// <returns>設定データ一覧 文字列</returns>
			public override string ToString()
			{
				complete();
				string strResult = "▽ ファイル入出力関連設定" + Environment.NewLine;
				strResult += "  コンテンツ フォルダ パス : " + dirContent + Environment.NewLine;
				strResult += "  設定ファイル パス        : " + fileConfigure + Environment.NewLine;
				strResult += "  ログ ファイル パス       : " + fileLog + Environment.NewLine;
				return strResult;
			}

			//* -----------------------------------------------------------------------*
			/// <summary>
			/// 設定データが不足している場合、自動補完可能な範囲で保管します。
			/// </summary>
			private void complete()
			{
				if(dirContent == null)
				{
					dirContent = "";
				}
				if(fileConfigure == null)
				{
					fileConfigure = "";
				}
				if(fileLog == null)
				{
					fileLog = "";
				}
			}
		}
		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>XACT初期設定用構造体。</summary>
		public struct SXACTInitializeData
		{

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>
			/// インデックスをアセット名に変換するためのコールバック用デリゲート。
			/// </summary>
			public Func<ushort, string> index2assert;

			/// <summary>効果音を繰り返し再生する際の間隔。</summary>
			public ushort loopSEInterval;

			/// <summary>XACTサウンドエンジン ファイル名。</summary>
			public string fileXGS;

			/// <summary>XACT再生キュー ファイル名。</summary>
			public string fileXSB;

			/// <summary>XACT波形バンク(効果音) ファイル名。</summary>
			public string fileXWBSE;

			/// <summary>XACT波形バンク(BGM) ファイル名。</summary>
			public string fileXWBBGM;

			//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* properties ──────────────────────────────*

			/// <summary>
			/// <para>正当でないデータに対するエラーメッセージ。</para>
			/// <para>特にエラーがなければ空文字を取得します。</para>
			/// </summary>
			public string errorMessage
			{
				get
				{

					complete();
					string strResult = "";
					if(fileXGS.Length > 0 && !fileXGS.ToLower().Contains(".xgs"))
					{
						strResult +=
							"XACTサウンドエンジンファイルの拡張子が正しくありません。" + Environment.NewLine;
					}
					if(fileXSB.Length > 0 && !fileXSB.ToLower().Contains(".xsb"))
					{
						strResult +=
							"XACT再生キューファイルの拡張子が正しくありません。" + Environment.NewLine;
					}
					if(fileXWBSE.Length > 0 && !fileXWBSE.ToLower().Contains(".xwb"))
					{
						strResult +=
							"XACT波形バンク(効果音)ファイルの拡張子が正しくありません。" + Environment.NewLine;
					}
					if(fileXWBBGM.Length > 0 && !fileXWBBGM.ToLower().Contains(".xwb"))
					{
						strResult +=
							"XACT波形バンク(BGM)ファイルの拡張子が正しくありません。" + Environment.NewLine;
					}
					return strResult;
				}
			}

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>XACT再生に必要な情報が入っているかどうかを取得します。</summary>
			/// 
			/// <param name="x">XACT初期設定用構造体</param>
			/// <returns>XACT再生に必要な情報が入っている場合、<c>true</c></returns>
			public static implicit operator bool(SXACTInitializeData x)
			{
				return (x.errorMessage.Length == 0 && x.index2assert != null &&
					x.fileXGS.Length > 0 && x.fileXSB.Length > 0 &&
					x.fileXWBBGM.Length > 0 && x.fileXWBSE.Length > 0);
			}

			//* -----------------------------------------------------------------------*
			/// <summary>設定データ一覧を表示します。</summary>
			/// 
			/// <returns>設定データ一覧 文字列</returns>
			public override string ToString()
			{
				complete();
				string strResult = "▽ ファイル入出力関連設定" + Environment.NewLine;
				strResult += "  XACT SoundEngine パス   : " + fileXGS + Environment.NewLine;
				strResult += "  XACT SoundBank パス     : " + fileXSB + Environment.NewLine;
				strResult += "  XACT Wavebank(SE) パス  : " + fileXWBSE + Environment.NewLine;
				strResult += "  XACT WaveBank(BGM) パス : " + fileXWBBGM + Environment.NewLine;
				strResult += "  効果音キャンセル時間    : " + loopSEInterval + Environment.NewLine;
				strResult += "  INDEX>ASSETメソッド     : ";
				if(index2assert == null)
				{
					strResult += Resources.NULL + Environment.NewLine;
				}
				else
				{
					strResult += index2assert.Method.ToString() + Environment.NewLine;
				}
				return strResult;
			}

			//* -----------------------------------------------------------------------*
			/// <summary>
			/// 設定データが不足している場合、自動補完可能な範囲で保管します。
			/// </summary>
			private void complete()
			{
				if(fileXGS == null)
				{
					fileXGS = "";
				}
				if(fileXSB == null)
				{
					fileXSB = "";
				}
				if(fileXWBSE == null)
				{
					fileXWBSE = "";
				}
				if(fileXWBBGM == null)
				{
					fileXWBBGM = "";
				}
			}
		}

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>開発中モードかどうか。</summary>
		public bool debug;

		/// <summary>秒間フレーム数。</summary>
		public byte fps;

		/// <summary>タイトル文字列。</summary>
		public string title;

		/// <summary>開発コードネーム(半角英数)</summary>
		public string codename;

		/// <summary>
		/// <para>プログラムへ渡される引数。</para>
		/// <para>設定されない場合、自動的に補完されます。</para>
		/// </summary>
		public string[] args;

		/// <summary>最初に開始されるシーン。</summary>
		public IScene sceneFirst;

		/// <summary>入力関係初期設定用構造体。</summary>
		public SInputInitializeData inputConfigure;

		/// <summary>ファイル入力関係初期設定用構造体。</summary>
		public SFileIOInitializeData fileIOConfigure;

		/// <summary>XACT初期設定用構造体。</summary>
		public SXACTInitializeData XACTConfigure;

		/// <summary>全ての検証が終了したかどうか。</summary>
		private bool isAllAvaliable;

#if WINDOWS
		/// <summary>ミューテックスオブジェクト</summary>
		private Mutex mutex;

#endif
		/// <summary>メインループ クラス。</summary>
		private CMainLoop<_T> m_mainLoop;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>
		/// <para>正当でないデータに対するエラーメッセージ。</para>
		/// <para>特にエラーがなければ空文字を取得します。</para>
		/// </summary>
		public string errorMessage
		{
			get
			{

				complete();
				string strResult = XACTConfigure.errorMessage;
				if(fps < 1)
				{
					strResult +=
						"秒間フレーム数は1以上に設定しなければなりません(60を推奨)" + Environment.NewLine;
				}
				if(sceneFirst == null)
				{
					strResult +=
						"最初のシーンは必ず実装する必要があります。" + Environment.NewLine;
				}
				return strResult;
			}
		}

		/// <summary>メインループ クラス。</summary>
		public CMainLoop<_T> mainLoop
		{
			get
			{
				return m_mainLoop;
			}
			private set
			{
				m_mainLoop = value;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>設定データ一覧を表示します。</summary>
		/// 
		/// <returns>設定データ一覧 文字列</returns>
		public override string ToString()
		{
			complete();
			string strResult = string.Format("◎◎ {0} 初期設定{1}", Resources.NAME, Environment.NewLine);
			strResult += "  開発中 バージョン      : " + debug.ToStringOX() + Environment.NewLine;
			strResult += "  秒間フレーム数         : " + fps + Environment.NewLine;
			strResult += "  アプリケーション名称   : " + title + Environment.NewLine;
			strResult += "  内部名(開発コードなど) : " + codename + Environment.NewLine;
			strResult += "  初回実行シーン         : ";
			if(sceneFirst == null)
			{
				strResult += Resources.NULL + Environment.NewLine;
			}
			else
			{
				strResult += sceneFirst.ToString() + Environment.NewLine;
			}
			strResult += "  引数一覧               : ";
			if(args == null || args.Length == 0)
			{
				strResult += Resources.NULL + Environment.NewLine;
			}
			else
			{
				strResult += string.Join(" ", args) + Environment.NewLine;
			}
			strResult += "  入力関連設定一覧       : " + Environment.NewLine + inputConfigure.ToString() + Environment.NewLine;
			strResult += "  File入出力関連設定一覧 : " + Environment.NewLine + fileIOConfigure.ToString() + Environment.NewLine;
			strResult += "  XACT関連設定一覧       : " + Environment.NewLine + XACTConfigure.ToString() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在の初期設定データでDNLを始動します。</summary>
		/// 
		/// <returns>正常に起動し、かつ正常に終了出来た場合、<c>true</c></returns>
		public bool run()
		{
			bool bResult = isAllAvaliable;
			if(!bResult)
			{
				bResult = validate();
			}
			if(isAllAvaliable)
			{
				mainLoop = new CMainLoop<_T>(this);
				mainLoop.Run();
#if WINDOWS
				mutex.Close();

#endif
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>起動前の各種検証をします。</summary>
		/// 
		/// <returns>起動検証に成功した場合、<c>true</c></returns>
		public bool validate()
		{
			bool bResult = true;

			try
			{
				complete();
				string strErrorMessage = errorMessage;
				CMessageBox.titleBar = title;
				if(strErrorMessage.Length != 0)
				{
					throw new ArgumentOutOfRangeException(
						strErrorMessage, "初期設定データの一部に不正な値を検出しました。");
				}
				CLogger.outFile = fileIOConfigure.fileLog;
				CLogger.add(ToString());
				using(CValidateEnvironment envValidator = new CValidateEnvironment())
				{
					CLogger.add(envValidator.ToString());
					if(!envValidator.AVALIABLE)
					{
						throw new PlatformNotSupportedException(envValidator.ERROR_MESSAGE);
					}
				}
				using(CValidateDirectX dxValidator = new CValidateDirectX())
				{
					CLogger.add(dxValidator.ToString());
					if(!dxValidator.isAvaliablePS11)
					{
						throw new PlatformNotSupportedException("ビデオチップがピクセルシェーダ1.1に対応していません。");
					}
				}
#if WINDOWS
				mutex = new Mutex(false, codename);
				if(!mutex.WaitOne(0, false))
				{
					throw new ApplicationException("多重起動されました。" + Environment.NewLine + "このアプリケーションは多重起動に対応しておりません。");
				}
#endif
			}
			catch(Exception e)
			{
				CMessageBox.show(
					"起動時の環境検証に失敗しました。非対応のハードウェアまたはOSを使用しているか、" + Environment.NewLine +
					"あるいはいくつかのランタイム ライブラリが不足している可能性があります。" +
					Environment.NewLine + Environment.NewLine + e.ToString());
				bResult = false;
			}
			isAllAvaliable = bResult;
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 設定データが不足している場合、自動補完可能な範囲で保管します。
		/// </summary>
		private void complete()
		{
			string strAssemblyName = Resources.NAME;
			if(fps < 1)
			{
				fps = 60;
			}
			if(title == null)
			{
				title = strAssemblyName;
			}
			if(codename == null)
			{
				codename = strAssemblyName;
			}
			if(args == null)
			{
#if WINDOWS
				try
				{
					args = Environment.GetCommandLineArgs();
				}
				catch(Exception)
				{
					args = new string[0];
				}
#else
				args = new string[ 0 ];
#endif
			}
		}
	}
}
