////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	Nineball Library Core for iPhone	Programmed by Mc/danmaq
//		──タスク管理
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#import "NCTaskManager.h"

@interface NCTaskManager ()

- (void) releaseArray;

@end


@implementation NCTaskManager

@synthesize pause;

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// コンストラクタ。
//
// return オブジェクト
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (id)init{
	self = [super init];
	if(self != nil){ [self reset]; }
	return self;
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// デストラクタ。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)dealloc{
	[self releaseArray];
	[super dealloc];
}

///////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 登録タスクを1フレーム分更新します。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)update{
	NSUInteger i = 0;
	while(i < [_aTasks count]){
		NSObject<NITask> *task = [_aTasks objectAtIndex:i];
		((self.pause && task.pauseAvaliable) || [task update]) ? i++ : [self remove:task];
	}
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 登録タスクの描画を1フレーム分更新します。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)draw{
	for(NSObject<NITask> *task in _aTasks){ [task draw]; }
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// タスクを登録します。
// 登録したタスクは自動的に優先度が固定されます。
//
//　param task 対象タスク
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)add:(NSObject<NITask> *)task{
	if(!(task == nil || [_aTasks indexOfObject:task] == NSNotFound)){
		task.lockPriority = YES;
		task.taskManager = self;
		if([task respondsToSelector:@selector(taskDidRegist)]){ [task taskDidRegist]; }
		const NSUInteger uPriority = task.priority;
		const NSUInteger uLength = [_aTasks count];
		for(NSUInteger i = 0; i < uLength; i++){
			if(((NSObject<NITask> *)[_aTasks objectAtIndex:i]).priority == uPriority){
				[_aTasks insertObject:task atIndex:i];
				return;
			}
		}
		[_aTasks addObject:task];
	}
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// タスクを削除します。
//
//　param task 対象タスク
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)remove:(NSObject<NITask> *)task{
	[_aTasks removeObject:task];
	[task release];
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 特定優先度に所属するタスクを削除します。
//
// param uPriority 優先度
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)removePriority:(NSUInteger)uPriority{
	const NSArray *aTarget = [self search:uPriority];
	for(NSObject<NITask> *task in aTarget){ [self remove:task]; }
	[aTarget release];
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 特定優先度帯域に所属するタスクを削除します。
//
// param range 帯域
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)removePriorityRange:(NSRange *)range{
	const NSUInteger uLimit = range->location + range->length;
	for(NSUInteger i = range->location; i < uLimit; i++){ [self removePriority:i]; }
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 特定優先度に所属するタスクの一覧を作成します。
//
// param uPriority 優先度
// return タスク一覧
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (NSArray *)search:(NSUInteger)uPriority{
	NSMutableArray *result = [NSMutableArray array];
	for(NSObject<NITask> *task in _aTasks){
		if(task.priority == uPriority){ [result addObject:task]; }
		ef(task.priority > uPriority){ break; }
	}
	return result;
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 初期状態に戻します。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)reset{
	self.pause = NO;
	[self releaseArray];
	_aTasks = [[NSMutableArray alloc] init];
}

///////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 配列を解放します。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)releaseArray{
	if(_aTasks != nil){
		for(NSObject<NITask> *task in _aTasks){ [task release]; }
		SAFE_RELEASE(_aTasks);
	}
}

///////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// タスクを一時停止しているかどうかを取得します。
//
// return タスクを一時停止している場合、YES
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (BOOL)pause{ return _bPause; }

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// タスクを一時停止するかどうかを設定します。
//
// param value タスクを一時停止するかどうか
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)setPause:(BOOL)value{ _bPause = value; }

@end
