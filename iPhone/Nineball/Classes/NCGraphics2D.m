////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	Nineball Library Core for iPhone	Programmed by Mc/danmaq
//		──2D描画クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#import "NCGraphics2D.h"

//テクスチャ頂点情報
const GLfloat panelVertices[]={
0,  0,		0, -1, //左 上／下
1,  0,		1, -1, //右 上／下
};

//テクスチャUV情報
const GLfloat panelUVs[]={
0.0f, 0.0f,		0.0f, 1.0f, //左 上／下
1.0f, 0.0f,		1.0f, 1.0f, //右 上／下
};

///////////////////////////////////////////////////////////
//%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%//
///////////////////////////////////////////////////////////

@implementation NCGraphics2D

// ========================================================
// コンストラクタ。
//
// RETURN (id) インスタンス オブジェクト
- (id)init{
	self = [super init];
	if(self != nil){
		_scaleW = 1.0f;
		_scaleH = 1.0f;
		backingWidth = 320;
		backingHeight = 460;
		colAlpha = 255;
	}
	return self;
}

///////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////

// ========================================================
// セットアップ
//
// PARAM (GLint)pBackingWidth バックバッファの横幅
// PARAM (GLint)pBackingHeight バックバッファの縦幅
- (void)setupW:(GLint)pBackingWidth h:(GLint)pBackingHeight{
	//バックバッファのサイズ設定
	backingWidth = pBackingWidth;
	backingHeight = pBackingHeight;
	//ビューポート／投影変換
	glViewport(0, 0, pBackingWidth, pBackingHeight);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	GLint backingWidthHalf = pBackingWidth / 2;
	GLint backingHeightHalf = pBackingHeight / 2;
	glOrthof(-backingWidthHalf, backingWidthHalf, -backingHeightHalf, backingHeightHalf, -100, 100);
	glTranslatef(-backingWidthHalf, backingHeightHalf, 0);
	//モデリング変換
	glMatrixMode(GL_MODELVIEW);
	glClearColor(0, 0, 0, 1);
	//頂点配列／UVの設定
	glVertexPointer(2, GL_FLOAT, 0, panelVertices);
	glEnableClientState(GL_VERTEX_ARRAY);
	glTexCoordPointer(2, GL_FLOAT, 0, panelUVs);
	//テクスチャ／ブレンド／ポイントの設定
	glEnableClientState(GL_TEXTURE_COORD_ARRAY);
	glEnable(GL_TEXTURE_2D);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);
	glEnable(GL_POINT_SMOOTH);
}

// ========================================================
// クリッピングの指定
//
// PARAM (GLfloat)x X座標
// PARAM (GLfloat)y Y座標
// PARAM (GLfloat)w 横幅
// PARAM (GLfloat)h 縦幅
- (void)clipRectX:(GLfloat)x y:(GLfloat)y w:(GLfloat)w h:(GLfloat)h{
	GLfloat area[4][4] = {
		{ 0, -1, 0, -y },	{ 0, 1, 0, y + h },
		{ 1, 0, 0, -x },	{ -1, 0, 0, x + h }
	};
	glClipPlanef(GL_CLIP_PLANE0, area[0]);
	glClipPlanef(GL_CLIP_PLANE1, area[1]);
	glClipPlanef(GL_CLIP_PLANE2, area[2]);
	glClipPlanef(GL_CLIP_PLANE3, area[3]);
	glEnable(GL_CLIP_PLANE0);
	glEnable(GL_CLIP_PLANE1);
	glEnable(GL_CLIP_PLANE2);
	glEnable(GL_CLIP_PLANE3);
}

// ========================================================
// クリッピングのクリア
- (void)clearClip{
	glDisable(GL_CLIP_PLANE0);
	glDisable(GL_CLIP_PLANE1);
	glDisable(GL_CLIP_PLANE2);
	glDisable(GL_CLIP_PLANE3);
}

// ========================================================
// 色の指定
//
// PARAM (GLbyte)r 赤
// PARAM (GLbyte)g 緑
// PARAM (GLbyte)b 青
// PARAM (GLbyte)a 透過度
- (void)setColorR:(GLbyte)r g:(GLbyte)g b:(GLbyte)b a:(GLbyte)a{
	colRed = r;
	colGreen = g;
	colBlue = b;
	colAlpha = a;
}

// ========================================================
// 色の指定
//
// PARAM (GLbyte)r 赤
// PARAM (GLbyte)g 緑
// PARAM (GLbyte)b 青
- (void)setColorR:(GLbyte)r g:(GLbyte)g b:(GLbyte)b{ [self setColorR:r g:g b:b a:255]; }

// ========================================================
// ライン幅の指定
//
// PARAM (GLfloat)pLineWidth ラインの幅
- (void)setLineWidth:(GLfloat)pLineWidth{
	glLineWidth(pLineWidth);
	glPointSize(pLineWidth * 0.8);
}

