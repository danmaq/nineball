////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.data;
using danmaq.nineball.entity.fonts;
using danmaq.nineball.old.core.data;
using danmaq.nineball.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.old.core.raw
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>フォント クラス。</summary>
	/// <remarks>
	/// このクラスは旧バージョンとの互換性維持のために残されています。近い将来、順次
	/// 新バージョンの物と置換されたり、機能自体が削除されたりする可能性があります。
	/// </remarks>
	[Obsolete("このクラスは今後サポートされません。danmaq.nineball.entity.fontsに同名のクラスがありますので、そちらを使用してください。")]
	public sealed class CFont
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>影のずれの大きさ。</summary>
		public Vector2 gapShadow = new Vector2(2);

		/// <summary>スプライトフォント リソース。</summary>
		public CResource<SpriteFont> font = null;

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

		/// <summary>グラデーションを使用するかどうか。</summary>
		public bool isUseGradation = false;

		/// <summary>表示対象文字列。</summary>
		private string m_strText = string.Empty;

		/// <summary>描画レイヤ。</summary>
		private float m_flayer = 0;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		///	<param name="__font">スプライトフォント リソース</param>
		public CFont(CResource<SpriteFont> __font)
		{
			font = __font;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="__font">フォントリソース</param>
		/// <param name="__strText">テキスト</param>
		public CFont(CResource<SpriteFont> __font, string __strText)
			: this(__font)
		{
			text = __strText;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>表示対象文字列。</summary>
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

		/// <summary>描画レイヤ。</summary>
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

		/// <summary>座標誤差。</summary>
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

		/// <summary>拡大倍率。</summary>
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

		/// <summary>色輝度。</summary>
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

		/// <summary>レンダリングされたグラデーション情報</summary>
		private SFontGradationInfo[] gradation
		{
			get
			{
				int nSize = text.Length;
				char[] szText = text.ToCharArray();
				SFontGradationInfo[] result = new SFontGradationInfo[nSize];
				float fNowX = 0;
				for(int i = 0; i < nSize; i++)
				{
					float fAlpha = colorAlpha.smooth(i, nSize);
					result[i].pos = new Vector2(gapX.smooth(i, nSize) + fNowX, gapY.smooth(i, nSize));
					result[i].scale = new Vector2(scaleX.smooth(i, nSize), scaleY.smooth(i, nSize));
					//					if( isHighQuality ) { result[ i ].scale /= 4; }
					// TODO : 描画時に計算する
					result[i].rotate = rotate.smooth(i, nSize);
					result[i].strByte = szText[i].ToString();
					result[i].charSize = font.resource.MeasureString(result[i].strByte);
					result[i].argbText = new Color(
						(byte)colorRed.smooth(i, nSize), (byte)colorGreen.smooth(i, nSize),
						(byte)colorBlue.smooth(i, nSize), (byte)fAlpha);
					if(isDrawShadow)
					{
						result[i].argbShadow = new Color(0, 0, 0, (byte)(fAlpha / 1.5f));
					}
					fNowX += result[i].charSize.X * result[i].scale.X * pitch.smooth(i, nSize);
				}
				return result;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>現在のテキストをメッセージにして例外を投げます。</summary>
		/// 
		/// <exception cref="System.ApplicationException">
		/// 現在のメッセージ
		/// </exception>
		public void throwMessage()
		{
			throw new ApplicationException(text);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>フォントを描画します。</summary>
		/// 
		/// <param name="pos">座標</param>
		/// <param name="sprite">スプライト描画管理クラス</param>
		public void draw(Vector2 pos, CSprite sprite)
		{
			if(!(font == null || font.resource == null))
			{
				if(isUseGradation)
				{
					__drawEx(pos, sprite);
				}
				else
				{
					__draw(pos, sprite);
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>グラデーション機能を使用せずにフォントを描画します。</summary>
		/// 
		/// <param name="pos">座標</param>
		/// <param name="sprite">スプライト描画管理クラス</param>
		private void __draw(Vector2 pos, CSprite sprite)
		{
			Vector2 origin = getOrigin();
			if(isDrawShadow)
			{
				sprite.add(font.resource, text, pos - origin + gapShadow,
					new Color(0, 0, 0, (byte)(colorAlpha / 1.5f)), 0.0f, Vector2.Zero, scale,
					SpriteEffects.None, layer + 0.0001f, blend);
			}
			sprite.add(font.resource, text, pos - origin,
				new Color(
					(byte)colorRed, (byte)colorGreen,
					(byte)colorBlue, (byte)colorAlpha),
				0.0f, Vector2.Zero, scale, SpriteEffects.None, layer, blend);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>グラデーション機能を使用してフォントを描画します。</summary>
		/// 
		/// <param name="pos">座標</param>
		/// <param name="sprite">スプライト描画管理クラス</param>
		private void __drawEx(Vector2 pos, CSprite sprite)
		{
			SFontGradationInfo[] _gradation = gradation;
			Vector2 origin = getOrigin(_gradation);
			Vector2 _pos;
			foreach(SFontGradationInfo g in _gradation)
			{
				_pos = pos + g.pos - origin;
				if(isDrawShadow)
				{
					sprite.add(font.resource, g.strByte, _pos + gapShadow, g.argbShadow, g.rotate,
						Vector2.Zero, g.scale, SpriteEffects.None, layer + 0.0001f, blend);
				}
				sprite.add(font.resource, g.strByte, _pos, g.argbText, g.rotate, Vector2.Zero,
					g.scale, SpriteEffects.None, layer, blend);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 登録されているスプライトフォントを基準に原点を算出します。
		/// </summary>
		/// 
		/// <returns>原点座標</returns>
		private Vector2 getOrigin()
		{
			return getOrigin(font.resource.MeasureString(text) * scale);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// レンダリングされたグラデーション情報より原点を算出します。
		/// </summary>
		/// 
		/// <param name="gradation">レンダリングされたグラデーション情報</param>
		/// <returns>原点座標</returns>
		private Vector2 getOrigin(SFontGradationInfo[] gradation)
		{
			Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 max = new Vector2(float.MinValue, float.MinValue);
			foreach(SFontGradationInfo g in gradation)
			{
				min = Vector2.Min(min, g.pos);
				max = Vector2.Max(max, g.pos + g.charSize * g.scale);
			}
			return getOrigin(max - min);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定の矩形情報より原点を算出します。</summary>
		/// 
		/// <param name="srcRect">矩形情報</param>
		/// <returns>原点座標</returns>
		private Vector2 getOrigin(Vector2 srcRect)
		{
			Vector2 origin = Vector2.Zero;
			switch(alignHorizontal)
			{
				case EAlign.LeftTop:
					origin.X = 0;
					break;
				case EAlign.Center:
					origin.X = srcRect.X / 2;
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
					origin.Y = srcRect.Y / 2;
					break;
				case EAlign.RightBottom:
					origin.Y = srcRect.Y;
					break;
			}
			return origin;
		}
	}
}
