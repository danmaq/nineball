////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.util.caps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.misc
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XNA・ビデオ環境検証クラス。</summary>
	public sealed class CStateCapsXNA
		: CStateCapsBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateCapsXNA instance = new CStateCapsXNA();

		/// <summary>接続されているXBOX360コントローラ一覧。</summary>
		private readonly List<PlayerIndex> connectedXBOX360ControllersList =
			new List<PlayerIndex>(4);

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateCapsXNA()
		{
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>使用可能なピクセル シェーダのバージョンを取得します。</summary>
		/// 
		/// <value>使用可能なピクセル シェーダのバージョン。</value>
		public ShaderProfile PixelShaderProfile
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>使用可能な頂点シェーダのバージョンを取得します。</summary>
		/// 
		/// <value>使用可能な頂点シェーダのバージョン。</value>
		public ShaderProfile VertexShaderProfile
		{
			get;
			private set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>XNA・ビデオ環境レポートを生成します。</summary>
		/// 
		/// <returns>レポート文字列。</returns>
		protected override string createReport()
		{
			string strResult = "◆◆◆ DirectX環境情報" + Environment.NewLine;
			int length = GraphicsAdapter.Adapters.Count;
			for (int i = 0; i < length; i++)
			{
				GraphicsAdapter adapter = GraphicsAdapter.Adapters[i];
				bool bCurrentDevice;
				ShaderProfile ps;
				ShaderProfile vs;
				strResult += adapter.createCapsReport(out bCurrentDevice, out ps, out vs)
					+ Environment.NewLine;
				if (bCurrentDevice)
				{
					PixelShaderProfile = ps;
					VertexShaderProfile = vs;
				}
			}
			try
			{
				PlayerIndex[] all =
				{
					PlayerIndex.One,
					PlayerIndex.Two,
					PlayerIndex.Three,
					PlayerIndex.Four
				};
				foreach (PlayerIndex i in all)
				{
					strResult += GamePad.GetCapabilities(i).createCapsReport(i);
				}
			}
			catch (Exception e)
			{
				strResult += "!▲! XBOX360コントローラ デバイスの性能取得に失敗。"
					+ Environment.NewLine + e.ToString();
			}
			return strResult;
		}
	}
}
