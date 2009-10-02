////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	Nineball Library Core for iPhone	Programmed by Mc/danmaq
//		──シーン管理
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#import "NCSceneManager.h"

@implementation NCSceneManager

@synthesize now;
@synthesize total;

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// コンストラクタ。
//
// return オブジェクト
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (id)init{
	self = [super init];
	if(self != nil){ _aSceneStack = [[NSMutableArray alloc] init]; }
	return self;
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// デストラクタ。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)dealloc{
	while([_aSceneStack count] > 0){ [self erase]; }
	[super dealloc];
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// シーンを追加します。現在アクティブなシーンはスリープ状態となります。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)add:(NSObject<NIScene> *)scene{ [_aSceneStack addObject:scene]; }

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 現在アクティブなシーンを終了／解放します。スリープなシーンが残っている場合、
// そのシーンがアクティブになります。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)erase{
	if(self.total > 0){
		[self.now release];
		[_aSceneStack removeLastObject];
	}
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 現在アクティブなシーンを1フレーム分更新します。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (BOOL)update{
	NSObject<NIScene> *scene = self.now;
	if(scene != nil){
		BOOL bContinue = [scene update];
		NSObject<NIScene> *nextScene = scene.nextScene;
		if(!bContinue){ [self erase]; }
		if(nextScene != nil){
			if(bContinue){ scene.nextScene = nil; }
			[self add:nextScene];
		}
	}
	return self.total > 0;
}

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 現在アクティブなシーンの描画を1フレーム分更新します。
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (void)draw{
	NSObject<NIScene> *scene = self.now;
	if(scene != nil){ [scene draw]; }
}

///////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 現在アクティブなシーンを取得します。
//
// return シーン オブジェクト。存在しない場合、nil
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (NSObject<NIScene> *) now{ return [_aSceneStack lastObject]; }

//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
// 現在アクティブなシーンを含んだ、積まれているスタックの総計を取得します。
//
// return 積まれているスタックの総計
//*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━*
- (NSUInteger) total{ return [_aSceneStack count]; }

@end
