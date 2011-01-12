////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#if WINDOWS
using Microsoft.DirectX.DirectInput;
#endif

namespace danmaq.nineball.util.caps
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>各設定を文字列化するクラス。</summary>
	public static class CCapsExtension
	{

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>ブール値から○×文字列を作成します。</summary>
		/// 
		/// <param name="b">ブール値</param>
		/// <returns><paramref name="b" />に対応する○×文字列</returns>
		public static string ToStringOX(this bool b)
		{
			return b ? "○" : "×";
		}

		//* -----------------------------------------------------------------------*
		/// <summary>グラフィック アダプタの環境レポートを作成します。</summary>
		/// 
		/// <param name="adapter">グラフィック アダプタ オブジェクト</param>
		/// <param name="bCurrentDevice">既定のデバイスかどうか</param>
		/// <param name="ps">ピクセル シェーダの対応バージョン</param>
		/// <param name="vs">頂点シェーダの対応バージョン</param>
		/// <returns>グラフィック アダプタの環境レポート 文字列</returns>
		public static string createCapsReport(this GraphicsAdapter adapter, out bool bCurrentDevice, out ShaderProfile ps, out ShaderProfile vs)
		{
			bCurrentDevice = adapter.IsDefaultAdapter;
			string strResult = "◎◎ グラフィック デバイス " + adapter.DeviceName + Environment.NewLine;
			strResult += "▽ グラフィック デバイス環境情報一覧" + Environment.NewLine;
			strResult += "  現在の画面モード     : " + adapter.CurrentDisplayMode.ToString() + Environment.NewLine;
			strResult += "  デバイスの名称・説明 : " + adapter.Description + Environment.NewLine;
			strResult += "  GUID                 : " + adapter.DeviceIdentifier.ToString() + Environment.NewLine;
			strResult += "  ベンダ識別ID         : " + adapter.VendorId + Environment.NewLine;
			strResult += "  デバイス識別ID       : " + adapter.DeviceId + Environment.NewLine;
			strResult += "  リビジョン レベル    : " + adapter.Revision + Environment.NewLine;
			strResult += "  サブシステム 識別ID  : " + adapter.SubSystemId + Environment.NewLine;
			strResult += "  ドライバ ファイル名  : " + adapter.DriverDll + Environment.NewLine;
			strResult += "  ドライバ バージョン  : " + adapter.DriverVersion.ToString() + Environment.NewLine;
			strResult += "  デフォルト デバイス  : " + bCurrentDevice.ToStringOX() + Environment.NewLine;
			strResult += "  ワイド画面 サポート  : " + adapter.IsWideScreen.ToStringOX() + Environment.NewLine;
			strResult += "  対応画面モード一覧   : " + Environment.NewLine;
			foreach(DisplayMode mode in adapter.SupportedDisplayModes)
			{
				strResult += "  >>  " + mode.ToString() + Environment.NewLine;
			}
			try
			{
				strResult += adapter.GetCapabilities(
					Microsoft.Xna.Framework.Graphics.DeviceType.Hardware
				).createCapsReport(out ps, out vs);
			}
			catch(Exception e)
			{
				strResult += "!▲! グラフィック デバイスの性能取得に失敗。" + Environment.NewLine + e.ToString();
				ps = ShaderProfile.Unknown;
				vs = ShaderProfile.Unknown;
			}
			return strResult;
		}
		//* -----------------------------------------------------------------------*
		/// <summary>グラフィック アダプタの性能レポートを作成します。</summary>
		/// 
		/// <param name="caps">グラフィック アダプタ性能オブジェクト</param>
		/// <param name="ps">ピクセル シェーダの対応バージョン</param>
		/// <param name="vs">頂点シェーダの対応バージョン</param>
		/// <returns>グラフィック アダプタの性能レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities caps, out ShaderProfile ps, out ShaderProfile vs)
		{
			ps = caps.MaxPixelShaderProfile;
			vs = caps.MaxVertexShaderProfile;
			string strResult = "▽ グラフィック デバイス基本能力一覧" + Environment.NewLine;
			strResult += "  ヘッド参照順序           : " + caps.AdapterOrdinalInGroup + Environment.NewLine;
			strResult += "  デバイス 種別            : " + caps.DeviceType.ToString() + Environment.NewLine;
			strResult += "  ExtentsAdjust            : " + caps.ExtentsAdjust.ToString("F02") + Environment.NewLine;
			strResult += "  ガードバンド 下端座標    : " + caps.GuardBandBottom.ToString("F02") + Environment.NewLine;
			strResult += "  ガードバンド 左端座標    : " + caps.GuardBandLeft.ToString("F02") + Environment.NewLine;
			strResult += "  ガードバンド 右端座標    : " + caps.GuardBandRight.ToString("F02") + Environment.NewLine;
			strResult += "  ガードバンド 上端座標    : " + caps.GuardBandTop.ToString("F02") + Environment.NewLine;
			strResult += "  マスタ デバイス識別ID    : " + caps.MasterAdapterOrdinal + Environment.NewLine;
			strResult += "  異方性フィルタ最大有効値 : " + caps.MaxAnisotropy + Environment.NewLine;
			strResult += "  PS30命令スロット最大数   : " + caps.MaxPixelShader30InstructionSlots + Environment.NewLine;
			strResult += "  対応最大PSバージョン     : " + caps.MaxPixelShaderProfile.ToString() + Environment.NewLine;
			strResult += "  点プリミティブ最大サイズ : " + caps.MaxPointSize.ToString("F02") + Environment.NewLine;
			strResult += "  プリミティブ最大描画数   : " + caps.MaxPrimitiveCount + Environment.NewLine;
			strResult += "  レンダリング対象最大数   : " + caps.MaxSimultaneousRenderTargets + Environment.NewLine;
			strResult += "  MaxSimultaneousTextures  : " + caps.MaxSimultaneousTextures + Environment.NewLine;
			strResult += "  データ ストリーム最大数  : " + caps.MaxStreams + Environment.NewLine;
			strResult += "  最大ストリームストライド : " + caps.MaxStreamStride + Environment.NewLine;
			strResult += "  Textureの最大Aspect比率  : " + caps.MaxTextureAspectRatio + Environment.NewLine;
			strResult += "  MaxTextureRepeat         : " + caps.MaxTextureRepeat + Environment.NewLine;
			strResult += "  対応テクスチャ最大縦幅   : " + caps.MaxTextureHeight + Environment.NewLine;
			strResult += "  対応テクスチャ最大横幅   : " + caps.MaxTextureWidth + Environment.NewLine;
			strResult += "  最大定義可能クリップ面数 : " + caps.MaxUserClipPlanes + Environment.NewLine;
			strResult += "  HWインデックス最大サイズ : " + caps.MaxVertexIndex + Environment.NewLine;
			strResult += "  VS30命令スロット最大数   : " + caps.MaxVertexShader30InstructionSlots + Environment.NewLine;
			strResult += "  定数用VSレジスタ数       : " + caps.MaxVertexShaderConstants + Environment.NewLine;
			strResult += "  対応最大VSバージョン     : " + caps.MaxVertexShaderProfile.ToString() + Environment.NewLine;
			strResult += "  対応最大Wベース深度値    : " + caps.MaxVertexW.ToString("F02") + Environment.NewLine;
			strResult += "  VolumeTexture最大サイズ  : " + caps.MaxVolumeExtent + Environment.NewLine;
			strResult += "  このグループのアダプタ数 : " + caps.NumberOfAdaptersInGroup + Environment.NewLine;
			strResult += "  PS算術Component最大値    : " + caps.PixelShader1xMaxValue.ToString("F02") + Environment.NewLine;
			strResult += "  Pixelシェーダ バージョン : " + caps.PixelShaderVersion.ToString() + Environment.NewLine;
			strResult += "  画面スワップ間隔         : " + caps.PresentInterval.ToString() + Environment.NewLine;
			strResult += "  頂点シェーダ バージョン  : " + caps.VertexShaderVersion.ToString() + Environment.NewLine;
			strResult += caps.DeviceCapabilities.createCapsReport();
			strResult += caps.DriverCapabilities.createCapsReport();
			strResult += caps.CursorCapabilities.createCapsReport();
			strResult += caps.DeclarationTypeCapabilities.createCapsReport();
			strResult += caps.LineCapabilities.createCapsReport();
			strResult += caps.PrimitiveCapabilities.createCapsReport();
			strResult += caps.TextureCapabilities.createCapsReport();
			strResult += caps.VertexFormatCapabilities.createCapsReport();
			strResult += caps.VertexProcessingCapabilities.createCapsReport();
			strResult += caps.VertexShaderCapabilities.createCapsReport();
			strResult += caps.PixelShaderCapabilities.createCapsReport();
			strResult += caps.RasterCapabilities.createCapsReport();
			strResult += caps.ShadingCapabilities.createCapsReport();
			strResult += caps.StencilCapabilities.createCapsReport();

			strResult += caps.AlphaCompareCapabilities.createCapsReport("アルファテスト");
			strResult += caps.DepthBufferCompareCapabilities.createCapsReport("深度バッファ");

			strResult += caps.CubeTextureFilterCapabilities.createCapsReport("キューブ テクスチャ");
			strResult += caps.TextureFilterCapabilities.createCapsReport("テクスチャ");
			strResult += caps.VertexTextureFilterCapabilities.createCapsReport("頂点シェーダ");
			strResult += caps.VolumeTextureFilterCapabilities.createCapsReport("ボリューム テクスチャ");

			strResult += caps.TextureAddressCapabilities.createCapsReport("テクスチャ");
			strResult += caps.VolumeTextureAddressCapabilities.createCapsReport("ボリューム テクスチャ");

			strResult += caps.DestinationBlendCapabilities.createCapsReport("転送先");
			strResult += caps.SourceBlendCapabilities.createCapsReport("転送元");

			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>グラフィック デバイス固有の機能レポートを作成します。</summary>
		/// 
		/// <param name="caps">グラフィック デバイス固有の機能オブジェクト</param>
		/// <returns>グラフィック デバイス固有の機能レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.DeviceCaps caps)
		{
			string strResult = "▽ グラフィック デバイス機能 対応能力一覧" + Environment.NewLine;
			strResult += "  (Primitive描画)DirectX5準拠   : " + caps.SupportsDrawPrimitives2.ToStringOX() + Environment.NewLine;
			strResult += "  (Primitive描画)DirectX7準拠   : " + caps.SupportsDrawPrimitives2Ex.ToStringOX() + Environment.NewLine;
			strResult += "  CopyRect2Rect(Texture source) : " + caps.IsDirect3D9Driver.ToStringOX() + Environment.NewLine;
			strResult += "  ハードウェアラスタライズ      : " + caps.SupportsHardwareRasterization.ToStringOX() + Environment.NewLine;
			strResult += "  ハードウェアTnL計算           : " + caps.SupportsHardwareTransformAndLight.ToStringOX() + Environment.NewLine;
			strResult += "  HALエクスポート               : " + caps.SupportsDrawPrimitivesTransformedVertex.ToStringOX() + Environment.NewLine;
			strResult += "  非Local VRAMへのブリット      : " + caps.CanDrawSystemToNonLocal.ToStringOX() + Environment.NewLine;
			strResult += "  フリップ後のレンダリング命令  : " + caps.CanRenderAfterFlip.ToStringOX() + Environment.NewLine;
			strResult += "  独立メモリプールのTexture処理 : " + caps.SupportsSeparateTextureMemories.ToStringOX() + Environment.NewLine;
			strResult += "  Textureに非Local VRAMを使用   : " + caps.SupportsTextureNonLocalVideoMemory.ToStringOX() + Environment.NewLine;
			strResult += "  TextureにSystemRAMを使用      : " + caps.SupportsTextureSystemMemory.ToStringOX() + Environment.NewLine;
			strResult += "  TextureにVideoRAM を使用      : " + caps.SupportsTextureVideoMemory.ToStringOX() + Environment.NewLine;
			strResult += "  実行バッファにSystemRAMを使用 : " + caps.SupportsExecuteSystemMemory.ToStringOX() + Environment.NewLine;
			strResult += "  実行バッファにVideoRAM を使用 : " + caps.SupportsExecuteVideoMemory.ToStringOX() + Environment.NewLine;
			strResult += "  頂点バッファにSystemRAMを使用 : " + caps.SupportsTransformedVertexSystemMemory.ToStringOX() + Environment.NewLine;
			strResult += "  頂点バッファにVideoRAM を使用 : " + caps.SupportsTransformedVertexVideoMemory.ToStringOX() + Environment.NewLine;
			strResult += "  頂点同士のオフセット共有      : " + caps.VertexElementScanSharesStreamOffset.ToStringOX() + Environment.NewLine;
			strResult += "  ストリーム オフセット         : " + caps.SupportsStreamOffset.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>グラフィック ドライバ固有の機能レポートを作成します。</summary>
		/// 
		/// <param name="caps">グラフィック ドライバ固有の機能オブジェクト</param>
		/// <returns>グラフィック ドライバ固有の機能レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.DriverCaps caps)
		{
			string strResult = "▽ グラフィック デバイス ドライバ機能 対応能力一覧" + Environment.NewLine;
			strResult += "  ミップマップの自動生成       : " + caps.CanAutoGenerateMipMap.ToStringOX() + Environment.NewLine;
			strResult += "  ダイナミック テクスチャ      : " + caps.SupportsDynamicTextures.ToStringOX() + Environment.NewLine;
			strResult += "  ドライバ側でのリソース管理   : " + caps.CanManageResource.ToStringOX() + Environment.NewLine;
			strResult += "  SystemRAM≫VRAMの高速転送    : " + caps.SupportsCopyToSystemMemory.ToStringOX() + Environment.NewLine;
			strResult += "  VRAM≫SystemRAMの高速転送    : " + caps.SupportsCopyToVideoMemory.ToStringOX() + Environment.NewLine;
			strResult += "  モニタから現在の走査線を取得 : " + caps.ReadScanLine.ToStringOX() + Environment.NewLine;
			strResult += "  ガンマ自動調整キャリブレータ : " + caps.CanCalibrateGamma.ToStringOX() + Environment.NewLine;
			strResult += "  Fullscreen時の動的ガンマ調整 : " + caps.SupportsFullScreenGamma.ToStringOX() + Environment.NewLine;
			strResult += "  AlphaFullScreenFlipOrDiscard : " + caps.SupportsAlphaFullScreenFlipOrDiscard.ToStringOX() + Environment.NewLine;
			strResult += "  Linear to SRGB presentation  : " + caps.SupportsLinearToSrgbPresentation.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ハードウェア カーソル対応能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">ハードウェア カーソル対応能力列挙オブジェクト</param>
		/// <returns>ハードウェア カーソル対応能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.CursorCaps caps)
		{
			string strResult = "▽ ハードウェア カーソル対応能力一覧" + Environment.NewLine;
			strResult += "  高解像度フルカラー : " + caps.SupportsColor.ToStringOX() + Environment.NewLine;
			strResult += "  低解像度フルカラー : " + caps.SupportsLowResolution.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>使用出来る頂点データ型レポートを作成します。</summary>
		/// 
		/// <param name="caps">使用出来る頂点データ型列挙オブジェクト</param>
		/// <returns>使用出来る頂点データ型レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.DeclarationTypeCaps caps)
		{
			string strResult = "▽ 使用出来る頂点データ型一覧" + Environment.NewLine;
			strResult += "  BYTE4            : " + caps.SupportsByte4.ToStringOX() + Environment.NewLine;
			strResult += "  HalfVector2      : " + caps.SupportsHalfVector2.ToStringOX() + Environment.NewLine;
			strResult += "  HalfVector4      : " + caps.SupportsHalfVector4.ToStringOX() + Environment.NewLine;
			strResult += "  Normalized101010 : " + caps.SupportsNormalized101010.ToStringOX() + Environment.NewLine;
			strResult += "  NormalizedSHORT2 : " + caps.SupportsNormalizedShort2.ToStringOX() + Environment.NewLine;
			strResult += "  NormalizedSHORT4 : " + caps.SupportsNormalizedShort4.ToStringOX() + Environment.NewLine;
			strResult += "  RG32             : " + caps.SupportsRg32.ToStringOX() + Environment.NewLine;
			strResult += "  RGBA32           : " + caps.SupportsRgba32.ToStringOX() + Environment.NewLine;
			strResult += "  RGBA64           : " + caps.SupportsRgba64.ToStringOX() + Environment.NewLine;
			strResult += "  UINT101010       : " + caps.SupportsUInt101010.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>線描画プリミティブ対応能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">線描画プリミティブ対応能力 列挙オブジェクト</param>
		/// <returns>線描画プリミティブ対応能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.LineCaps caps)
		{
			string strResult = "▽ 線描画プリミティブ対応能力一覧" + Environment.NewLine;
			strResult += "  フォグ効果            : " + caps.SupportsFog.ToStringOX() + Environment.NewLine;
			strResult += "  アルファ テスト       : " + caps.SupportsAlphaCompare.ToStringOX() + Environment.NewLine;
			strResult += "  深度バッファ テスト   : " + caps.SupportsDepthBufferTest.ToStringOX() + Environment.NewLine;
			strResult += "  アンチエイリアシング  : " + caps.SupportsAntiAlias.ToStringOX() + Environment.NewLine;
			strResult += "  ソース ブレンディング : " + caps.SupportsBlend.ToStringOX() + Environment.NewLine;
			strResult += "  テクスチャ マッピング : " + caps.SupportsTextureMapping.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>その他のプリミティブ対応能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">その他のプリミティブ対応能力 列挙オブジェクト</param>
		/// <returns>その他のプリミティブ対応能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.PrimitiveCaps caps)
		{
			string strResult = "▽ その他のドライバ プリミティブ対応能力一覧" + Environment.NewLine;
			strResult += "  レンダリングしないダミー デバイス   : " + caps.IsNullReference.ToStringOX() + Environment.NewLine;
			strResult += "  頂点毎のフォグ ブレンド係数のClamp  : " + caps.HasFogVertexClamped.ToStringOX() + Environment.NewLine;
			strResult += "  半透明色の独立したブレンド設定      : " + caps.SupportsSeparateAlphaBlend.ToStringOX() + Environment.NewLine;
			strResult += "  加算合成以外のブレンディング処理    : " + caps.SupportsBlendOperation.ToStringOX() + Environment.NewLine;
			strResult += "  変換後の頂点プリミティブをClipする  : " + caps.SupportsClipTransformedVertices.ToStringOX() + Environment.NewLine;
			strResult += "  Render対象への各色毎の書き込み      : " + caps.SupportsColorWrite.ToStringOX() + Environment.NewLine;
			strResult += "  トライアングル カリング(時計回り)   : " + caps.SupportsCullClockwiseFace.ToStringOX() + Environment.NewLine;
			strResult += "  トライアングル カリング(反時計回り) : " + caps.SupportsCullCounterClockwiseFace.ToStringOX() + Environment.NewLine;
			strResult += "  トライアングル カリングを行わない   : " + caps.SupportsCullNone.ToStringOX() + Environment.NewLine;
			strResult += "  個別フォグ及びスペキュラ アルファ   : " + caps.SupportsFogAndSpecularAlpha.ToStringOX() + Environment.NewLine;
			strResult += "  複数要素に独立した書き込みマスク    : " + caps.SupportsIndependentWriteMasks.ToStringOX() + Environment.NewLine;
			strResult += "  深度バッファ変更の有効・無効切替    : " + caps.SupportsMaskZ.ToStringOX() + Environment.NewLine;
			strResult += "  複数Render対象の異なるビット深度    : " + caps.SupportsMultipleRenderTargetsIndependentBitDepths.ToStringOX() + Environment.NewLine;
			strResult += "  複数Render対象のPS演算後の処理      : " + caps.SupportsMultipleRenderTargetsPostPixelShaderBlending.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>その他のテクスチャ マッピング対応能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">その他のテクスチャ マッピング対応能力 列挙オブジェクト</param>
		/// <returns>その他のテクスチャ マッピング対応能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.TextureCaps caps)
		{
			string strResult = "▽ その他のテクスチャ マッピング対応能力一覧" + Environment.NewLine;
			strResult += "  ピクセルのアルファ色         : " + caps.SupportsAlpha.ToStringOX() + Environment.NewLine;
			strResult += "  キューブ テクスチャ マップ   : " + caps.SupportsCubeMap.ToStringOX() + Environment.NewLine;
			strResult += "  ボリューム テクスチャ マップ : " + caps.SupportsVolumeMap.ToStringOX() + Environment.NewLine;
			strResult += "  MipMap化されたCubeMap        : " + caps.SupportsMipCubeMap.ToStringOX() + Environment.NewLine;
			strResult += "  MipMap化されたテクスチャ     : " + caps.SupportsMipMap.ToStringOX() + Environment.NewLine;
			strResult += "  MipMap化されたVolumeMap      : " + caps.SupportsMipVolumeMap.ToStringOX() + Environment.NewLine;
			strResult += "  NonPower2Conditional         : " + caps.SupportsNonPower2Conditional.ToStringOX() + Environment.NewLine;
			strResult += "  NoProjectedBumpEnvironment   : " + caps.SupportsNoProjectedBumpEnvironment.ToStringOX() + Environment.NewLine;
			strResult += "  パース補正テクスチャリング   : " + caps.SupportsPerspective.ToStringOX() + Environment.NewLine;
			strResult += "  ピクセル単位の射影除算       : " + caps.SupportsProjected.ToStringOX() + Environment.NewLine;
			strResult += "  TextureRepeatNotScaledBySize : " + caps.SupportsTextureRepeatNotScaledBySize.ToStringOX() + Environment.NewLine;
			strResult += "  Textureは正方形のみ対応      : " + caps.RequiresSquareOnly.ToStringOX() + Environment.NewLine;
			strResult += "  Textureの幅が2^nのみ対応     : " + caps.RequiresPower2.ToStringOX() + Environment.NewLine;
			strResult += "  CubeMapの幅が2^nのみ対応     : " + caps.RequiresCubeMapPower2.ToStringOX() + Environment.NewLine;
			strResult += "  VolumeMapの幅が2^nのみ対応   : " + caps.RequiresVolumeMapPower2.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>フレキシブル頂点フォーマット対応能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">フレキシブル頂点フォーマット対応能力 列挙オブジェクト</param>
		/// <returns>フレキシブル頂点フォーマット対応能力レポート 文字列</returns>
		public static string createCapsReport(this  GraphicsDeviceCapabilities.VertexFormatCaps caps)
		{
			string strResult = "▽ フレキシブル頂点フォーマット対応能力一覧" + Environment.NewLine;
			strResult += "  NumberSimultaneousTextureCoordinates : " + caps.NumberSimultaneousTextureCoordinates + Environment.NewLine;
			strResult += "  頂点要素を削除しなくて良い           : " + caps.SupportsDoNotStripElements.ToStringOX() + Environment.NewLine;
			strResult += "  頂点宣言からPointSizeを取得出来る    : " + caps.SupportsPointSize.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>頂点処理能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">頂点処理能力 列挙オブジェクト</param>
		/// <returns>頂点処理能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.VertexProcessingCaps caps)
		{
			string strResult = "▽ 頂点処理対応能力一覧" + Environment.NewLine;
			strResult += "  ローカル ビューアの実行          : " + caps.SupportsLocalViewer.ToStringOX() + Environment.NewLine;
			strResult += "  非LocalViewerModeでのTexture生成 : " + caps.SupportsNoTextureGenerationNonLocalViewer.ToStringOX() + Environment.NewLine;
			strResult += "  テクスチャ生成の実行             : " + caps.SupportsTextureGeneration.ToStringOX() + Environment.NewLine;
			strResult += "  TextureGenerationSphereMap       : " + caps.SupportsTextureGenerationSphereMap.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>頂点シェーダ2.0extended 対応能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">頂点シェーダ2.0extended 対応能力 列挙オブジェクト</param>
		/// <returns>頂点シェーダ2.0extended 対応能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.VertexShaderCaps caps)
		{
			string strResult = "▽ 頂点シェーダ2.0extended 対応能力一覧" + Environment.NewLine;
			strResult += "  動的フロー制御の最大深度 : " + caps.DynamicFlowControlDepth + Environment.NewLine;
			strResult += "  テンポラリ レジスタの数  : " + caps.NumberTemps + Environment.NewLine;
			strResult += "  静的フロー制御の最大深度 : " + caps.StaticFlowControlDepth + Environment.NewLine;
			strResult += "  命令のプレディケーション : " + caps.SupportsPredication.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ピクセル シェーダ2.0 対応能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">ピクセル シェーダ2.0 対応能力 列挙オブジェクト</param>
		/// <returns>ピクセル シェーダ2.0 対応能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.PixelShaderCaps caps)
		{
			string strResult = "▽ ピクセル シェーダ2.0 対応能力一覧" + Environment.NewLine;
			strResult += "  動的フロー制御の最大深度 : " + caps.DynamicFlowControlDepth + Environment.NewLine;
			strResult += "  静的フロー制御の最大深度 : " + caps.StaticFlowControlDepth + Environment.NewLine;
			strResult += "  命令スロットの数         : " + caps.NumberInstructionSlots + Environment.NewLine;
			strResult += "  テンポラリ レジスタの数  : " + caps.NumberTemps + Environment.NewLine;
			strResult += "  任意の入れ換え           : " + caps.SupportsArbitrarySwizzle.ToStringOX() + Environment.NewLine;
			strResult += "  グラデーション命令の使用 : " + caps.SupportsGradientInstructions.ToStringOX() + Environment.NewLine;
			strResult += "  命令のプレディケーション : " + caps.SupportsPredication.ToStringOX() + Environment.NewLine;
			strResult += "  命令毎の従属読込数の制限 : " + caps.SupportsNoDependentReadLimit.ToStringOX() + Environment.NewLine;
			strResult += "  tex命令の数の制限        : " + caps.SupportsNoTextureInstructionLimit.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ラスタ描画能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">ラスタ描画能力 列挙オブジェクト</param>
		/// <returns>ラスタ描画能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.RasterCaps caps)
		{
			string strResult = "▽ ラスタ描画対応能力一覧" + Environment.NewLine;
			strResult += "  異方性フィルタリング    : " + caps.SupportsAnisotropy.ToStringOX() + Environment.NewLine;
			strResult += "  色のパースペクティブ    : " + caps.SupportsColorPerspective.ToStringOX() + Environment.NewLine;
			strResult += "  レガシ深度バイアス      : " + caps.SupportsDepthBias.ToStringOX() + Environment.NewLine;
			strResult += "  深度Buffer不使用でのHRS : " + caps.SupportsDepthBufferLessHsr.ToStringOX() + Environment.NewLine;
			strResult += "  深度テスト処理          : " + caps.SupportsDepthBufferTest.ToStringOX() + Environment.NewLine;
			strResult += "  深度ベースのフォグ      : " + caps.SupportsDepthFog.ToStringOX() + Environment.NewLine;
			strResult += "  範囲ベースのフォグ      : " + caps.SupportsFogRange.ToStringOX() + Environment.NewLine;
			strResult += "  フォグ値のindex table化 : " + caps.SupportsFogTable.ToStringOX() + Environment.NewLine;
			strResult += "  頂点計算時にフォグ計算  : " + caps.SupportsFogVertex.ToStringOX() + Environment.NewLine;
			strResult += "  詳細レベルのBias調整    : " + caps.SupportsMipMapLevelOfDetailBias.ToStringOX() + Environment.NewLine;
			strResult += "  MultisampleのON/OFF切替 : " + caps.SupportsMultisampleToggle.ToStringOX() + Environment.NewLine;
			strResult += "  シザー テスト           : " + caps.SupportsScissorTest.ToStringOX() + Environment.NewLine;
			strResult += "  勾配スケールの深度Bias  : " + caps.SupportsSlopeScaleDepthBias.ToStringOX() + Environment.NewLine;
			strResult += "  w ベースのフォグ        : " + caps.SupportsWFog.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>シェーディング処理能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">シェーディング処理能力 列挙オブジェクト</param>
		/// <returns>シェーディング処理能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.ShadingCaps caps)
		{
			string strResult = "▽ シェーディング処理対応能力一覧" + Environment.NewLine;
			strResult += "  AlphaGouraudBlend  : " + caps.SupportsAlphaGouraudBlend.ToStringOX() + Environment.NewLine;
			strResult += "  ColorGouraudRgb    : " + caps.SupportsColorGouraudRgb.ToStringOX() + Environment.NewLine;
			strResult += "  FogGouraud         : " + caps.SupportsFogGouraud.ToStringOX() + Environment.NewLine;
			strResult += "  SpecularGouraudRgb : " + caps.SupportsSpecularGouraudRgb.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ステンシル バッファ処理能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">ステンシル バッファ処理能力 列挙オブジェクト</param>
		/// <returns>ステンシル バッファ処理能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.StencilCaps caps)
		{
			string strResult = "▽ ステンシル バッファ処理対応能力一覧" + Environment.NewLine;
			strResult += "  -1(ループ)     : " + caps.SupportsDecrement.ToStringOX() + Environment.NewLine;
			strResult += "  -1(カンスト)   : " + caps.SupportsDecrementSaturation.ToStringOX() + Environment.NewLine;
			strResult += "  +1(ループ)     : " + caps.SupportsIncrement.ToStringOX() + Environment.NewLine;
			strResult += "  +1(カンスト)   : " + caps.SupportsIncrementSaturation.ToStringOX() + Environment.NewLine;
			strResult += "  ビット反転     : " + caps.SupportsInvert.ToStringOX() + Environment.NewLine;
			strResult += "  更新されない   : " + caps.SupportsKeep.ToStringOX() + Environment.NewLine;
			strResult += "  参照値での置換 : " + caps.SupportsReplace.ToStringOX() + Environment.NewLine;
			strResult += "  2面ステンシル  : " + caps.SupportsTwoSided.ToStringOX() + Environment.NewLine;
			strResult += "  値の0設定      : " + caps.SupportsZero.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>比較能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">比較能力 列挙オブジェクト</param>
		/// <param name="strDescription">対象の解説文字列</param>
		/// <returns>比較能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.CompareCaps caps, string strDescription)
		{
			string strResult = string.Format("▽ {0} 比較対応能力一覧(A:現在、B:新しい値、R:比較結果)" + Environment.NewLine, strDescription);
			strResult += "  R = true    の演算 : " + caps.SupportsAlways.ToStringOX() + Environment.NewLine;
			strResult += "  R = false   の演算 : " + caps.SupportsNever.ToStringOX() + Environment.NewLine;
			strResult += "  R = (A ＝ B)の演算 : " + caps.SupportsEqual.ToStringOX() + Environment.NewLine;
			strResult += "  R = (A ≠ B)の演算 : " + caps.SupportsNotEqual.ToStringOX() + Environment.NewLine;
			strResult += "  R = (A ＞ B)の演算 : " + caps.SupportsGreater.ToStringOX() + Environment.NewLine;
			strResult += "  R = (A ≦ B)の演算 : " + caps.SupportsGreaterEqual.ToStringOX() + Environment.NewLine;
			strResult += "  R = (A ＞ B)の演算 : " + caps.SupportsLess.ToStringOX() + Environment.NewLine;
			strResult += "  R = (A ≧ B)の演算 : " + caps.SupportsLessEqual.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>テクスチャ フィルタリング能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">テクスチャ フィルタリング能力 列挙オブジェクト</param>
		/// <param name="strDescription">対象の解説文字列</param>
		/// <returns>テクスチャ フィルタリング能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.FilterCaps caps, string strDescription)
		{
			string strResult = string.Format("▽ {0}のテクスチャ フィルタリング対応能力一覧" + Environment.NewLine, strDescription);
			strResult += "  (拡大)異方性              : " + caps.SupportsMagnifyAnisotropic.ToStringOX() + Environment.NewLine;
			strResult += "  (縮小)異方性              : " + caps.SupportsMinifyAnisotropic.ToStringOX() + Environment.NewLine;
			strResult += "  (拡大)ガウス求積          : " + caps.SupportsMagnifyGaussianQuad.ToStringOX() + Environment.NewLine;
			strResult += "  (縮小)ガウス求積          : " + caps.SupportsMinifyGaussianQuad.ToStringOX() + Environment.NewLine;
			strResult += "  (拡大)バイリニア補完      : " + caps.SupportsMagnifyLinear.ToStringOX() + Environment.NewLine;
			strResult += "  (縮小)バイリニア補完      : " + caps.SupportsMinifyLinear.ToStringOX() + Environment.NewLine;
			strResult += "  (拡大)ポイントサンプル    : " + caps.SupportsMagnifyPoint.ToStringOX() + Environment.NewLine;
			strResult += "  (縮小)ポイントサンプル    : " + caps.SupportsMinifyPoint.ToStringOX() + Environment.NewLine;
			strResult += "  (拡大)ピラミッド サンプル : " + caps.SupportsMagnifyPyramidalQuad.ToStringOX() + Environment.NewLine;
			strResult += "  (縮小)ピラミッド サンプル : " + caps.SupportsMinifyPyramidalQuad.ToStringOX() + Environment.NewLine;
			strResult += "  (MipMap)トライリニア補完  : " + caps.SupportsMipMapLinear.ToStringOX() + Environment.NewLine;
			strResult += "  (MipMap)ポイントサンプル  : " + caps.SupportsMipMapPoint.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>テクスチャ アドレッシング能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">テクスチャ アドレッシング能力 列挙オブジェクト</param>
		/// <param name="strDescription">対象の解説文字列</param>
		/// <returns>テクスチャ アドレッシング能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.AddressCaps caps, string strDescription)
		{
			string strResult = string.Format("▽ {0}のテクスチャ アドレッシング対応能力一覧" + Environment.NewLine, strDescription);
			strResult += "  Border        : " + caps.SupportsBorder.ToStringOX() + Environment.NewLine;
			strResult += "  Clamp         : " + caps.SupportsClamp.ToStringOX() + Environment.NewLine;
			strResult += "  IndependentUV : " + caps.SupportsIndependentUV.ToStringOX() + Environment.NewLine;
			strResult += "  Mirror        : " + caps.SupportsMirror.ToStringOX() + Environment.NewLine;
			strResult += "  MirrorOnce    : " + caps.SupportsMirrorOnce.ToStringOX() + Environment.NewLine;
			strResult += "  Wrap          : " + caps.SupportsWrap.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>ブレンディング能力レポートを作成します。</summary>
		/// 
		/// <param name="caps">ブレンディング能力 列挙オブジェクト</param>
		/// <param name="strDescription">対象の解説文字列</param>
		/// <returns>ブレンディング能力レポート 文字列</returns>
		public static string createCapsReport(this GraphicsDeviceCapabilities.BlendCaps caps, string strDescription)
		{
			string strResult = string.Format("▽ {0} ブレンディング対応能力一覧" + Environment.NewLine, strDescription);
			strResult += "  Zero                    : " + caps.SupportsZero.ToStringOX() + Environment.NewLine;
			strResult += "  One                     : " + caps.SupportsOne.ToStringOX() + Environment.NewLine;
			strResult += "  BlendFactor             : " + caps.SupportsBlendFactor.ToStringOX() + Environment.NewLine;
			strResult += "  InverseSourceAlpha      : " + caps.SupportsInverseSourceAlpha.ToStringOX() + Environment.NewLine;
			strResult += "  InverseSourceColor      : " + caps.SupportsInverseSourceColor.ToStringOX() + Environment.NewLine;
			strResult += "  InverseDestinationAlpha : " + caps.SupportsInverseDestinationAlpha.ToStringOX() + Environment.NewLine;
			strResult += "  InverseDestinationColor : " + caps.SupportsInverseDestinationColor.ToStringOX() + Environment.NewLine;
			strResult += "  SourceAlphaSat          : " + caps.SupportsSourceAlphaSat.ToStringOX() + Environment.NewLine;
			strResult += "  SourceAlpha             : " + caps.SupportsSourceAlpha.ToStringOX() + Environment.NewLine;
			strResult += "  SourceColor             : " + caps.SupportsSourceColor.ToStringOX() + Environment.NewLine;
			strResult += "  DestinationAlpha        : " + caps.SupportsDestinationAlpha.ToStringOX() + Environment.NewLine;
			strResult += "  DestinationColor        : " + caps.SupportsDestinationColor.ToStringOX() + Environment.NewLine;
			strResult += "  BothInverseSourceAlpha  : " + caps.SupportsBothInverseSourceAlpha.ToStringOX() + Environment.NewLine;
			strResult += "  BothSourceAlpha         : " + caps.SupportsBothSourceAlpha.ToStringOX() + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>XBOX360コントローラの性能レポートを作成します。</summary>
		/// 
		/// <param name="caps">XBOX360コントローラの性能 列挙オブジェクト</param>
		/// <param name="index">XBOX360コントローラに対応しているプレイヤー番号</param>
		/// <returns>XBOX360コントローラの性能レポート 文字列</returns>
		public static string createCapsReport(this GamePadCapabilities caps, PlayerIndex index)
		{
			string strResult = "◎◎ プレイヤー番号 : " + index.ToString() + " XBOX360コントローラ対応能力一覧" + Environment.NewLine;
			if(caps.IsConnected)
			{
				strResult += "  コントローラ種別 : " + caps.GamePadType.ToString() + Environment.NewLine;
				strResult += "  Aボタン          : " + caps.HasAButton.ToStringOX() + Environment.NewLine;
				strResult += "  Bボタン          : " + caps.HasBButton.ToStringOX() + Environment.NewLine;
				strResult += "  Xボタン          : " + caps.HasXButton.ToStringOX() + Environment.NewLine;
				strResult += "  Yボタン          : " + caps.HasYButton.ToStringOX() + Environment.NewLine;
				strResult += "  Startボタン      : " + caps.HasStartButton.ToStringOX() + Environment.NewLine;
				strResult += "  Backボタン       : " + caps.HasBackButton.ToStringOX() + Environment.NewLine;
				strResult += "  方向キー上ボタン : " + caps.HasDPadUpButton.ToStringOX() + Environment.NewLine;
				strResult += "  方向キー下ボタン : " + caps.HasDPadDownButton.ToStringOX() + Environment.NewLine;
				strResult += "  方向キー左ボタン : " + caps.HasDPadLeftButton.ToStringOX() + Environment.NewLine;
				strResult += "  方向キー右ボタン : " + caps.HasDPadRightButton.ToStringOX() + Environment.NewLine;
				strResult += "  左サイドボタン   : " + caps.HasLeftShoulderButton.ToStringOX() + Environment.NewLine;
				strResult += "  右サイドボタン   : " + caps.HasRightShoulderButton.ToStringOX() + Environment.NewLine;
				strResult += "  左サイドトリガ   : " + caps.HasLeftTrigger.ToStringOX() + Environment.NewLine;
				strResult += "  右サイドトリガ   : " + caps.HasRightTrigger.ToStringOX() + Environment.NewLine;
				strResult += "  左スティック     : " + caps.HasLeftStickButton.ToStringOX() + Environment.NewLine;
				strResult += "  LeftXThumbStick  : " + caps.HasLeftXThumbStick.ToStringOX() + Environment.NewLine;
				strResult += "  LeftYThumbStick  : " + caps.HasLeftYThumbStick.ToStringOX() + Environment.NewLine;
				strResult += "  右スティック     : " + caps.HasRightStickButton.ToStringOX() + Environment.NewLine;
				strResult += "  RightXThumbStick : " + caps.HasRightXThumbStick.ToStringOX() + Environment.NewLine;
				strResult += "  RightYThumbStick : " + caps.HasRightYThumbStick.ToStringOX() + Environment.NewLine;
				strResult += "  左モーター       : " + caps.HasLeftVibrationMotor.ToStringOX() + Environment.NewLine;
				strResult += "  右モーター       : " + caps.HasRightVibrationMotor.ToStringOX() + Environment.NewLine;
				strResult += "  音声入出力       : " + caps.HasVoiceSupport.ToStringOX() + Environment.NewLine;
			}
			else
			{
				strResult += "  × 接続されていません。 ×" + Environment.NewLine;
			}
			return strResult;
		}

#if WINDOWS

		//* -----------------------------------------------------------------------*
		/// <summary>デバイスの識別情報レポートを作成します。</summary>
		/// 
		/// <param name="info">デバイスの識別情報 列挙オブジェクト</param>
		/// <returns>デバイスの識別情報レポート 文字列</returns>
		public static string createCapsReport(this DeviceInstance info)
		{
			string strResult = "▽ レガシ ゲームパッド デバイスの識別情報一覧\r\n";
			strResult += "  デバイス名・説明    : " + info.ProductName + Environment.NewLine;
			strResult += "  デバイスGUID        : " + info.ProductGuid.ToString() + Environment.NewLine;
			strResult += "  インスタンス登録名  : " + info.InstanceName + Environment.NewLine;
			strResult += "  インスタンスGUID    : " + info.InstanceGuid.ToString() + Environment.NewLine;
			strResult += "  ForceFeedback GUID  : " + info.FFDriverGuid.ToString() + Environment.NewLine;
			strResult += "  デバイス タイプ     : " + info.DeviceType.ToString() + Environment.NewLine;
			strResult += "  デバイス サブタイプ : " + info.DeviceSubType.ToString() + Environment.NewLine;
			strResult += "  HID 使用コード      : " + info.Usage + Environment.NewLine;
			strResult += "  HID 使用ページ      : " + info.UsagePage + Environment.NewLine;
			return strResult;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>デバイスの性能レポートを作成します。</summary>
		/// 
		/// <param name="caps">デバイスの性能 列挙オブジェクト</param>
		/// <returns>デバイスの性能レポート 文字列</returns>
		public static string createCapsReport(this DeviceCaps caps)
		{
			string strResult = "▽ レガシ ゲームパッド デバイスの能力一覧" + Environment.NewLine;
			strResult += "  ドライバ バージョン       : " + caps.FFDriverVersion + Environment.NewLine;
			strResult += "  ファームウェア バージョン : " + caps.FirmwareRevision + Environment.NewLine;
			strResult += "  ハードウェア バージョン   : " + caps.HardwareRevision + Environment.NewLine;
			strResult += "  デバイスの最小分解能      : " + caps.FFMinTimeResolution + " ミリ秒" + Environment.NewLine;
			strResult += "  フォース命令送信最小間隔  : " + caps.FFSamplePeriod + " ミリ秒" + Environment.NewLine;
			strResult += "  使用可能な軸の数          : " + caps.NumberAxes + Environment.NewLine;
			strResult += "  使用可能なボタンの数      : " + caps.NumberButtons + Environment.NewLine;
			strResult += "  使用可能なPOVの数         : " + caps.NumberPointOfViews + Environment.NewLine;
			strResult += "  別デバイスのエイリアス    : " + caps.Alias.ToStringOX() + Environment.NewLine;
			strResult += "  物理的にアタッチされた    : " + caps.Attatched.ToStringOX() + Environment.NewLine;
			strResult += "  Emulateされた仮想デバイス : " + caps.Hidden.ToStringOX() + Environment.NewLine;
			strResult += "  ユーザー モード デバイス  : " + caps.Emulated.ToStringOX() + Environment.NewLine;
			strResult += "  デッドバンド              : " + caps.DeadBand.ToStringOX() + Environment.NewLine;
			strResult += "  フォース フィードバック   : " + caps.ForceFeedback.ToStringOX() + Environment.NewLine;
			strResult += "  フェード エフェクト       : " + caps.Fade.ToStringOX() + Environment.NewLine;
			strResult += "  遅延フォース エフェクト   : " + caps.StartDelay.ToStringOX() + Environment.NewLine;
			strResult += "  条件エフェクトの飽和      : " + caps.Saturation.ToStringOX() + Environment.NewLine;
			strResult += "  PosNegCoefficients        : " + caps.PosNegCoefficients.ToStringOX() + Environment.NewLine;
			strResult += "  PosNegSaturation          : " + caps.PosNegSaturation.ToStringOX() + Environment.NewLine;
			strResult += "  プレース ホルダ           : " + caps.Phantom.ToStringOX() + Environment.NewLine;
			strResult += "  Attack                    : " + caps.Attack.ToStringOX() + Environment.NewLine;
			strResult += "  PolledDataFormat          : " + caps.PolledDataFormat.ToStringOX() + Environment.NewLine;
			strResult += "  PolledDevice              : " + caps.PolledDevice.ToStringOX() + Environment.NewLine;
			return strResult;
		}

#endif

	}
}
