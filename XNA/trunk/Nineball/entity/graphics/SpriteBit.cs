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
using System.Linq;
using System.Text;
using danmaq.nineball.entity.manager;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using danmaq.nineball.data;

namespace danmaq.nineball.entity.graphics
{

	//=========================================================================
	/// <summary>スプライト描画クラス。</summary>
	public class SpriteBit
		: ITask
	{

		// Constants ─────────────────────────────

		/// <summary>何も描画しません。</summary>
		private static Action<SpriteBit> DrawEmpty = self =>
		{
		};

		/// <summary>1フレーム分の描画処理を実行します。</summary>
		private static Action<SpriteBit> DrawInner = self =>
			self.SpriteManager.add(self.Texture, self.Position, self.AlignHorizontal, self.AlignVertical,
			self.SourceRectangle, self.Color, self.Rotation, 
				self.Scale, TextureAddressMode.Clamp, self.SpriteEffects, self.Depth, SpriteBlendMode.AlphaBlend);

		// Fields  ──────────────────────────────

		/// <summary>更新関数。</summary>
		private Func<SpriteBit, bool> executeAction;

		/// <summary>描画関数。</summary>
		private Action<SpriteBit> drawAction;

		/// <summary>表示されるかどうか。</summary>
		private bool visible;

		// Properties  ────────────────────────────

		/// <summary>スプライト管理クラスを取得および設定します。</summary>
		public CSpriteManager SpriteManager
		{
			get;
			set;
		}

		/// <summary>描画先座標を取得および設定します。</summary>
		public Vector2 Position
		{
			get;
			set;
		}

		/// <summary>描画元の切り出し矩形情報を取得および設定します。</summary>
		public Rectangle SourceRectangle
		{
			get;
			set;
		}

		/// <summary>乗算色を取得および設定します。</summary>
		public Color Color
		{
			get;
			set;
		}

		/// <summary>拡大率を取得および設定します。</summary>
		public Vector2 Scale
		{
			get;
			set;
		}

		/// <summary>水平揃え情報を取得、および設定します。</summary>
		public EAlign AlignHorizontal
		{
			get;
			set;
		}

		/// <summary>垂直揃え情報を取得、および設定します。</summary>
		public EAlign AlignVertical
		{
			get;
			set;
		}

		/// <summary>回転量(ラジアン)を取得および設定します。</summary>
		public float Rotation
		{
			get;
			set;
		}

		/// <summary>描画重ね合わせ深度(優先度)を取得および設定します。</summary>
		public float Depth
		{
			get;
			set;
		}

		/// <summary>テクスチャを取得および設定します。</summary>
		public Texture2D Texture
		{
			get;
			set;
		}

		/// <summary>テクスチャ・アドレッシングモードを取得、および設定します。</summary>
		public TextureAddressMode TextureAddressMode
		{
			get;
			set;
		}

		/// <summary>スプライト反転情報を取得および設定します。</summary>
		public SpriteEffects SpriteEffects
		{
			get;
			set;
		}

		/// <summary>スプライト合成モードを取得および設定します。</summary>
		public SpriteBlendMode SpriteBlendMode
		{
			get;
			set;
		}

		/// <summary>表示されるかどうかを取得および設定します。</summary>
		public bool Visible
		{
			get
			{
				return visible;
			}
			set
			{
				visible = value;
				if (value)
				{
					drawAction = DrawInner;
				}
				else
				{
					drawAction = DrawEmpty;
				}
			}
		}

		/// <summary>更新関数を取得および設定します。</summary>
		public Func<SpriteBit, bool> ExecuteDelegate
		{
			get
			{
				return executeAction;
			}
			set
			{
				executeAction = value ?? (Func<SpriteBit, bool>)(b => true);
			}
		}

		// Constructor ────────────────────────────

		//=====================================================================
		/// <summary>コンストラクタ。</summary>
		public SpriteBit()
		{
			Initialize();
		}

		// Methods ──────────────────────────────

		//=====================================================================
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public void update(GameTime gameTime)
		{
			ExecuteDelegate(this);
		}

		//=====================================================================
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public void draw(GameTime gameTime)
		{
			drawAction(this);
		}

		//=====================================================================
		/// <summary>タスクをリセットします。</summary>
		public void Dispose()
		{
			Initialize();
		}

		//=====================================================================
		/// <summary>初期化します。</summary>
		private void Initialize()
		{
			AlignHorizontal = EAlign.LeftTop;
			AlignVertical = EAlign.LeftTop;
			Color = Color.White;
			Position = Vector2.Zero;
			Rotation = 0f;
			Scale = Vector2.One;
			SourceRectangle = Rectangle.Empty;
			TextureAddressMode = TextureAddressMode.Clamp;
			SpriteEffects = SpriteEffects.None;
			SpriteBlendMode = SpriteBlendMode.AlphaBlend;
			Depth = 0f;
			SpriteManager = null;
			Texture = null;
			ExecuteDelegate = null;
			Visible = true;
		}
	}
}
