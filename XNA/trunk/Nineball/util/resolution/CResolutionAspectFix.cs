////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.data;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.util.resolution
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>アスペクト比を4:3に固定した解像度管理クラス。</summary>
	public sealed class CResolutionAspectFix : CResolution
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>現在の解像度とVGAとの比率ギャップ。</summary>
		private float m_fScaleGapFromVGA;

		/// <summary>水平位置揃え。</summary>
		private EAlign m_align = EAlign.LeftTop;

		/// <summary>水平位置揃えによる配置する座標の誤差。</summary>
		private int m_nXGap = 0;

		/// <summary>微調整用拡大率。</summary>
		private float m_scale = 1.0f;

		/// <summary>微調整用拡大率設定時の左上座標。</summary>
		private Vector2 m_pos = Vector2.Zero;

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>現在の解像度とVGAとの比率ギャップ。</summary>
		public new float scaleGapFromVGA
		{
			get
			{
				return m_fScaleGapFromVGA * m_scale;
			}
			private set
			{
				m_fScaleGapFromVGA = value;
			}
		}

		/// <summary>現在の解像度。</summary>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 予約値を設定しようとした場合。
		/// </exception>
		public override EResolution now
		{
			get
			{
				return base.now;
			}
			protected set
			{
				base.now = value;
				if(vertical)
				{
					scaleGapFromVGA =
						getScaleGap(rotate(EResolution.VGA.toRect()), value.toRect()).Y;
				}
				else
				{
					scaleGapFromVGA = getScaleGap(value, EResolution.VGA).Y;
				}
			}
		}

		/// <summary>水平位置揃え。</summary>
		public EAlign align
		{
			get
			{
				return m_align;
			}
			set
			{
				m_align = value;
				if(value == EAlign.LeftTop)
				{
					m_nXGap = 0;
				}
				else
				{
					int nGap = rect.Width - resizeFromVGA(EResolution.VGA.toRect()).Width;
					switch(value)
					{
						case EAlign.Center:
							m_nXGap = nGap >> 1;
							break;
						case EAlign.RightBottom:
							m_nXGap = nGap;
							break;
					}
				}
			}
		}

		/// <summary>微調整用拡大率。</summary>
		public float scale
		{
			get
			{
				return m_scale;
			}
			set
			{
				m_pos = Vector2.Zero;
				m_scale = 1.0f;
				Rectangle r = resizeFromVGA(EResolution.VGA.toRect());
				r.Width += m_nXGap;
				m_pos.X = (r.Width - r.Width * value) * 0.5f;
				m_pos.Y = (r.Height - r.Height * value) * 0.5f;
				m_scale = value;
			}
		}

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		public CResolutionAspectFix() : this(null)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="graphicsDeviceManager">
		/// グラフィック デバイスの構成・管理クラス。
		/// </param>
		public CResolutionAspectFix(GraphicsDeviceManager graphicsDeviceManager)
			: base(graphicsDeviceManager)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="r">解像度定数</param>
		public CResolutionAspectFix(EResolution r) : base(r)
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>VGA基準の値を引き延ばします。</summary>
		/// 
		/// <param name="srcRect">VGA基準の値</param>
		/// <returns>現在の解像度基準の値</returns>
		public override Rectangle resizeFromVGA(Rectangle srcRect)
		{
			float fScaleGap = scaleGapFromVGA;
			return new Rectangle(
				(int)(m_pos.X + fScaleGap * srcRect.X + scale * m_nXGap),
				(int)(m_pos.Y + fScaleGap * srcRect.Y),
				(int)(fScaleGap * srcRect.Width),
				(int)(fScaleGap * srcRect.Height));
		}

		//* -----------------------------------------------------------------------*
		/// <summary>VGA基準の値を引き延ばします。</summary>
		/// 
		/// <param name="srcPoint">VGA基準の値</param>
		/// <returns>現在の解像度基準の値</returns>
		public override Vector2 resizeFromVGA(Vector2 srcPoint)
		{
			Vector2 result;
			result = srcPoint * scaleGapFromVGA;
			result.X += m_nXGap * scale;
			return result + m_pos;
		}
	}
}
