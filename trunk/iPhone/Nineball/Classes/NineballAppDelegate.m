//
//  NineballAppDelegate.m
//  Nineball
//
//  Created by 野村 周平 on 09/07/07.
//  Copyright 株式会社プロディジ 開発室 2009. All rights reserved.
//

#import "NineballAppDelegate.h"

@implementation NineballAppDelegate

@synthesize window;


- (void)applicationDidFinishLaunching:(UIApplication *)application {    

    // Override point for customization after application launch
    [window makeKeyAndVisible];
}


- (void)dealloc {
    [window release];
    [super dealloc];
}


@end
