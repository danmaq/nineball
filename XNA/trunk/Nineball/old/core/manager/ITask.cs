////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.old.core.raw;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.old.core.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>タスク本体のインターフェイスです。</para>
	/// <para>
	/// タスク管理クラスCTaskManagerに登録するタスクを作成するためには、
	/// このクラスを実装するか、CTaskBaseを継承します。
	/// </para>
	/// </summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete("このクラスは今後サポートされません。GameComponentクラスなどを使用してください。")]
	public interface ITask : IDisposable
	{

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>タスク管理クラス オブジェクト。</summary>
		/// <remarks>
		/// このタスクを管理クラスに登録すると、
		/// 自動的にこのプロパティに代入されます。
		/// </remarks>
		CTaskManager manager
		{
			set;
		}

		/// <summary>所属レイヤ番号。</summary>
		/// <remarks>
		/// レイヤ値の若い方から順に処理されます。
		/// 同一値が複数ある場合、登録された順に処理されます。
		/// </remarks>
		/// <remarks>
		/// !!注意!!：管理クラス登録後(initialize呼出後)はレイヤ変更しないでください。
		/// </remarks>
		byte layer
		{
			get;
		}

		/// <summary>一時停止に対応しているかどうか。</summary>
		/// <remarks>
		/// 一時停止に対応しているタスクは、登録されている管理クラスにおいて
		/// pauseプロパティがtrueの間、updateメソッドに制御が移りません。
		/// </remarks>
		bool isAvailablePause
		{
			get;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>タスクが管理クラスに登録された直後に、1度だけ自動的に呼ばれます。</summary>
		/// <remarks>
		/// 直前にmanagerプロパティが自動的に代入されるので、
		/// タスク管理クラスが必要な初期化処理などの用途に便利です。
		/// </remarks>
		/// <remarks>
		/// !!注意!!：このメソッドが管理クラスから呼ばれた後、レイヤ変更しないでください。
		/// </remarks>
		void initialize();

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を記述します。</summary>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		/// <returns>次フレームも存続し続ける場合、<c>true</c></returns>
		bool update(GameTime gameTime);

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を記述します。</summary>
		/// 
		/// <param name="gameTime">前フレームからの経過時間</param>
		/// <param name="sprite">スプライト描画管理クラス</param>
		void draw(GameTime gameTime, CSprite sprite);
	}
}
