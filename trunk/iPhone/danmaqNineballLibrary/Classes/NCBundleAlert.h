//
//  NCBundleAlert.h
//  iWMP
//
//  Created by Yamasaki Goro on 09/04/03.
//  Copyright 2009 Prodigy co., ltd.. All rights reserved.
//

#import "nineball.h"
#include <AudioToolbox/AudioToolbox.h>

@interface NCBundleAlert : NSObject {
	SystemSoundID _soundFileObject;
	CFURLRef _soundFileURLRef;
}

- (id)init:(CFStringRef)strBundleName ext:(CFStringRef)strExt;
- (void)play;

@end
