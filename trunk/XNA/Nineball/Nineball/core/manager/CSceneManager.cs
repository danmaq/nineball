////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library / Copyright (c) 2008 danmaq all rights reserved.
//		──シーン進行を管理するクラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.Nineball.core.raw;
using Microsoft.Xna.Framework;

namespace danmaq.Nineball.core.manager {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>シーン進行を管理するクラス。</para>
	/// <para>このクラスにシーンを登録し、そしてこのクラスを通じ実行させます。</para>
	/// </summary>
	/// <remarks>
	/// 複数シーンのスタックを積むことも出来ます。
	/// (この場合、一番若いシーンが実行されます)
	/// </remarks>
	public sealed class CSceneManager {

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>シーンのスタック オブジェクト</summary>
		private readonly Stack<IScene> scenes;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>空のシーンスタックで初期化されます。</para>
		/// </summary>
		public CSceneManager() : this( new Stack<IScene>() ) { }

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>シーン スタックに最初のシーンを挿入して初期化します。</para>
		/// </summary>
		/// 
		/// <param name="sceneFirst">最初のシーン オブジェクト</param>
		public CSceneManager( IScene sceneFirst ) : this() { nowScene = sceneFirst; }

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定のシーン スタックで初期化します。</para>
		/// </summary>
		/// 
		/// <param name="__stackScene">シーンのスタック</param>
		public CSceneManager( Stack<IScene> __stackScene ) { scenes = __stackScene; }

		//* -----------------------------------------------------------------------*
		/// <summary>デストラクタ。</summary>
		~CSceneManager() { reset(); }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>
		/// <para>現在アクティブなシーン。</para>
		/// <para>nullを代入すると現在のシーンを終了し、スタックを掘り起こします。</para>
		/// </summary>
		public IScene nowScene {
			get { return ( scenes.Count == 0 ? null : scenes.Peek() ); }
			set {
				if( value == null ) {
					if( scenes.Count > 0 ) { scenes.Pop().Dispose(); }
				}
				else { scenes.Push( value ); }
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>登録されているすべてのシーンを終了・抹消します。</summary>
		public void reset() {
			while( scenes.Count > 0 ) { nowScene = null; }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の現在のシーンの更新処理をします。</summary>
		/// <remarks>
		/// 複数シーンを積んである場合は、一番若いシーンのみが実行されます。
		/// (ハノイの塔で一番上のブロックが実行されるイメージ)
		/// </remarks>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		/// <returns>まだアクティブなシーンが残っている場合、<c>true</c></returns>
		public bool update( GameTime gameTime ) {
			IScene scene = nowScene;
			bool bResult = false;
			if( nowScene != null ) {
				bool bContinue = scene.update( gameTime );
				bool bChangeScene = !bContinue;
				IScene nextScene = scene.nextScene;
				if( !bContinue ) { nowScene = null; }
				if( nextScene != null ) {
					scene.nextScene = null;
					nowScene = nextScene;
					bChangeScene = true;
				}
				bResult = scenes.Count > 0;
				if( bChangeScene ) { GC.Collect(); }
			}
			return bResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の現在のシーンの描画処理をします。</summary>
		/// <remarks>
		/// 複数シーンを積んである場合は、一番若いシーンのみが実行されます。
		/// (ハノイの塔で一番上のブロックが実行されるイメージ)
		/// </remarks>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		/// <param name="sprite">スプライト描画管理クラス</param>
		public void draw( GameTime gameTime, CSprite sprite ) {
			IScene scene = nowScene;
			if( nowScene != null ) { nowScene.draw( gameTime, sprite ); }
		}
	}
}
