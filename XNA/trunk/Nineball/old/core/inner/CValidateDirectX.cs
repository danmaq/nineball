////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.util.caps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.old.core.inner
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XNA・ビデオ環境検証クラス。</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete("このクラスは今後サポートされません。CStateCapsXNAを使用してください。")]
	class CValidateDirectX
		: IDisposable
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>ピクセルシェーダ1.1が使用可能かどうか。</summary>
		private bool m_bavaliablePS11 = false;

		/// <summary>接続されているXBOX360コントローラ一覧。</summary>
		private List<PlayerIndex> m_listXBOX360ControllerConnected = new List<PlayerIndex>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>デストラクタ。</summary>
		~CValidateDirectX()
		{
			Dispose();
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>ピクセルシェーダ1.1が使用可能かどうか。</summary>
		public bool isAvaliablePS11
		{
			get
			{
				return m_bavaliablePS11;
			}
			private set
			{
				m_bavaliablePS11 = m_bavaliablePS11 || value;
			}
		}

		/// <summary>接続されているXBOX360コントローラ一覧。</summary>
		public PlayerIndex[] XBOX360ContollerConnected
		{
			get
			{
				return m_listXBOX360ControllerConnected.ToArray();
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
		/// <summary>DirectXの環境レポートを作成します。</summary>
		/// 
		/// <returns>DirectXの環境レポート 文字列</returns>
		public override string ToString()
		{
			string strResult = "◆◆◆ DirectX環境情報" + Environment.NewLine;
			ShaderProfile ps, vs;
			bool bCurrent;
			foreach(GraphicsAdapter adapter in GraphicsAdapter.Adapters)
			{
				strResult +=
					adapter.createCapsReport(out bCurrent, out ps, out vs) + Environment.NewLine;
				isAvaliablePS11 = ps != ShaderProfile.Unknown;
			}
			try
			{
				PlayerIndex[] all = new PlayerIndex[] {
					PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };
				foreach(PlayerIndex i in all)
				{
					strResult += GamePad.GetCapabilities(i).createCapsReport(i);
				}
			}
			catch(Exception e)
			{
				strResult += "!▲! XBOX360コントローラ デバイスの性能取得に失敗。" + Environment.NewLine + e.ToString();
			}
			return strResult;
		}
	}
}
