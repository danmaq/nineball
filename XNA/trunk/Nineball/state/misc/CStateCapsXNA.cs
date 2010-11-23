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
using danmaq.nineball.entity;
using danmaq.nineball.util.caps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.misc
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XNA・ビデオ環境検証クラス。</summary>
	public sealed class CStateCapsXNA : CState
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateCapsXNA instance = new CStateCapsXNA();

		/// <summary>接続されているXBOX360コントローラ一覧。</summary>
		private readonly List<PlayerIndex> connectedXBOX360ControllersList =
			new List<PlayerIndex>(4);

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>
		/// <para>次に移行する状態。</para>
		/// <para>
		/// 呼び出し時にリセットされますので、この状態を
		/// 適用する際に毎回設定し直す必要があります。
		/// </para>
		/// </summary>
		public IState nextState = empty;

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

		//* -----------------------------------------------------------------------*
		/// <summary>XNA・ビデオ環境レポートを取得します。</summary>
		/// 
		/// <value>XNA・ビデオ環境レポート文字列。</value>
		public string report
		{
			get;
			private set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>状態が開始された時に呼び出されます。</para>
		/// <para>このメソッドは、遷移元の<c>teardown</c>よりも後に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されたオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		public override void setup(IEntity entity, object privateMembers)
		{
			report = createReport();
			base.setup(entity, privateMembers);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(IEntity entity, object privateMembers, GameTime gameTime)
		{
			entity.nextState = nextState;
			nextState = empty;
			base.update(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>XNA・ビデオ環境レポートを生成します。</summary>
		private string createReport()
		{
			string strResult = "◆◆◆ DirectX環境情報" + Environment.NewLine;
			int length = GraphicsAdapter.Adapters.Count;
			for (int i = 0; i < length; i++)
			{
				GraphicsAdapter adapter = GraphicsAdapter.Adapters[i];
				bool bCurrentDevice;
				ShaderProfile ps;
				ShaderProfile vs;
				strResult += adapter.createCapsReport(out bCurrentDevice, out ps, out vs) + Environment.NewLine;
				if (bCurrentDevice)
				{
					PixelShaderProfile = ps;
					VertexShaderProfile = vs;
				}
			}
			try
			{
				PlayerIndex[] all = {
					PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };
				foreach (PlayerIndex i in all)
				{
					strResult += GamePad.GetCapabilities(i).createCapsReport(i);
				}
			}
			catch (Exception e)
			{
				strResult += "!▲! XBOX360コントローラ デバイスの性能取得に失敗。" + Environment.NewLine + e.ToString();
			}
			return strResult;
		}
	}
}
