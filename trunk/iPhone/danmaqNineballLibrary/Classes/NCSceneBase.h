////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	Nineball Library Core for iPhone	Programmed by Mc/danmaq
//		──シーン基底クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#import "nineball.h"
#import "NIScene.h"
#import "NCTaskManager.h"
#import "NCPhaseManager.h"

@interface NCSceneBase : NSObject<NIScene> {
	NSObject<NIScene> *_nextScene;
	NCPhaseManager *_mgrPhase;
	NCTaskManager *_mgrTask;
}

// 次のシーン。
@property (assign) NSObject<NIScene> *nextScene;

// フェーズ／カウンタ管理クラス。
@property (readonly) NCPhaseManager *phaseManager;

// タスク管理クラス。
@property (readonly) NCTaskManager *taskManager;

// シーンを1フレーム分更新します。
- (BOOL)update;

// シーンの描画を1フレーム分更新します。
- (void)draw;

@end
