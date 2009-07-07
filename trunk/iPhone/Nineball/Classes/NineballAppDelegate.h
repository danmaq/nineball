//
//  NineballAppDelegate.h
//  Nineball
//
//  Created by 野村 周平 on 09/07/07.
//  Copyright 株式会社プロディジ 開発室 2009. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface NineballAppDelegate : NSObject <UIApplicationDelegate> {
    UIWindow *window;
}

@property (nonatomic, retain) IBOutlet UIWindow *window;

@end

