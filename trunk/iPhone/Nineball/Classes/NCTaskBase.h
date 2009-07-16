////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	Nineball Library Core for iPhone	Programmed by Mc/danmaq
//		──タスク基底クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#import "nineball.h"
#import "NITask.h"
#import "NCPhaseManager.h"

//*-----------------------------------------------------------------*
// タスク用基底クラスです。
// タスク管理クラスに登録するためには、このクラスを継承するか、
// またはNITaskインターフェイスを実装します。
//*-----------------------------------------------------------------*
@interface NCTaskBase : NSObject<NITask> {
@private
	// 優先度。
	NSUInteger _uPriority;
	
	// 優先度をロックするかどうか。
	BOOL _bLockPriority;
	
	// 一時停止が有効かどうか。
	BOOL _bPauseAvaliable;
	
	// このタスクを自殺させるかどうか。
	BOOL _bDead;
	
	// フェーズ・カウンタ管理クラスへのポインタ。
	NCPhaseManager *_mgrPhase;
	
	// タスク管理クラスへのポインタ。
	NCTaskManager *_mgrTask;
}

// 優先度。
@property NSUInteger priority;

// 一時停止が有効かどうか。
@property BOOL pauseAvaliable;

// 優先度をロックするかどうか。
@property BOOL lockPriority;

// タスク管理クラスへのポインタ。
@property (retain) NCTaskManager *taskManager;

// このタスクを自殺させるかどうか。
@property BOOL dead;

// フェーズ・カウンタ管理クラスへのポインタ。
@property (readonly)NCPhaseManager *phaseManager;

// タスクを1フレーム分更新します。
- (BOOL)update;

// タスクの描画を1フレーム分更新します。
- (void)draw;

@end
