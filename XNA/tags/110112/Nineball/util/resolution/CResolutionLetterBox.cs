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
using Microsoft.Xna.Framework;

namespace danmaq.nineball.util.resolution
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レターボックス座標変換する解像度管理クラス。</summary>
	public sealed class CResolutionLetterBox
		: CResolutionBase
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>水平位置揃えのX座標減衰率プリセット一覧。</summary>
		private readonly float[] decayRate;

		/// <summary>最小値または最大値を取得するためのデリゲート。</summary>
		private readonly Func<float, float, float> minmax;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>水平位置揃え情報。</summary>
		private EAlign m_alignHorizontal;

		/// <summary>垂直位置揃え情報。</summary>
		private EAlign m_alignVertical;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="source">変換元の解像度。</param>
		/// <param name="destination">変換先の解像度。</param>
		/// <param name="cut">
		/// アスペクト比が異り、かつこの値が<c>true</c>である場合、拡大してはみ出した
		/// 分をカットします。一方<c>false</c>の場合、縮小して余白を表示します。
		/// アスペクト比が一致する場合、この引数によって何も変化しません。
		/// </param>
		public CResolutionLetterBox(EResolution source, EResolution destination, bool cut)
			: this(source, destination, EAlign.Center, EAlign.Center, cut)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="source">変換元の解像度。</param>
		/// <param name="destination">変換先の解像度。</param>
		/// <param name="cut">
		/// アスペクト比が異り、かつこの値が<c>true</c>である場合、拡大してはみ出した
		/// 分をカットします。一方<c>false</c>の場合、縮小して余白を表示します。
		/// アスペクト比が一致する場合、この引数によって何も変化しません。
		/// </param>
		public CResolutionLetterBox(Rectangle source, Rectangle destination, bool cut)
			: this(source, destination, EAlign.Center, EAlign.Center, cut)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="source">変換元の解像度。</param>
		/// <param name="destination">変換先の解像度。</param>
		/// <param name="alignHorizontal">水平位置揃え情報。</param>
		/// <param name="alignVertical">垂直位置揃え情報。</param>
		/// <param name="cut">
		/// アスペクト比が異り、かつこの値が<c>true</c>である場合、拡大してはみ出した
		/// 分をカットします。一方<c>false</c>の場合、縮小して余白を表示します。
		/// アスペクト比が一致する場合、この引数によって何も変化しません。
		/// </param>
		public CResolutionLetterBox(EResolution source, EResolution destination,
			EAlign alignHorizontal, EAlign alignVertical, bool cut)
			: this(source.toRect(), destination.toRect(), alignHorizontal, alignVertical, cut)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="source">変換元の解像度。</param>
		/// <param name="destination">変換先の解像度。</param>
		/// <param name="alignHorizontal">水平位置揃え情報。</param>
		/// <param name="alignVertical">垂直位置揃え情報。</param>
		/// <param name="cut">
		/// アスペクト比が異り、かつこの値が<c>true</c>である場合、拡大してはみ出した
		/// 分をカットします。一方<c>false</c>の場合、縮小して余白を表示します。
		/// アスペクト比が一致する場合、この引数によって何も変化しません。
		/// </param>
		public CResolutionLetterBox(Rectangle source, Rectangle destination,
			EAlign alignHorizontal, EAlign alignVertical, bool cut)
				: base(source, destination)
		{
			decayRate = new float[(int)EAlign.__reserved];
			decayRate[(int)EAlign.LeftTop] = 0;
			decayRate[(int)EAlign.Center] = 0.5f;
			decayRate[(int)EAlign.RightBottom] = 1f;
			m_alignHorizontal = alignHorizontal;
			m_alignVertical = alignVertical;
			minmax = cut ?
				(Func<float, float, float>)MathHelper.Max :
				(Func<float, float, float>)MathHelper.Min;
			calcurate();
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>水平位置揃え情報を取得/設定します。</summary>
		/// 
		/// <value>水平位置揃え情報。</value>
		public EAlign alignHorizontal
		{
			get
			{
				return m_alignHorizontal;
			}
			set
			{
				m_alignHorizontal = value;
				calcurate();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>垂直位置揃え情報を取得/設定します。</summary>
		/// 
		/// <value>垂直位置揃え情報。</value>
		public EAlign alignVertical
		{
			get
			{
				return m_alignVertical;
			}
			set
			{
				m_alignVertical = value;
				calcurate();
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>現在設定されている座標で計算します。</summary>
		protected override void calcurate()
		{
			base.calcurate();
			bool side = ((int)top & 1) == 1;
			Vector2 src = new Vector2(
				side ? source.Height : source.Width, side ? source.Width : source.Height);
			Vector2 dst = new Vector2(destination.Width, destination.Height);
			scale = new Vector2(minmax(dst.X / src.X, dst.Y / src.Y));
			Vector2 _gap = dst - src * scale;
			gap = new Vector2(
				_gap.X * decayRate[(int)alignHorizontal], _gap.Y * decayRate[(int)alignVertical]);
		}
	}
}
