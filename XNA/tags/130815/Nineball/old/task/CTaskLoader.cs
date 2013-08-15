////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.old.core.raw;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace danmaq.nineball.old.task
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ローダータスク クラス。</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete("このクラスは今後サポートされません。CContentLoaderを使用してください。")]
	public class CTaskLoader : CTaskBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>読み込み対象キュー(テクスチャ)</summary>
		public readonly Queue<string> queueTexture = new Queue<string>();

		/// <summary>読み込み対象キュー(モデル)</summary>
		public readonly Queue<string> queueModel = new Queue<string>();

		/// <summary>読み込み対象キュー(フォント)</summary>
		public readonly Queue<string> queueFont = new Queue<string>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>読み込みターンの待機フレーム数。</summary>
		private byte m_byWait = 1;

		/// <summary>1読み込みターン毎に読み込むファイル数。</summary>
		private byte m_byLoadPerFrame = 1;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="mgrTexture">リソース管理クラス(テクスチャ)</param>
		/// <param name="mgrModel">リソース管理クラス(モデル)</param>
		/// <param name="mgrFont">リソース管理クラス(フォント)</param>
		/// <param name="mgrContent">コンテンツ管理クラス</param>
		public CTaskLoader(
			CResourceManager<Texture2D> mgrTexture,
			CResourceManager<Model> mgrModel,
			CResourceManager<SpriteFont> mgrFont,
			ContentManager mgrContent
		)
		{
			coRoutineManager.add(threadLoad(mgrTexture, mgrModel, mgrFont, mgrContent));
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>現在のキューの総数</summary>
		public int totalQueueCount
		{
			get
			{
				return queueModel.Count + queueTexture.Count + queueFont.Count;
			}
		}

		/// <summary>読み込みターンの待機フレーム数。</summary>
		public byte wait
		{
			get
			{
				return m_byWait;
			}
			set
			{
				m_byWait = Math.Max((byte)1, value);
			}
		}

		/// <summary>1読み込みターン毎に読み込むファイル数。</summary>
		public byte loadPerFrame
		{
			get
			{
				return m_byLoadPerFrame;
			}
			set
			{
				m_byLoadPerFrame = Math.Max((byte)1, value);
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>タスク終了時の処理です。</summary>
		public override void Dispose()
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>読み込み処理をするスレッドです。</summary>
		/// 
		/// <param name="mgrTexture">リソース管理クラス(テクスチャ)</param>
		/// <param name="mgrModel">リソース管理クラス(モデル)</param>
		/// <param name="mgrFont">リソース管理クラス(フォント)</param>
		/// <param name="mgrContent">コンテンツ管理クラス</param>
		/// <returns>スレッドが実行される間、<c>null</c></returns>
		private IEnumerator<object> threadLoad(
			CResourceManager<Texture2D> mgrTexture,
			CResourceManager<Model> mgrModel,
			CResourceManager<SpriteFont> mgrFont,
			ContentManager mgrContent
		)
		{
			yield return null;
			while (true)
			{
				for (int i = 0; i < wait; i++)
				{
					yield return null;
				}
				for (int i = 0; i < loadPerFrame; i++)
				{
					if (queueTexture.Count > 0)
					{
						mgrTexture.load(queueTexture.Dequeue(), mgrContent);
					}
					else if (queueModel.Count > 0)
					{
						mgrModel.load(queueModel.Dequeue(), mgrContent);
					}
					else if (queueFont.Count > 0)
					{
						mgrFont.load(queueFont.Dequeue(), mgrContent);
					}
				}
			}
		}
	}
}
