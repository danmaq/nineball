////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	Nineball Library Core for iPhone	Programmed by Mc/danmaq
//		──内分カウンタ
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#import "nineball.h"

@interface NCInterpolate : NSObject {}

+ (float) lerp:(float)fRate start:(float)fStart end:(float)fEnd limiter:(BOOL)bLimiter;
+ (float) smooth:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd limiter:(BOOL)bLimiter;
+ (float) slowdown:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd limiter:(BOOL)bLimiter;
+ (float) accelerate:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd limiter:(BOOL)bLimiter;
+ (float) splineFSF:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd limiter:(BOOL)bLimiter;
+ (float) splineSFS:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd limiter:(BOOL)bLimiter;
+ (float) neville:(float)fNow limit:(float)fLimit start:(float)fStart middle:(float)fMiddle end:(float)fEnd limiter:(BOOL)bLimiter;
+ (float) bezier:(float)fNow limit:(float)fLimit start:(float)fStart middle:(float)fMiddle end:(float)fEnd limiter:(BOOL)bLimiter;

+ (float) lerp:(float)fRate start:(float)fStart end:(float)fEnd;
+ (float) smooth:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd;
+ (float) slowdown:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd;
+ (float) accelerate:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd;
+ (float) splineFSF:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd;
+ (float) splineSFS:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd;
+ (float) neville:(float)fNow limit:(float)fLimit start:(float)fStart middle:(float)fMiddle end:(float)fEnd;
+ (float) bezier:(float)fNow limit:(float)fLimit start:(float)fStart middle:(float)fMiddle end:(float)fEnd;

@end
