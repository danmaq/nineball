#import "nineball.h"
#import <OpenGLES/EAGL.h>
#import <OpenGLES/ES1/gl.h>
#import <OpenGLES/ES1/glext.h>

//EAGLViewの実装
@interface NCGLView : UIView {    
@private
    GLint backingWidth; //バックバッファの幅
    GLint backingHeight;//バックバッファの高さ
	
    EAGLContext *context;//コンテキスト
    
    GLuint viewRenderbuffer; //レンダーバッファ
    GLuint viewFramebuffer;  //フレームバッファ
    GLuint depthRenderbuffer;//デプスレンダーバッファ
    
    NSTimer        *animationTimer;   //アニメーションタイマー
    NSTimeInterval animationInterval;//アニメーションインターバル
    
    BOOL initFlag;//初期化フラグ
}

//プロパティ
@property(readonly) GLint backingWidth;
@property(readonly) GLint backingHeight;
@property NSTimeInterval animationInterval;

//メソッド
- (void)startAnimation;
- (void)stopAnimation;
- (void)setup;
- (void)draw;
@end