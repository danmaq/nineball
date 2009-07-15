////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	Nineball Library Core for iPhone	Programmed by Mc/danmaq
//		──フェーズ・カウンタ管理
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#import "nineball.h"

//*-----------------------------------------------------------------*
// フェーズ・カウンタ進行の管理をするクラス。
// 主にフレームカウンタとして使用します。(1フレームに1度countをインクリメントする)
//*-----------------------------------------------------------------*
@interface NCPhaseManager : NSObject {
	
	// カウンタ値。
	NSUInteger _uCount;
	
	// フェーズ値。
	NSUInteger _uPhase;
	
	// 現在のフェーズ開始時のカウンタ値。
	NSUInteger _uPhaseStartTime;
	
	// 前回のフェーズ値。
	NSUInteger _uPrevPhase;
	
	// 次フレーム以降に移行するフェーズ値。
	NSInteger _nNextPhase;
}

// フェーズ値。
@property NSUInteger phase;

// カウンタ値。
@property NSUInteger count;

// 次フレーム以降に移行するフェーズ値。
@property NSInteger nextPhase;

// 次フレーム以降次のフェーズへ移行するかどうか。
@property BOOL isReservedNextPhase;

// 現在フェーズのカウンタ値。
@property(readonly) NSUInteger phaseCount;

// 前回のフェーズ値。
@property(readonly) NSUInteger prevPhase;

// 現在のフェーズ開始時のカウンタ値。
@property(readonly) NSUInteger phaseStartTime;

// フェーズ・カウンタを初期状態に戻します。
- (void)reset;

// このインスタンスの状態を文字列で取得します。
- (NSString *)toString;

@end
