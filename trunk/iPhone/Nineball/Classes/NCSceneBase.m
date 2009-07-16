//
//  NCSceneBase.m
//  danmaqNineballLibrary
//
//  Created by Shuhei Nomura on 09/05/21.
//  Copyright 2009 PRODIGY. All rights reserved.
//

#import "NCSceneBase.h"

@implementation NCSceneBase

@synthesize nextScene;
@synthesize phaseManager;
@synthesize taskManager;

- (id)init{
	self = [super init];
	if(self != nil){ _mgrPhase = [[NCPhaseManager alloc] init]; }
	return self;
}

- (void)dealloc{
	SAFE_RELEASE(_mgrPhase);
	[super dealloc];
}

- (BOOL)update{
	[self.taskManager update];
	self.phaseManager.count++;
	return self.nextScene == nil;
}

- (void)draw{ [self.taskManager draw]; }

- (NCPhaseManager *)phaseManager{ return _mgrPhase; }

- (NCTaskManager *)taskManager{ return _mgrTask; }

- (NSObject<NIScene> *)nextScene{ return _nextScene; }

- (void)setNextScene:(NSObject<NIScene> *)value{ _nextScene = value; }

@end
