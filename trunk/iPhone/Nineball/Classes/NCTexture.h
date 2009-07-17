////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	Nineball Library Core for iPhone	Programmed by Mc/danmaq
//		──テクスチャ管理クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#import "Nineball.h"
#import <OpenGLES/EAGL.h>
#import <OpenGLES/ES1/gl.h>
#import <OpenGLES/ES1/glext.h>

//*-----------------------------------------------------------------*
// テクスチャクラスです。
// NOTE : 一部"ん・ぱか工房"のプログラムコードを使用しています。
//*-----------------------------------------------------------------*
@interface NCTexture : NSObject {
	u_char	*data;
	GLuint	name;
	GLsizei	width;
	GLsizei	height;
}

@property u_char	*data;
@property GLuint	name;
@property GLsizei	width;
@property GLsizei	height;

+ (NCTexture *)loadTexture:(NSString *)fileName;
+ (NCTexture *)makeTexture:(NSString *)text font:(UIFont *)font color:(UIColor *)color;
+ (NCTexture *)makeTexture:(UIImage *)image;
@end