// ========================================================
// 平行移動の指定
//
// PARAM (GLfloat)transX X誤差
// PARAM (GLfloat)transY Y誤差
- (void)setTranslateX:(GLfloat)transX y:(GLfloat)transY{ glTranslatef(transX, -transY, 0); }

// ========================================================
// 回転角度の指定
//
// PARAM (GLfloat)rotate 回転率
- (void)setRotate:(GLfloat)rotate{ _rotate = rotate; }

// ========================================================
// スケールの指定
//
// PARAM (GLfloat)scaleW 横方向拡大率
// PARAM (GLfloat)scaleH 縦方向拡大率
- (void)setScaleW:(GLfloat)scaleW h:(GLfloat)scaleH{
	_scaleW = scaleW;
	_scaleH = scaleH;
}

// ========================================================
// 
- (void)commitMatrix{
	glScalef(_scaleW, _scaleH, 1);
	glRotatef(_rotate, 0, 0, 1);
}

// ========================================================
// 行列のプッシュ
- (void)pushMatrix{ glPushMatrix(); }

// ========================================================
// 行列のポップ
- (void)popMatrix{ glPopMatrix(); }

// ========================================================
// ラインの描画
//
// PARAM (GLfloat)x0 始点X座標
// PARAM (GLfloat)y0 始点Y座標
// PARAM (GLfloat)x1 終点X座標
// PARAM (GLfloat)y1 終点Y座標
- (void)drawLineX0:(GLfloat)x0 y0:(GLfloat)y0 x1:(GLfloat)x1 y1:(GLfloat)y1{
	GLfloat x[2] = { x0, x1 };
	GLfloat y[2] = { y0, y1 };
	[self drawPolylineX:x y:y length:2 fill:0];
}

// ========================================================
// ポリラインの描画
//
// PARAM (GLfloat)x0 始点X座標
// PARAM (GLfloat)y0 始点Y座標
// PARAM (GLfloat)x1 終点X座標
// PARAM (GLfloat)y1 終点Y座標
// PARAM (char)fill 'B':BOX / 'F':FILL
- (void)drawPolylineX:(GLfloat[])x y:(GLfloat[])y length:(GLint)length fill:(char)fill{
	//頂点配列情報
	GLfloat *vertexs = (GLfloat *)malloc(length * 3 * sizeof(GLfloat));
	for(int i = 0; i < length; i++){
		vertexs[i * 3 + 0] = x[i];
		vertexs[i * 3 + 1] = -y[i];
		vertexs[i * 3 + 2] = 0;
	}
	//カラー配列情報
	GLbyte *colors = (GLbyte *)malloc(length * 4 * sizeof(GLbyte));
	for(int i = 0; i < length; i++){
		colors[i * 4 + 0] = colRed;
		colors[i * 4 + 1] = colGreen;
		colors[i * 4 + 2] = colBlue;
		colors[i * 4 + 3] = colAlpha;
	}
	//ラインの描画
	glBindTexture(GL_TEXTURE_2D, 0);
	glEnableClientState(GL_COLOR_ARRAY);
	glVertexPointer(3, GL_FLOAT, 0, vertexs);
	glColorPointer(4, GL_UNSIGNED_BYTE, 0, colors);
	glPushMatrix();
	u_int mode;
	switch(fill){
		case 'F':	mode = GL_TRIANGLE_STRIP;	break;
		case 'B':	mode = GL_LINE_LOOP;		break;
		default:	mode = GL_LINE_STRIP;		break;
	}
	[self commitMatrix];
	glDrawArrays(mode, 0, length);
	if(length > 2){ glDrawArrays(GL_POINTS, 0, length); }
	glPopMatrix();
	//メモリ解放
	SAFE_FREE(colors);
	SAFE_FREE(vertexs);
}

// ========================================================
// ポリラインの描画
//
// PARAM (GLfloat[])x X座標一覧の配列
// PARAM (GLfloat[])y Y座標一覧の配列
// PARAM (GLint)length 配列長
- (void)drawPolylineX:(GLfloat[])x y:(GLfloat[])y length:(GLint)length{
	[self drawPolylineX:x y:y length:length fill:0];
}

// ========================================================
// 矩形の描画
//
// PARAM (GLfloat)x X座標
// PARAM (GLfloat)y X座標
// PARAM (GLfloat)w 横幅
// PARAM (GLfloat)h 縦幅
- (void)drawRectX:(GLfloat)x y:(GLfloat)y w:(GLfloat)w h:(GLfloat)h{
	GLfloat _x[4] = { x, x, x + w, x + w };
	GLfloat _y[4] = { y, y + h, y + h, y };
	[self drawPolylineX:_x y:_y length:4 fill:'B'];
}

