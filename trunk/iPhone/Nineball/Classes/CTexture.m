////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	Nineball Library Core for iPhone	Programmed by Mc/danmaq
//		──テクスチャ管理クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#import <QuartzCore/QuartzCore.h>
#import "CTexture.h"

@interface CTexture (TexturePrivate)

+ (BOOL)makeTexture:(UIImage *)image
	toOutput:(u_char **)textureData andTextureSize:(int *)lpTextureSize 
	andTextureWidth:(int *)lpTextureWidth andTextureHeight:(int *)lpTextureHeight;

@end

@implementation CTexture

@synthesize data;
@synthesize name;
@synthesize width;
@synthesize height;

- (void)dealloc {
	GLuint num = self.name;
	if(num != 0){ glDeleteTextures(1, &num); }
	if(data != NULL){ free(data); }
	[super dealloc];
}

//テクスチャの読み込み
+ (CTexture *)loadTexture:(NSString *)fileName {
	NSString *path = [[NSBundle mainBundle] pathForResource:fileName ofType:nil];
	UIImage *image = [[[UIImage alloc] initWithContentsOfFile:path] autorelease];
	return [CTexture makeTexture:image];
}

//テクスチャの生成
+ (CTexture *)makeTexture:(NSString *)text font:(UIFont *)font color:(UIColor *)color {
	//テキストサイズの計算
	NSArray *textArray = [text componentsSeparatedByString:@"\n"];
	CGSize textSize = CGSizeMake(0,0);
	float h = 0;
	for(int i = 0; i < textArray.count; i++){
		CGSize size = [[textArray objectAtIndex:i] sizeWithFont:font];	
		if(textSize.width < size.width){ textSize.width=size.width; }
		if(h < size.height){ h=size.height; }
	}	
	textSize.height = h * textArray.count;
	
	//ラベルの生成
	UILabel *label=[[[UILabel alloc] initWithFrame:
		CGRectMake(0,0,textSize.width,textSize.height)] autorelease];
	[label setText:text];
	[label setFont:font];
	[label setTextColor:color];
	[label setBackgroundColor:[UIColor clearColor]];
	[label setNumberOfLines:0];
	
	//テクスチャの生成
	UIGraphicsBeginImageContext(label.frame.size);
	[label.layer renderInContext:UIGraphicsGetCurrentContext()];
	UIImage *image=UIGraphicsGetImageFromCurrentImageContext();
	UIGraphicsEndImageContext();
	return [CTexture makeTexture:image];
}

//テクスチャの生成
+ (CTexture *)makeTexture:(UIImage *)image {
	u_char	*textureData;
	GLuint	textureName;
	GLsizei	textureSize;
	GLsizei	textureWidth;
	GLsizei	textureHeight;
	CTexture *lpTexture = nil;
	//テクスチャの生成
	if ([CTexture makeTexture:image toOutput:(u_char **)&textureData andTextureSize:&textureSize 
			andTextureWidth:&textureWidth andTextureHeight:&textureHeight]){
		//テクスチャの設定
		glGenTextures(1, &textureName);
		glBindTexture(GL_TEXTURE_2D, textureName);
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, textureSize, textureSize,
			0, GL_RGBA, GL_UNSIGNED_BYTE, textureData);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR); 
		
		//テクスチャオブジェクトの生成
		CTexture *texture=[[[CTexture alloc] init] autorelease];
		texture.data = textureData;
		texture.name = textureName;
		texture.width = textureWidth;
		texture.height = textureHeight;
		lpTexture = texture;
	}
	return lpTexture;
}

//テクスチャの生成
+ (BOOL)makeTexture:(UIImage *)image toOutput:(u_char **)textureData
	andTextureSize:(int *)lpTextureSize 
	andTextureWidth:(int *)lpTextureWidth andTextureHeight:(int *)lpTextureHeight {
	CGImageRef			imageRef;
	NSUInteger			i;
	int					textureSize;
	int					imageWidth;
	int					imageHeight;
	NSUInteger			maxImageSize;
	CGContextRef		contextRef=nil;
	CGColorSpaceRef		colorSpace;
	BOOL				hasAlpha;
	size_t				bitsPerComponent;
	CGImageAlphaInfo	info;

	if (!image) return NO;
	//イメージ情報の取得
	imageRef = [image CGImage];   
	imageWidth = CGImageGetWidth(imageRef);
	imageHeight = CGImageGetHeight(imageRef);
	maxImageSize = imageWidth > imageHeight ? imageWidth : imageHeight;
	for(i=2; i<=1024; i*=2){
		if(i >= maxImageSize){
			textureSize = i;
			break;
		}
	}
	*lpTextureSize = textureSize;
	*lpTextureWidth = imageWidth;
	*lpTextureHeight = imageHeight;
	info = CGImageGetAlphaInfo(imageRef);
	
	//アルファ成分チェック
	hasAlpha = ((info == kCGImageAlphaPremultipliedLast) || 
			  (info == kCGImageAlphaPremultipliedFirst) || 
			  (info == kCGImageAlphaLast) || 
			  (info == kCGImageAlphaFirst) ? YES : NO);
	colorSpace = CGColorSpaceCreateDeviceRGB();
	*textureData = (u_char *)malloc(textureSize * textureSize * 4);
	if(!*textureData){ return NO; }
	bitsPerComponent = hasAlpha ? kCGImageAlphaPremultipliedLast : kCGImageAlphaNoneSkipLast;
	contextRef = CGBitmapContextCreate(*textureData, textureSize, textureSize, 8,
		4 * textureSize, colorSpace, bitsPerComponent | kCGBitmapByteOrder32Big);
	CGColorSpaceRelease(colorSpace);
	
	//画像ファイルの画像サイズ!=テクスチャのサイズの時
	if((textureSize != imageWidth) || (textureSize != imageHeight)){
		CGContextScaleCTM(contextRef, (CGFloat)textureSize / imageWidth,
						  (CGFloat)textureSize / imageHeight);
	}
	CGContextDrawImage(contextRef, CGRectMake(0, 0, CGImageGetWidth(imageRef),
		CGImageGetHeight(imageRef)), imageRef);
	CGContextRelease(contextRef);
	return YES;
}

@end
