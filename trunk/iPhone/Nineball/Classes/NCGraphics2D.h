////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	Nineball Library Core for iPhone	Programmed by Mc/danmaq
//		──2D描画クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#import "NCTexture.h"

@interface NCGraphics2D : NSObject{
	GLfloat _scaleW;
	GLfloat _scaleH;
	GLfloat _rotate;
	GLint backingWidth;
	GLint backingHeight;
	GLbyte colRed;
	GLbyte colGreen;
	GLbyte colBlue;
	GLbyte colAlpha;
}

- (void)setupW:(GLint)pBackingWidth h:(GLint)pBackingHeight;
- (void)clipRectX:(GLfloat)x y:(GLfloat)y w:(GLfloat)w h:(GLfloat)h; 
- (void)clearClip;
- (void)setColorR:(GLbyte)r g:(GLbyte)g b:(GLbyte)b a:(GLbyte)a;
- (void)setColorR:(GLbyte)r g:(GLbyte)g b:(GLbyte)b;
- (void)setLineWidth:(GLfloat)pLineWidth;
- (void)setTranslateX:(GLfloat)transX y:(GLfloat)transY;
- (void)setRotate:(GLfloat)rotate;
- (void)setScaleW:(GLfloat)scaleW h:(GLfloat)scaleH;
- (void)commitMatrix;
- (void)pushMatrix;
- (void)popMatrix;
- (void)drawLineX0:(GLfloat)x0 y0:(GLfloat)y0 x1:(GLfloat)x1 y1:(GLfloat)y1;
- (void)drawPolylineX:(GLfloat[])x y:(GLfloat[])y length:(GLint)length fill:(char)fill;
- (void)drawPolylineX:(GLfloat[])x y:(GLfloat[])y length:(GLint)length;
- (void)drawRectX:(GLfloat)x y:(GLfloat)y w:(GLfloat)w h:(GLfloat)h;
- (void)fillRectX:(GLfloat)x y:(GLfloat)y w:(GLfloat)w h:(GLfloat)h;
- (void)drawCircleX:(GLfloat)x y:(GLfloat)y r:(GLfloat)r;
- (void)fillCircleX:(GLfloat)x y:(GLfloat)y r:(GLfloat)r;
- (void)drawTexture:(NCTexture *)texture x:(GLfloat)x y:(GLfloat)y;

@end
