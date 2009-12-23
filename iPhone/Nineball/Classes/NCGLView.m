#import <QuartzCore/QuartzCore.h>
#import <OpenGLES/EAGLDrawable.h>
#import "NCGLView.h"

#define USE_DEPTH_BUFFER 0

//EAGLViewのプライベートメソッド
@interface NCGLView ()
@property (nonatomic,retain) EAGLContext* context;
@property (nonatomic,assign) NSTimer* animationTimer;
- (BOOL)createFramebuffer;
- (void)destroyFramebuffer;
- (void)drawView;
@end


//EAGLViewの実装
@implementation NCGLView

//シンセサイズ
@synthesize context;
@synthesize animationTimer;
@synthesize backingWidth;
@synthesize backingHeight;
@synthesize animationInterval;


//レイヤーの取得
+ (Class)layerClass {
    return [CAEAGLLayer class];
}

//初期化
- (id)init {
    if ((self=[super init])) {
        initFlag=NO;
    }
    return self;
}

//フレームの初期化
- (id)initWithFrame:(CGRect)frame {
    if (self=[super initWithFrame:frame]) {
        //レイヤーの生成
        CAEAGLLayer* eaglLayer=(CAEAGLLayer*)self.layer;
        eaglLayer.opaque=YES;
        eaglLayer.drawableProperties=[NSDictionary dictionaryWithObjectsAndKeys:
									  [NSNumber numberWithBool:NO], 
									  kEAGLDrawablePropertyRetainedBacking, 
									  kEAGLColorFormatRGBA8, 
									  kEAGLDrawablePropertyColorFormat, 
									  nil];
        
        //コンテキストの生成
        context=[[EAGLContext alloc] initWithAPI:kEAGLRenderingAPIOpenGLES1];
        if (!context || ![EAGLContext setCurrentContext:context]) {
            [self release];
            return nil;
        }
        
        //アニメーションインターバルの指定
        animationInterval=1.0/60.0;
    }
    return self;
}

//メモリの解放
- (void)dealloc {    
    [self stopAnimation];    
    if ([EAGLContext currentContext]==context) {
        [EAGLContext setCurrentContext:nil];
    }    
    [context release];  
    [super dealloc];
}

//ビューの描画
- (void)drawView {
    //前処理
    [EAGLContext setCurrentContext:context];
    glBindFramebufferOES(GL_FRAMEBUFFER_OES,viewFramebuffer);
	
    //描画
    if (!initFlag) {
        [self setup];
        initFlag=YES;
    }
    [self draw];  
	
    //後処理
    glBindRenderbufferOES(GL_RENDERBUFFER_OES,viewRenderbuffer);
    [context presentRenderbuffer:GL_RENDERBUFFER_OES];
}

//セットアップ(オーバーライド用)
- (void)setup {
}

//描画(オーバーライド用)
- (void)draw {
}

//サブビューのレイアウト
- (void)layoutSubviews {
    [EAGLContext setCurrentContext:context];
    [self destroyFramebuffer];
    [self createFramebuffer];
    [self drawView];
}

//フレームバッファの生成
- (BOOL)createFramebuffer {
    glGenFramebuffersOES(1,&viewFramebuffer);
    glGenRenderbuffersOES(1,&viewRenderbuffer);
    glBindFramebufferOES(GL_FRAMEBUFFER_OES,viewFramebuffer);
    glBindRenderbufferOES(GL_RENDERBUFFER_OES,viewRenderbuffer);
    [context renderbufferStorage:GL_RENDERBUFFER_OES fromDrawable:(CAEAGLLayer*)self.layer];
    glFramebufferRenderbufferOES(
		GL_FRAMEBUFFER_OES,GL_COLOR_ATTACHMENT0_OES,GL_RENDERBUFFER_OES,viewRenderbuffer);   
    glGetRenderbufferParameterivOES(GL_RENDERBUFFER_OES,GL_RENDERBUFFER_WIDTH_OES,&backingWidth);
    glGetRenderbufferParameterivOES(GL_RENDERBUFFER_OES,GL_RENDERBUFFER_HEIGHT_OES,&backingHeight);
    if (USE_DEPTH_BUFFER) {
        glGenRenderbuffersOES(1,&depthRenderbuffer);
        glBindRenderbufferOES(GL_RENDERBUFFER_OES,depthRenderbuffer);
        glRenderbufferStorageOES(
			GL_RENDERBUFFER_OES,GL_DEPTH_COMPONENT16_OES,backingWidth,backingHeight);
        glFramebufferRenderbufferOES(
			GL_FRAMEBUFFER_OES,GL_DEPTH_ATTACHMENT_OES,GL_RENDERBUFFER_OES,depthRenderbuffer);
    }
	return (glCheckFramebufferStatusOES(GL_FRAMEBUFFER_OES) == GL_FRAMEBUFFER_COMPLETE_OES);
}

//フレームバッファの破棄
- (void)destroyFramebuffer {    
    glDeleteFramebuffersOES(1,&viewFramebuffer);
    viewFramebuffer=0;
    glDeleteRenderbuffersOES(1,&viewRenderbuffer);
    viewRenderbuffer=0;    
    if (depthRenderbuffer) {
        glDeleteRenderbuffersOES(1,&depthRenderbuffer);
        depthRenderbuffer=0;
    }
}

//アニメーションの開始
- (void)startAnimation {
    self.animationTimer=[NSTimer scheduledTimerWithTimeInterval:animationInterval 
														 target:self selector:@selector(drawView)userInfo:nil repeats:YES];
}

//アニメーションの停止
- (void)stopAnimation {
    self.animationTimer=nil;
}

//アニメーションタイマーの指定
- (void)setAnimationTimer:(NSTimer*)newTimer {
    [animationTimer invalidate];
    animationTimer=newTimer;
}

//アニメーションインターバルの指定
- (void)setAnimationInterval:(NSTimeInterval)interval {
    animationInterval=interval;
    if (animationTimer) {
        [self stopAnimation];
        [self startAnimation];
    }
}
@end
