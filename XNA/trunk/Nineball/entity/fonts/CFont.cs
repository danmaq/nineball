////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.data;
using danmaq.nineball.entity.manager;
using danmaq.nineball.state;
using danmaq.nineball.state.fonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.entity.fonts
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>フォント クラス。</summary>
	public class CFont
		: CEntity
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>生存タイマー。</summary>
		public int timer = int.MinValue;

		/// <summary>影のずれの大きさ。</summary>
		public Vector2 gapShadow = new Vector2(2);

		/// <summary>描画先の座標。</summary>
		public Vector2 pos = Vector2.Zero;

		/// <summary>スプライトフォント リソース。</summary>
		public SpriteFont font = null;

		/// <summary>スプライト管理クラス。</summary>
		public CSpriteManager sprite = null;

		/// <summary>グラデーション情報(X座標誤差)</summary>
		public SGradation gapX = new SGradation();

		/// <summary>グラデーション情報(Y座標誤差)</summary>
		public SGradation gapY = new SGradation();

		/// <summary>グラデーション情報(X座標倍率)</summary>
		public SGradation scaleX = 1;

		/// <summary>グラデーション情報(Y座標倍率)</summary>
		public SGradation scaleY = 1;

		/// <summary>グラデーション情報(ラジアン回転量)</summary>
		public SGradation rotate = new SGradation();

		/// <summary>グラデーション情報(文字間隔)</summary>
		public SGradation pitch = 1;

		/// <summary>グラデーション情報(不透明度)</summary>
		public SGradation colorAlpha = new SGradation(255, 255, 0, 255);

		/// <summary>グラデーション情報(赤輝度)</summary>
		public SGradation colorRed = new SGradation(255, 255, 0, 255);

		/// <summary>グラデーション情報(緑輝度)</summary>
		public SGradation colorGreen = new SGradation(255, 255, 0, 255);

		/// <summary>グラデーション情報(青輝度)</summary>
		public SGradation colorBlue = new SGradation(255, 255, 0, 255);

		/// <summary>水平位置揃え情報。</summary>
		public EAlign alignHorizontal = EAlign.Center;

		/// <summary>垂直位置揃え情報。</summary>
		public EAlign alignVertical = EAlign.Center;

		/// <summary>合成モード。</summary>
		public SpriteBlendMode blend = SpriteBlendMode.AlphaBlend;

		/// <summary>影を描画するかどうか。</summary>
		public bool isDrawShadow = true;

		/// <summary>表示対象文字列。</summary>
		private string m_strText = string.Empty;

		/// <summary>描画レイヤ。</summary>
		private float m_flayer = 0;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CFont()
			: base(CStateDefault.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		///	<param name="font">スプライトフォント リソース。</param>
		public CFont(SpriteFont font) : this()
		{
			this.font = font;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="strText">テキスト。</param>
		public CFont(string strText) : this()
		{
			text = strText;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="font">フォントリソース。</param>
		/// <param name="strText">テキスト。</param>
		public CFont(SpriteFont font, string strText)
			: this()
		{
			this.font = font;
			text = strText;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>表示対象文字列を設定/取得します。</summary>
		/// 
		/// <value>表示対象文字列。</value>
		public string text
		{
			get
			{
				return m_strText;
			}
			set
			{
				m_strText = value ?? string.Empty;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>描画レイヤを設定/取得します。</summary>
		/// 
		/// <value>描画レイヤ(<c>0.0</c>～<c>1.0</c>)。</value>
		public float layer
		{
			get
			{
				return m_flayer;
			}
			set
			{
				m_flayer = MathHelper.Clamp(value, 0, 1);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>座標誤差を設定/取得します。</summary>
		/// 
		/// <value>座標誤差。</value>
		public Vector2 gap
		{
			get
			{
				return new Vector2(gapX, gapY);
			}
			set
			{
				gapX.set(value.X);
				gapY.set(value.Y);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>拡大倍率を設定/取得します。</summary>
		/// 
		/// <value>拡大倍率。</value>
		public Vector2 scale
		{
			get
			{
				return new Vector2(scaleX, scaleY);
			}
			set
			{
				scaleX.set(value.X);
				scaleY.set(value.Y);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>色輝度を設定/取得します。</summary>
		/// 
		/// <value>色輝度。</value>
		public Color color
		{
			get
			{
				return new Color(
					(byte)colorRed, (byte)colorGreen,
					(byte)colorBlue, (byte)colorAlpha);
			}
			set
			{
				colorRed.set(value.R);
				colorGreen.set(value.G);
				colorBlue.set(value.B);
				colorAlpha.set(value.A);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>グラデーションが有効かどうかを設定/取得します。</summary>
		/// 
		/// <value>グラデーションが有効である場合、<c>true</c>。</value>
		public bool gradationMode
		{
			get
			{
				return currentState != CStateDefault.instance;
			}
			set
			{
				IState<CFont, object> next = value ?
					(IState<CFont, object>)CStateGradation.instance :
					(IState<CFont, object>)CStateDefault.instance;
				if(currentState != next)
				{
					nextState = next;
				}
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(GameTime gameTime)
		{
			if(timer >= 0 && --timer == 0)
			{
				Dispose();
			}
			base.update(gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>影のレイヤ番号を算出します。</summary>
		/// 
		/// <param name="fLayer">文字のレイヤ番号。</param>
		/// <param name="fShadowLayer">影のレイヤ番号。</param>
		public void getShadowLayer(out float fLayer, out float fShadowLayer)
		{
			if(layer >= 1f)
			{
				fLayer = layer;
				fShadowLayer = layer - 0.0001f;
			}
			else
			{
				fLayer = layer + 0.0001f;
				fShadowLayer = layer;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定の矩形情報より原点を算出します。</summary>
		/// 
		/// <param name="srcRect">矩形情報。</param>
		/// <returns>原点座標。</returns>
		public Vector2 getOrigin(Vector2 srcRect)
		{
			Vector2 origin = Vector2.Zero;
			switch(alignHorizontal)
			{
				case EAlign.LeftTop:
					origin.X = 0;
					break;
				case EAlign.Center:
					origin.X = srcRect.X * 0.5f;
					break;
				case EAlign.RightBottom:
					origin.X = srcRect.X;
					break;
			}
			switch(alignVertical)
			{
				case EAlign.LeftTop:
					origin.Y = 0;
					break;
				case EAlign.Center:
					origin.Y = srcRect.Y * 0.5f;
					break;
				case EAlign.RightBottom:
					origin.Y = srcRect.Y;
					break;
			}
			return origin;
		}
	}
}
