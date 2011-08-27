////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.entity.fonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.state.fonts
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>グラデーション付きのフォント状態。</summary>
	public sealed class CStateGradation
		: CState<CFont, object>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateGradation instance = new CStateGradation();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateGradation()
		{
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
		public override void setup(CFont entity, object privateMembers)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(CFont entity, object privateMembers, GameTime gameTime)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(CFont entity, object privateMembers, GameTime gameTime)
		{
			if(entity.sprite != null && entity.font != null && entity.text.Length > 0)
			{
				SFontGradationInfo[] _gradation = createGradation(entity);
				Vector2 origin = getOrigin(entity, _gradation);
				Vector2 _pos;
				float fLayer;
				float fShadowLayer;
				entity.getShadowLayer(out fLayer, out fShadowLayer);
				for (int i = _gradation.Length; --i >= 0; )
				{
					SFontGradationInfo info = _gradation[i];
					_pos = entity.pos + info.pos - origin;
					if (entity.isDrawShadow)
					{
						entity.sprite.add(entity.font, info.strByte, _pos + entity.gapShadow,
							info.argbShadow, info.rotate, Vector2.Zero, info.scale,
							SpriteEffects.None, fShadowLayer, entity.blend);
					}
					entity.sprite.add(entity.font, info.strByte, _pos, info.argbText, info.rotate,
						Vector2.Zero, info.scale, SpriteEffects.None, fLayer, entity.blend);
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>オブジェクトが別の状態へ移行する時に呼び出されます。</para>
		/// <para>このメソッドは、遷移先の<c>setup</c>よりも先に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を終了したオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="nextState">オブジェクトが次に適用する状態。</param>
		public override void teardown(CFont entity, object privateMembers, IState nextState)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// レンダリングされたグラデーション情報より原点を算出します。
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="gradation">レンダリングされたグラデーション情報</param>
		/// <returns>原点座標</returns>
		private Vector2 getOrigin(CFont entity, SFontGradationInfo[] gradation)
		{
			Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 max = new Vector2(float.MinValue, float.MinValue);
			for (int i = gradation.Length; --i >= 0; )
			{
				SFontGradationInfo info = gradation[i];
				min = Vector2.Min(min, info.pos);
				max = Vector2.Max(max, info.pos + info.charSize * info.scale);
			}
			return entity.getOrigin(max - min);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// レンダリングされたグラデーション情報を作成します。
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <returns>レンダリングされたグラデーション情報。</returns>
		private SFontGradationInfo[] createGradation(CFont entity)
		{
			int size = entity.text.Length;
			string[] text = entity.charBuffer;
			SFontGradationInfo[] result = entity.gradationBuffer;
			float fNowX = 0;
			for (int i = 0; i < size; i++)
			{
				float fAlpha = entity.colorAlpha.smooth(i, size);
				result[i].pos = new Vector2(entity.gapX.smooth(i, size) + fNowX, entity.gapY.smooth(i, size));
				result[i].scale = new Vector2(entity.scaleX.smooth(i, size), entity.scaleY.smooth(i, size));
				result[i].rotate = entity.rotate.smooth(i, size);
				result[i].strByte = text[i];
				result[i].charSize = entity.font.MeasureString(result[i].strByte);
				byte r = (byte)entity.colorRed.smooth(i, size);
				byte g = (byte)entity.colorGreen.smooth(i, size);
				byte b = (byte)entity.colorBlue.smooth(i, size);
				result[i].argbText = new Color(r, g, b, (byte)fAlpha);
				if (entity.isDrawShadow)
				{
					result[i].argbShadow = new Color(0, 0, 0, (byte)(fAlpha / 1.5f));
				}
				fNowX += result[i].charSize.X * result[i].scale.X * entity.pitch.smooth(i, size);
			}
			return result;
		}
	}
}
