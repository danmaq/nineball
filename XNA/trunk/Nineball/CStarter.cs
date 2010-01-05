////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.entity;
using danmaq.nineball.entity.component;
using danmaq.nineball.entity.manager;
using danmaq.nineball.state.manager;
using Microsoft.Xna.Framework;

#if WINDOWS
using System.Threading;
using danmaq.nineball.Properties;
using System;
#endif

namespace danmaq.nineball {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>Nineballスターター クラス。</summary>
	public static class CStarter {

#if WINDOWS
		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>ミューテックスオブジェクトのラッパー クラス。</summary>
		private class CMutexObject {

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>ミューテックスオブジェクト。</summary>
			private readonly Mutex mutex = new Mutex( false, Resources.NAME );

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <exception cref="System.ApplicationException">多重起動した場合。</exception>
			public CMutexObject() {
				if ( !mutex.WaitOne( 0, false ) ) {
					throw new ApplicationException( "多重起動されました。" + Environment.NewLine +
						"このアプリケーションは多重起動に対応しておりません。" );
				}
			}

			//* -----------------------------------------------------------------------*
			/// <summary>デストラクタ。</summary>
			~CMutexObject() { mutex.Close(); }
		}
#endif

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>既定のゲーム クラス。</summary>
		private sealed class CGame : Game {

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			public CGame() { this.startNineball(); }
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

#if WINDOWS
		/// <summary>ミューテックスオブジェクト。</summary>
		private static readonly CMutexObject mutex = new CMutexObject();
#endif

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>シーン オブジェクトを取得します。</summary>
		/// 
		/// <value>シーン オブジェクト。</value>
		public static CEntity scene {
			get { return CStateMainLoopDefault.instance.scene; }
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>既定のゲーム クラスを新規作成して、Nineballを起動します。</summary>
		/// 
		/// <returns>グラフィック デバイスの構成・管理クラス。</returns>
		public static void startNineball() {
			using( CGame game = new CGame() ) { game.Run(); }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 既定のグラフィック デバイス マネージャを新規作成して、Nineballを起動します。
		/// </summary>
		/// 
		/// <param name="game">Nineballを実行するゲームクラス。</param>
		/// <returns>グラフィック デバイスの構成・管理クラス。</returns>
		public static GraphicsDeviceManager startNineball( this Game game ) {
			GraphicsDeviceManager graphicsDeviceManager = new GraphicsDeviceManager( game );
			game.startNineball( graphicsDeviceManager );
			return graphicsDeviceManager;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 指定のグラフィック デバイス マネージャを使用して、Nineballを起動します。
		/// </summary>
		/// 
		/// <param name="game">Nineballを実行するゲームクラス。</param>
		/// <param name="graphicsDeviceManager">グラフィック デバイスの構成・管理クラス。</param>
		public static void startNineball(
			this Game game, GraphicsDeviceManager graphicsDeviceManager
		) {
			CStateMainLoopDefault.instance.graphicsDeviceManager = graphicsDeviceManager;
			new CDrawableGameComponent<CMainLoop>( game, new CMainLoop( game ), true );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ここからプログラムが開始されます。</summary>
		/// <remarks>通常ここが実行されることがありません。</remarks>
		/// 
		/// <param name="args">プログラムへ渡される引数。</param>
		private static void Main( string[] args ) { startNineball(); }
	}
}
