////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.util;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.old.core.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>シーン本体のインターフェイスです。</para>
	/// <para>
	/// シーン管理クラスCSceneManagerに登録するタスクを作成するためには、
	/// このクラスを実装するか、CSceneBaseを継承します。
	/// </para>
	/// </summary>
	public interface IScene : IDisposable
	{

		/// <summary>次に移行するシーン オブジェクト</summary>
		/// <remarks>
		/// ここに次のシーン オブジェクトを代入することで
		/// 次のフレームからそのシーンが呼ばれるようになります。
		/// </remarks>
		/// <remarks>
		/// シーンを終了せずに次のシーンへ移行した場合
		/// </remarks>
		/// <remarks>
		/// 次のシーンへ以降出来た際、このプロパティには自動的にnullが代入されます。
		/// </remarks>
		IScene nextScene
		{
			get;
			set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を記述します。</summary>
		/// <remarks>
		/// 管理クラスから毎フレーム呼び出されます。
		/// </remarks>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		/// <returns>
		/// 現在のフレームでこのシーンが終了しない場合、<c>true</c>
		/// </returns>
		bool update(GameTime gameTime);

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を記述します。</summary>
		/// <remarks>
		/// 描画処理の準備が出来た際、管理クラスから毎フレーム呼び出されます。
		/// </remarks>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		/// <param name="sprite">スプライト描画管理クラス</param>
		void draw(GameTime gameTime, CSprite sprite);
	}
}
