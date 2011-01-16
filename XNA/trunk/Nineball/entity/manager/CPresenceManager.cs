////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.data;
using danmaq.nineball.state;
using danmaq.nineball.state.manager;
using danmaq.nineball.util.storage;
using Microsoft.Xna.Framework.GamerServices;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲーマー情報制御クラス。</summary>
	public sealed class CPresenceManager
		: CEntity
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CPresenceManager instance = new CPresenceManager();

		/// <summary>設定されるプレゼンス一覧。</summary>
		public readonly List<SPresence> presenceList = new List<SPresence>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>プレゼンスの更新間隔。</summary>
		/// <remarks>
		/// 10～60秒(600～3600フレーム)が理想値です。値を極端に小さくすると、
		/// プレゼンス情報が反映される前に更新されてしまう可能性があります。
		/// </remarks>
		public ushort interval = 60 * 30;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		private CPresenceManager()
			: base(CStatePresenceManager.instance)
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>現在登録されているプレゼンスを破棄し、初期状態に戻します。</summary>
		public void resetPresence()
		{
			presenceList.Clear();
			setPresence(SPresence.none);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>プレゼンス情報を設定します。</summary>
		/// 
		/// <param name="presence">プレゼンス情報。</param>
		public void setPresence(SPresence presence)
		{
			setPresence(presence.mode, presence.value);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>プレゼンス情報を設定します。</summary>
		/// 
		/// <param name="mode">現在の状態モード。</param>
		/// <param name="value">カスタム状態値。</param>
		public void setPresence(GamerPresenceMode mode, int? value)
		{
			if (CGuideWrapper.instance.isAvaliableUseGamerService)
			{
				SignedInGamerCollection collection = SignedInGamer.SignedInGamers;
				for (int i = collection.Count; --i >= 0; )
				{
					GamerPresence presence = collection[i].Presence;
					presence.PresenceMode = mode;
					if (value.HasValue)
					{
						presence.PresenceValue = value.Value;
					}
				}
			}
		}
	}
}
