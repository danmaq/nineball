////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	Nineball Library Core for iPhone	Programmed by Mc/danmaq
//		──内分カウンタ
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
// !!! FIXME !!! : なんか四捨五入されるんだが

#import "NCInterpolate.h"

@implementation NCInterpolate

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 2つの値の間を線形補間します。
//
// return 補間された値
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
+ (float) lerp:(float)fRate start:(float)fStart end:(float)fEnd limiter:(BOOL)bLimiter{
	float fResult = fStart + (fEnd - fStart) * fRate;
	if(bLimiter){
		if(fRate <= 0.0f){ fResult = fStart; }
		ef(fRate >= 1.0f){ fResult = fEnd; }
	}
	return fResult;
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 等速変化する内分カウンタです。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
+ (float) smooth:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd limiter:(BOOL)bLimiter{
	return fLimit == 0 ? fEnd : [NCInterpolate lerp:fNow / fLimit start:fStart end:fEnd limiter:bLimiter];
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 減速変化する内分カウンタです。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
+ (float) slowdown:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd limiter:(BOOL)bLimiter{
	return fLimit == 0 ? fEnd : [NCInterpolate lerp:1 - pow(1 - fNow / fLimit, 2) start:fStart end:fEnd limiter:bLimiter];
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 加速変化する内分カウンタです。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
+ (float) accelerate:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd limiter:(BOOL)bLimiter{
	return fLimit == 0 ? fEnd : [NCInterpolate lerp:pow(fNow / fLimit, 2) start:fStart end:fEnd limiter:bLimiter];
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 減速変化>加速変化を組み合わせスプラインのような動きを模する内分カウンタです。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
+ (float) splineFSF:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd limiter:(BOOL)bLimiter{
	const float fCenter = [NCInterpolate smooth:1 limit:2 start:fStart end:fEnd];
	const float fHalfLimit = fLimit / 2;
	return fNow < fHalfLimit ?
	[NCInterpolate slowdown:fNow limit:fHalfLimit start:fStart end:fCenter] :
	[NCInterpolate accelerate:fNow - fHalfLimit limit:fHalfLimit start:fCenter end:fEnd limiter:bLimiter];
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 加速変化>減速変化を組み合わせスプラインのような動きを模する内分カウンタです。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
+ (float) splineSFS:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd limiter:(BOOL)bLimiter{
	const float fCenter = [NCInterpolate smooth:1 limit:2 start:fStart end:fEnd limiter:bLimiter];
	const float fHalfLimit = fLimit / 2;
	return fNow < fHalfLimit ?
	[NCInterpolate accelerate:fNow limit:fHalfLimit start:fStart end:fCenter limiter:bLimiter] :
	[NCInterpolate slowdown:fNow - fHalfLimit limit:fHalfLimit start:fCenter end:fEnd limiter:bLimiter];
}
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// ネヴィル・スプラインのシミュレータです。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
+ (float) neville:(float)fNow limit:(float)fLimit start:(float)fStart middle:(float)fMiddle end:(float)fEnd limiter:(BOOL)bLimiter{
	float fTimePoint = fNow / fLimit * 2.0f;
	fMiddle = fEnd + (fEnd - fMiddle) * (fTimePoint - 2.0f);
	float fResult = fMiddle + (fMiddle - (fMiddle + (fMiddle - fStart) * (fTimePoint - 1.0f))) * (fTimePoint - 2.0f) * 0.5f;
	if(bLimiter){
		if(fNow <= 0){ fResult = fStart; }
		ef(fNow >= fLimit || fStart == fEnd || fLimit == 0){ fResult = fEnd; }
	}
	return fResult;
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// ベジェ・スプラインのシミュレータです。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
+ (float) bezier:(float)fNow limit:(float)fLimit start:(float)fStart middle:(float)fMiddle end:(float)fEnd limiter:(BOOL)bLimiter{
	float fTimePoint = fNow / fLimit * 2.0f;
	float fResidual = 1.0f - fTimePoint;
	float fResult = (pow(fResidual, 2) * fStart) + (2 * fResidual * fTimePoint * fMiddle) + (pow(fTimePoint, 2) * fEnd);
	if(bLimiter){
		if(fNow <= 0){ fResult = fStart; }
		ef(fNow >= fLimit || fStart == fEnd || fLimit == 0){ fResult = fEnd; }
	}
	return fResult;
}

+ (float) lerp:(float)fRate start:(float)fStart end:(float)fEnd{
	return [NCInterpolate lerp:fRate start:fStart end:fEnd limiter:YES];
}
+ (float) smooth:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd{
	return [NCInterpolate smooth:fNow limit:fLimit start:fStart end:fEnd limiter:YES];
}
+ (float) slowdown:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd{
	return [NCInterpolate slowdown:fNow limit:fLimit start:fStart end:fEnd limiter:YES];
}
+ (float) accelerate:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd{
	return [NCInterpolate accelerate:fNow limit:fLimit start:fStart end:fEnd limiter:YES];
}
+ (float) splineFSF:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd{
	return [NCInterpolate splineFSF:fNow limit:fLimit start:fStart end:fEnd limiter:YES];
}
+ (float) splineSFS:(float)fNow limit:(float)fLimit start:(float)fStart end:(float)fEnd{
	return [NCInterpolate splineSFS:fNow limit:fLimit start:fStart end:fEnd limiter:YES];
}
+ (float) neville:(float)fNow limit:(float)fLimit start:(float)fStart middle:(float)fMiddle end:(float)fEnd{
	return [NCInterpolate neville:fNow limit:fLimit start:fStart middle:fMiddle end:fEnd limiter:YES];
}
+ (float) bezier:(float)fNow limit:(float)fLimit start:(float)fStart middle:(float)fMiddle end:(float)fEnd{
	return [NCInterpolate bezier:fNow limit:fLimit start:fStart middle:fMiddle end:fEnd limiter:YES];
}


@end
