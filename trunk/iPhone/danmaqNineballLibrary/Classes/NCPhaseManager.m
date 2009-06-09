////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	Nineball Library Core for iPhone	Programmed by Mc/danmaq
//		──フェーズ・カウンタ管理
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#import "NCPhaseManager.h"

@implementation NCPhaseManager

@synthesize phase;
@synthesize count;
@synthesize nextPhase;
@synthesize isReservedNextPhase;
@synthesize phaseCount;
@synthesize prevPhase;
@synthesize phaseStartTime;

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// コンストラクタ。
//
// return オブジェクト
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (id) init{
	self = [super init];
	if(self != nil){ [self reset]; }
	return self;
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// デストラクタ。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)dealloc{
	[self reset];
	[super dealloc];
}

///////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 現在のフェーズ値を取得します。
//
// return フェーズ値
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (NSUInteger) phase{ return _uPhase; }

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 現在のフェーズ値を設定します。
//
// param value フェーズ値
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void) setPhase:(NSUInteger)value{
	_uPrevPhase = self.phase;
	_uPhase = value;
	_uPhaseStartTime = self.count;
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 現在のカウンタ値を取得します。
//
// return カウンタ値
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (NSUInteger) count{ return _uCount; }

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 現在のカウンタ値を設定します。
// 通常はインクリメントのみでほかの値は入れません。
// 次のフェーズへ行く予約がある場合、自動的に次のフェーズへ移行します。
//
// param value カウンタ値
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void) setCount:(NSUInteger)value{
	_uCount = value;
	if( self.isReservedNextPhase ){
		self.phase = self.nextPhase;
		self.isReservedNextPhase = NO;
	}
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// カウンタ値変化時に進むフェーズ値を取得します。
//
// return フェーズ値
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (NSInteger) nextPhase{ return _nNextPhase; }

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// カウンタ値変化時に進むフェーズ値を設定します。
//
// param value フェーズ値
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void) setNextPhase:(NSInteger)value{
	if( self.phase != value ){ _nNextPhase = value; }
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// カウンタ値変化時に次のフェーズへ進むかどうかを取得します。
//
// return カウンタ値変化時に次のフェーズへ進む場合、真値
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (BOOL) isReservedNextPhase{ return self.nextPhase >= 0; }

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// カウンタ値変化時に次のフェーズへ進むかどうかを設定します。
//
// param value カウンタ値変化時に次のフェーズへ進むかどうか
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void) setIsReservedNextPhase:(BOOL)value{ self.nextPhase = value ? self.phase + 1 : -1; }

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 現在フェーズ内におけるカウンタ値を取得します。
//
// return カウンタ値
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (NSUInteger) phaseCount{ return self.count - self.phaseStartTime; }

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 現在のフェーズが開始されたときのカウンタ値を取得します。
//
// return カウンタ値
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (NSUInteger) phaseStartTime{ return _uPhaseStartTime; }

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 前回のフェーズ値を取得します。
//
// return フェーズ値
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (NSUInteger) prevPhase{ return _uPrevPhase; }

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// フェーズ・カウンタなど、内部データを初期かします。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void) reset{
	_uCount = 0;
	_uPhase = 0;
	_uPrevPhase = 0;
	_uPhaseStartTime = 0;
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// このクラスの現在の状態を取得します。
//
// return このクラスの現在の状態
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (NSString *) toString{
	return [[NSString alloc] initWithFormat:@"Phase[Now:%u,Next:%d,Prev:%u],Count[Total:%u,Phase:%u]",
			self.phase ,self.nextPhase, self.prevPhase, self.count, self.phaseCount];
}

@end
