//
//  NCBundleAlert.m
//  iWMP
//
//  Created by Yamasaki Goro on 09/04/03.
//  Copyright 2009 Prodigy co., ltd.. All rights reserved.
//

#import "NCBundleAlert.h"

@implementation NCBundleAlert

- (id)init:(CFStringRef)strBundleName ext:(CFStringRef)strExt{
	self = [super init];
	if(self != nil){
		CFBundleRef mainBundle = CFBundleGetMainBundle();
		_soundFileURLRef = CFBundleCopyResourceURL(mainBundle, strBundleName, strExt, NULL);
		AudioServicesCreateSystemSoundID(_soundFileURLRef, &_soundFileObject);
	}
	return self;
}

- (void)play{ AudioServicesPlayAlertSound(_soundFileObject); }

- (void)dealloc{
	AudioServicesDisposeSystemSoundID(_soundFileObject);
	CFRelease(_soundFileURLRef);
	[super dealloc];
}

@end
