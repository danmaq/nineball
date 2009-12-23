////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	Nineball Library Core for iPhone	Programmed by Mc/danmaq
//		──タスク管理
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#import "nineball.h"
#import "NITask.h"

@interface NCTaskManager : NSObject {
	
	// タスク一覧。
	NSMutableArray *_aTasks;
	
	// 一時停止中かどうか。
	BOOL _bPause;
}

// 一時停止するかどうか。
@property BOOL pause;

// 登録タスクを1フレーム分更新します。
- (void)update;

// 登録タスクの描画を1フレーム分更新します。
- (void)draw;

// タスクを登録します。
- (void)add:(NSObject<NITask> *)task;

// タスクを削除します。
- (void)remove:(NSObject<NITask> *)task;

// 特定優先度に所属するタスクを削除します。
- (void)removePriority:(NSUInteger)uPriority;

// 特定優先度一帯に所属するタスクを削除します。
- (void)removePriorityRange:(NSRange *)range;

// 特定優先度に所属するタスクを検索します。
- (NSArray *)search:(NSUInteger)uPriority;

// 初期状態に戻します。
- (void)reset;

@end
