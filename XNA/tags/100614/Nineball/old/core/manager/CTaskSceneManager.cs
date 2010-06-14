////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.old.task;
using danmaq.nineball.util;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.old.core.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>シーン進行を管理するクラス。</para>
	/// <para>このクラスにシーンを登録し、そしてこのクラスを通じ実行させます。</para>
	/// </summary>
	/// <remarks>
	/// <para>
	/// 複数シーンのスタックを積むことも出来ます。
	/// (この場合、一番若いシーンが実行されます)
	/// </para>
	/// <para>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </para>
	/// </remarks>
	[Obsolete("このクラスは今後サポートされません。CEntityを使用してください。")]
	public sealed class CTaskSceneManager : CTaskBase
	{

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
		public CTaskSceneManager() : this(new Stack<IScene>())
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>シーン スタックに最初のシーンを挿入して初期化します。</para>
		/// </summary>
		/// 
		/// <param name="sceneFirst">最初のシーン オブジェクト</param>
		public CTaskSceneManager(IScene sceneFirst) : this()
		{
			nowScene = sceneFirst;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定のシーン スタックで初期化します。</para>
		/// </summary>
		/// 
		/// <param name="stackScene">シーンのスタック</param>
		public CTaskSceneManager(Stack<IScene> stackScene)
		{
			scenes = stackScene;
			coRoutineManager.add(thread());
		}

		//* -----------------------------------------------------------------------*
		/// <summary>デストラクタ。</summary>
		~CTaskSceneManager()
		{
			reset();
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>
		/// <para>現在アクティブなシーン。</para>
		/// <para>nullを代入すると現在のシーンを終了し、スタックを掘り起こします。</para>
		/// </summary>
		public IScene nowScene
		{
			get
			{
				return (scenes.Count == 0 ? null : scenes.Peek());
			}
			set
			{
				if(value == null)
				{
					if(scenes.Count > 0)
					{
						scenes.Pop().Dispose();
					}
				}
				else
				{
					scenes.Push(value);
				}
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>タスク終了時の処理です。</summary>
		public override void Dispose()
		{
			reset();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>登録されているすべてのシーンを終了・抹消します。</summary>
		public void reset()
		{
			while(scenes.Count > 0)
			{
				nowScene = null;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在のシーンの更新処理をします。</summary>
		/// <remarks>
		/// 複数シーンを積んである場合は、一番若いシーンのみが実行されます。
		/// (ハノイの塔で一番上のブロックが実行されるイメージ)
		/// </remarks>
		/// 
		/// <returns>スレッドが実行される間、<c>null</c></returns>
		private IEnumerator<object> thread()
		{
			do
			{
				yield return null;
				IScene scene = nowScene;
				if(nowScene != null)
				{
					bool bContinue = scene.update(gameTime);
					bool bChangeScene = !bContinue;
					IScene nextScene = scene.nextScene;
					if(!bContinue)
					{
						nowScene = null;
					}
					if(nextScene != null)
					{
						scene.nextScene = null;
						nowScene = nextScene;
						bChangeScene = true;
					}
					if(bChangeScene)
					{
						GC.Collect();
					}
				}
			}
			while(scenes.Count > 0);
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
		public override void draw(GameTime gameTime, CSprite sprite)
		{
			IScene scene = nowScene;
			if(nowScene != null)
			{
				nowScene.draw(gameTime, sprite);
			}
		}
	}
}