// ========================================================
// 矩形の塗り潰し
//
// PARAM (GLfloat)x X座標
// PARAM (GLfloat)y X座標
// PARAM (GLfloat)w 横幅
// PARAM (GLfloat)h 縦幅
- (void)fillRectX:(GLfloat)x y:(GLfloat)y w:(GLfloat)w h:(GLfloat)h{
	GLfloat _x[4] = { x, x, x + w, x + w };
	GLfloat _y[4] = { y, y + h, y, y + h };
	[self drawPolylineX:_x y:_y length:4 fill:'F'];
}

// ========================================================
// 円の描画
//
// PARAM (GLfloat)x X座標
// PARAM (GLfloat)y X座標
// PARAM (GLfloat)r 半径
// PARAM (BOOL)fill 塗りつぶすかどうか
- (void)drawCircleX:(GLfloat)x y:(GLfloat)y r:(GLfloat)r fill:(BOOL)fill{
	int addv = fill ? 2 : 0;
	int length = 100 + addv;
	//頂点配列情報
	GLfloat *vertexs = (GLfloat *)malloc(length * 3 * sizeof(GLfloat));
	if(fill){
		vertexs[0] = x;
		vertexs[1] = -y;
		vertexs[2] = 0;
	}
	for(int i = fill ? 1 : 0; i < length; i++){
		float angle = 2 * M_PI * i / (length - addv);
		vertexs[i * 3 + 0] = x + cos(angle) * r;
		vertexs[i * 3 + 1] = -y + sin(angle) * r;
		vertexs[i * 3 + 2] = 0;
	}
	//カラー配列情報
	GLbyte *colors = (GLbyte *)malloc(length * 4 * sizeof(GLbyte));
	for(int i = 0;i < length; i++){
		colors[i * 4 + 0] = colRed;
		colors[i * 4 + 1] = colGreen;
		colors[i * 4 + 2] = colBlue;
		colors[i * 4 + 3] = colAlpha;
	}
	//ラインの描画
	glBindTexture(GL_TEXTURE_2D, 0);
	glEnableClientState(GL_COLOR_ARRAY);
	glVertexPointer(3, GL_FLOAT, 0, vertexs);
	glColorPointer(4, GL_UNSIGNED_BYTE, 0, colors);
	glPushMatrix();
	[self commitMatrix];
	glDrawArrays(fill ? GL_TRIANGLE_FAN : GL_LINE_LOOP, 0, length);
	if(!fill){ glDrawArrays(GL_POINTS, 0, length); }
	glPopMatrix();
	//メモリ解放
	SAFE_FREE(colors);
	SAFE_FREE(vertexs);
}

// ========================================================
// 円の描画
//
// PARAM (GLfloat)x X座標
// PARAM (GLfloat)y X座標
// PARAM (GLfloat)r 半径
- (void)drawCircleX:(GLfloat)x y:(GLfloat)y r:(GLfloat)r{ [self drawCircleX:x y:y r:r fill:NO]; }

// ========================================================
// 円の塗り潰し
//
// PARAM (GLfloat)x X座標
// PARAM (GLfloat)y X座標
// PARAM (GLfloat)r 半径
- (void)fillCircleX:(GLfloat)x y:(GLfloat)y r:(GLfloat)r{ [self drawCircleX:x y:y r:r fill:YES]; }

// ========================================================
// テクスチャの描画
//
// PARAM (NCTexture *)texture テクスチャへのポインタ
// PARAM (GLfloat)x X座標
// PARAM (GLfloat)y X座標
- (void)drawTexture:(NCTexture *)texture x:(GLfloat)x y:(GLfloat)y{
	//カラー配列情報
	int length = 4;
	GLbyte *colors = (GLbyte *)malloc(length * 4 * sizeof(GLbyte));
	for(int i = 0;i < length; i++){
		colors[i * 4 + 0] = colRed;
		colors[i * 4 + 1] = colGreen;
		colors[i * 4 + 2] = colBlue;
		colors[i * 4 + 3] = colAlpha;
	}
	
	glBindTexture(GL_TEXTURE_2D, texture.name);
	glDisableClientState(GL_COLOR_ARRAY);
	glColorPointer(4, GL_UNSIGNED_BYTE, 0, colors);
	
	glVertexPointer(2, GL_FLOAT, 0, panelVertices);
	glPushMatrix();
	GLsizei halfWidth = texture.width / 2;
	GLsizei halfHeight = texture.height / 2;
	glTranslatef(x, -(y), 0);
	glTranslatef(halfWidth, -halfHeight, 0);
	[self commitMatrix];
	glTranslatef(-halfWidth, halfHeight, 0);
	glScalef(texture.width, texture.height, 1);
	glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);
	glPopMatrix();
	//メモリ解放
	SAFE_FREE(colors);
}

@end
